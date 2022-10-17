using Network_Tracer.View;

using System.Windows.Controls;

namespace Network_Tracer.Model.Graph.AbstractGraph
{
    public abstract class CreatorLine
    {
        public abstract LineConnect LineCreate( Canvas canvas );
    }
}
