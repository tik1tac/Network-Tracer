using Network_Tracer.View;

using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Network_Tracer.Model.Graph
{
    public abstract class Device : UserControl
    {

        public static ArrayList D2 = new ArrayList();

        public virtual List<LineConnect> Lines { get; set; }

        public virtual string LabelName { get; set; }

        public virtual string city { get; set; }

        public static PEG pegcount { get; set; }

        public static PEGSpare pegsparecount { get; set; }

        public abstract bool AddLine( LineConnect line );

        public virtual int Number { get; set; }

        public static MainWindow Window { get; set; }

        public static LineConnect NewLine { get; set; }

        public abstract void UpdateLocation();

        public Canvas canvas { get; set; }

        //public abstract int GetPort( LineConnect line );

        //public abstract void SetPort( Device D2 );

        public Device( Canvas canvas ) => this.canvas = canvas;

        public abstract bool RemoveLine( bool deep, LineConnect line = null );

        public abstract void Remove( object sender, RoutedEventArgs e );


        static int count = 1;
        protected override void OnMouseLeftButtonDown( MouseButtonEventArgs e )
        {
            base.OnMouseLeftButtonDown(e);
            Point p = e.GetPosition(Window);
            try
            {
                if ( Window.SelectedTool == Tools.Connection )
                {
                    if ( count == 1 )
                    {
                        Device.NewLine = new LineConnect(canvas)
                        {
                            X1 = Canvas.GetLeft(this) + ( this.Width / 2 ),
                            Y1 = Canvas.GetTop(this) + ( Height / 2 ),
                            X2 = p.X,
                            Y2 = p.Y,
                            D1 = this
                        };
                        if ( this.AddLine(Device.NewLine) )
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
                    if ( count == 2 )
                    {
                        if ( Device.NewLine != null && Device.NewLine.D1 != this )
                        {
                            if ( this.AddLine(Device.NewLine) )
                            {
                                Device.NewLine.X2 = Canvas.GetLeft(this) + ( this.Width / 2 );
                                Device.NewLine.Y2 = Canvas.GetTop(this) + ( this.Height / 2 );
                                Device.NewLine.D2 = this;
                                if ( !D2.Contains(Device.NewLine.D2) )
                                {
                                    D2.Add(Device.NewLine.D2);
                                }
                                Device.NewLine.SetCost(Device.NewLine.D1);
                                Device.NewLine.MouseLeftButtonDown += Window.OnLineLeftButtonDown;
                                Device.NewLine.D2.UpdateLocation();
                                count = 1;
                            }
                            else
                            {
                                Device.NewLine.Remove(null, null);
                            }
                            //SetPort(NewLine.D2);
                            Device.NewLine = null;
                        }
                    }
                    if ( Device.NewLine != null )
                    {
                        count++;
                    }
                }
            }
            catch ( System.Exception )
            {
                Window.SelectedTool = Tools.Cursor;
            }

        }
        protected override void OnMouseMove( MouseEventArgs e )
        {
            base.OnMouseMove(e);

            if ( e.LeftButton == MouseButtonState.Pressed & Window.SelectedTool != Tools.Connection )
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
