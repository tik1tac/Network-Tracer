using Network_Tracer.Model.Graph;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network_Tracer.Model
{
    public class Scheme
    {
        public static readonly HashSet<string> Labelsname = new HashSet<string>();

        public static string GenerateName( string baseword )
        {
            string namelabel = baseword;
            if ( Labelsname.Contains(namelabel) )
            {
                for ( int i = 1 ; i < 10000 ; ++i )
                {
                    string suffix = i.ToString();

                    if ( !Labelsname.Contains(namelabel + suffix) )
                    {
                        namelabel += suffix;
                        break;
                    }
                }
            }
            Labelsname.Add(namelabel);
            return namelabel;
        }
        public static void NewList()
        {
            Device.Vertex.Clear();
            Scheme.Labelsname.Clear();

        }
    }
}
