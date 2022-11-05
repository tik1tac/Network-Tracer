using Network_Tracer.View;

using System.Collections;
using System.Windows.Controls;

namespace Network_Tracer.Model.Graph
{
    public abstract class Nodes : Device
    {
        public Nodes(Canvas canvas) : base(canvas)
        {
        }

        public virtual Port port { get; set; }
        public virtual int Weight { get; set; }

        public virtual int NumberPorts { get; set; }
        public virtual int FreePorts { get; set; }

        public virtual StatePort StatePort { get; set; }

    }
}
