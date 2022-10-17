using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network_Tracer.Model.Graph
{
    internal class CalculateGraph
    {
        Device Graph { get; set; }
        public CalculateGraph( Device graph ) => Graph = graph;

        public Device CalcGraph()
        {
            var graph = Device.GetGraph();

            //graph.


            return null;
        }
    }
}
