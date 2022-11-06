using Network_Tracer.Model;
using Network_Tracer.Model.Graph;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Network_Tracer.View
{
    /// <summary>
    /// Логика взаимодействия для VZG.xaml
    /// </summary>
    public partial class VZG : Nodes
    {

        public VZG() : this(null)
        {

        }
        public VZG(Canvas canvas) : base(canvas)
        {
            InitializeComponent();
            Weight = 5;
            this.LabelName = Scheme.GenerateName(Properties.Resources.VSGLabelName);
            this.canvas = canvas;
            NumberPorts = (int)CountPorts.eight;
            FreePorts = NumberPorts;
            this.PortsN = NumberPorts;
            Number = 3;
            PowerSuuply = false;
            Lines = new System.Collections.Generic.List<LineConnect>();
            _neighbours = new List<Device>();
            NamePorts = new Dictionary<LineConnect, NamePorts>();
            port = new Port();
        }
        public override Port port { get; set; }
        public override List<Device> _neighbours { get => base._neighbours; set => base._neighbours = value; }
        private Canvas canvas { get; set; }
        private LineConnect[] ports;
        public override void AddNEighbours(Device D)
        {
            _neighbours.Add(D);
        }
        public override bool PowerSuuply { get => base.PowerSuuply; set => base.PowerSuuply = value; }

        public override bool ISVisited { get => base.ISVisited; set => base.ISVisited = value; }
        public override int Number { get => base.Number; set => base.Number = value; }

        public new Dictionary<LineConnect, NamePorts> NamePorts { get; set; }

        public override Brush RectBorder { get => VZGX.Fill; set => VZGX.Fill = value; }
        public int PortsN
        {
            get => ports.Length;
            set => ports = new LineConnect[value];
        }
        public override string LabelName
        {
            get
            {
                return base.LabelName;
            }

            set
            {
                base.LabelName = value;
                NameVZG1.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            }
        }
        public override string city
        {
            get => base.city;
            set
            {
                base.city = value;
                CITY.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            }
        }
        public override bool AddLine(LineConnect line)
        {
            for (int i = 0; i < this.ports.Length; ++i)
            {
                if (this.ports[i] == null)
                {
                    this.ports[i] = line;
                    port.line = ports[i];
                    Lines.Add(line);
                    return true;
                }
            }

            return false;
        }
        public async override void SetPort()
        {
            port.IsConnected = false;
            port.ShowDialog();
            for (int i = 0; i < this.ports.Length; ++i)
            {
                if (this.ports[i] != null)
                {
                    port.line = ports[i];
                    if (port.IsConnected & !NamePorts.ContainsKey(ports[i]))
                    {
                        NamePorts.Add(ports[i], port.SelectedPorts);
                        port.IsClose = false;
                    }

                }
            }
            await Task.Delay(0);
        }
        public override void UpdateLocation()
        {
            for (int i = 0; i < this.ports.Length; ++i)
            {
                if (this.ports[i] != null)
                {
                    this.ports[i].UpdateLocation(this, Canvas.GetLeft(this) + (this.Width / 2), Canvas.GetTop(this) + (this.Height / 2));
                }
            }
        }
        public override void Remove(object sender, System.Windows.RoutedEventArgs e)
        {
            this.RemoveLine(true);
            if (Vertex.Contains(this))
            {
                Vertex.Remove(this);
            }
            for (int i = 0; i < _neighbours.Count; i++)
            {
                _neighbours[i]._neighbours.Remove(this);
            }
            if (Scheme.Labelsname.Contains(this.LabelName))
            {
                Scheme.Labelsname.Remove(this.LabelName);
            }
            this.canvas.Children.Remove(this);
        }

        public override bool RemoveLine(bool deep, LineConnect line = null)
        {
            for (int i = 0; i < this.PortsN; ++i)
            {
                if (this.ports[i] != null && (line == null || this.ports[i] == line))
                {
                    DeletePort(i);
                    NamePorts.Remove(NamePorts.Where(n => n.Key == ports[i]).First().Key);
                    if (deep)
                    {
                        this.ports[i].Remove(this);
                        port.PortLine.Remove(ports[i]);
                        this.Lines.Remove(line);
                    }

                    this.ports[i] = null;
                }
            }
            return true;
        }
        public override void DeletePort(int i)
        {
            foreach (var item in port.grid.Children)
            {
                if (item is Button)
                {
                    if (!(item as Button).IsEnabled & (item as Button).Name == port.PortLine.Where(n => n.Key == ports[i]).First().Value)
                    {
                        (item as Button).IsEnabled = true;
                        port.BlockOpen[port.PortLine.Where(n => n.Key == ports[i]).First().Value] = StatePort.Open;
                    }
                }
            }
        }
    }
}
