//using GalaSoft.MvvmLight;


using Network_Tracer.Model.Graph.AbstractGraph;
using Network_Tracer.View;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Network_Tracer.MainWindowViewModel
{

    public class MainViewModelGraph
    {
        //static MainWindow mv = new MainWindow();

        //public RelayCommand<MouseButtonEventArgs> CommandOne { get; set; } = new RelayCommand<MouseButtonEventArgs>(CanvasField_MouseLeftButtonDown);


        //static public Tools SelectedTool
        //{
        //    get
        //    {
        //        if ( mv.Connection.Opacity > 0.9 )
        //        {
        //            return Tools.Connection;
        //        }

        //        if ( mv.VZGButton.Opacity > 0.9 )
        //        {
        //            return Tools.VZG;
        //        }

        //        if ( mv.PEGButton.Opacity > 0.9 )
        //        {
        //            return Tools.PEG;
        //        }

        //        if ( mv.SEButton.Opacity > 0.9 )
        //        {
        //            return Tools.SE;
        //        }

        //        return Tools.Cursor;
        //    }

        //    private set
        //    {
        //        switch ( value )
        //        {
        //            case Tools.Connection:
        //                mv.SEButton.Opacity = mv.VZGButton.Opacity = mv.PEGButton.Opacity = mv.CursorButton.Opacity = 0.4;
        //                mv.Connection.Opacity = 1.0;
        //                break;

        //            case Tools.VZG:
        //                mv.Connection.Opacity = mv.PEGButton.Opacity = mv.SEButton.Opacity = mv.CursorButton.Opacity = 0.4;
        //                mv.VZGButton.Opacity = 1.0;
        //                break;

        //            case Tools.PEG:
        //                mv.Connection.Opacity = mv.VZGButton.Opacity = mv.SEButton.Opacity = mv.CursorButton.Opacity = 0.4;
        //                mv.PEGButton.Opacity = 1.0;
        //                break;

        //            case Tools.SE:
        //                mv.Connection.Opacity = mv.VZGButton.Opacity = mv.PEGButton.Opacity = mv.CursorButton.Opacity = 0.4;
        //                mv.SEButton.Opacity = 1.0;
        //                break;

        //            default:
        //                mv.Connection.Opacity = mv.VZGButton.Opacity = mv.PEGButton.Opacity = mv.SEButton.Opacity = 0.4;
        //                mv.CursorButton.Opacity = 1.0;
        //                break;
        //        }
        //    }
        //}

        //static public void CanvasField_MouseLeftButtonDown( MouseButtonEventArgs e )
        //{
        //    CreatorNodes cn = new FactoryNodes();
        //    Point p = Mouse.GetPosition(mv.CanvasField);
        //    switch ( SelectedTool )
        //    {
        //        case Tools.PEG:
        //            PEG peg = cn.CreatePeg(mv.CanvasField);
        //            Canvas.SetLeft(peg, p.X);
        //            Canvas.SetTop(peg, p.Y);
        //            //peg.MouseLeftButtonDown += this.OnComputerMouseLeftButtonDown;
        //            mv.CanvasField.Children.Add(peg);
        //            break;

        //        case Tools.VZG:
        //            VZG vzg = cn.CreateVZG(mv.CanvasField);
        //            Canvas.SetLeft(vzg, p.X);
        //            Canvas.SetTop(vzg, p.Y);
        //            //server.MouseLeftButtonDown += this.OnComputerMouseLeftButtonDown;
        //            mv.CanvasField.Children.Add(vzg);
        //            break;

        //        case Tools.SE:
        //            SE se = new SE(mv.CanvasField);
        //            Canvas.SetLeft(se, p.X);
        //            Canvas.SetTop(se, p.Y);
        //            //se.MouseLeftButtonDown += this.OnBridgeMouseLeftButtonDown;
        //            mv.CanvasField.Children.Add(se);
        //            break;

        //        case Tools.Connection:
        //            break;
        //    }
        //}
        ////RelayCommand command;
        ////public void ExecuteCommandPEG()
        ////{
        ////    this.SelectedTool = Tools.PEG;
        ////}
        ////public bool CanExecuteCommand() => true;

        ////public ICommand SelectPG => command = new RelayCommand(ExecuteCommandPEG, CanExecuteCommand);

        ////public ICommand OnMouseLeftButtownDown => command = new RelayCommand(ExecuteCommandOnMouseLeftButtownDown, CanExecuteCommand);



        ////public RelayCommand ExecuteCommandOnMouseLeftButtownDown
        ////{
        ////   
        ////}


    }
}
