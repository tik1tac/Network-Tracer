using Network_Tracer.Enums;
using Network_Tracer.Model.Graph;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Network_Tracer.View
{
    [Serializable]
    /// <summary>
    /// Логика взаимодействия для InputElements.xaml
    /// </summary>
    public partial class InputElements : Window
    {
        public InputElements()
        {
            InitializeComponent();
        }

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
        public void PaintLine(TextBlock textBlock, InOrOutPort switcher, Source source)
        {
            ArrowInput Arrow;
            switch (switcher)
            {
                case InOrOutPort.InEnerg:
                    Arrow = new ArrowInput
                    {
                        X1 = Canvas.GetLeft(textBlock) + 75,
                        X2 = Canvas.GetLeft(GSE) + 33,
                        Y1 = Canvas.GetTop(textBlock) + 50,
                        Y2 = Canvas.GetTop(GSE) + 33,
                        StrokeArrow = Brushes.Black
                    };
                    grid.Children.Add(Arrow);
                    break;
                case InOrOutPort.Out:
                    Arrow = new ArrowInput
                    {
                        X1 = Canvas.GetLeft(GSE) + 33,
                        X2 = Canvas.GetLeft(textBlock) + 75,
                        Y1 = Canvas.GetTop(GSE) + 33,
                        Y2 = Canvas.GetTop(textBlock) + 50,
                        StrokeArrow = Brushes.Black
                    };
                    grid.Children.Add(Arrow);
                    break;
                default:
                    break;
            }

        }
    }
}
