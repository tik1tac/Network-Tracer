using Network_Tracer.Model.Graph;

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
using System.Windows.Shapes;

namespace Network_Tracer.View
{
    /// <summary>
    /// Логика взаимодействия для Port.xaml
    /// </summary>
    public partial class Port : Window
    {
        public Port()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.Manual;
            PortLine = new Dictionary<LineConnect, string>();
        }
        public Dictionary<LineConnect, string> PortLine;
        public LineConnect line;
        public NamePorts SelectedPorts
        {
            get;
            set;
        }
        public bool IsConnected = false;
        public bool IsClose = false;
        private void S4_Click(object sender, RoutedEventArgs e)
        {
            SelectedPorts = NamePorts.S41;
            IsConnected = true;
            S41.IsEnabled = false;
            PortLine.Add(line,"S41");
            this.Hide();
        }

        private void S16_Click(object sender, RoutedEventArgs e)
        {
            SelectedPorts = NamePorts.S161;
            IsConnected = true;
            S161.IsEnabled = false;
            PortLine.Add(line, "S161");
            this.Hide();
        }

        private void T4_Click(object sender, RoutedEventArgs e)
        {
            SelectedPorts = NamePorts.T41;
            IsConnected = true;
            T41.IsEnabled = false;
            PortLine.Add(line, "T41");
            this.Hide();
        }

        private void T3_Click(object sender, RoutedEventArgs e)
        {
            SelectedPorts = NamePorts.T31;
            IsConnected = true;
            T31.IsEnabled = false;
            PortLine.Add(line,"T31");
            this.Hide();
        }

        private void S4_Click_2(object sender, RoutedEventArgs e)
        {
            SelectedPorts = NamePorts.S42;
            IsConnected = true;
            S42.IsEnabled = false;
            PortLine.Add(line, "S42");
            this.Hide();
        }

        private void S16_Click_2(object sender, RoutedEventArgs e)
        {
            SelectedPorts = NamePorts.S162;
            IsConnected = true;
            S162.IsEnabled = false;
            PortLine.Add(line, "S162");
            this.Hide();
        }

        private void T4_Click_2(object sender, RoutedEventArgs e)
        {
            SelectedPorts = NamePorts.T42;
            IsConnected = true;
            T42.IsEnabled = false;
            PortLine.Add(line, "T42");
            this.Hide();
        }

        private void T3_Click_2(object sender, RoutedEventArgs e)
        {
            SelectedPorts = NamePorts.T32;
            IsConnected = true;
            T32.IsEnabled = false;
            PortLine.Add(line, "S41");
            this.Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = Mouse.GetPosition(Device.Window.CanvasField).X;
            this.Top = Mouse.GetPosition(Device.Window.CanvasField).Y;
        }
    }
}
