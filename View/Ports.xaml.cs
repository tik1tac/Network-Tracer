using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Network_Tracer.View
{
    /// <summary>
    /// Логика взаимодействия для Ports.xaml
    /// </summary>
    public partial class Ports : UserControl
    {
        public Ports()
        {
            InitializeComponent();
        }

        public NamePorts SelectedPorts
        {
            get;
            set;
        }
        private void S4_Click( object sender, RoutedEventArgs e )
        {
            SelectedPorts = NamePorts.S4;
        }

        private void S16_Click( object sender, RoutedEventArgs e )
        {
            SelectedPorts = NamePorts.S16;
        }

        private void T4_Click( object sender, RoutedEventArgs e )
        {
            SelectedPorts = NamePorts.T4;
        }

        private void T3_Click( object sender, RoutedEventArgs e )
        {
            SelectedPorts = NamePorts.T3;
        }
    }
}
