using Network_Tracer.Model.Graph;
using Network_Tracer.Model.Graph.AbstractGraph;
using Network_Tracer.View;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace Network_Tracer.Model
{
    public class Scheme
    {
        public static readonly HashSet<string> Labelsname = new HashSet<string>();

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
            Dictionary<Device, string> DeviceLabelName = new Dictionary<Device, string>();
            string source = null;
            string destination = null;
            Dictionary<Device, Dictionary<string, string>> DevicePort = new Dictionary<Device, Dictionary<string, string>>();
            Dictionary<LineConnect, List<string>> LineDevice = new Dictionary<LineConnect, List<string>>();
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
                                Device.Vertex.Add(NodeNoPort);
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
                                            DeviceLabelName.Add(NodeNoPort, reader.Value);
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
                                canvas.Children.Add(NodeNoPort);
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
                            Device.Vertex.Add(NodeWithPort);
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
                                        break;
                                }
                                if (!reader.Value.Contains("-") & !DeviceLabelName.ContainsKey(NodeWithPort))
                                {
                                    DeviceLabelName.Add(NodeWithPort, reader.Value);
                                }
                                else if(!DeviceLabelName.ContainsKey(NodeWithPort))
                                {
                                    DevicePort.Add(NodeWithPort, new Dictionary<string, string>() { [reader.Name] = reader.Value });
                                }
                            }
                            canvas.Children.Add(NodeWithPort);
                            break;
                        case "LineConnect":
                            LineConnect line = creatorLine.LineCreate(canvas);
                            while (reader.MoveToNextAttribute())
                            {
                                switch (reader.Name)
                                {
                                    case "NameLine":
                                        line.NameLine = reader.Value;
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
                            break;
                        default:
                            break;
                    }

                }
            }
        }
    }
}
//public static void LoadScheme(
//    string filename,
//    Canvas canvas,
//    MouseButtonEventHandler onVZGMouseDown,
//    MouseButtonEventHandler onSEMouseDown,
//    MouseButtonEventHandler onNodeMouseDown,
//    MouseButtonEventHandler onLineMouseDown)
//{
//    Device[] pair = new Device[2];

//    Dictionary<string, Device> devices = new Dictionary<string, Device>();
//    string srchash, desthash;
//    int value;
//    bool flag;
//    CreatorNodes cn = new FactoryNodes();

//    using (XmlReader reader = XmlReader.Create(filename, new XmlReaderSettings()))
//    {
//        while (reader.Read())
//        {
//            if (reader.IsStartElement())
//            {
//                switch (reader.Name)
//                {
//                    case "SE":
//                        SE se = cn.CreateSe(canvas);
//                        Canvas.SetTop(se, 0);
//                        Canvas.SetLeft(se, 0);

//                        while (reader.MoveToNextAttribute())
//                        {
//                            switch (reader.Name)
//                            {
//                                case "HashCode":
//                                    devices[reader.Value] = se;
//                                    break;

//                                case "Position":
//                                    SetPosition(se, reader.Value);
//                                    break;
//                            }
//                        }

//                        if (onSEMouseDown != null)
//                        {
//                            se.MouseLeftButtonDown += onSEMouseDown;
//                        }

//                        canvas.Children.Add(se);
//                        break;

//                    case "PEG":
//                    case "PEGSpare":
//                    case "User":
//                        NodesWithoutPort node=null;

//                        if (reader.Name == "PC")
//                        {
//                            node = cn.CreatePeg(canvas);
//                        }
//                        if (reader.Name == "PEGSpare")
//                        {
//                            node = cn.CreatePegSpare(canvas);
//                        }
//                        if (reader.Name == "User")
//                        {
//                            node = cn.CreateUser(canvas);
//                        }

//                        Canvas.SetTop(node, 0);
//                        Canvas.SetLeft(node, 0);

//                        while (reader.MoveToNextAttribute())
//                        {
//                            switch (reader.Name)
//                            {
//                                case "HashCode":
//                                    devices[reader.Value] = node;
//                                    break;

//                                case "Position":
//                                    SetPosition(node, reader.Value);
//                                    break;

//                                case "Label":
//                                    node.LabelName = reader.Value;
//                                    break;
//                            }
//                        }

//                        if (onNodeMouseDown != null)
//                        {
//                            node.MouseLeftButtonDown += onNodeMouseDown;
//                        }

//                        canvas.Children.Add(node);
//                        break;

//                    case "LineConnect":
//                        reader.MoveToAttribute("Source");
//                        pair[0] = GetDevice(devices, reader.Value);
//                        srchash = reader.Value;

//                        reader.MoveToAttribute("Destination");
//                        pair[1] = GetDevice(devices, reader.Value);
//                        desthash = reader.Value;

//                        LineConnect line = new LineConnect(canvas)
//                        {
//                            X1 = Canvas.GetLeft(pair[0]) + (pair[0].Width / 2),
//                            Y1 = Canvas.GetTop(pair[0]) + (pair[0].Height / 2),
//                            X2 = Canvas.GetLeft(pair[1]) + (pair[1].Width / 2),
//                            Y2 = Canvas.GetTop(pair[1]) + (pair[1].Height / 2),
//                            D1 = pair[0],
//                            D2 = pair[1]
//                        };

//                        if (onLineMouseDown != null)
//                        {
//                            line.MouseLeftButtonDown += onLineMouseDown;
//                        }

//                        Canvas.SetZIndex(line, -1);
//                        canvas.Children.Add(line);

//                        reader.MoveToAttribute("SourcePort");
//                        if (int.TryParse(reader.Value, out value))
//                        {
//                            if (value < 0)
//                            {
//                                node = pair[0] as NodesWithoutPort;

//                                if (node == null)
//                                {
//                                    throw new FormatException(
//                                        "Устройство с таким хеш-кодом" + " \"" + srchash + "\" " + "не устройство");
//                                }

//                                if (node.Line != null)
//                                {
//                                    node.Line.Remove();
//                                }

//                                node.Line = line;
//                            }
//                            else
//                            {
//                                se = pair[0] as SE;

//                                if (se == null)
//                                {
//                                    throw new FormatException(
//                                        "Устройство с таким хеш-кодом" + " \"" + srchash + "\" " + "не устройство");
//                                }

//                            }
//                        }
//                        else
//                        {
//                            throw new FormatException(
//                                "Устройство с таким хеш-кодом" + " \"" + srchash + "\" " + "не устройство");
//                        }

//                        reader.MoveToAttribute("DestinationPort");
//                        if (int.TryParse(reader.Value, out value))
//                        {
//                            if (value < 0)
//                            {
//                                node = pair[1] as NodesWithoutPort;

//                                if (node == null)
//                                {
//                                    throw new FormatException(
//                                        "Устройство с таким хеш-кодом" + " \"" + srchash + "\" " + "не устройство");
//                                }

//                                if (node.Line != null)
//                                {
//                                    node.Line.Remove();
//                                }

//                                node.Line = line;
//                            }
//                            else
//                            {
//                                se = pair[1] as SE;

//                                if (se == null)
//                                {
//                                    throw new FormatException(
//                                        "Устройство с таким хеш-кодом" + " \"" + srchash + "\" " + "не устройство");
//                                }
//                            }
//                        }
//                        else
//                        {
//                            throw new FormatException(
//                                        "Устройство с таким хеш-кодом" + " \"" + srchash + "\" " + "не устройство");
//                        }

//                        line.D2.UpdateLocation();
//                        break;
//                }
//            }
//        }
//    }
//}
