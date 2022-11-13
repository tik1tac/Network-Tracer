using Network_Tracer.Model;
using Network_Tracer.Model.Graph;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Network_Tracer.View
{
    [Serializable]
    /// <summary>
    /// Логика взаимодействия для VZG.xaml
    /// </summary>
    public partial class VZG : NodesWithPort
    {

        public VZG() : this(null)
        {

        }
        public VZG(Canvas canvas) : base(canvas)
        {
            InitializeComponent();
            Weight = 5;
            Number = 3;
            this.LabelName = Scheme.GenerateName(Properties.Resources.VSGLabelName);
        }
        
        public override int Number { get => base.Number; set => base.Number = value; }
 
        public override Brush RectBorder { get => VZGX.Fill; set => VZGX.Fill = value; }

        public override string LabelName
        {
            get
            {
                return base.LabelName;
            }

            set
            {
                base.LabelName = value;
                NameVZG1.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
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
