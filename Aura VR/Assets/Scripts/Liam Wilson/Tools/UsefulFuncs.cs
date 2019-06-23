using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;
using UnityEngine;

public static class UsefulFuncs
{
    public static string NeatTime(float seconds)
    {
        return NeatTime((int)seconds);
    }

    public static string NeatTime(int seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);

        if (seconds < 3600) return time.ToString(@"mm\:ss");
        else return time.ToString(@"hh\:mm\:ss");
    }

    public static double CosineSimilarity(Vector3 V1, Vector3 V2)
    {
        return ((V1.x * V2.x) + (V1.y * V2.y) + (V1.z * V2.z))
               / (Math.Sqrt(Math.Pow(V1.x, 2) + Math.Pow(V1.y, 2) + Math.Pow(V1.z, 2))
                  * Math.Sqrt(Math.Pow(V2.x, 2) + Math.Pow(V2.y, 2) + Math.Pow(V2.z, 2)));
    }
}

namespace AuraHull.AuraVRGame
{
    public static class Xml
    {
        public class Node
        {
            private string _name;
            private string _data;
            private List<Node> _childNodes;

            public string Name
            {
                get { return _name; }
            }

            public string Data
            {
                get { return _data; }
            }

            public ref List<Node> ChildNodes
            {
                get { return ref _childNodes; }
            }

            public Node(string name)
            {
                _name = name;
                _data = null;
                _childNodes = new List<Node>();
            }

            public void AddChild(string name)
            {
                _childNodes.Add(new Node(name));
            }

            public void AddChild<T>(string name, T data)
            {
                AddChild(name);
                _childNodes.Last().SetData(data);
            }

            public void AddChild(string name, out Node newNode)
            {
                AddChild(name);
                newNode = _childNodes.Last();
            }

            public void AddChild<T>(string name, T data, out Node newNode)
            {
                AddChild(name);
                _childNodes.Last().SetData(data);
                newNode = _childNodes.Last();
            }

            public void SetData<T>(T data)
            {
                _data = data.ToString();
            }
        }

        public static XmlNodeList[] Read(string filePath, params string[] nodeTagNames)
        {
            List<XmlNodeList> list = new List<XmlNodeList>();

            try
            {
                if (File.Exists(filePath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);

                    for (int i = 0; i < nodeTagNames.Length; i++)
                    {
                        XmlNodeList elementList = doc.GetElementsByTagName(nodeTagNames[i]);
                        if (elementList != null) list.Add(elementList);
                        else Console.WriteLine($"XML tag not found, skipping. ({nodeTagNames[i]})");
                    }
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed to read XML file. ({filePath})\n{e}"); }

            return list.ToArray();
        }

        public static bool Write(string filePath, Node rootNode)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    string newFolder = Path.GetDirectoryName(filePath);
                    Directory.CreateDirectory(newFolder);
                    Console.WriteLine($"New directory created. ({filePath})");
                }

                XmlDocument doc = new XmlDocument();
                XmlNode rootNodeEl = doc.CreateElement(rootNode.Name);
                doc.AppendChild(rootNodeEl);

                foreach (Node child in rootNode.ChildNodes)
                {
                    XmlNode childNodeEl = doc.CreateElement(child.Name);

                    foreach (Node subChild in child.ChildNodes)
                    {
                        XmlNode subChildEl = doc.CreateElement(subChild.Name);
                        subChildEl.InnerText = subChild.Data;
                        childNodeEl.AppendChild(subChildEl);
                    }

                    rootNodeEl.AppendChild(childNodeEl);
                }

                doc.Save(filePath);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to write to XML file. ({filePath})\n{e}");
                return false;
            }
        }
    }
}