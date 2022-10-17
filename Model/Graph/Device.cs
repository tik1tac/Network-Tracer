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

        public static List<LineConnect> ListLine = new List<LineConnect>();

        public static ArrayList ArrayNodes = new ArrayList();

        public static Dictionary<List<LineConnect>, ArrayList> graph = new Dictionary<List<LineConnect>, ArrayList>();

        public virtual string LabelName { get; set; }

        public virtual string city { get; set; }

        public abstract bool AddLine( LineConnect line );

        public virtual int Number { get; set; }

        public static MainWindow Window { get; set; }

        public static LineConnect NewLine { get; set; }

        public abstract void UpdateLocation();

        public Canvas canvas { get; set; }

        public abstract int GetPort( LineConnect line );

        public Device( Canvas canvas ) => this.canvas = canvas;

        public abstract bool RemoveLine( bool deep, LineConnect line = null );

        public abstract void Remove( object sender, RoutedEventArgs e );

        public static Dictionary<List<LineConnect>, ArrayList> GetGraph()
        {
            graph.Add(ListLine, ArrayNodes);
            return graph;
        }

        protected override void OnMouseMove( MouseEventArgs e )
        {
            base.OnMouseMove(e);

            if ( e.LeftButton == MouseButtonState.Pressed )
            {
                Point p = e.GetPosition(Window);
                DataObject data = new DataObject();
                data.SetData("Device", this);

                // Creation of a new wire
                if ( Window.SelectedTool == Tools.Connection )
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
                        DragDrop.DoDragDrop(this, data, DragDropEffects.Link);
                    }
                    else
                    {
                        Device.NewLine.Remove(null, null);
                        Device.NewLine = null;
                    }
                }
                else
                {
                    // Otherwise move the object
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                }
                e.Handled = true;
            }
        }

        protected override void OnDrop( DragEventArgs e )
        {
            base.OnDrop(e);

            if ( Device.NewLine != null && Device.NewLine.D1 != this )
            {
                if ( this.AddLine(Device.NewLine) )
                {
                    Device.NewLine.X2 = Canvas.GetLeft(this) + ( this.Width / 2 );
                    Device.NewLine.Y2 = Canvas.GetTop(this) + ( this.Height / 2 );
                    Device.NewLine.D2 = this;
                    Device.NewLine.MouseLeftButtonDown += Window.OnLineLeftButtonDown;
                    Device.NewLine.D2.UpdateLocation();
                }
                else
                {
                    Device.NewLine.Remove(null, null);
                }

                Device.NewLine = null;
            }
        }
        public Device CalculateGraph()
        {

            return null;
        }
    }
}
