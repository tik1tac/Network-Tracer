using Network_Tracer.Model.Graph;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

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
            foreach (var item in canvas.Children)
            {
                using (StreamWriter streamWriter = File.CreateText(filename))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    serializer.Serialize(streamWriter, item);
                    //streamWriter.WriteLine(JsonUtility);
                    //string jsonfile= JsonConvert.SerializeObject(item, Formatting.None,
                    //    new JsonSerializerSettings()
                    //    {
                    //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    //    });
                    //streamWriter.Write(jsonfile);
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

        //public static void WriteSchemeToFile(string filename, Canvas canvas)
        //{
        //    XmlWriterSettings settings = new XmlWriterSettings();
        //    settings.Indent = true;
        //    settings.IndentChars = "\t";

        //    using (XmlWriter writer = XmlWriter.Create(filename, settings))
        //    {
        //        writer.WriteStartElement("Scheme");

        //        foreach (var item in canvas.Children)
        //        {
        //            SE se = item as SE;
        //            if (se != null)
        //            {
        //                writer.WriteStartElement("SE");
        //                writer.WriteAttributeString("HashCode", null, se.GetHashCode().ToString());
        //                writer.WriteAttributeString("Position", null, Canvas.GetLeft(se).ToString() + "," + Canvas.GetTop(se).ToString());
        //                writer.WriteAttributeString("Label", null, se.LabelName);
        //                writer.WriteAttributeString("Занятые порты", null, se.port.BlockOpen.ToString());
        //                writer.WriteEndElement();
        //                continue;
        //            }
        //            VZG vzg = item as VZG;
        //            if (se != null)
        //            {
        //                writer.WriteStartElement("VZG");
        //                writer.WriteAttributeString("HashCode", null, vzg.GetHashCode().ToString());
        //                writer.WriteAttributeString("Position", null, Canvas.GetLeft(vzg).ToString() + "," + Canvas.GetTop(vzg).ToString());
        //                writer.WriteAttributeString("Label", null, vzg.LabelName);
        //                writer.WriteAttributeString("Занятые порты",null,vzg.port.BlockOpen.ToString());
        //                writer.WriteEndElement();
        //                continue;
        //            }

        //            NodesWithoutPort node = item as NodesWithoutPort;
        //            if (node != null)
        //            {
        //                writer.WriteStartElement(item is PEG ? "PEG" : "PEGSpare");
        //                writer.WriteAttributeString("HashCode", null, node.GetHashCode().ToString());
        //                writer.WriteAttributeString("Position", null, Canvas.GetLeft(node).ToString() + "," + Canvas.GetTop(node).ToString());
        //                writer.WriteAttributeString("city", null, node.city);
        //                writer.WriteEndElement();
        //                continue;
        //            }

        //            LineConnect line = item as LineConnect;
        //            if (line != null)
        //            {
        //                writer.WriteStartElement("LineConnect");
        //                writer.WriteAttributeString("Source", null, line.D1.GetHashCode().ToString());
        //                writer.WriteAttributeString("Destination", null, line.D2.GetHashCode().ToString());
        //                writer.WriteEndElement();
        //                continue;
        //            }
        //        }

        //        writer.WriteEndElement();
        //    }
        //}

        //private static void SetPosition(Device dev, string value)
        //{
        //    string[] xy = value.Split(',');
        //    double left, top;

        //    if (xy.Length == 2 && double.TryParse(xy[0], out left) && double.TryParse(xy[1], out top))
        //    {
        //        Canvas.SetLeft(dev, left);
        //        Canvas.SetTop(dev, top);
        //    }
        //    else
        //    {
        //        throw new FormatException("Неправильный атрибут" + " Position=\"" + value + "\"");
        //    }
        //}

        //private static Device GetDevice(Dictionary<string, Device> dictionary, string hashcode)
        //{
        //    Device device;
        //    if (dictionary.TryGetValue(hashcode, out device))
        //    {
        //        return device;
        //    }

        //    throw new FormatException("Неизвестное устройство" + " \"" + hashcode + "\"");
        //}
    }
}
