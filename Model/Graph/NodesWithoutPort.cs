using Network_Tracer.View;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
        public virtual int Weight { get; set; }
        public virtual int NumberPorts { get; set; }
        public override List<Device> _neighbours { get => base._neighbours; set => base._neighbours = value; }
        public override bool ISVisited { get => base.ISVisited; set => base.ISVisited = value; }
        public override bool PowerSuuply { get => base.PowerSuuply; set => base.PowerSuuply = value; }
        public LineConnect Line { get; set; }
        public override Port port { get; set; }

        public override InputElements InputElements { get; set; }

        public async override void AddNEighbours(Device D)
        {
            _neighbours.Add(D);
            await Task.Delay(0);
        }
        public async override Task<bool> AddLine(LineConnect line)
        {
            if (!EnergizeSheme.IsEnergy)
            {
                if (Line == null)
                {
                    Line = line;
                    Lines.Add(line);
                    Device.Window.Modified = true;
                    return true;
                }
            }
            else
            {
                MessageBox.Show("Выключите питание");
            }
            await Task.Delay(0);
            return false;
        }
        public override async void Remove(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!EnergizeSheme.IsEnergy)
            {
                await this.RemoveLine(true);
                pegelement = null;
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
            else
            {
                MessageBox.Show("Выключите питание");
            }
        }

        public async override Task<bool> RemoveLine(bool deep, LineConnect line = null)
        {
            if (!EnergizeSheme.IsEnergy)
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
            }
            else
            {
                MessageBox.Show("Выключите питание");
            }
            await Task.Delay(0);
            return false;
        }

        public override async void UpdateLocation()
        {
            if (Line != null)
            {
                await Line.UpdateLocation(this, Canvas.GetLeft(this) + (this.Width / 2), Canvas.GetTop(this) + (this.Height / 2));
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
