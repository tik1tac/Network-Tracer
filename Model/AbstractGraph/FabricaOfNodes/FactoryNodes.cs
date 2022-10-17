using Network_Tracer.View;

using System.Windows.Controls;

namespace Network_Tracer.Model.Graph.AbstractGraph
{
    public class FactoryNodes : CreatorNodes
    {
        public override PEG CreatePeg( Canvas canvas )
        {
            return new PEG(canvas);
        }

        public override VZG CreateVZG( Canvas canvas )
        {
            return new VZG(canvas);
        }

        public override SE CreateSe( Canvas canvas )
        {
            return new SE(canvas);
        }
    }
}
