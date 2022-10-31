using Network_Tracer.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Network_Tracer.View
{
    /// <summary>
    /// Логика взаимодействия для PEGSpare.xaml
    /// </summary>
    public partial class PEGSpare : Nodes
    {
        public PEGSpare() : this(null)
        {

        }
        public PEGSpare( Canvas canvas ) : base(canvas)
        {
            InitializeComponent();
            Weight = 10;
            this.canvas = canvas;
            NumberPorts = (int)CountPorts.one;
            FreePorts = NumberPorts;
            Number = 2;
            this.LabelName = Scheme.GenerateName(Properties.Resources.PEGLabelName);
            PowerSuuply = false;
            Lines = new System.Collections.Generic.List<LineConnect>();
            _neighbours = new List<Device>();
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

        public override Brush RectBorder { get => PEGspX.Fill; set => PEGspX.Fill = value; }

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
        //        NamePEGSpare.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
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
            pegsparecount = null;
            if ( Vertex.Contains(this) )
            {
                Vertex.Remove(this);
            }
            for (int i = 0; i < _neighbours.Count; i++)
            {
                _neighbours[i]._neighbours.Remove(this);
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
