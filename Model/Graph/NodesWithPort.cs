using Network_Tracer.View;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Network_Tracer.Model.Graph
{
    [Serializable]
    public class NodesWithPort : Device
    {
        public NodesWithPort(Canvas canvas) : base(canvas)
        {
            PowerSuuply = false;
            _neighbours = new System.Collections.Generic.List<Device>();
            port = new Port();
            InputElements = new InputElements();
            Lines = new System.Collections.Generic.List<LineConnect>();
            _neighbours = new List<Device>();
            NamePorts = new Dictionary<LineConnect, NamePorts>();
            NumberPorts = (int)CountPorts.eight;
            this.PortsN = NumberPorts;
        }
        public virtual int Weight { get; set; }
        public virtual int NumberPorts { get; set; }
        public override InputElements InputElements { get; set; }
        public override Port port { get; set; }
        public override List<Device> _neighbours { get => base._neighbours; set => base._neighbours = value; }
        private LineConnect[] ports;
        public override bool PowerSuuply { get => base.PowerSuuply; set => base.PowerSuuply = value; }
        public override bool ISVisited { get => base.ISVisited; set => base.ISVisited = value; }
        public new Dictionary<LineConnect, NamePorts> NamePorts { get; set; }
        public int PortsN
        {
            get => ports.Length;
            set => ports = new LineConnect[value];
        }
        public override void AddNEighbours(Device D)
        {
            _neighbours.Add(D);
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
            if (line != null)
            {
                foreach (var elem in this.Lines)
                {
                    if (elem.D1.LabelName == line.D1.LabelName)
                    {
                        this._neighbours.Remove(elem.D1);
                        elem.D1._neighbours.Remove(this);
                    }
                    if (elem.D2.LabelName == line.D2.LabelName)
                    {
                        this._neighbours.Remove(elem.D2);
                        elem.D2._neighbours.Remove(this);
                    }
                }
            }
            for (int i = 0; i < this.PortsN; ++i)
            {
                if (this.ports[i] != null && (line == null || this.ports[i] == line))
                {
                    DeletePort(i);
                    NamePorts.Remove(NamePorts.Where(n => n.Key == ports[i]).First().Key);
                    this.Lines.Remove(line);
                    port.PortLine.Remove(ports[i]);
                    if (deep)
                    {
                        this.ports[i].Remove(this);
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
