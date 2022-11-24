using Network_Tracer.Model;
using Network_Tracer.Model.Graph;

using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Network_Tracer.View
{
    [Serializable]
    /// <summary>
    /// Логика взаимодействия для User.xaml
    /// </summary>
    public partial class User : NodesWithoutPort
    {
        public User() : this(null)
        {

        }
        public User(Canvas canvas) : base(canvas)
        {
            InitializeComponent();
            Weight = 0;
            Number = 5;
            LabelName = Scheme.GenerateName("Пользователь");
        }
        public override int Number { get => base.Number; set => base.Number = value; }
        public override string city
        {
            get => base.city;
            set
            {
                base.city = value;
                CITY.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            }
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
                NameUser.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            }
        }
    }
}
