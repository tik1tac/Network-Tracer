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

        private static void CalculationMax()
        {
            int MaxCost = 0;
            LinesChageColor = new List<LineConnect>();
            //Device.lineConnects
            //foreach ( Device D2 in Device.D2 )
            //{
            //    MaxCost = D2.Lines.Max(cost => cost.Cost);
            //    LinesChageColor.Add(D2.Lines.Where(s => s.Cost == MaxCost).First());
            //}
        }
        public static async void PaintingLine( Source source )
        {
            CalculationMax();

            switch ( source )
            {
                case Source.Peg:
                    LinesChageColor = ChangeSource.GetLines(LinesChageColor, Source.Peg);
                    for ( int i = 0 ; i < LinesChageColor.Count ; i++ )
                    {
                        LinesChageColor[i].ColorConnection = Brushes.Red;
                    }
                    break;
                case Source.Vzg:
                    for ( int i = 0 ; i < LinesChageColor.Count ; i++ )
                    {
                        LinesChageColor[i].ColorConnection = Brushes.Green;
                    }
                    LinesChageColor = ChangeSource.GetLines(LinesChageColor, Source.Vzg);
                    break;
                case Source.PegSpare:
                    for ( int i = 0 ; i < LinesChageColor.Count ; i++ )
                    {
                        LinesChageColor[i].ColorConnection = Brushes.Yellow;
                    }
                    LinesChageColor = ChangeSource.GetLines(LinesChageColor, Source.PegSpare);
                    break;
                case Source.GSE:
                    for ( int i = 0 ; i < LinesChageColor.Count ; i++ )
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
