using Network_Tracer.View;

using System.Windows.Controls;

namespace Network_Tracer.Model.Graph.AbstractGraph
{
    public abstract class CreatorNodes
    {
        public abstract PEG CreatePeg(Canvas canvas);
        public abstract SE CreateSe(Canvas canvas);
        public abstract VZG CreateVZG(Canvas canvas);
        public abstract PEGSpare CreatePegSpare(Canvas canvas);
        public abstract User CreateUser(Canvas canvas);
    }
}
