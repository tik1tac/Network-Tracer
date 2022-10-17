using Network_Tracer.Model;
using Network_Tracer.Model.Graph;

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
        public SE( Canvas canvas ) : base(canvas)
        {
            InitializeComponent();
            Weight = 2;
            this.LabelName = Scheme.GenerateName(Properties.Resources.SELabelName);
            this.canvas = canvas;
            NumberPorts = (int)CountPorts.infinity;
            FreePorts = NumberPorts;
            this.Ports = NumberPorts;
            Number = 3;
        }
        private LineConnect[] ports;
        private Canvas canvas { get; set; }
        //Количество портов у девайса
        public override int NumberPorts { get => base.NumberPorts; set => base.NumberPorts = value; }

        //Количество свободных портов у узла
        public override int FreePorts { get => base.FreePorts; set => base.FreePorts = value; }
        public override int Weight { get => base.Weight; set => base.Weight = value; }
        public override double X { get => base.X; set => base.X = value; }
        public override double Y { get => base.Y; set => base.Y = value; }
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

        public override bool AddLine( LineConnect line )
        {
            for ( int i = 0 ; i < this.ports.Length ; ++i )
            {
                if ( this.ports[i] == null )
                {
                    this.ports[i] = line;
                    return true;
                }
            }

            return false;
        }

        public override void UpdateLocation()
        {
            for ( int i = 0 ; i < this.ports.Length ; ++i )
            {
                if ( this.ports[i] != null )
                {
                    this.ports[i].UpdateLocation(this, Canvas.GetLeft(this) + ( this.Width / 2 ), Canvas.GetTop(this) + ( this.Height / 2 ));
                }
            }
        }
        public override void Remove( object sender, System.Windows.RoutedEventArgs e )
        {
            this.RemoveLine(true);

            this.canvas.Children.Remove(this);
        }

        public override bool RemoveLine( bool deep, LineConnect line = null )
        {
            for ( int i = 0 ; i < this.Ports ; ++i )
            {
                if ( this.ports[i] != null && ( line == null || this.ports[i] == line ) )
                {
                    if ( deep )
                    {
                        this.ports[i].Remove(this);
                    }

                    this.ports[i] = null;
                }
            }
            return true;
        }
    }
}
