﻿using Network_Tracer.Model;
using Network_Tracer.Model.Graph;

using System.Windows.Controls;

namespace Network_Tracer.View
{
    /// <summary>
    /// Логика взаимодействия для VZG.xaml
    /// </summary>
    public partial class VZG : Nodes
    {

        public VZG() : this(null)
        {

        }
        public VZG( Canvas canvas ) : base(canvas)
        {
            InitializeComponent();
            Weight = 5;
            this.LabelName = Scheme.GenerateName(Properties.Resources.VSGLabelName);
            this.canvas = canvas;
            NumberPorts = (int)CountPorts.infinity;
            FreePorts = NumberPorts;
            this.Ports = NumberPorts;
            Number = 2;
            PowerSuuply = false;
            Lines = new System.Collections.Generic.List<LineConnect>();
        }
        private Canvas canvas { get; set; }
        private LineConnect[] ports;

        public override bool PowerSuuply { get => base.PowerSuuply; set => base.PowerSuuply = value; }


        public override int Number { get => base.Number; set => base.Number = value; }

        public int Ports
        {
            get => ports.Length;
            set => ports = new LineConnect[value];
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
        public override bool AddLine( LineConnect line )
        {
            for ( int i = 0 ; i < this.ports.Length ; ++i )
            {
                if ( this.ports[i] == null )
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
            if ( D2.Contains(this) )
            {
                D2.Remove(this);
            }

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
        //public override void SetPort( Device D2 )
        //{
        //    throw new System.NotImplementedException();
        //}
        //public override int GetPort( LineConnect line )
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}