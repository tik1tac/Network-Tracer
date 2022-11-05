using Network_Tracer.Model;
using Network_Tracer.Model.Graph;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Network_Tracer.View
{
    /// <summary>
    /// Логика взаимодействия для PEG.xaml
    /// </summary>
    public partial class PEG : Nodes
    {

        public PEG() : this(null)
        {

        }
        public PEG(Canvas canvas) : base(canvas)
        {
            InitializeComponent();
            Weight = 10;
            this.canvas = canvas;
            NumberPorts = (int)CountPorts.one;
            FreePorts = NumberPorts;
            Number = 1;
            this.LabelName = Scheme.GenerateName(Properties.Resources.PEGLabelName);
            PowerSuuply = false;
            Lines = new System.Collections.Generic.List<LineConnect>();
            _neighbours = new System.Collections.Generic.List<Device>();
            port = new Port();
        }
        public override List<Device> _neighbours { get => base._neighbours; set => base._neighbours = value; }

        public override Port port { get; set; }
        public override void AddNEighbours(Device D)
        {
            _neighbours.Add(D);
        }
        private Canvas canvas { get; set; }

        public override bool ISVisited { get => base.ISVisited; set => base.ISVisited = value; }

        public override bool PowerSuuply { get => base.PowerSuuply; set => base.PowerSuuply = value; }

        public override int Number { get => base.Number; set => base.Number = value; }

        public override NamePorts NamePorts { get; set; }

        private LineConnect Line { get; set; }
        public override Brush RectBorder { get => PEGX.Fill; set => PEGX.Fill = value; }
        //Количество свободных портов у узла
        public override int FreePorts { get => base.FreePorts; set => base.FreePorts = value; }

        public bool SetPort(LineConnect line)
        {
            return false;
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
            if (Line == null)
            {
                Line = line;
                port.line = line;
                Lines.Add(line);
                return true;
            }
            return false;
        }
        public async override void SetPort()
        {
            port.IsConnected = false;
            port.ShowDialog();
            if (port.IsConnected)
            {
                NamePorts = port.SelectedPorts;
                port.IsClose = false;
            }
            await Task.Delay(0);
        }
        public override void Remove(object sender, System.Windows.RoutedEventArgs e)
        {
            this.RemoveLine(true);
            pegcount = null;
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
            if (Line != null)
            {
                if (line == null || line == Line)
                {
                    if (deep)
                    {
                        Line.Remove(this);
                        foreach (var item in port.grid.Children)
                        {
                            if (item is Button)
                            {
                                if (!(item as Button).IsEnabled)
                                {
                                    (item as Button).IsEnabled = true;
                                }
                            }
                        }
                        port.PortLine.Remove(Line);
                        this.Lines.Remove(line);
                    }
                    Line = null;
                    return true;
                }
            }

            return false;
        }
        public override void UpdateLocation()
        {
            if (Line != null)
            {
                Line.UpdateLocation(this, Canvas.GetLeft(this) + (this.Width / 2), Canvas.GetTop(this) + (this.Height / 2));
            }

        }
    }
}
