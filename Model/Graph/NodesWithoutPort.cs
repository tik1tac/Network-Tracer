using Network_Tracer.View;

using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Network_Tracer.Model.Graph
{
    [Serializable]
    public class NodesWithoutPort : Device
    {
        public NodesWithoutPort(Canvas canvas) : base(canvas)
        {
            Lines = new System.Collections.Generic.List<LineConnect>();
            PowerSuuply = false;
            _neighbours = new System.Collections.Generic.List<Device>();
            port = new Port();
        }
        [JsonProperty]
        public virtual int Weight { get; set; }
        [JsonProperty]
        public virtual int NumberPorts { get; set; }
        public override List<Device> _neighbours { get => base._neighbours; set => base._neighbours = value; }
        [JsonProperty]
        public override bool ISVisited { get => base.ISVisited; set => base.ISVisited = value; }
        [JsonProperty]
        public override bool PowerSuuply { get => base.PowerSuuply; set => base.PowerSuuply = value; }
        public LineConnect Line { get; set; }
        public override Port port { get; set; }

        public override InputElements InputElements { get; set; }

        public override void AddNEighbours(Device D)
        {
            _neighbours.Add(D);
        }
        public override bool AddLine(LineConnect line)
        {
            if (Line == null)
            {
                Line = line;
                Lines.Add(line);
                Device.Window.Modified = true;
                return true;
            }
            return false;
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
            Device.Window.Modified = true;
            this.canvas.Children.Remove(this);
        }

        public override bool RemoveLine(bool deep, LineConnect line = null)
        {
            if (Line != null)
            {
                if (line == null || line == Line)
                {
                    this.Lines.Remove(line);
                    this._neighbours.Clear();
                    if (deep)
                    {
                        Line.Remove(this);
                    }
                    Device.Window.Modified = true;
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
                Device.Window.Modified = true;
            }

        }
        public override void SetPort()
        {
            ;
        }

        public override void DeletePort(int i)
        {
            ;
        }
    }
}
