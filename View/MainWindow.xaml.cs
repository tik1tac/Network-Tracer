using Network_Tracer.Model.Graph;
using Network_Tracer.Model.Graph.AbstractGraph;
using Network_Tracer.View;

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Network_Tracer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += OnWindowClosing;
            Device.Window = this;
        }
        LineConnect SelectedLine;
        Device SelectedDevice;
        public Tools SelectedTool
        {
            get
            {
                if ( this.Connection.Opacity > 0.9 )
                {
                    return Tools.Connection;
                }

                if ( this.VZGButton.Opacity > 0.9 )
                {
                    return Tools.VZG;
                }

                if ( this.PEGButton.Opacity > 0.9 )
                {
                    return Tools.PEG;
                }

                if ( this.SEButton.Opacity > 0.9 )
                {
                    return Tools.SE;
                }

                return Tools.Cursor;
            }

            set
            {
                switch ( value )
                {
                    case Tools.VZG:
                        this.Connection.Opacity = this.PEGButton.Opacity = this.SEButton.Opacity = this.CursorButton.Opacity = 0.4;
                        this.VZGButton.Opacity = 1.0;
                        break;

                    case Tools.Connection:
                        this.SEButton.Opacity = this.VZGButton.Opacity = this.PEGButton.Opacity = this.CursorButton.Opacity = 0.4;
                        this.Connection.Opacity = 1.0;
                        break;

                    case Tools.PEG:
                        this.Connection.Opacity = this.VZGButton.Opacity = this.SEButton.Opacity = this.CursorButton.Opacity = 0.4;
                        this.PEGButton.Opacity = 1.0;
                        break;

                    case Tools.SE:
                        this.Connection.Opacity = this.VZGButton.Opacity = this.PEGButton.Opacity = this.CursorButton.Opacity = 0.4;
                        this.SEButton.Opacity = 1.0;
                        break;

                    default:
                        this.Connection.Opacity = this.VZGButton.Opacity = this.PEGButton.Opacity = this.SEButton.Opacity = 0.4;
                        this.CursorButton.Opacity = 1.0;
                        break;
                }
            }
        }
        protected override void OnMouseLeftButtonDown( MouseButtonEventArgs e )
        {
            base.OnMouseLeftButtonDown(e);
            CreatorNodes cn = new FactoryNodes();
            Point p = e.GetPosition(this);
            switch ( this.SelectedTool )
            {
                case Tools.PEG:
                    PEG peg = new PEG(CanvasField);
                    Canvas.SetLeft(peg, p.X - ( peg.Width / 2 ));
                    Canvas.SetTop(peg, p.Y - ( peg.Height / 2 ));
                    peg.MouseLeftButtonDown += this.OnPEGLeftButtonDown;
                    CanvasField.Children.Add(peg);
                    break;

                case Tools.VZG:
                    VZG vzg = cn.CreateVZG(CanvasField);
                    Canvas.SetLeft(vzg, p.X - ( vzg.Width / 2 ));
                    Canvas.SetTop(vzg, p.Y - ( vzg.Height / 2 ));
                    vzg.MouseLeftButtonDown += this.OnVZGLeftButtonDown;
                    CanvasField.Children.Add(vzg);
                    break;

                case Tools.SE:
                    SE se = new SE(CanvasField);
                    Canvas.SetLeft(se, p.X - ( se.Width / 2 ));
                    Canvas.SetTop(se, p.Y - ( se.Height / 2 ));
                    se.MouseLeftButtonDown += this.OnSELeftButtonDown;
                    CanvasField.Children.Add(se);
                    break;

                case Tools.Connection:
                    break;
            }
        }

        protected override void OnMouseLeftButtonUp( MouseButtonEventArgs e )
        {
            base.OnMouseLeftButtonUp(e);

            if ( Device.NewLine != null )
            {
                Device.NewLine.Remove(null, null);
                Device.NewLine = null;
            }
        }

        protected override void OnDragOver( DragEventArgs e )
        {
            base.OnDragOver(e);

            if ( e.Data.GetDataPresent("Device") )
            {
                Device obj = (Device)e.Data.GetData("Device");
                Point p = e.GetPosition(this);

                // Creation of a new wire
                if ( Device.NewLine != null )
                {
                    Device.NewLine.X2 = p.X;

                    // Menubar height
                    Device.NewLine.Y2 = p.Y - 20;
                }
                else
                {
                    // Drag & drop of an object
                    Canvas.SetLeft(obj, p.X - ( obj.Width / 2 ));
                    Canvas.SetTop(obj, p.Y - ( obj.Height / 2 ));
                    obj.UpdateLocation();
                }
            }
        }

        protected override void OnDrop( DragEventArgs e )
        {
            base.OnDrop(e);

            if ( Device.NewLine != null )
            {
                Device.NewLine.Remove(null, null);
                Device.NewLine = null;
            }
        }

        private void VZGButton_Click( object sender, RoutedEventArgs e )
        {

            this.SelectedTool = Tools.VZG;
        }

        private void PEGButton_Click( object sender, RoutedEventArgs e )
        {
            this.SelectedTool = Tools.PEG;
        }

        private void SEButton_Click( object sender, RoutedEventArgs e )
        {
            this.SelectedTool = Tools.SE;
        }

        private void CursorButton_Click( object sender, RoutedEventArgs e )
        {
            this.SelectedTool = Tools.Cursor;
        }

        private void Connection_Click( object sender, RoutedEventArgs e )
        {
            this.SelectedTool = Tools.Connection;
        }

        public void OnPEGLeftButtonDown( object sender, RoutedEventArgs e )
        {
            PEG peg = (PEG)sender;
            this.SelectedDevice = peg;
            NameDevice.Text = peg.LabelName;
            LineExpender.Visibility = Visibility.Collapsed;
            DeviceExpander.Visibility = Visibility.Visible;
        }
        public void OnVZGLeftButtonDown( object sender, RoutedEventArgs e )
        {
            VZG vzg = (VZG)sender;
            this.SelectedDevice = vzg;
            NameDevice.Text = vzg.LabelName;
            LineExpender.Visibility = Visibility.Collapsed;
            DeviceExpander.Visibility = Visibility.Visible;
        }
        public void OnSELeftButtonDown( object sender, RoutedEventArgs e )
        {
            SE se = (SE)sender;
            this.SelectedDevice = se;
            NameDevice.Text = se.LabelName;
            LineExpender.Visibility = Visibility.Collapsed;
            DeviceExpander.Visibility = Visibility.Visible;
        }
        public void OnLineLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            LineConnect line = (LineConnect)sender;
            this.SelectedLine = line;
            Device1.Text = SelectedLine.D1.LabelName;
            Device2.Text = SelectedLine.D2.LabelName;
            DeviceExpander.Visibility = Visibility.Collapsed;
            LineExpender.Visibility = Visibility.Visible;
        }
        public void OnWindowClosing( object sender, CancelEventArgs e )
        {
            if ( !this.CloseScheme() )
            {
                e.Cancel = true;
            }
        }
        private bool CloseScheme()
        {
            switch ( MessageBox.Show("Схема была изменена", "Схема была изменена", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel) )
            {
                case MessageBoxResult.Cancel:
                    return false;

                case MessageBoxResult.Yes:
                    //this.SaveScheme(null, null);
                    return false;

                case MessageBoxResult.No:
                    break;
            }

            return true;
        }

        private void CityDevice_TextChanged( object sender, TextChangedEventArgs e )
        {
            SelectedDevice.city = CityDevice.Text;
        }

        private void DeviceDelete_Click( object sender, RoutedEventArgs e )
        {
            this.SelectedDevice.Remove(null, null);
        }

        private void LineDelete_Click( object sender, RoutedEventArgs e )
        {
            this.SelectedLine.Remove(null, null);
        }

        private void CreateNewScheme_Click( object sender, RoutedEventArgs e )
        {

        }

        private void OpenScheme_Click( object sender, RoutedEventArgs e )
        {

        }

        private void SaveScheme_Click( object sender, RoutedEventArgs e )
        {

        }
    }
}
