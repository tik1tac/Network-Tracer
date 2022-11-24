using Network_Tracer.Model.Graph;
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
                            writer.WriteAttributeString("Weight", Convert.ToString(peg.Weight));
                            writer.WriteAttributeString("city", peg.city);
                            writer.WriteStartElement("Neighbours");
                            writer.WriteElementString(peg._neighbours[0].LabelName, peg._neighbours[0].LabelName);
                            writer.WriteEndElement();
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
                            writer.WriteAttributeString("Weight", Convert.ToString(pegsp.Weight));
                            writer.WriteAttributeString("city", pegsp.city);
                            writer.WriteStartElement("Neighbours");
                            writer.WriteElementString(pegsp._neighbours[0].LabelName, pegsp._neighbours[0].LabelName);
                            writer.WriteEndElement();
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
                            writer.WriteAttributeString("Weight", Convert.ToString(user.Weight));
                            writer.WriteAttributeString("city", user.city);
                            writer.WriteStartElement("Neighbours");
                            writer.WriteElementString(user._neighbours[0].LabelName, user._neighbours[0].LabelName);
                            writer.WriteEndElement();
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
                            writer.WriteAttributeString("Weight", Convert.ToString(se.Weight));
                            writer.WriteAttributeString("city", se.city);
                            writer.WriteStartElement("Neighbours");
                            for (int i = 0; i < se._neighbours.Count; i++)
                            {
                                writer.WriteElementString(se._neighbours[i].LabelName, se._neighbours[i].LabelName);
                            }
                            writer.WriteEndElement();
                            writer.WriteStartElement("Port");
                            for (int i = 0; i < se.port.PortLine.Count; i++)
                            {
                                writer.WriteElementString(se.port.PortLine.Keys.ElementAt(i).NameLine, se.port.PortLine.Values.ElementAt(i));
                            }
                            writer.WriteEndElement();
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
                            writer.WriteAttributeString("Weight", Convert.ToString(vzg.Weight));
                            writer.WriteAttributeString("city", vzg.city);
                            writer.WriteStartElement("Neighbours");
                            for (int i = 0; i < vzg._neighbours.Count; i++)
                            {
                                writer.WriteElementString(vzg._neighbours[i].LabelName, vzg._neighbours[i].LabelName);
                            }
                            writer.WriteEndElement();
                            writer.WriteStartElement("Port");
                            for (int i = 0; i < vzg.port.PortLine.Count; i++)
                            {
                                writer.WriteElementString(vzg.port.PortLine.Keys.ElementAt(i).NameLine, vzg.port.PortLine.Values.ElementAt(i));
                            }
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            continue;
                        }
                    }
                }
                writer.WriteEndElement();
            }
        }

        public static void LoadShceme(Canvas canvas, string filename)
        {

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
