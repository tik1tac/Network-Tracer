using Network_Tracer.View;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Network_Tracer.Model.Graph
{
    internal class LineChangeColor
    {
        private static List<LineConnect> LinesChageColor;

        static List<Device> State;
        static Device StartState;
        static List<Device> RemainingDev;
        static private List<Device> Neighbo;
        public static bool IsEnergy = false;
        public static void CalculationMax(Source source)
        {
            Neighbo = new List<Device>();
            State = new List<Device>();
            RemainingDev = new List<Device>();
            if (Device._lines.Count != 0)
            {
                switch (source)
                {
                    case Source.Peg:
                        //ПЭГ
                        StartState = Device.Vertex.Where(vert => vert.Number == 1).First();
                        BrushLineIsSourcePEGorPEGsp();
                        break;
                    case Source.Vzg:
                        break;
                    case Source.PegSpare:
                        StartState = Device.Vertex.Where(vert => vert.Number == 2).First();
                        var peg= Device.Vertex.Where(vert => vert.Number == 1).First();
                        peg._neighbours.Remove(peg);
                        BrushLineIsSourcePEGorPEGsp();
                        break;
                    case Source.GSE:
                        break;
                    default:
                        break;
                }
            }

        }

        private static void BrushLineIsSourceGSE()
        {

        }
        private static void BrushLineIsSourceVZG()
        {

        }
        private static void BrushLineIsSourcePEGorPEGsp()
        {
            StartState.Lines[0].ColorConnection = Brushes.Red;
            StartState.PowerSuuply = true;
            StartState.Lines[0].Arrow(StartState);
            StartState.Lines[0].IsArrow = true;
            StartState.RectBorder = Brushes.Red;
            State.Add(StartState);
            State.Add(StartState.Lines[0].D2);
            State[1].RectBorder = Brushes.Red;
            int count = Device._countdevicesoncanvas;
            count -= 2;
            int i = 1;
            //Для всего графа
            while (true)
            {
                State[i].ISVisited = true;
                State[i].PowerSuuply = true;
                if (count == 0)
                {
                    IsEnergy = true;
                    break;
                }
                for (int iter = 0; iter < State[i]._neighbours.Count; iter++)
                {
                    if (!State[i]._neighbours[iter].PowerSuuply)
                    {
                        State[i].Lines
                            .Where(n => n.D2.LabelName == State[i]._neighbours[iter].LabelName || n.D1.LabelName == State[i]._neighbours[iter].LabelName)
                            .First().ColorConnection = Brushes.Red;
                        State[i]._neighbours[iter].PowerSuuply = true;
                        State[i]._neighbours[iter].RectBorder = Brushes.Red;
                        State[i].Lines[iter].Arrow(State[i]);
                        State[i].Lines[iter].IsArrow = true;
                        count--;
                    }
                }
                if (count == 0)
                {
                    IsEnergy = true;
                    break;
                }
                Neighbo.Clear();
                for (int iter = 0; iter < State[i].Lines.Count; iter++)
                {
                    if (State[i]._neighbours[iter]._neighbours.Count > 1 & State[i]._neighbours[iter] != State[i - 1]
                        & State[i]._neighbours[iter].ISVisited == false)
                    {
                        Neighbo.Add(State[i]._neighbours[iter]);
                    }
                }
                if (count == 0)
                {
                    IsEnergy = true;
                    break;
                }

                if (Neighbo.Count == 0)
                {
                    i--;
                }
                else
                {
                    State.Add(Neighbo.First());
                    Neighbo.Remove(Neighbo.First());
                    if (Neighbo.Count != 0)
                    {
                        for (int iter = 0; iter < Neighbo.Count; iter++)
                        {
                            RemainingDev.Add(Neighbo[iter]);
                        }
                    }
                    i++;
                    i = State.Count - 1;
                }

            }
        }
    }
}
