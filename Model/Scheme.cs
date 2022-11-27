using Network_Tracer.Model.Graph;
using Network_Tracer.Model.Graph.AbstractGraph;
using Network_Tracer.View;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace Network_Tracer.Model
{
    public class Scheme
    {
        public static readonly HashSet<string> Labelsname = new HashSet<string>();
        static List<Device> dev = new List<Device>();
        public static string GenerateName(string baseword)
        {
            string namelabel = baseword;
            if (Labelsname.Contains(namelabel))
            {
                for (int i = 1; i < 10000; ++i)
                {
                    string suffix = i.ToString();

                    if (!Labelsname.Contains(namelabel + suffix))
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

        public static void WriteSchemeToFile(string filename, Canvas canvas)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                writer.WriteStartElement("Scheme");
                foreach (var item in canvas.Children)
                {

                    if (item is NodesWithoutPort)
                    {
                        PEG peg = item as PEG;
                        if (peg != null)
                        {
                            writer.WriteStartElement("PEG");
                            writer.WriteAttributeString("LabelName", peg.LabelName);
                            writer.WriteAttributeString("PositionLeft", Canvas.GetLeft(peg).ToString());
                            writer.WriteAttributeString("PositionTop", Canvas.GetTop(peg).ToString());
                            writer.WriteAttributeString("city", peg.city);
                            writer.WriteAttributeString("Neighbours", peg._neighbours[0].LabelName);
                            writer.WriteEndElement();
                            continue;
                        }
                        PEGSpare pegsp = item as PEGSpare;
                        if (pegsp != null)
                        {
                            writer.WriteStartElement("PEGSpare");
                            writer.WriteAttributeString("LabelName", pegsp.LabelName);
                            writer.WriteAttributeString("PositionLeft", Canvas.GetLeft(pegsp).ToString());
                            writer.WriteAttributeString("PositionTop", Canvas.GetTop(pegsp).ToString());
                            writer.WriteAttributeString("city", pegsp.city);
                            writer.WriteAttributeString("Neighbours", pegsp._neighbours[0].LabelName);
                            writer.WriteEndElement();
                            continue;
                        }
                        User user = item as User;
                        if (user != null)
                        {
                            writer.WriteStartElement("User");
                            writer.WriteAttributeString("LabelName", user.LabelName);
                            writer.WriteAttributeString("PositionLeft", Canvas.GetLeft(user).ToString());
                            writer.WriteAttributeString("PositionTop", Canvas.GetTop(user).ToString());
                            writer.WriteAttributeString("city", user.city);
                            writer.WriteAttributeString("Neighbours", user._neighbours[0].LabelName);
                            writer.WriteEndElement();
                            continue;
                        }
                    }
                    if (item is NodesWithPort)
                    {
                        SE se = item as SE;
                        if (se != null)
                        {
                            writer.WriteStartElement("SE");
                            writer.WriteAttributeString("LabelName", se.LabelName);
                            writer.WriteAttributeString("PositionLeft", Canvas.GetLeft(se).ToString());
                            writer.WriteAttributeString("PositionTop", Canvas.GetTop(se).ToString());
                            writer.WriteAttributeString("city", se.city);
                            for (int i = 0; i < se._neighbours.Count; i++)
                            {
                                writer.WriteAttributeString(se._neighbours[i].LabelName, se._neighbours[i].LabelName);
                            }
                            for (int i = 0; i < se.port.PortLine.Count; i++)
                            {
                                writer.WriteAttributeString(se.port.PortLine.Keys.ElementAt(i).NameLine, se.port.PortLine.Values.ElementAt(i));
                            }
                            writer.WriteEndElement();
                            continue;
                        }
                        VZG vzg = item as VZG;
                        if (vzg != null)
                        {
                            writer.WriteStartElement("VZG");
                            writer.WriteAttributeString("LabelName", vzg.LabelName);
                            writer.WriteAttributeString("PositionLeft", Canvas.GetLeft(vzg).ToString());
                            writer.WriteAttributeString("PositionTop", Canvas.GetTop(vzg).ToString());
                            writer.WriteAttributeString("city", vzg.city);
                            for (int i = 0; i < vzg._neighbours.Count; i++)
                            {
                                writer.WriteAttributeString(vzg._neighbours[i].LabelName, vzg._neighbours[i].LabelName);
                            }
                            for (int i = 0; i < vzg.port.PortLine.Count; i++)
                            {
                                writer.WriteAttributeString(vzg.port.PortLine.Keys.ElementAt(i).NameLine, vzg.port.PortLine.Values.ElementAt(i));
                            }
                            writer.WriteEndElement();
                            continue;
                        }
                    }
                    if (item is LineConnect)
                    {
                        LineConnect line = item as LineConnect;
                        writer.WriteStartElement("LineConnect");
                        writer.WriteAttributeString("NameLine", line.NameLine);
                        writer.WriteAttributeString("Cost", line.Cost.ToString());
                        writer.WriteAttributeString("D1", line.D1.LabelName);
                        writer.WriteAttributeString("D2", line.D2.LabelName);
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();
            }
        }
        public static void LoadScheme(Canvas canvas, string filename)
        {
            Dictionary<Device, List<string>> DeviceLabelName = new Dictionary<Device, List<string>>();
            string source = null;
            string destination = null;
            Dictionary<Device, Dictionary<string, string>> DevicePort = new Dictionary<Device, Dictionary<string, string>>();
            Dictionary<string, string> LinePortName = new Dictionary<string, string>();
            Dictionary<LineConnect, List<string>> LineDevice = new Dictionary<LineConnect, List<string>>();
            List<string> Neighbours = new List<string>();
            CreatorNodes CreatorNodes = new FactoryNodes();
            CreatorLine creatorLine = new FactoryLine();
            using (XmlReader reader = XmlReader.Create(filename, new XmlReaderSettings()))
            {
                while (reader.Read())
                {
                    switch (reader.Name)
                    {
                        case "PEG":
                        case "PEGSpare":
                        case "User":
                            {
                                NodesWithoutPort NodeNoPort = null;
                                if (reader.Name == "PEG")
                                {
                                    NodeNoPort = CreatorNodes.CreatePeg(canvas);
                                    Device.pegcount = NodeNoPort as PEG;
                                }
                                if (reader.Name == "PEGSpare")
                                {
                                    NodeNoPort = CreatorNodes.CreatePegSpare(canvas);
                                    Device.pegsparecount = NodeNoPort as PEGSpare;
                                }
                                if (reader.Name == "User")
                                {
                                    NodeNoPort = CreatorNodes.CreateUser(canvas);
                                }
                                while (reader.MoveToNextAttribute())
                                {
                                    switch (reader.Name)
                                    {
                                        case "LabelName":
                                            NodeNoPort.LabelName = reader.Value;
                                            break;
                                        case "PositionLeft":
                                            Canvas.SetLeft(NodeNoPort, Convert.ToDouble(reader.Value));
                                            break;
                                        case "PositionTop":
                                            Canvas.SetTop(NodeNoPort, Convert.ToDouble(reader.Value));
                                            break;
                                        case "city":
                                            NodeNoPort.city = reader.Value;
                                            break;
                                        case "Neighbours":
                                            DeviceLabelName.Add(NodeNoPort, new List<string>() { reader.Value });
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                if (NodeNoPort is PEG)
                                {
                                    NodeNoPort.MouseLeftButtonDown += Device.Window.OnPEGLeftButtonDown;
                                }
                                if (NodeNoPort is PEGSpare)
                                {
                                    NodeNoPort.MouseLeftButtonDown += Device.Window.OnPEGSpareLeftButtonDown;
                                }
                                Device.Vertex.Add(NodeNoPort);
                                break;
                            }
                        case "VZG":
                        case "SE":
                            NodesWithPort NodeWithPort = null;
                            if (reader.Name == "SE")
                            {
                                NodeWithPort = CreatorNodes.CreateSe(canvas);
                            }
                            if (reader.Name == "VZG")
                            {
                                NodeWithPort = CreatorNodes.CreateVZG(canvas);
                            }
                            Neighbours = new List<string>();
                            LinePortName = new Dictionary<string, string>();
                            while (reader.MoveToNextAttribute())
                            {
                                switch (reader.Name)
                                {
                                    case "LabelName":
                                        NodeWithPort.LabelName = reader.Value;
                                        continue;
                                    case "PositionLeft":
                                        Canvas.SetLeft(NodeWithPort, Convert.ToDouble(reader.Value));
                                        continue;
                                    case "PositionTop":
                                        Canvas.SetTop(NodeWithPort, Convert.ToDouble(reader.Value));
                                        continue;
                                    case "city":
                                        NodeWithPort.city = reader.Value;
                                        continue;
                                    default:
                                        if (!reader.Name.Contains("-"))
                                            Neighbours.Add(reader.Value);
                                        else
                                            LinePortName.Add(reader.Name, reader.Value);
                                        break;
                                }

                            }
                            DeviceLabelName.Add(NodeWithPort, Neighbours);
                            DevicePort.Add(NodeWithPort, LinePortName);
                            if (NodeWithPort is SE)
                            {
                                NodeWithPort.MouseLeftButtonDown += Device.Window.OnSELeftButtonDown;
                                NodeWithPort.MouseDoubleClick += Device.Window.Se_MouseDoubleClick;
                            }
                            if (NodeWithPort is VZG)
                            {
                                NodeWithPort.MouseLeftButtonDown += Device.Window.OnVZGLeftButtonDown;
                                NodeWithPort.MouseDoubleClick += Device.Window.Vzg_MouseDoubleClick;
                            }
                            Device.Vertex.Add(NodeWithPort);
                            break;
                        case "LineConnect":
                            source = null;
                            destination = null;
                            LineConnect line = creatorLine.LineCreate(canvas);
                            while (reader.MoveToNextAttribute())
                            {
                                switch (reader.Name)
                                {
                                    case "NameLine":
                                        line.NameLine = reader.Value;
                                        break;
                                    case "Cost":
                                        line.Cost = Convert.ToInt32(reader.Value);
                                        break;
                                    case "D1":
                                        source = reader.Value;
                                        break;
                                    case "D2":
                                        destination = reader.Value;
                                        break;
                                    default:
                                        break;
                                }
                                if (source != null & destination != null & !LineDevice.ContainsKey(line))
                                {
                                    LineDevice.Add(line, new List<string>() { source, destination });
                                }
                            }
                            Device._lines.Add(line);
                            break;
                        default:
                            break;
                    }

                }
                foreach (var item in DevicePort)
                {
                    canvas.Children.Add(MappingPortDevice(item.Key as NodesWithPort, item.Value));
                }
                foreach (var item in DeviceLabelName)
                {
                    if (!canvas.Children.Contains(item.Key))
                    {
                        canvas.Children.Add(MappingNeighbours(item.Value, item.Key));
                    }
                    else
                    {
                        canvas.Children.Remove(item.Key);
                        canvas.Children.Add(MappingNeighbours(item.Value, item.Key));
                    }
                }
                foreach (var item in LineDevice)
                {
                    canvas.Children.Add(MappingDeviceLine(item.Key, item.Value));
                }
            }
        }

        private static LineConnect MappingDeviceLine(LineConnect line, List<string> ports)
        {
            line.D1 = Device.Vertex.First(l => l.LabelName == ports[0]);
            line.D1.Lines.Add(line);
            line.D2 = Device.Vertex.First(l => l.LabelName == ports[1]);
            line.D2.Lines.Add(line);

            line.X1 = Canvas.GetLeft(line.D1) + (line.D1.Width / 2);
            line.Y1 = Canvas.GetTop(line.D1) + (line.D1.Height / 2);
            line.X2 = Canvas.GetLeft(line.D2) + (line.D2.Width / 2);
            line.Y2 = Canvas.GetTop(line.D2) + (line.D2.Height / 2);
            Canvas.SetZIndex(line, -1);

            line.D1.UpdateLocation();
            line.D2.UpdateLocation();

            if ((line.D1 is NodesWithPort & line.D2 is NodesWithPort))
            {
                line.Port1 = Device.Vertex.First(d => d.LabelName == ports[0]).NamePorts[line];
                line.Port2 = Device.Vertex.First(d => d.LabelName == ports[1]).NamePorts[line];
                for (int i = 0; i < (line.D1 as NodesWithPort).ports.Length; ++i)
                {
                    if ((line.D1 as NodesWithPort).ports[i] == null)
                    {
                        (line.D1 as NodesWithPort).ports[i] = line;
                        break;
                    }
                }
                for (int i = 0; i < (line.D2 as NodesWithPort).ports.Length; ++i)
                {
                    if ((line.D2 as NodesWithPort).ports[i] == null)
                    {
                        (line.D2 as NodesWithPort).ports[i] = line;
                        break;
                    }
                }
            }
            if (line.D1 is NodesWithPort & line.D2 is NodesWithoutPort)
            {
                line.Port1 = Device.Vertex.First(d => d.LabelName == ports[0]).NamePorts[line];
                for (int i = 0; i < (line.D1 as NodesWithPort).ports.Length; ++i)
                {
                    if ((line.D1 as NodesWithPort).ports[i] == null)
                    {
                        (line.D1 as NodesWithPort).ports[i] = line;
                        break;
                    }
                }
                (line.D2 as NodesWithoutPort).Line = line;
            }
            if (line.D2 is NodesWithPort & line.D1 is NodesWithoutPort)
            {
                line.Port2 = Device.Vertex.First(d => d.LabelName == ports[1]).NamePorts[line];
                for (int i = 0; i < (line.D2 as NodesWithPort).ports.Length; ++i)
                {
                    if ((line.D2 as NodesWithPort).ports[i] == null)
                    {
                        (line.D2 as NodesWithPort).ports[i] = line;
                        break;
                    }
                }
                (line.D1 as NodesWithoutPort).Line = line;
            }
            return line;
        }

        private static Device MappingPortDevice(NodesWithPort device, Dictionary<string, string> port)
        {
            NamePorts namePorts = NamePorts.S41;
            foreach (var item in port)
            {
                device.port.PortLine.Add(Device._lines.First(l => l.NameLine == item.Key), item.Value);
                device.port.IsConnected = true;
                foreach (var elem in device.port.grid.Children)
                {
                    if ((elem as Button).Name == item.Value)
                    {
                        (elem as Button).IsEnabled = false;
                        (elem as Button).Background = Brushes.Red;
                    }

                }
                switch (item.Value)
                {
                    case "S41":
                        namePorts = NamePorts.S41;
                        break;
                    case "T41":
                        namePorts = NamePorts.T41;
                        break;
                    case "S161":
                        namePorts = NamePorts.S161;
                        break;
                    case "T31":
                        namePorts = NamePorts.T31;
                        break;
                    case "S42":
                        namePorts = NamePorts.S42;
                        break;
                    case "T42":
                        namePorts = NamePorts.T42;
                        break;
                    case "S162":
                        namePorts = NamePorts.S162;
                        break;
                    case "T32":
                        namePorts = NamePorts.T32;
                        break;
                    default:
                        break;
                }
                device.NamePorts.Add(Device._lines.First(l => l.NameLine == item.Key), namePorts);
                device.port.BlockOpen[item.Key] = StatePort.Blocked;
            }
            return device;
        }

        private static Device MappingNeighbours(List<string> NameNeighbour, Device Node)
        {
            foreach (var item in NameNeighbour)
            {
                Node._neighbours.Add(Device.Vertex.Where(n => n.LabelName == item).First());
            }
            return Node;
        }
    }
}
