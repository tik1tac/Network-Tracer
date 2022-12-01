using Network_Tracer.Model.Graph;

using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Network_Tracer.View
{
    [Serializable]
    /// <summary>
    /// Логика взаимодействия для PEG.xaml
    /// </summary>
    public partial class PEG : NodesWithoutPort
    {
        public PEG() : this(null)
        {

        }
        public PEG(Canvas canvas) : base(canvas)
        {
            InitializeComponent();
            Weight = 10;
            Number = 1;
            this.LabelName = "ПЭГ";
        }
        public override int Number { get => base.Number; set => base.Number = value; }
        public override Brush RectBorder { get => PEGX.Fill; set => PEGX.Fill = value; }

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
