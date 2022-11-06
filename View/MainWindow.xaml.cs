using Network_Tracer.Model;
using Network_Tracer.Model.Graph;
using Network_Tracer.Model.Graph.AbstractGraph;
using Network_Tracer.View;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

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
            Device.Window = this;
            SelectedTool = Tools.Cursor;
        }

        #region Элементы на канвасе
        LineConnect SelectedLine;
        Device SelectedDevice;
        PEG pegcount = null;
        PEGSpare pegsparecount = null;
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
                return Tools.Cursor;
            }
            set
            {
                switch (value)
                {
                    case Tools.VZG:
                        this.Connection.Opacity = this.PEGButton.Opacity = this.SEButton.Opacity = this.CursorButton.Opacity = this.PEGSpare.Opacity = 0.4;
                        this.VZGButton.Opacity = 1.0;
                        break;

                    case Tools.Connection:
                        this.SEButton.Opacity = this.VZGButton.Opacity = this.PEGButton.Opacity = this.CursorButton.Opacity = this.PEGSpare.Opacity = 0.4;
                        this.Connection.Opacity = 1.0;
                        break;

                    case Tools.PEG:
                        this.Connection.Opacity = this.VZGButton.Opacity = this.SEButton.Opacity = this.CursorButton.Opacity = this.PEGSpare.Opacity = 0.4;
                        this.PEGButton.Opacity = 1.0;
                        break;

                    case Tools.SE:
                        this.Connection.Opacity = this.VZGButton.Opacity = this.PEGButton.Opacity = this.CursorButton.Opacity = this.PEGSpare.Opacity = 0.4;
                        this.SEButton.Opacity = 1.0;
                        break;
                    case Tools.PEGSpare:
                        this.Connection.Opacity = this.VZGButton.Opacity = this.PEGButton.Opacity = this.CursorButton.Opacity = SEButton.Opacity = 0.4;
                        this.PEGSpare.Opacity = 1.0;
                        break;

                    default:
                        this.Connection.Opacity = this.VZGButton.Opacity = this.PEGButton.Opacity = this.SEButton.Opacity = this.PEGSpare.Opacity = 0.4;
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
                    peg.MouseLeftButtonDown += this.OnPEGLeftButtonDown;
                    CanvasField.Children.Add(peg);
                    SelectedTool = Tools.Cursor;
                    Device.pegcount = peg;
                    Device.Vertex.Add(peg);
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
                    break;

                case Tools.SE:
                    SE se = new SE(CanvasField);
                    Canvas.SetLeft(se, p.X - (se.Width / 2));
                    Canvas.SetTop(se, p.Y - (se.Height / 2));
                    se.MouseLeftButtonDown += this.OnSELeftButtonDown;
                    se.MouseDoubleClick += Se_MouseDoubleClick;
                    CanvasField.Children.Add(se);
                    SelectedTool = Tools.Cursor;
                    Device.Vertex.Add(se);
                    break;

                case Tools.PEGSpare:
                    PEGSpare pegspare = cn.CreatePegSpare(CanvasField);
                    Canvas.SetLeft(pegspare, p.X - (pegspare.Width / 2));
                    Canvas.SetTop(pegspare, p.Y - (pegspare.Height / 2));
                    pegspare.MouseLeftButtonDown += this.OnPEGSpareLeftButtonDown;
                    CanvasField.Children.Add(pegspare);
                    SelectedTool = Tools.Cursor;
                    Device.pegsparecount = pegspare;
                    Device.Vertex.Add(pegspare);
                    break;

                case Tools.Connection:
                    break;
            }
        }

        private void Se_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedDevice.InputElements.Show();
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
                                    if ((tb as TextBlock).Name == NamePorts[naras])
                                        (tb as TextBlock).Background = Brushes.Red;
                                }
                            }
                        }
                    }
                }
            }

            //SelectedDevice.InputElements.XGse = SelectedDevice.InputElements.GSE.PointToScreen(new Point(SelectedDevice.InputElements.GSE.ActualWidth, SelectedDevice.InputElements.GSE.ActualHeight)).X;
            //SelectedDevice.InputElements.YGse = SelectedDevice.InputElements.GSE.PointToScreen(new Point(SelectedDevice.InputElements.GSE.ActualWidth, SelectedDevice.InputElements.GSE.ActualHeight)).Y;
            //foreach (var item in SelectedDevice.InputElements.grid.Children)
            //{
            //    if (item is TextBlock)
            //    {
            //        if ((item as TextBlock).Name == SelectedDevice.port.PortLine.Where(n => n.Key == ports[i]).First().Value)
            //        {
            //            (item as TextBlock).Background = Brushes.Red;
            //            XBut = (item as TextBlock).PointToScreen(new Point((item as TextBlock).ActualWidth, (item as TextBlock).ActualHeight)).X;
            //            YBut = (item as TextBlock).PointToScreen(new Point((item as TextBlock).ActualWidth, (item as TextBlock).ActualHeight)).Y;
            //            InputElements.grid.Children.Add(new ArrowInput(XBut, XGse, YBut, YGse));
            //        }
            //    }
            //}
        }

        private void Vzg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedDevice.InputElements.Show();
        }

        #region Drag&Drop
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
                }
            }
        }
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
        private void VZGButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedTool = Tools.VZG;
        }

        private void PEGButton_Click(object sender, RoutedEventArgs e)
        {
            if (Device.pegcount == null)
            {
                SelectedTool = Tools.Cursor;
                this.SelectedTool = Tools.PEG;
            }
            else
            {
                MessageBox.Show("Основной ПЭГ уже создан");
            }
        }

        private void SEButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedTool = Tools.Cursor;
            this.SelectedTool = Tools.SE;
        }

        private void CursorButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedTool = Tools.Cursor;
        }

        private void Connection_Click(object sender, RoutedEventArgs e)
        {
            SelectedTool = Tools.Cursor;
            this.SelectedTool = Tools.Connection;
        }

        private void PEGSpare_Click(object sender, RoutedEventArgs e)
        {
            if (Device.pegsparecount == null)
            {
                SelectedTool = Tools.Cursor;
                this.SelectedTool = Tools.PEGSpare;
            }
            else
            {
                MessageBox.Show("Резервный ПЭГ уже создан");
            }
        }

        public void OnPEGLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PEG peg = (PEG)sender;
            this.SelectedDevice = peg;
            NameDevice.Text = "Основной ПЭГ";
            LineExpender.Visibility = Visibility.Collapsed;
            DeviceExpander.Visibility = Visibility.Visible;
        }
        public void OnPEGSpareLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PEGSpare pegspare = (PEGSpare)sender;
            this.SelectedDevice = pegspare;
            NameDevice.Text = "Резервный ПЭГ";
            LineExpender.Visibility = Visibility.Collapsed;
            DeviceExpander.Visibility = Visibility.Visible;
        }
        public void OnVZGLeftButtonDown(object sender, RoutedEventArgs e)
        {
            VZG vzg = (VZG)sender;
            this.SelectedDevice = vzg;
            NameDevice.Text = vzg.LabelName;
            LineExpender.Visibility = Visibility.Collapsed;
            DeviceExpander.Visibility = Visibility.Visible;
        }
        public void OnSELeftButtonDown(object sender, RoutedEventArgs e)
        {
            SE se = (SE)sender;
            this.SelectedDevice = se;
            NameDevice.Text = se.LabelName;
            LineExpender.Visibility = Visibility.Collapsed;
            DeviceExpander.Visibility = Visibility.Visible;
        }
        public void OnLineLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LineConnect line = (LineConnect)sender;
            this.SelectedLine = line;
            Device1.Text = SelectedLine.D1.LabelName;
            Device2.Text = SelectedLine.D2.LabelName;
            DeviceExpander.Visibility = Visibility.Collapsed;
            LineExpender.Visibility = Visibility.Visible;
        }
        #endregion
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            foreach (var item in (CanvasField.Children))
            {
                if (item is Device)
                {
                    (item as Device).port.Close();
                }
            }
            if (!this.CloseScheme())
            {
                e.Cancel = true;

            }
        }
        private bool CloseScheme()
        {
            switch (MessageBox.Show("Схема была изменена", "Схема была изменена", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel))
            {
                case MessageBoxResult.Cancel:
                    return false;

                case MessageBoxResult.Yes:
                    //this.SaveScheme(null, null);
                    return false;

                case MessageBoxResult.No:
                    break;
            }

            return true;
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            if (SourceBox.SelectedItem != null || Device.Vertex.Count != 0)
            {
                switch ((SourceBox.SelectedItem as ComboBoxItem).Content.ToString())
                {
                    case "ПЭГ":
                        IsEnabledF();
                        EnergizeSheme.Energize(Source.Peg);
                        break;
                    case "ВЗГ":
                        IsEnabledF();
                        EnergizeSheme.Energize(Source.Vzg);
                        break;
                    case "ПЭГ рез.":
                        IsEnabledF();
                        EnergizeSheme.Energize(Source.PegSpare);
                        break;
                    case "ГСЭ":
                        IsEnabledF();
                        EnergizeSheme.Energize(Source.GSE);
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
        public void IsEnabledF()
        {
            EnergBut.IsEnabled = false;
            PEGButton.IsEnabled = false;
            VZGButton.IsEnabled = false;
            SEButton.IsEnabled = false;
            PEGSpare.IsEnabled = false;
            Connection.IsEnabled = false;
        }
        public void IsEnabledT()
        {
            EnergBut.IsEnabled = true;
            PEGButton.IsEnabled = true;
            VZGButton.IsEnabled = true;
            SEButton.IsEnabled = true;
            PEGSpare.IsEnabled = true;
            Connection.IsEnabled = true;
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
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
                    Device.Vertex[i].Lines[l].ArrowToLine();
                    Device.Vertex[i].Lines[l].ColorConnection = Brushes.Black;
                }
            }
        }

        private void CityDevice_TextChanged(object sender, TextChangedEventArgs e)
        {
            SelectedDevice.city = CityDevice.Text;
        }

        private void DeviceDelete_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedDevice.Remove(null, null);
        }

        private void LineDelete_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedLine.Remove(null, null);
            Device.NewLine = null;
        }

        private void CreateNewScheme_Click(object sender, RoutedEventArgs e)
        {
            Clear_Click(null, null);
            CanvasField.Children.Clear();
            Scheme.NewList();
        }

        private void OpenScheme_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveScheme_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
