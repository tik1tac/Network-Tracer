using Microsoft.Win32;

using Network_Tracer.Model;
using Network_Tracer.Model.Graph;
using Network_Tracer.Model.Graph.AbstractGraph;
using Network_Tracer.View;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Network_Tracer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += OnWindowClosing;
            this.Closed += CloseMainWindow;
            Device.Window = this;
            SelectedTool = Tools.Cursor;
        }

        #region Элементы на канвасе
        LineConnect SelectedLine;
        Device SelectedDevice;
        Source source;
        private string fileName;
        public bool Modified { get; set; } = false;
        private string FileName
        {
            get
            {
                return this.fileName;
            }

            set
            {
                this.fileName = value;

                if (!string.IsNullOrEmpty(this.fileName))
                {
                    this.Title = this.Title + " - " + this.fileName.Split('\\').Last();
                }
                else
                {
                    this.Title = this.Title + " - " + "Новая схема";
                }
            }
        }
        public Tools SelectedTool
        {
            get
            {
                if (this.Connection.Opacity > 0.9)
                {
                    return Tools.Connection;
                }

                if (this.VZGButton.Opacity > 0.9)
                {
                    return Tools.VZG;
                }

                if (this.PEGButton.Opacity > 0.9)
                {
                    return Tools.PEG;
                }

                if (this.SEButton.Opacity > 0.9)
                {
                    return Tools.SE;
                }
                if (this.PEGSpare.Opacity > 0.9)
                {
                    return Tools.PEGSpare;
                }
                if (this.User.Opacity > 0.9)
                {
                    return Tools.User;
                }
                return Tools.Cursor;
            }
            set
            {
                switch (value)
                {
                    case Tools.VZG:
                        this.Connection.Opacity = this.PEGButton.Opacity = this.SEButton.Opacity = this.CursorButton.Opacity = this.PEGSpare.Opacity = this.User.Opacity = 0.4;
                        this.VZGButton.Opacity = 1.0;
                        break;

                    case Tools.Connection:
                        this.SEButton.Opacity = this.VZGButton.Opacity = this.PEGButton.Opacity = this.CursorButton.Opacity = this.PEGSpare.Opacity = this.User.Opacity = 0.4;
                        this.Connection.Opacity = 1.0;
                        break;

                    case Tools.PEG:
                        this.Connection.Opacity = this.VZGButton.Opacity = this.SEButton.Opacity = this.CursorButton.Opacity = this.PEGSpare.Opacity = this.User.Opacity = 0.4;
                        this.PEGButton.Opacity = 1.0;
                        break;

                    case Tools.SE:
                        this.Connection.Opacity = this.VZGButton.Opacity = this.PEGButton.Opacity = this.CursorButton.Opacity = this.PEGSpare.Opacity = this.User.Opacity = 0.4;
                        this.SEButton.Opacity = 1.0;
                        break;
                    case Tools.PEGSpare:
                        this.Connection.Opacity = this.VZGButton.Opacity = this.PEGButton.Opacity = this.CursorButton.Opacity = SEButton.Opacity = this.User.Opacity = 0.4;
                        this.PEGSpare.Opacity = 1.0;
                        break;
                    case Tools.User:
                        this.Connection.Opacity = this.VZGButton.Opacity = this.PEGButton.Opacity = this.CursorButton.Opacity = SEButton.Opacity = this.PEGSpare.Opacity = 0.4;
                        this.User.Opacity = 1.0;
                        break;

                    default:
                        this.Connection.Opacity = this.VZGButton.Opacity = this.PEGButton.Opacity = this.SEButton.Opacity = this.PEGSpare.Opacity = this.User.Opacity = 0.4;
                        this.CursorButton.Opacity = 1.0;
                        break;
                }
            }
        }
        #region Анимация
        public int Mode = 1;

        public int Delay = 0;
        public int Speed = 2000;

        private readonly List<LineConnect> _lines = new List<LineConnect>();
        private System.Threading.Thread _animation;

        private void Animate(object sender = null, RoutedEventArgs e = null)
        {

            StopAnimation();
            CanvasField.IsEnabled = false;

            LineConnect.CompleteAnimationCallback callback = delegate (LineConnect l)
            {

                Task.Run(delegate ()
                {
                    _animation = System.Threading.Thread.CurrentThread; //берём поток нащей функии чтобы потом в любой момент убить его
                    foreach (LineConnect line in _lines)
                    {
                        if (_animation == null) return;
                        System.Threading.Thread.Sleep(Delay); //задержка
                        App.Current.Dispatcher.Invoke(delegate ()
                        {
                            line.SetSpeed(new TimeSpan(0, 0, 0, 0, Speed)); //установка скорости
                            line.BeginAnimation(); //старт анимации
                        });

                    }
                });
            };

            _lines.Last().CompleteAnimationEvent += callback; //подписиваемся на событие

            foreach (LineConnect line in _lines)
            {
                line.Line.X1 = line.Line.X2 = line.Line.Y1 = line.Line.Y2 = 0; //скрываем линии
            }
            callback(null);

        }

        private void StopAnimation(object sender = null, RoutedEventArgs e = null)
        {

            _animation?.Abort(); //убиваем поток если он есть
            _animation = null;

            foreach (LineConnect line in _lines) //убираем функции с события и останавливаем анимации
            {
                line.StopAnimation();
                line.RemoveAllHandles_CompleteAnimationEvent();
            }
            CanvasField.IsEnabled = true;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (Device.NewLine == null) return;
            Device.NewLine.X2 = Mouse.GetPosition(this).X - CanvasField.Margin.Left;
            Device.NewLine.Y2 = Mouse.GetPosition(this).Y - CanvasField.Margin.Top;
        }
        #endregion
        /// <summary>
        /// Выбор элементов на панели
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            CreatorNodes cn = new FactoryNodes();
            Point p = e.GetPosition(this);
            switch (this.SelectedTool)
            {
                case Tools.PEG:
                    PEG peg = cn.CreatePeg(CanvasField);
                    Canvas.SetLeft(peg, p.X - (peg.Width / 2));
                    Canvas.SetTop(peg, p.Y - (peg.Height / 2));
                    peg.MouseLeftButtonDown += this.OnPEGLeftButtonDownAsync;
                    CanvasField.Children.Add(peg);
                    SelectedTool = Tools.Cursor;
                    Device.pegelement = peg;
                    Device.Vertex.Add(peg);
                    Modified = true;
                    break;

                case Tools.VZG:
                    VZG vzg = cn.CreateVZG(CanvasField);
                    Canvas.SetLeft(vzg, p.X - (vzg.Width / 2));
                    Canvas.SetTop(vzg, p.Y - (vzg.Height / 2));
                    vzg.MouseLeftButtonDown += this.OnVZGLeftButtonDown;
                    vzg.MouseDoubleClick += Vzg_MouseDoubleClick;
                    CanvasField.Children.Add(vzg);
                    SelectedTool = Tools.Cursor;
                    Device.Vertex.Add(vzg);
                    Modified = true;
                    break;

                case Tools.SE:
                    SE se = cn.CreateSe(CanvasField);
                    Canvas.SetLeft(se, p.X - (se.Width / 2));
                    Canvas.SetTop(se, p.Y - (se.Height / 2));
                    se.MouseLeftButtonDown += this.OnSELeftButtonDown;
                    se.MouseDoubleClick += Se_MouseDoubleClick;
                    CanvasField.Children.Add(se);
                    SelectedTool = Tools.Cursor;
                    Device.Vertex.Add(se);
                    Modified = true;
                    break;

                case Tools.PEGSpare:
                    PEGSpare pegspare = cn.CreatePegSpare(CanvasField);
                    Canvas.SetLeft(pegspare, p.X - (pegspare.Width / 2));
                    Canvas.SetTop(pegspare, p.Y - (pegspare.Height / 2));
                    pegspare.MouseLeftButtonDown += this.OnPEGSpareLeftButtonDownAsync;
                    CanvasField.Children.Add(pegspare);
                    SelectedTool = Tools.Cursor;
                    Device.pegspareelement = pegspare;
                    Device.Vertex.Add(pegspare);
                    Modified = true;
                    break;
                case Tools.User:
                    User user = cn.CreateUser(CanvasField);
                    Canvas.SetLeft(user, p.X - (user.Width / 2));
                    Canvas.SetTop(user, p.Y - (user.Height / 2));
                    user.MouseLeftButtonDown += this.OnUserMouseLeftButtonDown;
                    CanvasField.Children.Add(user);
                    SelectedTool = Tools.Cursor;
                    Device.Vertex.Add(user);
                    Modified = true;
                    break;

                case Tools.Connection:
                    break;
            }
        }
        /// <summary>
        /// Открыть внутренние элементы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Se_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedDevice.InputElements.Show();
            await ShowEnergizeInputElements();
        }
        /// <summary>
        /// Открыть внутренние элементы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Vzg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedDevice.InputElements.Show();
            await ShowEnergizeInputElements();
        }
        /// <summary>
        /// Показать внутренние элементы 
        /// </summary>
        private async Task ShowEnergizeInputElements()
        {
            if (EnergizeSheme.IsEnergy)
            {
                for (int i = 0; i < Device.Vertex.Count; i++)
                {
                    if (Device.Vertex[i] is VZG || Device.Vertex[i] is SE)
                    {
                        var NamePorts = Device.Vertex[i].port.BlockOpen.Where(n => n.Value == StatePort.Blocked).Select(k => k.Key).ToList();
                        foreach (var tb in Device.Vertex[i].InputElements.grid.Children)
                        {
                            if (tb is TextBlock)
                            {
                                for (int naras = 0; naras < NamePorts.Count; naras++)
                                {
                                    switch (source)
                                    {
                                        case Source.Peg:
                                            if ((tb as TextBlock).Name == NamePorts[naras])
                                                (tb as TextBlock).Background = Brushes.Red;
                                            break;
                                        case Source.Vzg:
                                            if ((tb as TextBlock).Name == NamePorts[naras])
                                                (tb as TextBlock).Background = Brushes.Yellow;
                                            break;
                                        case Source.PegSpare:
                                            if ((tb as TextBlock).Name == NamePorts[naras])
                                                (tb as TextBlock).Background = Brushes.Blue;
                                            break;
                                        default:
                                            break;
                                    }

                                }
                            }
                        }
                    }
                }
                foreach (var elemInPut in SelectedDevice.InputElements.grid.Children.OfType<UIElement>().ToList())
                {
                    if (elemInPut is TextBlock)
                    {
                        if (SelectedDevice.port.InOrOutPortDict[(elemInPut as TextBlock).Name] == Enums.InOrOutPort.InEnerg)
                            await SelectedDevice.InputElements.PaintLine(elemInPut as TextBlock, Enums.InOrOutPort.InEnerg, source);
                        else if (SelectedDevice.port.InOrOutPortDict[(elemInPut as TextBlock).Name] == Enums.InOrOutPort.Out)
                            await SelectedDevice.InputElements.PaintLine(elemInPut as TextBlock, Enums.InOrOutPort.Out, source);
                    }

                }

            }
        }

        #region Drag&Drop
        /// <summary>
        /// DragOver
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
            if (!EnergizeSheme.IsEnergy)
            {
                if (e.Data.GetDataPresent("Device"))
                {
                    Device obj = (Device)e.Data.GetData("Device");
                    Point p = e.GetPosition(this);
                    // Drag & drop of an object
                    Canvas.SetLeft(obj, p.X - (obj.Width / 2));
                    Canvas.SetTop(obj, p.Y - (obj.Height / 2));
                    obj.UpdateLocation();
                    Modified = true;
                }
            }
        }
        /// <summary>
        /// Удалить линию нажатием правой кнопкой мыши
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);

            if (Device.NewLine != null)
            {
                if (SelectedDevice.port.IsActive)
                {
                    SelectedDevice.port.Hide();
                }

                Device.NewLine.Remove(null, null);
                Device.NewLine = null;
            }
        }
        #endregion
        /// <summary>
        /// Выбрать VZG
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VZGButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedTool = Tools.VZG;
        }
        /// <summary>
        /// Выбрать PEG
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PEGButton_Click(object sender, RoutedEventArgs e)
        {
            if (Device.pegelement == null)
            {
                SelectedTool = Tools.Cursor;
                this.SelectedTool = Tools.PEG;
            }
            else
            {
                MessageBox.Show("Основной ПЭГ уже создан");
            }
        }
        /// <summary>
        /// Выбрать SE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SEButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedTool = Tools.Cursor;
            this.SelectedTool = Tools.SE;
        }
        /// <summary>
        /// Выбрать Курсор
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CursorButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedTool = Tools.Cursor;
        }
        /// <summary>
        /// Выбрать Line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_Click(object sender, RoutedEventArgs e)
        {
            SelectedTool = Tools.Cursor;
            this.SelectedTool = Tools.Connection;
        }
        /// <summary>
        /// Выбрать User
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void User_Click(object sender, RoutedEventArgs e)
        {
            SelectedTool = Tools.Cursor;
            this.SelectedTool = Tools.User;

        }
        /// <summary>
        /// Выбрать PEGSpare
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PEGSpare_Click(object sender, RoutedEventArgs e)
        {
            if (Device.pegspareelement == null)
            {
                SelectedTool = Tools.Cursor;
                this.SelectedTool = Tools.PEGSpare;
            }
            else
            {
                MessageBox.Show("Резервный ПЭГ уже создан");
            }
        }
        /// <summary>
        /// Выбрать элемент "PEG" и получить по нему данные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void OnPEGLeftButtonDownAsync(object sender, RoutedEventArgs e)
        {
            PEG peg = (PEG)sender;
            this.SelectedDevice = peg;
            NameDevice.Text = "Основной ПЭГ";
            LineExpender.Visibility = Visibility.Collapsed;
            DeviceExpander.Visibility = Visibility.Visible;
            await ListPorts();
            ListPort.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Выбрать элемент "PEGSpare" и получить по нему данные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void OnPEGSpareLeftButtonDownAsync(object sender, RoutedEventArgs e)
        {
            PEGSpare pegspare = (PEGSpare)sender;
            this.SelectedDevice = pegspare;
            NameDevice.Text = "Резервный ПЭГ";
            LineExpender.Visibility = Visibility.Collapsed;
            DeviceExpander.Visibility = Visibility.Visible;
            await ListPorts();
            ListPort.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Выбрать элемент "VZG" и получить по нему данные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void OnVZGLeftButtonDown(object sender, RoutedEventArgs e)
        {
            VZG vzg = (VZG)sender;
            this.SelectedDevice = vzg;
            NameDevice.Text = vzg.LabelName;
            LineExpender.Visibility = Visibility.Collapsed;
            DeviceExpander.Visibility = Visibility.Visible;
            await ListPorts();
            ListPort.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Выбрать элемент "SE" и получить по нему данные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void OnSELeftButtonDown(object sender, RoutedEventArgs e)
        {
            SE se = (SE)sender;
            this.SelectedDevice = se;
            NameDevice.Text = se.LabelName;
            LineExpender.Visibility = Visibility.Collapsed;
            DeviceExpander.Visibility = Visibility.Visible;
            await ListPorts();
            ListPort.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Выбрать Элемент "User" и получить по нему данные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void OnUserMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            User user = (User)sender;
            this.SelectedDevice = user;
            NameDevice.Text = "Пользователь";
            LineExpender.Visibility = Visibility.Collapsed;
            DeviceExpander.Visibility = Visibility.Visible;
            await ListPorts();
            ListPort.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Выбрать линию и получить по ней данные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnLineLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LineConnect line = (LineConnect)sender;
            this.SelectedLine = line;
            Device1.Text = SelectedLine.D1.LabelName;
            Device2.Text = SelectedLine.D2.LabelName;
            Port1.Text = SelectedLine.Port1.ToString();
            Port2.Text = SelectedLine.Port2.ToString();
            DeviceExpander.Visibility = Visibility.Collapsed;
            LineExpender.Visibility = Visibility.Visible;
        }
        #endregion
        /// <summary>
        /// Показать выбранные порты на устройстве
        /// </summary>
        /// <returns></returns>
        public async Task ListPorts()
        {
            foreach (var item in ListPort.Items.OfType<Label>().ToList())
            {
                item.Background = Brushes.White;
            }
            foreach (var item in SelectedDevice.port.BlockOpen)
            {
                if (item.Value == StatePort.Blocked)
                {
                    foreach (var listelem in ListPort.Items.OfType<Label>().ToList())
                    {
                        if (listelem.Name == item.Key)
                        {
                            listelem.Background = Brushes.Red;
                        }
                    }
                }
            }
            await Task.Delay(0);
        }
        /// <summary>
        /// Зарядить схему по выбранному элементу в ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Energize_Click(object sender, RoutedEventArgs e)
        {
            if (SourceBox.SelectedItem != null & Device.Vertex.Count != 0)
            {
                switch ((SourceBox.SelectedItem as ComboBoxItem).Content.ToString())
                {
                    case "ПЭГ":
                        IsEnabledF();
                        source = Source.Peg;
                        await EnergizeSheme.Energize(Source.Peg);
                        break;
                    case "ВЗГ":
                        IsEnabledF();
                        source = Source.Vzg;
                        await EnergizeSheme.Energize(Source.Vzg);
                        break;
                    case "ПЭГ рез.":
                        IsEnabledF();
                        source = Source.PegSpare;
                        await EnergizeSheme.Energize(Source.PegSpare);
                        break;
                    case "ГСЭ":
                        IsEnabledF();
                        source = Source.GSE;
                        await EnergizeSheme.Energize(Source.GSE);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите источник синхронизации и проверьте схему на правильность");
            }
        }
        /// <summary>
        /// Запретить использование элементов
        /// </summary>
        public void IsEnabledF()
        {
            EnergBut.IsEnabled = false;
            PEGButton.IsEnabled = false;
            VZGButton.IsEnabled = false;
            SEButton.IsEnabled = false;
            PEGSpare.IsEnabled = false;
            Connection.IsEnabled = false;
        }
        /// <summary>
        /// Разрешить использование элементов
        /// </summary>
        public void IsEnabledT()
        {
            EnergBut.IsEnabled = true;
            PEGButton.IsEnabled = true;
            VZGButton.IsEnabled = true;
            SEButton.IsEnabled = true;
            PEGSpare.IsEnabled = true;
            Connection.IsEnabled = true;
        }
        /// <summary>
        /// Очищение ресурсов и отключение схемы от энергии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ClearResource(object sender, RoutedEventArgs e)
        {
            IsEnabledT();
            EnergizeSheme.IsEnergy = false;
            for (int i = 0; i < Device.Vertex.Count; i++)
            {
                Device.Vertex[i].PowerSuuply = false;
                Device.Vertex[i].ISVisited = false;
                Device.Vertex[i].RectBorder = Brushes.Silver;

                for (int l = 0; l < Device.Vertex[i].Lines.Count; l++)
                {
                    Device.Vertex[i].Lines[l].IsArrow = false;
                    await Device.Vertex[i].Lines[l].ArrowToLine();
                    Device.Vertex[i].Lines[l].ColorConnection = Brushes.Black;
                }
            }
            for (int i = 0; i < Device.Vertex.Count; i++)
            {
                if (Device.Vertex[i] is VZG || Device.Vertex[i] is SE)
                {
                    var NamePorts = Device.Vertex[i].port.BlockOpen.Where(n => n.Value == StatePort.Blocked).Select(k => k.Key).ToList();
                    for (int j = 0; j < NamePorts.Count; j++)
                    {
                        Device.Vertex[i].port.InOrOutPortDict[NamePorts[j]] = Enums.InOrOutPort.Default;
                    }
                    if (Device.Vertex[i] is VZG || Device.Vertex[i] is SE)
                    {
                        foreach (var tb in Device.Vertex[i].InputElements.grid.Children.OfType<UIElement>().ToList())
                        {
                            if (tb is TextBlock)
                            {
                                (tb as TextBlock).Background = Brushes.White;
                            }
                            if (tb is ArrowInput)
                            {
                                Device.Vertex[i].InputElements.grid.Children.Remove(tb as ArrowInput);
                            }
                        }
                    }
                }
            }
            Modified = true;
        }
        /// <summary>
        /// Изменение города
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CityDevice_TextChanged(object sender, TextChangedEventArgs e)
        {
            SelectedDevice.city = CityDevice.Text;
            Modified = true;
        }
        /// <summary>
        /// Удаление выбранного элемента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceDelete_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedDevice.Remove(null, null);
            Modified = true;
        }
        /// <summary>
        /// Удаление выбранной линии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineDelete_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedLine.Remove(null, null);
            Device.NewLine = null;
            Modified = true;
        }
        /// <summary>
        /// Создать новую схему
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CreateNewScheme(object sender, RoutedEventArgs e)
        {
            if (await CloseScheme())
            {
                await ClearCanvas();
                Modified = false;
            }
        }
        /// <summary>
        /// Открытие схемы из файла 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OpenScheme(object sender, RoutedEventArgs e)
        {
            if (await CloseScheme())
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Файлы схемы" + " (*.scheme)|*.scheme|" + "Все файлы" + "|*";
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        await ClearCanvas();
                        Scheme.LoadScheme(this.CanvasField, dialog.FileName);
                        Modified = false;
                        this.FileName = dialog.FileName;
                    }
                    catch (Exception ex)
                    {
                        this.CreateNewScheme(null, null);
                        MessageBox.Show("Невозможно открыть" + ": " + ex.Message, "Ошибка");
                    }
                }

            }
        }
        /// <summary>
        /// Очищение канваса
        /// </summary>
        private async Task ClearCanvas()
        {
            this.ClearResource(null, null);
            CanvasField.Children.Clear();
            Scheme.NewList();
            await Task.Delay(0);
        }
        /// <summary>
        /// Сохранить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveScheme(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.FileName))
            {
                try
                {
                    Scheme.WriteSchemeToFile(this.FileName, this.CanvasField);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не правильное сохранение" + ex.Message);
                }
            }
            else
            {
                this.SaveSchemeAs(sender, e);
            }
            await Task.Delay(0);
        }
        /// <summary>
        /// Сохранить как
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSchemeAs(object sender, RoutedEventArgs e)
        {
            ClearResource(null, null);
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Файлы схем" + " (*.scheme)|*.scheme";
            dialog.RestoreDirectory = true;

            try
            {
                if (dialog.ShowDialog() == true)
                {
                    Scheme.WriteSchemeToFile(dialog.FileName, this.CanvasField);
                    this.FileName = dialog.FileName;
                    Modified = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не правильное сохранение" + ex.Message);
            }
        }
        /// <summary>
        /// Событие на закрытие
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void OnWindowClosing(object sender, CancelEventArgs e)
        {
            foreach (var item in (CanvasField.Children))
            {
                if (item is NodesWithPort & (item.GetType() == typeof(VZG) || item.GetType() == typeof(SE)))
                {
                    (item as NodesWithPort).port.Close();
                    (item as NodesWithPort).InputElements.Close();
                }
            }
            if (Modified)
            {
                if (!(await CloseScheme()))
                {
                    e.Cancel = true;
                }
            }
        }
        /// <summary>
        /// Окно подтверждение закрытия
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CloseScheme()
        {
            if (Modified)
            {
                switch (MessageBox.Show("Закрыть приложение", "Схема была изменена", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel))
                {
                    case MessageBoxResult.Cancel:
                        return false;

                    case MessageBoxResult.Yes:
                        this.SaveScheme(null, null);
                        return false;

                    case MessageBoxResult.No:
                        break;
                }

            }
            await Task.Delay(0);
            return true;
        }
        /// <summary>
        /// Убивает процесс в пуле процессов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseMainWindow(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
        /// <summary>
        /// Информация о программе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReferenceOpen(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Авторы:\n" +
                "к-т Овешников Ю.М. 394 учебная группа;\n" +
                "Безручко В.М.;\n" +
                "Тезин А.B.");
        }
    }
}
