using Network_Tracer.View;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Network_Tracer.Model.Graph
{
    [Serializable]
    public abstract class Device : UserControl
    {
        public Device(Canvas canvas) => this.canvas = canvas;
        public static bool _connected { get; set; } = true;
        /// <summary>
        /// Коллекция вершин
        /// </summary>
        public static List<Device> Vertex = new List<Device>();
        /// <summary>
        /// Коллекция линий для одного устройства
        /// </summary>
        public virtual List<LineConnect> Lines { get; set; }
        /// <summary>
        /// Коллекция соседей для одного устройства
        /// </summary>
        public virtual List<Device> _neighbours { get; set; }
        /// <summary>
        /// BackGround для устройства
        /// </summary>
        public virtual Brush RectBorder { get; set; }
        /// <summary>
        /// Экземпляр портов для одного устройства
        /// </summary>
        public virtual Port port { get; set; }
        /// <summary>
        /// Ээкземпляр внутренних элементов для одного устройства
        /// </summary>
        public abstract InputElements InputElements { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public virtual string LabelName { get; set; }
        /// <summary>
        /// Город
        /// </summary>
        public virtual string city { get; set; }
        /// <summary>
        /// экземпляр PEG
        /// </summary>
        public static PEG pegelement { get; set; }
        /// <summary>
        /// Экземпляр PEGSpare
        /// </summary>
        public static PEGSpare pegspareelement { get; set; }
        public Canvas canvas { get; set; }
        /// <summary>
        /// Номер
        /// </summary>
        public virtual int Number { get; set; }
        public virtual bool PowerSuuply { get; set; }
        /// <summary>
        /// Коллекция линий
        /// </summary>
        public static List<LineConnect> _lines = new List<LineConnect>();
        public virtual bool ISVisited { get; set; }
        public static MainWindow Window { get; set; }
        /// <summary>
        /// Новая линия для одного устройства
        /// </summary>
        public static LineConnect NewLine { get; set; }
        /// <summary>
        /// Словарь для устройств с портами
        /// </summary>
        public virtual Dictionary<LineConnect, NamePorts> NamePorts { get; set; }

        /// <summary>
        /// Установить порт на устройство SE или VZG
        /// </summary>
        public abstract void SetPort();
        /// <summary>
        /// Обновить координаты
        /// </summary>
        public abstract void UpdateLocation();
        /// <summary>
        /// Добавить линиию к устройству
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public abstract Task<bool> AddLine(LineConnect line);
        /// <summary>
        /// Добавить соседей
        /// </summary>
        /// <param name="D"></param>
        public abstract void AddNEighbours(Device D);
        /// <summary>
        /// Удалить порт
        /// </summary>
        /// <param name="i"></param>
        public abstract void DeletePort(int i);
        /// <summary>
        /// Удалить линию
        /// </summary>
        /// <param name="deep"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public abstract Task<bool> RemoveLine(bool deep, LineConnect line = null);
        /// <summary>
        /// Удалить элемент с канваса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void Remove(object sender, RoutedEventArgs e);
        /// <summary>
        /// Создание линии
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            Point p = e.GetPosition(Window);
            try
            {
                if (Window.SelectedTool == Tools.Connection)
                {
                    if (!EnergizeSheme.IsEnergy)
                    {
                        if (_connected == true)
                        {
                            Device.NewLine = new LineConnect(canvas)
                            {
                                X1 = Canvas.GetLeft(this) + (this.Width / 2),
                                Y1 = Canvas.GetTop(this) + (Height / 2),
                                X2 = p.X,
                                Y2 = p.Y,
                                D1 = this
                            };
                            _lines.Add(Device.NewLine);
                            Device.NewLine.NameLine = NewLine.D1.LabelName + "-Линия-";
                            if (await this.AddLine(Device.NewLine))
                            {
                                if (Device.NewLine.D1 is VZG || Device.NewLine.D1 is SE)
                                {
                                    SetPort();
                                    Device.NewLine.Port1 = port.SelectedPorts;
                                }
                                Canvas.SetZIndex(Device.NewLine, -1);
                                canvas.Children.Add(Device.NewLine);
                                Window.Modified = true;
                            }
                            else
                            {
                                Device.NewLine.Remove(null, null);
                                MessageBox.Show("Нет свободных портов");
                                Device.NewLine = null;
                                _connected = true;
                            }
                        }
                        if (_connected == false)
                        {
                            if (Device.NewLine != null && Device.NewLine.D1 != this)
                            {
                                if (!Device.NewLine.D1._neighbours.Contains(this))
                                {
                                    if (await this.AddLine(Device.NewLine))
                                    {
                                        Device.NewLine.X2 = Canvas.GetLeft(this) + (this.Width / 2);
                                        Device.NewLine.Y2 = Canvas.GetTop(this) + (this.Height / 2);
                                        Device.NewLine.D2 = this;
                                        Device.NewLine.NameLine += Device.NewLine.D2.LabelName;
                                        Device.NewLine.SetCost(Device.NewLine.D1);
                                        Device.NewLine.D1.AddNEighbours(Device.NewLine.D2);
                                        Device.NewLine.D2.AddNEighbours(Device.NewLine.D1);
                                        Device.NewLine.MouseLeftButtonDown += Window.OnLineLeftButtonDown;
                                        Device.NewLine.D2.UpdateLocation();
                                        Window.Modified = true;
                                        if (Device.NewLine.D2 is VZG || Device.NewLine.D2 is SE)
                                        {
                                            SetPort();
                                            Device.NewLine.Port2 = port.SelectedPorts;
                                        }
                                        _connected = true;
                                    }
                                    else
                                    {
                                        Device.NewLine.Remove(null, null);
                                        MessageBox.Show("Вышла ошибочка");
                                    }
                                    Device.NewLine = null;
                                }
                                else
                                {
                                    await Device.NewLine.D1.RemoveLine(true, Device.NewLine);
                                    Device.NewLine.Remove(null, null);
                                    Device.NewLine = null;
                                    MessageBox.Show("Эти элементы уже соединены");
                                }
                            }
                        }
                        if (Device.NewLine != null)
                        {
                            _connected = false;
                        }
                        else
                        {
                            _connected = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выключите питание");
                    }
                }
            }
            catch (System.Exception)
            {
                Window.SelectedTool = Tools.Cursor;
            }

        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton == MouseButtonState.Pressed & Window.SelectedTool != Tools.Connection)
            {
                Point p = e.GetPosition(Window);
                DataObject data = new DataObject();
                data.SetData("Device", this);
                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                e.Handled = true;
            }
        }

    }
}
