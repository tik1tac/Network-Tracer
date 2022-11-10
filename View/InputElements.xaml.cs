using Network_Tracer.Enums;
using Network_Tracer.Model.Graph;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Network_Tracer.View
{
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
            switch (switcher)
            {
                case InOrOutPort.InEnerg:
                    switch (source)
                    {
                        case Source.Peg:
                            grid.Children.Add(new ArrowInput(Canvas.GetLeft(textBlock) + 75, Canvas.GetLeft(GSE) + 33, Canvas.GetTop(textBlock) + 50, Canvas.GetTop(GSE) + 33));
                            break;
                        case Source.Vzg:
                            break;
                        case Source.PegSpare:
                            break;
                        default:
                            break;
                    }

                    //grid.Children.Add(new ArrowInput(Canvas.GetLeft(textBlock), Canvas.GetLeft(textBlock) + 75, Canvas.GetTop(textBlock) + 50, Canvas.GetTop(textBlock) + 50));
                    break;
                case InOrOutPort.Out:
                    grid.Children.Add(new ArrowInput(Canvas.GetLeft(GSE) + 33, Canvas.GetLeft(textBlock) + 75, Canvas.GetTop(GSE) + 33, Canvas.GetTop(textBlock) + 50));
                    break;
                default:
                    break;
            }

        }
    }
}
