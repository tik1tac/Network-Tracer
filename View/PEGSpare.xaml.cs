using Network_Tracer.Model;
using Network_Tracer.Model.Graph;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    [Serializable]
    /// <summary>
    /// Логика взаимодействия для PEGSpare.xaml
    /// </summary>
    public partial class PEGSpare : NodesWithoutPort
    {
        public PEGSpare() : this(null)
        {

        }
        public PEGSpare(Canvas canvas) : base(canvas)
        {
            InitializeComponent();
            Weight = 10;
            Number = 2;
            this.LabelName = "ПЭГрез.";
        }

        public override int Number { get => base.Number; set => base.Number = value; }

        public override Brush RectBorder { get => PEGspX.Fill; set => PEGspX.Fill = value; }


        public override string city
        {
            get => base.city;
            set
            {
                base.city = value;
                CITY.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            }
        }
    }
}
