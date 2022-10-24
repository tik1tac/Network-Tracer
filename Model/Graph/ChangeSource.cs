using Network_Tracer.View;

using System.Collections.Generic;
using System.Linq;

namespace Network_Tracer.Model.Graph
{
    public class ChangeSource
    {
        public static List<LineConnect> GetLines( List<LineConnect> LinesInput, Source source )
        {
            switch ( source )
            {
                case Source.Peg:
                    LinesInput.Remove(LinesInput.Where(c => c.Cost == 7).First());
                    return LinesInput;
                    
                case Source.Vzg:
                    LinesInput.Remove(LinesInput.Where(c => c.Cost == 10).First());
                    LinesInput.Remove(LinesInput.Where(c => c.Cost == 5).First());
                    return LinesInput;

                case Source.PegSpare:
                    LinesInput.Remove(LinesInput.Where(c => c.Cost == 10).First());
                    return LinesInput;

                case Source.GSE:
                    LinesInput.Remove(LinesInput.Where(c => c.Cost == 10).First());
                    LinesInput.Remove(LinesInput.Where(c => c.Cost == 5).First());
                    return LinesInput;

                default:
                    return LinesInput;
            }

            return LinesInput;
        }
    }
}
