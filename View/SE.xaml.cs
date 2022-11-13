using Network_Tracer.Model;
using Network_Tracer.Model.Graph;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Network_Tracer.View
{
    [Serializable]
    /// <summary>
    /// Логика взаимодействия для SE.xaml
    /// </summary>
    public partial class SE : NodesWithPort
    {

        public SE() : this(null)
        {

        }
        public SE(Canvas canvas) : base(canvas)
        {
            InitializeComponent();
            Weight = 2;
            Number = 4;
            this.LabelName = Scheme.GenerateName(Properties.Resources.SELabelName);
        }
        public override Brush RectBorder { get => SeX.Fill; set => SeX.Fill = value; }
      
        public override int Number { get => base.Number; set => base.Number = value; }
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
    }
}
