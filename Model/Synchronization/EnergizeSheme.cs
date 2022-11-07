using Network_Tracer.View;

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Network_Tracer.Model.Graph
{
    internal class EnergizeSheme
    {
        static List<Device> State;
        static Device StartState;
        static private List<Device> Neighbo;
        public static bool IsEnergy = false;
        public static List<Device> VZGStart;
        static Device ExcludedDev;
        static bool IsAdd = false;
        private static List<Device> NextNeighbo;
        static int count;
        /// <summary>
        /// Запитать схему от source
        /// </summary>
        /// <param name="source"></param>
        public static void Energize(Source source)
        {
            Neighbo = new List<Device>();
            State = new List<Device>();
            VZGStart = new List<Device>();
            NextNeighbo = new List<Device>();
            count = Device.Vertex.Where(n => n.Lines.Count != 0).Count();

            if (Device._lines.Count != 0)
            {
                switch (source)
                {
                    case Source.Peg:
                        if (Device.pegcount != null)
                        {
                            CleanUp();
                            BrushLineIfSourcePEGorPEGsp(Source.Peg);
                        }
                        else
                        {
                            MessageBox.Show("На схеме нет ПЭГ");
                            Device.Window.IsEnabledT();
                        }
                        break;
                    case Source.Vzg:
                        CleanUp();
                        BrushLineIfSourceVZG();
                        if (Device.pegcount != null || Device.pegsparecount != null)
                        {
                            ExcludedDev = Device.Vertex.Where(vert => vert.Number == 1).First();
                            ExcludedDev.RectBorder = Brushes.Silver;
                            ExcludedDev.Lines[0].ColorConnection = Brushes.Black;
                            ExcludedDev.Lines[0].ArrowToLine();
                            ExcludedDev = Device.Vertex.Where(vert => vert.Number == 2).First();
                            ExcludedDev.RectBorder = Brushes.Silver;
                            ExcludedDev.Lines[0].ColorConnection = Brushes.Black;
                            ExcludedDev.Lines[0].ArrowToLine();
                        }
                        else
                        {
                            MessageBox.Show("На схеме нет ПЭГ или ПЭГ рез.");
                            Device.Window.IsEnabledT();
                        }
                        break;
                    case Source.PegSpare:
                        if (Device.pegsparecount != null)
                        {
                            CleanUp();
                            BrushLineIfSourcePEGorPEGsp(Source.PegSpare);
                            ExcludedDev = Device.Vertex.Where(vert => vert.Number == 1).First();
                            ExcludedDev.RectBorder = Brushes.Silver;
                            ExcludedDev.Lines[0].ColorConnection = Brushes.Black;
                            ExcludedDev.Lines[0].ArrowToLine();
                        }
                        else
                        {
                            MessageBox.Show("На схеме нет ПЭГ рез.");
                            Device.Window.IsEnabledT();
                        }
                        break;
                    case Source.GSE:
                        CleanUp();
                        BrushLineIfSourceGSE();
                        break;
                    default:
                        break;
                }
            }

        }
        /// <summary>
        /// Очистить ресурсы
        /// </summary>
        private static void CleanUp()
        {
            VZGStart.Clear();
            Neighbo.Clear();
            NextNeighbo.Clear();
            State.Clear();
        }
        /// <summary>
        /// Запитать схему от СЭ
        /// </summary>
        private static void BrushLineIfSourceGSE()
        {
            for (int i = 0; i < Device.Vertex.Count; i++)
            {
                if (Device.Vertex[i].GetType() != typeof(PEG)
                    & Device.Vertex[i].GetType() != typeof(PEGSpare)
                    & Device.Vertex[i].GetType() != typeof(VZG))
                {
                    Device.Vertex[i].RectBorder = Brushes.Green;
                    Device.Vertex[i].InputElements.GSE.Background = Brushes.Green;
                }
            }
        }/// <summary>
         /// Запитать схему от ВЗГ
         /// </summary>
        private static void BrushLineIfSourceVZG()
        {
            if (VZGStart.Count != 0)
            {
                for (int i = 0; i < Device.Vertex.Count; i++)
                {
                    if (Device.Vertex[i].GetType() == typeof(VZG))
                    {
                        VZGStart.Add(Device.Vertex[i]);
                    }
                }
                count -= VZGStart.Count;
                foreach (var item in VZGStart)
                {
                    foreach (var elem in item.port.BlockOpen)
                    {
                        if (elem.Value == StatePort.Blocked)
                        {
                            item.port.InOrOutPortDict[elem.Key] = Enums.InOrOutPort.Out;
                        }
                    }
                }
                string Port;
                if (!VZGStart[0].PowerSuuply)
                {
                    for (int iter = 0; iter < VZGStart.Count; iter++)
                    {
                        VZGStart[(int)iter].ISVisited = true;
                        VZGStart[(int)iter].PowerSuuply = true;
                        IsAdd = false;
                        VZGStart[(int)iter].RectBorder = Brushes.Yellow;

                        for (int n = 0; n < VZGStart[iter].Lines.Count; n++)
                        {
                            if (!VZGStart[iter]._neighbours[n].PowerSuuply)
                            {
                                VZGStart[iter]._neighbours[n].PowerSuuply = true;
                                VZGStart[(int)iter].Lines
                                .Where(u => u.D2.LabelName == VZGStart[(int)iter]._neighbours[n].LabelName || u.D1.LabelName == VZGStart[(int)iter]._neighbours[n].LabelName)
                                .First().ColorConnection = Brushes.Yellow;
                                VZGStart[(int)iter].Lines[n].LineToArrow(VZGStart[(int)iter]);
                                VZGStart[(int)iter]._neighbours[n].RectBorder = Brushes.Yellow;
                                VZGStart[iter].Lines[n].IsArrow = true;
                                Port = VZGStart[iter]._neighbours[n].port.PortLine.Where(k => k.Key == VZGStart[iter].Lines[n]).First().Value;
                                VZGStart[iter]._neighbours[n].port.InOrOutPortDict[Port] = Enums.InOrOutPort.InEnerg;
                                foreach (var item in VZGStart[iter]._neighbours[n].port.BlockOpen)
                                {
                                    if (item.Value == StatePort.Blocked & item.Key != Port)
                                    {
                                        VZGStart[iter]._neighbours[n].port.InOrOutPortDict[item.Key] = Enums.InOrOutPort.Out;
                                    }
                                }
                                Neighbo.Clear();
                                if (!IsAdd)
                                {
                                    for (int iterator = 0; iterator < VZGStart[(int)iter].Lines.Count; iterator++)
                                    {
                                        if (VZGStart[(int)iter]._neighbours[iterator]._neighbours.Count > 1
                                            & VZGStart[(int)iter]._neighbours[iterator].ISVisited == false)
                                        {
                                            State.Add(VZGStart[(int)iter]._neighbours[iterator]);
                                        }
                                    }
                                }
                                IsAdd = true;
                                count--;
                            }
                        }
                    }
                }

                int st = 0;
                if (State.Count != 0)
                {
                    while (count != 0)
                    {
                        if (count == 0)
                        {
                            IsEnergy = true;
                            Neighbo.Clear();
                            break;
                        }
                        State[st].RectBorder = Brushes.Yellow;
                        State[st].ISVisited = true;
                        State[st].PowerSuuply = true;

                        for (int i = 0; i < State[st]._neighbours.Count; i++)
                        {
                            if (!State[st]._neighbours[i].PowerSuuply & !State[st]._neighbours[i].ISVisited)
                            {
                                State[st]._neighbours[i].PowerSuuply = true;
                                State[st].Lines
                                .Where(u => u.D2.LabelName == State[st]._neighbours[i].LabelName || u.D1.LabelName == State[st]._neighbours[i].LabelName)
                                .First().ColorConnection = Brushes.Yellow;
                                State[st].Lines[i].LineToArrow(State[st]);
                                State[st].Lines[i].IsArrow = true;
                                State[st]._neighbours[i].RectBorder = Brushes.Yellow;

                                Port = State[st]._neighbours[i].port.PortLine.Where(k => k.Key == State[st].Lines[i]).First().Value;
                                State[st]._neighbours[i].port.InOrOutPortDict[Port] = Enums.InOrOutPort.InEnerg;
                                foreach (var item in State[st]._neighbours[i].port.BlockOpen)
                                {
                                    if (item.Value == StatePort.Blocked & item.Key != Port)
                                    {
                                        State[st]._neighbours[i].port.InOrOutPortDict[item.Key] = Enums.InOrOutPort.Out;
                                    }
                                }
                                count--;
                            }
                        }
                        Neighbo.Clear();
                        for (int iterator = 0; iterator < State[st].Lines.Count; iterator++)
                        {
                            if (State[st]._neighbours[iterator]._neighbours.Count > 1
        & State[st]._neighbours[iterator].GetType() != typeof(VZG)
        & State[st]._neighbours[iterator].ISVisited == false)
                            {
                                Neighbo.Add(State[st]._neighbours[iterator]);
                            }
                        }
                        if (Neighbo.Count == 0)
                        {
                            Neighbo.Clear();
                            st++;

                            continue;
                        }
                        int pos = 1;
                        NextNeighbo.Clear();
                        NextNeighbo.Add(State[st]);
                        NextNeighbo.Add(Neighbo.First());
                        while (true)
                        {
                            NextNeighbo[pos].ISVisited = true;
                            for (int i = 0; i < NextNeighbo[pos]._neighbours.Count; i++)
                            {
                                if (!NextNeighbo[pos]._neighbours[i].PowerSuuply)
                                {
                                    NextNeighbo[pos]._neighbours[i].PowerSuuply = true;
                                    NextNeighbo[pos].Lines
        .Where(n => n.D2.LabelName == NextNeighbo[pos]._neighbours[i].LabelName || n.D1.LabelName == NextNeighbo[pos]._neighbours[i].LabelName)
        .First().ColorConnection = Brushes.Yellow;
                                    NextNeighbo[pos]._neighbours[i].RectBorder = Brushes.Yellow;

                                    NextNeighbo[pos]._neighbours[i].PowerSuuply = true;
                                    NextNeighbo[pos].Lines[i].LineToArrow(NextNeighbo[pos]);
                                    NextNeighbo[pos].Lines[i].IsArrow = true;
                                    NextNeighbo[pos]._neighbours[i].RectBorder = Brushes.Yellow;

                                    Port = NextNeighbo[pos]._neighbours[i].port.PortLine.Where(k => k.Key == NextNeighbo[pos].Lines[i]).First().Value;
                                    NextNeighbo[pos]._neighbours[i].port.InOrOutPortDict[Port] = Enums.InOrOutPort.InEnerg;
                                    foreach (var item in NextNeighbo[pos]._neighbours[i].port.BlockOpen)
                                    {
                                        if (item.Value == StatePort.Blocked & item.Key != Port)
                                        {
                                            NextNeighbo[pos]._neighbours[i].port.InOrOutPortDict[item.Key] = Enums.InOrOutPort.Out;
                                        }
                                    }
                                    count--;
                                }
                            }
                            int visit = 0;
                            for (int i = 0; i < State[st]._neighbours.Count; i++)
                            {
                                if (!State[st]._neighbours[i].ISVisited)
                                {
                                    visit++;
                                }
                            }
                            if (count == 0 || (pos == 0 & visit == 0))
                            {
                                st++;
                                Neighbo.Clear();
                                break;
                            }
                            Neighbo.Clear();
                            for (int i = 0; i < NextNeighbo[pos].Lines.Count; i++)
                            {
                                if (NextNeighbo[pos]._neighbours[i]._neighbours.Count > 1
    & NextNeighbo[pos]._neighbours[i].GetType() != typeof(VZG)
    & NextNeighbo[pos]._neighbours[i].ISVisited == false)
                                {
                                    Neighbo.Add(NextNeighbo[pos]._neighbours[i]);
                                }
                            }
                            if (Neighbo.Count == 0)
                            {
                                pos--;
                            }
                            else
                            {
                                NextNeighbo.Add(Neighbo.First());
                                pos++;
                                pos = NextNeighbo.Count - 1;
                            }
                        }
                        if (count == 0)
                        {
                            Neighbo.Clear();
                            IsEnergy = true;
                            break;
                        }
                    }

                }
            }
            else
            {
                Device.Window.IsEnabledT();
                MessageBox.Show("На схеме нет ни одного ВЗГ");
            }
        }
        /// <summary>
        /// Запитать схему от ПЭГ или ПЭГ рез.
        /// </summary>
        /// <param name="source"></param>
        private static void BrushLineIfSourcePEGorPEGsp(Source source)
        {

            if (source == Source.Peg)
            {
                StartState = Device.Vertex.Where(vert => vert.Number == 1).First();
            }
            else
            {
                StartState = Device.Vertex.Where(vert => vert.Number == 2).First();
            }
            State.Add(StartState);
            int i = 0;
            count--;
            string Port;
            //Для всего графа
            while (true)
            {
                State[i].ISVisited = true;
                State[i].PowerSuuply = true;

                if (source == Source.Peg)
                {
                    State[i].RectBorder = Brushes.Red;
                }
                else
                {
                    State[i].RectBorder = Brushes.Blue;
                }
                Port = State[0]._neighbours[0].port.PortLine.Where(k => k.Key == State[0].Lines[0]).First().Value;
                State[0]._neighbours[0].port.InOrOutPortDict[Port] = Enums.InOrOutPort.InEnerg;
                foreach (var item in State[0]._neighbours[0].port.BlockOpen)
                {
                    if (item.Value == StatePort.Blocked & item.Key != Port)
                    {
                        State[0]._neighbours[0].port.InOrOutPortDict[item.Key] = Enums.InOrOutPort.Out;
                    }
                }
                for (int iter = 0; iter < State[i]._neighbours.Count; iter++)
                {
                    if (!State[i]._neighbours[iter].PowerSuuply)
                    {
                        if (source == Source.Peg)
                        {
                            State[i].Lines
    .Where(n => n.D2.LabelName == State[i]._neighbours[iter].LabelName || n.D1.LabelName == State[i]._neighbours[iter].LabelName)
    .First().ColorConnection = Brushes.Red;
                            State[i]._neighbours[iter].RectBorder = Brushes.Red;
                        }
                        else
                        {
                            State[i].Lines
.Where(n => n.D2.LabelName == State[i]._neighbours[iter].LabelName || n.D1.LabelName == State[i]._neighbours[iter].LabelName)
.First().ColorConnection = Brushes.Blue;
                            State[i]._neighbours[iter].RectBorder = Brushes.Blue;
                        }
                        State[i]._neighbours[iter].PowerSuuply = true;
                        State[i].Lines[iter].LineToArrow(State[i]);
                        State[i].Lines[iter].IsArrow = true;
                        Port = State[i]._neighbours[iter].port.PortLine.Where(k => k.Key == State[i].Lines[iter]).First().Value;
                        State[i]._neighbours[iter].port.InOrOutPortDict[Port] = Enums.InOrOutPort.InEnerg;
                        foreach (var item in State[i]._neighbours[iter].port.BlockOpen)
                        {
                            if (item.Value == StatePort.Blocked & item.Key != Port)
                            {
                                State[i]._neighbours[iter].port.InOrOutPortDict[item.Key] = Enums.InOrOutPort.Out;
                            }
                        }
                        count--;
                    }
                }
                if (count == 0)
                {
                    IsEnergy = true;
                    break;
                }
                Neighbo.Clear();
                for (int iter = 0; iter < State[i].Lines.Count; iter++)
                {
                    if (State[i]._neighbours[iter]._neighbours.Count > 1
                        & !State[i]._neighbours[iter].ISVisited)
                    {
                        Neighbo.Add(State[i]._neighbours[iter]);
                    }
                }

                if (Neighbo.Count == 0)
                {
                    i--;
                }
                else
                {
                    State.Add(Neighbo.First());
                    i++;
                    i = State.Count - 1;
                }

            }
        }
    }
}
