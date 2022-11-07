using Network_Tracer.Enums;
using Network_Tracer.Model.Graph;

using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

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
            BlockOpen = new Dictionary<string, StatePort>();
            InOrOutPortDict = new Dictionary<string, InOrOutPort>();
            BlockOpen.Add("S41", StatePort.Open);
            BlockOpen.Add("S42", StatePort.Open);
            BlockOpen.Add("T41", StatePort.Open);
            BlockOpen.Add("T42", StatePort.Open);
            BlockOpen.Add("T31", StatePort.Open);
            BlockOpen.Add("T32", StatePort.Open);
            BlockOpen.Add("S161", StatePort.Open);
            BlockOpen.Add("S162", StatePort.Open);

            InOrOutPortDict.Add("S41", InOrOutPort.Default);
            InOrOutPortDict.Add("S42", InOrOutPort.Default);
            InOrOutPortDict.Add("T41", InOrOutPort.Default);
            InOrOutPortDict.Add("T42", InOrOutPort.Default);
            InOrOutPortDict.Add("T31", InOrOutPort.Default);
            InOrOutPortDict.Add("T32", InOrOutPort.Default);
            InOrOutPortDict.Add("S161", InOrOutPort.Default);
            InOrOutPortDict.Add("S162", InOrOutPort.Default);
        }
        public Dictionary<LineConnect, string> PortLine;
        public Dictionary<string, StatePort> BlockOpen;
        public Dictionary<string, InOrOutPort> InOrOutPortDict;
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
            PortLine.Add(line, "S41");
            BlockOpen["S41"] = StatePort.Blocked;
            this.Hide();
        }

        private void S16_Click(object sender, RoutedEventArgs e)
        {
            SelectedPorts = NamePorts.S161;
            IsConnected = true;
            S161.IsEnabled = false;
            PortLine.Add(line, "S161");
            BlockOpen["S161"] = StatePort.Blocked;
            this.Hide();
        }

        private void T4_Click(object sender, RoutedEventArgs e)
        {
            SelectedPorts = NamePorts.T41;
            IsConnected = true;
            T41.IsEnabled = false;
            PortLine.Add(line, "T41");
            BlockOpen["T41"] = StatePort.Blocked;
            this.Hide();
        }

        private void T3_Click(object sender, RoutedEventArgs e)
        {
            SelectedPorts = NamePorts.T31;
            IsConnected = true;
            T31.IsEnabled = false;
            PortLine.Add(line, "T31");
            BlockOpen["T31"] = StatePort.Blocked;
            this.Hide();
        }

        private void S4_Click_2(object sender, RoutedEventArgs e)
        {
            SelectedPorts = NamePorts.S42;
            IsConnected = true;
            S42.IsEnabled = false;
            PortLine.Add(line, "S42");
            BlockOpen["S42"] = StatePort.Blocked;
            this.Hide();
        }

        private void S16_Click_2(object sender, RoutedEventArgs e)
        {
            SelectedPorts = NamePorts.S162;
            IsConnected = true;
            S162.IsEnabled = false;
            PortLine.Add(line, "S162");
            BlockOpen["S162"] = StatePort.Blocked;
            this.Hide();
        }

        private void T4_Click_2(object sender, RoutedEventArgs e)
        {
            SelectedPorts = NamePorts.T42;
            IsConnected = true;
            T42.IsEnabled = false;
            PortLine.Add(line, "T42");
            BlockOpen["T42"] = StatePort.Blocked;
            this.Hide();
        }

        private void T3_Click_2(object sender, RoutedEventArgs e)
        {
            SelectedPorts = NamePorts.T32;
            IsConnected = true;
            T32.IsEnabled = false;
            PortLine.Add(line, "S41");
            BlockOpen["T32"] = StatePort.Blocked;
            this.Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = Mouse.GetPosition(Device.Window.CanvasField).X;
            this.Top = Mouse.GetPosition(Device.Window.CanvasField).Y;
        }
    }
}
