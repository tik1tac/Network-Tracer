using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace Network_Tracer.View
{
    internal class ArrowInput : Shape
    {
        public ArrowInput(double X1, double X2, double Y1, double Y2)
        {
            this.X1 = X1;
            this.X2 = X2;
            this.Y1 = Y1;
            this.Y2 = Y2;
        }
        public double X1 { get; set; }


        public double Y1 { get; set; }

        public double X2 { get; set; }


        public double Y2 { get; set; }


        protected override Geometry DefiningGeometry
        {
            get
            {
                // координаты центра отрезка
                double X3 = (this.X1 + this.X2) / 2;
                double Y3 = (this.Y1 + this.Y2) / 2;

                // длина отрезка
                double d = Math.Sqrt(Math.Pow(this.X2 - this.X1, 2) + Math.Pow(this.Y2 - this.Y1, 2));

                // координаты вектора
                double X = this.X2 - this.X1;
                double Y = this.Y2 - this.Y1;

                // координаты точки, удалённой от центра к началу отрезка на 10px
                double X4 = X3 - (X / d) * 10;
                double Y4 = Y3 - (Y / d) * 10;

                // из уравнения прямой { (x - x1)/(x1 - x2) = (y - y1)/(y1 - y2) } получаем вектор перпендикуляра
                // (x - x1)/(x1 - x2) = (y - y1)/(y1 - y2) =>
                // (x - x1)*(y1 - y2) = (y - y1)*(x1 - x2) =>
                // (x - x1)*(y1 - y2) - (y - y1)*(x1 - x2) = 0 =>
                // полученные множители x и y => координаты вектора перпендикуляра
                double Xp = this.Y2 - this.Y1;
                double Yp = this.X1 - this.X2;

                // координаты перпендикуляров, удалённой от точки X4;Y4 на 5px в разные стороны
                double X5 = X4 + (Xp / d) * 5;
                double Y5 = Y4 + (Yp / d) * 5;
                double X6 = X4 - (Xp / d) * 5;
                double Y6 = Y4 - (Yp / d) * 5;

                GeometryGroup geometryGroup = new GeometryGroup();

                LineGeometry lineGeometry = new LineGeometry(new Point(this.X1, this.Y1), new Point(this.X2, this.Y2));
                LineGeometry arrowPart1Geometry = new LineGeometry(new Point(X3, Y3), new Point(X5, Y5));
                LineGeometry arrowPart2Geometry = new LineGeometry(new Point(X3, Y3), new Point(X6, Y6));

                geometryGroup.Children.Add(lineGeometry);
                geometryGroup.Children.Add(arrowPart1Geometry);
                geometryGroup.Children.Add(arrowPart2Geometry);

                return geometryGroup;
            }
        }
    }
}
