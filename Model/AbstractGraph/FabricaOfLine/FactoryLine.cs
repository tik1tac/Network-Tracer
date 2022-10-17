using Network_Tracer.View;

using System.Windows.Controls;

namespace Network_Tracer.Model.Graph.AbstractGraph
{
    public class FactoryLine : CreatorLine
    {
        public override LineConnect LineCreate( Canvas canvas)
        {
            return new LineConnect( canvas);
        }
    }
}
