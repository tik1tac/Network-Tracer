using Network_Tracer.Model.Graph;
using Network_Tracer.Model.Graph.AbstractGraph;

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Network_Tracer.View
{
    /// <summary>
    /// Логика взаимодействия для LineConnect.xaml
    /// </summary>
    public partial class LineConnect : UserControl
    {
        public LineConnect(Canvas canvas)
        {
            InitializeComponent();
            this.canvas = canvas;
        }
        #region Анимация
        static LineConnect()
        {
            X1Line = DependencyProperty.Register("X1", typeof(double), typeof(LineConnect), new PropertyMetadata(0.1, X1_PC));
            X2Line = DependencyProperty.Register("X2", typeof(double), typeof(LineConnect), new PropertyMetadata(0.1, X2_PC));
            Y1Line = DependencyProperty.Register("Y1", typeof(double), typeof(LineConnect), new PropertyMetadata(0.1, Y1_PC));
            Y2Line = DependencyProperty.Register("Y2", typeof(double), typeof(LineConnect), new PropertyMetadata(0.1, Y2_PC));
        }
        #region Property

        public double X1
        {
            get { return (double)GetValue(X1Line); }
            set { SetValue(X1Line, value); }
        }

        public double X2
        {
            get { return (double)GetValue(X2Line); }
            set { SetValue(X2Line, value); }
        }

        public double Y1
        {
            get { return (double)GetValue(Y1Line); }
            set { SetValue(Y1Line, value); }
        }

        public double Y2
        {
            get { return (double)GetValue(Y2Line); }
            set { SetValue(Y2Line, value); }
        }

        #endregion

        #region DependencyProperty

        public static readonly DependencyProperty X1Line;
        public static readonly DependencyProperty X2Line;
        public static readonly DependencyProperty Y1Line;
        public static readonly DependencyProperty Y2Line;

        #endregion

        #region Change calback functions

        public static void X1_PC(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            LineConnect c = obj as LineConnect;
            double nv = (double)e.NewValue;
            c.X1 = c.Line.X1 = nv;

            c.ChangeAnimationValue();
        }

        public static void X2_PC(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            LineConnect c = obj as LineConnect;
            double nv = (double)e.NewValue;
            c.X2 = c.Line.X2 = nv;
            //c.ArrowRight.X1 = nv;
            //c.ArrowLeft.X1 = nv;
            //c.ArrowRight.X2 = nv - 10;
            //c.ArrowLeft.X2 = nv - 20;
            c.ChangeAnimationValue();
        }

        public static void Y1_PC(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            LineConnect c = obj as LineConnect;
            double nv = (double)e.NewValue;
            c.Y1 = c.Line.Y1 = nv;
            c.ChangeAnimationValue();
        }

        public static void Y2_PC(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            LineConnect c = obj as LineConnect;
            double nv = (double)e.NewValue;
            c.Y2 = c.Line.Y2 = nv;
            //c.ArrowRight.Y1 = nv;
            //c.ArrowLeft.Y1 = nv;
            //c.ArrowRight.Y2 = nv - 20;
            //c.ArrowLeft.Y2 = nv - 13;
            c.ChangeAnimationValue();
        }




        #endregion

        #region Delegates structure

        public delegate void CompleteAnimationCallback(LineConnect lineControl);

        #endregion

        #region Class variables
        //спикок функций(CompleteAnimationCallback) подписанных на событие CompleteAnimationEvent 
        private readonly List<CompleteAnimationCallback> _listHandles = new List<CompleteAnimationCallback>();

        public event CompleteAnimationCallback CompleteAnimationEvent
        {
            add
            {
                _listHandles.Add(value);
            }

            remove
            {
                _listHandles.Remove(value);
            }
        }
        public TimeSpan SpeedAnimation { get; private set; }

        #endregion

        private void ChangeAnimationValue()
        {
            TimelineCollection collection = a.Storyboard.Children;//берём колекцию колекций кадров
            //устанавливаем значения
            (collection[0] as DoubleAnimationUsingKeyFrames).KeyFrames[0].Value = (collection[1] as DoubleAnimationUsingKeyFrames).KeyFrames[0].Value = (collection[0] as DoubleAnimationUsingKeyFrames).KeyFrames[1].Value = X1;
            (collection[2] as DoubleAnimationUsingKeyFrames).KeyFrames[0].Value = (collection[3] as DoubleAnimationUsingKeyFrames).KeyFrames[0].Value = (collection[2] as DoubleAnimationUsingKeyFrames).KeyFrames[1].Value = Y1;

            (collection[0] as DoubleAnimationUsingKeyFrames).KeyFrames[2].Value = X2;
            (collection[1] as DoubleAnimationUsingKeyFrames).KeyFrames[1].Value = X2;
            (collection[2] as DoubleAnimationUsingKeyFrames).KeyFrames[2].Value = Y2;
            (collection[3] as DoubleAnimationUsingKeyFrames).KeyFrames[1].Value = Y2;
        }

        public void BeginAnimation()
        {
            a.Storyboard.Begin();
        }

        public void StopAnimation()
        {
            a.Storyboard.Stop();

            //возвращаем значения
            Line.X1 = X1;
            Line.X2 = X2;
            Line.Y1 = Y1;
            Line.Y2 = Y2;
        }

        public void RemoveAllHandles_CompleteAnimationEvent()
        {
            for (int i = 0; i < _listHandles.Count; i++)
            {
                CompleteAnimationEvent -= _listHandles[i];
                //_listHandles.Remove(_listHandles[i]);
            }
        }

        public void SetSpeed(TimeSpan time)
        {
            StopAnimation();

            TimelineCollection collection = a.Storyboard.Children;//берём колекцию колекций кадров
            //устанавливаем значения
            (collection[0] as DoubleAnimationUsingKeyFrames).KeyFrames[1].KeyTime = (collection[1] as DoubleAnimationUsingKeyFrames).KeyFrames[1].KeyTime = (collection[2] as DoubleAnimationUsingKeyFrames).KeyFrames[1].KeyTime = (collection[3] as DoubleAnimationUsingKeyFrames).KeyFrames[1].KeyTime = new TimeSpan(0, 0, 0, 0, (int)Math.Round(time.TotalMilliseconds / 2));
            (collection[0] as DoubleAnimationUsingKeyFrames).KeyFrames[2].KeyTime = (collection[2] as DoubleAnimationUsingKeyFrames).KeyFrames[2].KeyTime = time;
            SpeedAnimation = time;
        }

        private void CompleteAnimation(object sender, EventArgs e)
        {
            //CompleteAnimationEvent?.Invoke(this);
            foreach (CompleteAnimationCallback callback in _listHandles)
            {
                callback(this);//вызываем функции
            }
        }
        #endregion

        public Brush ColorConnection { get => Line.Stroke; set => Line.Stroke = value; }

        public bool IsArrow { get; set; }
        public int Cost { get; private set; }

        public Device D1 { get; set; }
        public Device D2 { get; set; }

        public NamePorts Port1 { get => Port1; set => Port1 = value; }
        public NamePorts Port2 { get => Port2; set => Port2 = value; }


        Canvas canvas { get; set; }

        public void SetCost(Device D)
        {
            var TypeD = D.GetType();
            if (TypeD == typeof(PEG))
            {
                Cost = 10;
            }
            if (TypeD == typeof(VZG))
            {
                Cost = 5;
            }
            if (TypeD == typeof(SE))
            {
                Cost = 2;
            }
            if (TypeD == typeof(PEGSpare))
            {
                Cost = 7;
            }
        }

        public void UpdateLocation(Device device, double x, double y)
        {
            if (this.D1 == device)
            {
                this.X1 = (int)x;
                this.Y1 = (int)y;
            }
            else if (this.D2 == device)
            {
                this.X2 = (int)x;
                this.Y2 = (int)y;
            }

            Canvas.SetTop(this.WireBorder, (this.Y1 + this.Y2 - this.WireBorder.Height) / 2);
            Canvas.SetLeft(this.WireBorder, (this.X1 + this.X2 - this.WireBorder.Width) / 2);
        }

        public void Remove(object sender = null, RoutedEventArgs e = null)
        {

            if (this.D1 != null && this.D1 != sender)
            {
                this.D1.RemoveLine(false, this);
            }

            if (this.D2 != null && this.D2 != sender)
            {
                this.D2.RemoveLine(false, this);
            }

            if (this.canvas != null)
            {
                this.canvas.Children.Remove(this);
            }
        }
        public void Arrow(Device dev)
        {
            double X3 = (this.X1 + this.X2) / 2;
            double Y3 = (this.Y1 + this.Y2) / 2;

            // длина отрезка
            double d = Math.Sqrt(Math.Pow(this.X2 - this.X1, 2) + Math.Pow(this.Y2 - this.Y1, 2));

            // координаты вектора
            double X = this.X2 - this.X1;
            double Y = this.Y2 - this.Y1;

            //центр
            double midX = (this.X1 + this.X2) / 2;
            double midY = (this.Y1 + this.Y2) / 2;
            double X4;
            double Y4;
            // координаты точки, удалённой от центра к началу отрезка на 10px
            if (dev.LabelName == this.D1.LabelName)
            {
                X4 = X3 - (X / d) * 10;
                Y4 = Y3 - (Y / d) * 10;
            }
            else
            {
                X4 = X3 + (X / d) * 10;
                Y4 = Y3 + (Y / d) * 10;

            }

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

            ArrowRight.X1 = midX;
            ArrowRight.Y1 = midY;
            ArrowLeft.X1 = midX;
            ArrowLeft.Y1 = midY;

            ArrowRight.X2 = X5;
            ArrowRight.Y2 = Y5;
            ArrowLeft.X2 = X6;
            ArrowLeft.Y2 = Y6;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            a.Storyboard.Completed += CompleteAnimation; //подписываемся на событие оканчании анимации
        }
    }
}
