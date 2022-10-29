﻿using Network_Tracer.View;

using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Network_Tracer.Model.Graph
{
    public abstract class Device : UserControl
    {
        public Device(Canvas canvas) => this.canvas = canvas;

        public static List<Device> Vertex = new List<Device>();

        public virtual List<LineConnect> Lines { get; set; }

        public virtual List<Device> _neighbours { get; set; }

        public virtual string LabelName { get; set; }

        //public abstract string PowerEnergized { get; set; }

        public virtual string city { get; set; }

        public static PEG pegcount { get; set; }

        public static PEGSpare pegsparecount { get; set; }

        public Canvas canvas { get; set; }

        public virtual int Number { get; set; }

        public virtual bool PowerSuuply { get; set; }

        public static MainWindow Window { get; set; }

        public static LineConnect NewLine { get; set; }

        public abstract void UpdateLocation();
        public abstract bool AddLine(LineConnect line);

        public abstract void AddNEighbours(Device D);

        //public abstract int GetPort( LineConnect line );

        //public abstract void SetPort( Device D2 );


        public abstract bool RemoveLine(bool deep, LineConnect line = null);

        public abstract void Remove(object sender, RoutedEventArgs e);

        public static int _countdevicesoncanvas { get; set; }

        static int _count = 1;
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
                        if (!Vertex.Contains(Device.NewLine.D1))
                        {
                            Vertex.Add(Device.NewLine.D1);
                        }
                        if (this.AddLine(Device.NewLine))
                        {
                            Canvas.SetZIndex(Device.NewLine, -1);
                            canvas.Children.Add(Device.NewLine);
                        }
                        else
                        {
                            Device.NewLine.Remove(null, null);
                            MessageBox.Show("Нет свободных портов");
                            Device.NewLine = null;
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
                                if (!Vertex.Contains(Device.NewLine.D2))
                                {
                                    Vertex.Add(Device.NewLine.D2);
                                }
                                Device.NewLine.D1.AddNEighbours(Device.NewLine.D2);
                                Device.NewLine.D2.AddNEighbours(Device.NewLine.D1);
                                Device.NewLine.SetCost(Device.NewLine.D1);
                                Device.NewLine.MouseLeftButtonDown += Window.OnLineLeftButtonDown;
                                Device.NewLine.D2.UpdateLocation();
                                _count = 1;
                            }
                            else
                            {
                                Device.NewLine.Remove(null, null);
                            }
                            //SetPort(NewLine.D2);
                            Device.NewLine = null;
                        }
                    }
                    if (Device.NewLine != null)
                    {
                        _count++;
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
