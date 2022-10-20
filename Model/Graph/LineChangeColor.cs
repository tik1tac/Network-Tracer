using Network_Tracer.View;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Network_Tracer.Model.Graph
{
    internal class LineChangeColor
    {
        private static List<LineConnect> Lines;

        private static void CalcGraph(Source source)
        {
            int MaxCost = 0;
            Lines = new List<LineConnect>();
            foreach ( Device D2 in Device.D2 )
            {
                MaxCost = D2.Lines.Max(cost => cost.Cost);
                Lines.Add(D2.Lines.Where(s => s.Cost == MaxCost).First());
            }
        }
        public static void PaintingLine(Source source)
        {
            CalcGraph(source);
            Parallel.For(0, Lines.Count, ( i ) => Lines[i].ColorConnection = Brushes.Red);
        }
    }
}
