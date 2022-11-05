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
using System.Windows.Shapes;

namespace Network_Tracer.View
{
    /// <summary>
    /// Логика взаимодействия для SE.xaml
    /// </summary>
    public partial class SE : Nodes
    {

        public SE() : this(null)
        {

        }
        public SE(Canvas canvas) : base(canvas)
        {
            InitializeComponent();
            Weight = 2;
            this.LabelName = Scheme.GenerateName(Properties.Resources.SELabelName);
            this.canvas = canvas;
            NumberPorts = (int)CountPorts.eight;
            FreePorts = NumberPorts;
            this.PortsN = NumberPorts;
            Number = 4;
            PowerSuuply = false;
            Lines = new System.Collections.Generic.List<LineConnect>();
            _neighbours = new List<Device>();
            NamePorts = new Dictionary<LineConnect, NamePorts>();
            port = new Port();
        }
        public override Port port { get; set; }
        public override List<Device> _neighbours { get => base._neighbours; set => base._neighbours = value; }
        public override void AddNEighbours(Device D)
        {
            _neighbours.Add(D);
        }
        private LineConnect[] ports;
        private Canvas canvas { get; set; }

        //Количество портов у девайса
        public override int NumberPorts { get => base.NumberPorts; set => base.NumberPorts = value; }

        public override Brush RectBorder { get => SeX.Fill; set => SeX.Fill = value; }
        public override bool ISVisited { get => base.ISVisited; set => base.ISVisited = value; }

        public new Dictionary<LineConnect, NamePorts> NamePorts { get; set; }

        public override bool PowerSuuply { get => base.PowerSuuply; set => base.PowerSuuply = value; }

        public override int Number { get => base.Number; set => base.Number = value; }
        public int PortsN
        {
            get => ports.Length;
            set => this.ports = new LineConnect[value];
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
                NameSE1.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
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
                    if (deep)
                    {
                        this.ports[i].Remove(this);
                        foreach (var item in port.grid.Children)
                        {
                            if (item is Button)
                            {
                                if (!(item as Button).IsEnabled & (item as Button).Name == port.PortLine.Where(n => n.Key == ports[i]).First().Value)
                                {
                                    (item as Button).IsEnabled = true;
                                }
                            }
                        }
                        NamePorts.Remove(NamePorts.Where(n => n.Key == ports[i]).First().Key);
                        port.PortLine.Remove(ports[i]);
                        this.Lines.Remove(line);
                    }
                    this.ports[i] = null;
                }
            }
            return true;
        }
    }
}
