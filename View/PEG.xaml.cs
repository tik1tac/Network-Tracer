﻿using Network_Tracer.Model;
using Network_Tracer.Model.Graph;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
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
        [JsonIgnore]
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
