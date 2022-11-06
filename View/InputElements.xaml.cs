using Network_Tracer.Model.Graph;

using System.Windows;
using System.Windows.Controls;

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

        public double XGse { get; set; }
        public double YGse { get; set; }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void EnergizeInputElement(Device dev)
        {

        }
    }
}
