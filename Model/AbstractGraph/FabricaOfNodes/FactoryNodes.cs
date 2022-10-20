using Network_Tracer.View;

using System.Windows.Controls;

namespace Network_Tracer.Model.Graph.AbstractGraph
{
    public class FactoryNodes : CreatorNodes
    {
        public override PEG CreatePeg(Canvas canvas) => new PEG(canvas);

        public override VZG CreateVZG( Canvas canvas ) => new VZG(canvas);

        public override SE CreateSe( Canvas canvas ) => new SE(canvas);
    }
}
