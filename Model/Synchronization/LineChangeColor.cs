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

        private static void CalculationMax(Source source)
        {
            int MaxCost = 0;
            LinesChageColor = new List<LineConnect>();
            State = new List<Device>();
            switch (source)
            {
                case Source.Peg:

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
            //ПЭГ
            StartState = Device.Vertex.Where(vert => vert.Number == 1).First();
            StartState.Lines[0].ColorConnection = Brushes.Red;
            State.Add(StartState.Lines[0].D2);
            Device._countdevicesoncanvas--;
            //Для всего графа
            for (int i = 0; i != Device._countdevicesoncanvas; i++)
            {
                for (int iter = 0; iter < State[i].Lines.Count; iter++)
                {
                    State[i].Lines[iter].ColorConnection = Brushes.Red;
                }
                for (int neigh = 0; neigh < State[i]._neighbours.Count; neigh++)
                {
                    //State[i]._neighbours[neigh]
                }
                State[i].Lines.First();
            }
            //Device.lineConnects
            //foreach ( Device D2 in Device.D2 )
            //{
            //    MaxCost = D2.Lines.Max(cost => cost.Cost);
            //    LinesChageColor.Add(D2.Lines.Where(s => s.Cost == MaxCost).First());
            //}
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
