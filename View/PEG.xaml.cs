using Network_Tracer.Model;
using Network_Tracer.Model.Graph;

using System.Collections.Generic;
using System.Windows.Controls;

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
        public PEG( Canvas canvas ) : base(canvas)
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
        }
        public override List<Device> _neighbours { get => base._neighbours; set => base._neighbours = value; }

        public override void AddNEighbours(Device D)
        {
            _neighbours.Add(D);
        }
        private Canvas canvas { get; set; }

        public override bool ISVisited { get => base.ISVisited; set => base.ISVisited = value; }

        public override bool PowerSuuply { get => base.PowerSuuply; set => base.PowerSuuply = value; }

        public override int Number { get => base.Number; set => base.Number = value; }


        private LineConnect Line { get; set; }

        //Количество свободных портов у узла
        public override int FreePorts { get => base.FreePorts; set => base.FreePorts = value; }

        public bool SetPort( LineConnect line )
        {
            return false;
        }

        //public override string LabelName
        //{
        //    get
        //    {
        //        return base.LabelName;
        //    }

        //    set
        //    {
        //        base.LabelName = value;
        //        NamePEG1.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        //    }
        //}
        public override string city
        {
            get => base.city;
            set
            {
                base.city = value;
                CITY.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            }
        }
        //public override int GetPort( LineConnect line )
        //{
        //    if ( Line == line )
        //    {
        //        return -1;
        //    }

        //    return -2;
        //}
        public override bool AddLine( LineConnect line )
        {
            if ( Line == null )
            {
                Line = line;
                Lines.Add(line);
                return true;
            }

            return false;
        }
        public override void Remove( object sender, System.Windows.RoutedEventArgs e )
        {
            this.RemoveLine(true);
            pegcount = null;
            if ( Vertex.Contains(this) )
            {
                Vertex.Remove(this);
            }
            Device._countdevicesoncanvas--;
            this.canvas.Children.Remove(this);
        }
        public override bool RemoveLine( bool deep, LineConnect line = null )
        {
            if ( Line != null )
            {
                if ( line == null || line == Line )
                {
                    if ( deep )
                    {
                        Line.Remove(this);
                    }
                    Line = null;
                    return true;
                }
            }

            return false;
        }
        public override void UpdateLocation()
        {
            if ( Line != null )
            {
                Line.UpdateLocation(this, Canvas.GetLeft(this) + ( this.Width / 2 ), Canvas.GetTop(this) + ( this.Height / 2 ));
            }

        }
        //public override void SetPort( Device D2 )
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
