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
        static List<Device> State;
        static Device StartState;
        static private List<Device> Neighbo;
        public static bool IsEnergy = false;
        public static List<Device> VZGStart;
        static Device ExcludedDev;
        static object blocked;
        static Dictionary<Device, List<Device>> NeighboursForVZG;
        public static void Energize(Source source)
        {
            Neighbo = new List<Device>();
            State = new List<Device>();
            VZGStart = new List<Device>();
            NeighboursForVZG = new Dictionary<Device, List<Device>>();
            blocked = new object();
            if (Device._lines.Count != 0)
            {
                switch (source)
                {
                    case Source.Peg:
                        //ПЭГ
                        StartState = Device.Vertex.Where(vert => vert.Number == 1).First();
                        BrushLineIfSourcePEGorPEGsp(Source.Peg);
                        break;
                    case Source.Vzg:

                        BrushLineIfSourceVZG();
                        ExcludedDev = Device.Vertex.Where(vert => vert.Number == 1).First();
                        ExcludedDev.RectBorder = Brushes.Silver;
                        ExcludedDev.Lines[0].ColorConnection = Brushes.Black;
                        ExcludedDev.Lines[0].ArrowToLine();
                        ExcludedDev = Device.Vertex.Where(vert => vert.Number == 2).First();
                        ExcludedDev.RectBorder = Brushes.Silver;
                        ExcludedDev.Lines[0].ColorConnection = Brushes.Black;
                        ExcludedDev.Lines[0].ArrowToLine();
                        break;
                    case Source.PegSpare:
                        StartState = Device.Vertex.Where(vert => vert.Number == 2).First();
                        BrushLineIfSourcePEGorPEGsp(Source.PegSpare);
                        ExcludedDev = Device.Vertex.Where(vert => vert.Number == 1).First();
                        ExcludedDev.RectBorder = Brushes.Silver;
                        ExcludedDev.Lines[0].ColorConnection = Brushes.Black;
                        ExcludedDev.Lines[0].ArrowToLine();
                        break;
                    case Source.GSE:
                        BrushLineIfSourceGSE();
                        break;
                    default:
                        break;
                }
            }

        }
        private static void BrushLineIfSourceGSE()
        {
            for (int i = 0; i < Device.Vertex.Count; i++)
            {
                if (Device.Vertex[i].GetType() != typeof(PEG) & Device.Vertex[i].GetType() != typeof(PEGSpare)
                    & Device.Vertex[i].GetType() != typeof(VZG))
                {
                    Device.Vertex[i].RectBorder = Brushes.Green;
                }
            }
        }
        private static void BrushLineIfSourceVZG()
        {
            int count = Device._countdevicesoncanvas;
            for (int i = 0; i < Device.Vertex.Count; i++)
            {
                if (Device.Vertex[i].GetType() == typeof(VZG))
                {

                    VZGStart.Add(Device.Vertex[i]);
                    State = new List<Device>();
                    NeighboursForVZG.Add(Device.Vertex[i], State);

                }
            }
            count -= VZGStart.Count;
            for (int i = 0; i < count; i++)
            {
                VZGStart[(int)i].ISVisited = true;
                VZGStart[(int)i].PowerSuuply = true;

                for (int iter = 0; iter < VZGStart.Count; iter++)
                {
                    lock (blocked)
                    {
                        for (int n = 0; n < NeighboursForVZG[VZGStart[(int)i]].Count; n++)
                        {
                            if (!NeighboursForVZG[VZGStart[(int)i]][iter]._neighbours[n].PowerSuuply)
                            {
                                NeighboursForVZG[VZGStart[(int)i]][iter]._neighbours[n].PowerSuuply = true;
                                VZGStart[(int)i].Lines
                                .Where(u => u.D2.LabelName == VZGStart[(int)i].LabelName || u.D1.LabelName == VZGStart[(int)i].LabelName)
                                .First().ColorConnection = Brushes.Yellow;
                                VZGStart[(int)i].Lines[n].LineToArrow(VZGStart[(int)i]);
                                count--;
                            }
                        }
                    }
                }

            }
        }
        private static void BrushLineIfSourcePEGorPEGsp(Source source)
        {
            if (source == Source.Peg)
            {
                StartState.Lines[0].ColorConnection = Brushes.Red;
            }
            else
            {
                StartState.Lines[0].ColorConnection = Brushes.Blue;
            }
            StartState.PowerSuuply = true;
            StartState.Lines[0].LineToArrow(StartState);
            StartState.Lines[0].IsArrow = true;
            if (source == Source.Peg)
            {
                StartState.RectBorder = Brushes.Red;
            }
            else
            {
                StartState.RectBorder = Brushes.Blue;
            }

            State.Add(StartState);
            State.Add(StartState.Lines[0].D2);
            if (source == Source.Peg)
            {
                State[1].RectBorder = Brushes.Red;
            }
            else
            {
                State[1].RectBorder = Brushes.Blue;
            }

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
                        if (source == Source.Peg)
                        {
                            State[i].Lines
    .Where(n => n.D2.LabelName == State[i]._neighbours[iter].LabelName || n.D1.LabelName == State[i]._neighbours[iter].LabelName)
    .First().ColorConnection = Brushes.Red;
                            State[i]._neighbours[iter].RectBorder = Brushes.Red;
                        }
                        else
                        {
                            State[i].Lines
.Where(n => n.D2.LabelName == State[i]._neighbours[iter].LabelName || n.D1.LabelName == State[i]._neighbours[iter].LabelName)
.First().ColorConnection = Brushes.Blue;
                            State[i]._neighbours[iter].RectBorder = Brushes.Blue;
                        }
                        State[i]._neighbours[iter].PowerSuuply = true;
                        State[i].Lines[iter].LineToArrow(State[i]);
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
                    i++;
                    i = State.Count - 1;
                }

            }
        }
    }
}
