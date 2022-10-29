using Network_Tracer.View;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Network_Tracer.Model.Graph
{
    internal class LineChangeColor
    {
        private static List<LineConnect> LinesChageColor;

        static List<Device> State;
        static Device StartState;

        static private List<Device> Neighbo;

        public static void CalculationMax(Source source)
        {
            int MaxCost = 0;
            LinesChageColor = new List<LineConnect>();
            Neighbo = new List<Device>();
            State = new List<Device>();
            switch (source)
            {
                case Source.Peg:
                    //ПЭГ
                    StartState = Device.Vertex.Where(vert => vert.Number == 1).First();
                    StartState.Lines[0].ColorConnection = Brushes.Red;
                    StartState.PowerSuuply = true;
                    State.Add(StartState);
                    State.Add(StartState.Lines[0].D2);
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
                            break;
                        }
                        for (int iter = 0; iter < State[i]._neighbours.Count; iter++)
                        {
                            if (!State[i]._neighbours[iter].PowerSuuply)
                            {
                                var line = Device.GetLineBetween(State[i], State[i]._neighbours[iter]);
                                line.ColorConnection = Brushes.Red;
                                State[i]._neighbours[iter].PowerSuuply = true;
                                count--;
                            }
                        }
                        if (count == 0)
                        {
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
                            break;
                        }
                        i++;

                        State.Add(Neighbo.First());
                    }
                    break;
                case Source.Vzg:
                    break;
                case Source.PegSpare:
                    break;
                case Source.GSE:
                    break;
                default:
                    break;
            }

        }
        public static async void PaintingLine(Source source)
        {

            switch (source)
            {
                case Source.Peg:
                    LinesChageColor = ChangeSource.GetLines(LinesChageColor, Source.Peg);
                    for (int i = 0; i < LinesChageColor.Count; i++)
                    {
                        LinesChageColor[i].ColorConnection = Brushes.Red;
                    }
                    break;
                case Source.Vzg:
                    for (int i = 0; i < LinesChageColor.Count; i++)
                    {
                        LinesChageColor[i].ColorConnection = Brushes.Green;
                    }
                    LinesChageColor = ChangeSource.GetLines(LinesChageColor, Source.Vzg);
                    break;
                case Source.PegSpare:
                    for (int i = 0; i < LinesChageColor.Count; i++)
                    {
                        LinesChageColor[i].ColorConnection = Brushes.Yellow;
                    }
                    LinesChageColor = ChangeSource.GetLines(LinesChageColor, Source.PegSpare);
                    break;
                case Source.GSE:
                    for (int i = 0; i < LinesChageColor.Count; i++)
                    {
                        LinesChageColor[i].ColorConnection = Brushes.Blue;
                    }
                    LinesChageColor = ChangeSource.GetLines(LinesChageColor, Source.GSE);
                    break;
                default:
                    break;
            }
            await Task.Delay(0);

        }
    }
}
