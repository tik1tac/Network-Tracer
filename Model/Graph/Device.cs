using Network_Tracer.View;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public static List<Device> Vertex = new List<Device>();

        public virtual List<LineConnect> Lines { get; set; }

        public virtual List<Device> _neighbours { get; set; }

        public virtual Brush RectBorder { get; set; }
        public virtual Port port { get; set; }

        public abstract InputElements InputElements { get; set; }

        public static List<LineConnect> _lines = new List<LineConnect>();

        public virtual string LabelName { get; set; }

        public virtual string city { get; set; }

        public static PEG pegcount { get; set; }

        public static PEGSpare pegsparecount { get; set; }

        public Canvas canvas { get; set; }

        public virtual int Number { get; set; }

        public virtual bool PowerSuuply { get; set; }

        public virtual bool ISVisited { get; set; }

        public static MainWindow Window { get; set; }

        public static LineConnect NewLine { get; set; }

        public virtual NamePorts NamePorts { get; set; }

        public abstract void SetPort();

        public abstract void UpdateLocation();

        public abstract bool AddLine(LineConnect line);

        public abstract void AddNEighbours(Device D);

        public abstract void DeletePort(int i);

        public abstract bool RemoveLine(bool deep, LineConnect line = null);

        public abstract void Remove(object sender, RoutedEventArgs e);

        public static int _count = 1;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
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
                        if (this.AddLine(Device.NewLine))
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
                            if (this.AddLine(Device.NewLine))
                            {
                                Device.NewLine.X2 = Canvas.GetLeft(this) + (this.Width / 2);
                                Device.NewLine.Y2 = Canvas.GetTop(this) + (this.Height / 2);
                                Device.NewLine.D2 = this;
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
