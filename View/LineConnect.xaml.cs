using Network_Tracer.Model.Graph;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Network_Tracer.View
{
    /// <summary>
    /// Логика взаимодействия для LineConnect.xaml
    /// </summary>
    public partial class LineConnect : UserControl
    {
        public LineConnect( Canvas canvas )
        {

            InitializeComponent();
            this.canvas = canvas;
        }
        public double X1
        {
            get => Line.X1;
            set
            {
                Line.X1 = value;
                Highlight.X1 = value;
            }
        }
        public double Y1
        {
            get => Line.Y1;
            set
            {
                Line.Y1 = value;
                Highlight.Y1 = value;
            }
        }
        public double X2
        {
            get => Line.X2;
            set
            {
                Highlight.X2 = value; 
                Line.X2 = value;
            }
        }
        public double Y2
        {
            get => Line.Y2;
            set
            {
                Highlight.Y2 = value;
                Line.Y2 = value;
            }
        }

        public Brush ColorConnection { get => Line.Stroke; set => Line.Stroke = value; }

        public int Cost { get => Cost; set => Cost = value; }

        public Device D1 { get; set; }
        public Device D2 { get; set; }

        public NamePorts Port1 { get => Port1; set => Port1 = value; }
        public NamePorts Port2 { get => Port2; set => Port2 = value; }

        Canvas canvas { get; set; }

        public void UpdateLocation( Device device, double x, double y )
        {
            if ( this.D1 == device )
            {
                this.X1 = (int)x;
                this.Y1 = (int)y;
            }
            else if ( this.D2 == device )
            {
                this.X2 = (int)x;
                this.Y2 = (int)y;
            }

            Canvas.SetTop(this.WireBorder, ( this.Y1 + this.Y2 - this.WireBorder.Height ) / 2);
            Canvas.SetLeft(this.WireBorder, ( this.X1 + this.X2 - this.WireBorder.Width ) / 2);
        }

        public void Remove( object sender = null, RoutedEventArgs e = null )
        {

            if ( this.D1 != null && this.D1 != sender )
            {
                this.D1.RemoveLine(false, this);
            }

            if ( this.D2 != null && this.D2 != sender )
            {
                this.D2.RemoveLine(false, this);
            }

            if ( this.canvas != null )
            {
                this.canvas.Children.Remove(this);
            }
        }
        public LineConnect ArrowLine( LineConnect lineConnect )
        {
            // координаты центра отрезка
            double X3 = ( this.X1 + this.X2 ) / 2;
            double Y3 = ( this.Y1 + this.Y2 ) / 2;

            // длина отрезка
            double d = Math.Sqrt(Math.Pow(this.X2 - this.X1, 2) + Math.Pow(this.Y2 - this.Y1, 2));

            // координаты вектора
            double X = this.X2 - this.X1;
            double Y = this.Y2 - this.Y1;

            // координаты точки, удалённой от центра к началу отрезка на 10px
            double X4 = X3 - ( X / d ) * 10;
            double Y4 = Y3 - ( Y / d ) * 10;

            // из уравнения прямой { (x - x1)/(x1 - x2) = (y - y1)/(y1 - y2) } получаем вектор перпендикуляра
            // (x - x1)/(x1 - x2) = (y - y1)/(y1 - y2) =>
            // (x - x1)*(y1 - y2) = (y - y1)*(x1 - x2) =>
            // (x - x1)*(y1 - y2) - (y - y1)*(x1 - x2) = 0 =>
            // полученные множители x и y => координаты вектора перпендикуляра
            double Xp = this.Y2 - this.Y1;
            double Yp = this.X1 - this.X2;

            // координаты перпендикуляров, удалённой от точки X4;Y4 на 5px в разные стороны
            double X5 = X4 + ( Xp / d ) * 5;
            double Y5 = Y4 + ( Yp / d ) * 5;
            double X6 = X4 - ( Xp / d ) * 5;
            double Y6 = Y4 - ( Yp / d ) * 5;

            ArrowRight.X1 = X3;
            ArrowRight.Y1 = Y3;
            ArrowLeft.X1 = X3;
            ArrowLeft.Y1 = Y3;

            ArrowRight.X2 = X6;
            ArrowRight.Y2 = Y6;
            ArrowLeft.X2 = X5;
            ArrowLeft.Y2 = Y5;

            //GeometryGroup geometryGroup = new GeometryGroup();

            //LineGeometry lineGeometry = new LineGeometry(new Point(this.X1, this.Y1), new Point(this.X2, this.Y2));
            //LineGeometry arrowPart1Geometry = new LineGeometry(new Point(X3, Y3), new Point(X5, Y5));
            //LineGeometry arrowPart2Geometry = new LineGeometry(new Point(X3, Y3), new Point(X6, Y6));

            //geometryGroup.Children.Add(lineGeometry);
            //geometryGroup.Children.Add(arrowPart1Geometry);
            //geometryGroup.Children.Add(arrowPart2Geometry);

            return this;
        }
    }
}
