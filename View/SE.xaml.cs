using Network_Tracer.Model;
using Network_Tracer.Model.Graph;

using System.Collections.Generic;
using System.Windows.Controls;

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
            NumberPorts = (int)CountPorts.infinity;
            FreePorts = NumberPorts;
            this.Ports = NumberPorts;
            Number = 4;
            PowerSuuply = false;
            Lines = new System.Collections.Generic.List<LineConnect>();
            _neighbours = new List<Device>();
        }
        public override List<Device> _neighbours { get => base._neighbours; set => base._neighbours = value; }
        public override void AddNEighbours(Device D)
        {
            _neighbours.Add(D);
        }
        private LineConnect[] ports;
        private Canvas canvas { get; set; }

        //Количество портов у девайса
        public override int NumberPorts { get => base.NumberPorts; set => base.NumberPorts = value; }

        public override bool ISVisited { get => base.ISVisited; set => base.ISVisited = value; }
        public override bool PowerSuuply { get => base.PowerSuuply; set => base.PowerSuuply = value; }

        public override int Number { get => base.Number; set => base.Number = value; }
        public int Ports
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
                    Lines.Add(line);
                    return true;
                }
            }

            return false;
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
            Device._countdevicesoncanvas--;
            this.canvas.Children.Remove(this);
        }

        public override bool RemoveLine(bool deep, LineConnect line = null)
        {
            for (int i = 0; i < this.Ports; ++i)
            {
                if (this.ports[i] != null && (line == null || this.ports[i] == line))
                {
                    if (deep)
                    {
                        this.ports[i].Remove(this);
                    }

                    this.ports[i] = null;
                }
            }
            return true;
        }

        //public override int GetPort( LineConnect line )
        //{
        //    throw new System.NotImplementedException();
        //}

        //public override void SetPort( Device D2 )
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
