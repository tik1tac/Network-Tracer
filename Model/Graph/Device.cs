using Network_Tracer.View;

using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Network_Tracer.Model.Graph
{
    [Serializable]
    public abstract class Device : UserControl
    {
        public Device(Canvas canvas) => this.canvas = canvas;
        public static List<Device> Vertex = new List<Device>();
        public virtual List<LineConnect> Lines { get; set; }
        public virtual List<Device> _neighbours { get; set; }
        public virtual Brush RectBorder { get; set; }
        public virtual Port port { get; set; }
        public abstract InputElements InputElements { get; set; }
        public virtual string LabelName { get; set; }
        public virtual string city { get; set; }
        public static PEG pegcount { get; set; }
        public static PEGSpare pegsparecount { get; set; }
        public Canvas canvas { get; set; }
        public virtual int Number { get; set; }
        public virtual bool PowerSuuply { get; set; }
        public static List<LineConnect> _lines = new List<LineConnect>();
        public virtual bool ISVisited { get; set; }
        public static MainWindow Window { get; set; }
        public static LineConnect NewLine { get; set; }

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

        public static int _count = 1;

        protected async override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            Point p = e.GetPosition(Window);
            try
            {
                if (Window.SelectedTool == Tools.Connection)
                {
                    if (_count == 1)
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
                        }
                        else
                        {
                            Device.NewLine.Remove(null, null);
                            MessageBox.Show("Нет свободных портов");
                            Device.NewLine = null;
                            _count = 1;
                        }
                    }
                    if (_count == 2)
                    {
                        if (Device.NewLine != null && Device.NewLine.D1 != this)
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
                                if (Device.NewLine.D2 is VZG || Device.NewLine.D2 is SE)
                                {
                                    SetPort();
                                    Device.NewLine.Port2 = port.SelectedPorts;
                                }
                                _count = 1;
                            }
                            else
                            {
                                Device.NewLine.Remove(null, null);
                            }
                            Device.NewLine = null;
                        }
                    }
                    if (Device.NewLine != null)
                    {
                        _count++;
                    }
                    else
                    {
                        _count = 1;
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
