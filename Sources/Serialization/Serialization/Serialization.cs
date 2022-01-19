using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

using System.Xml.Serialization;

namespace Serialization
{
    public class Serialization
    {
        public static void Serialize(IEnumerable<Node> nodes, string filename, FormatType format)
        {
            if (format == FormatType.Json)
            {
                string file = filename;
                string json = JsonConvert.SerializeObject(nodes);
                File.WriteAllText(file, json);
            }
            else if (format == FormatType.Xml)
            {
                string file = filename ;
                List<Node> rew = nodes.ToList();
                XmlSerializer formatter = new XmlSerializer(typeof(List<Node>));
                using (FileStream fs = new FileStream(file, FileMode.Create))
                {
                    formatter.Serialize(fs, rew);
                }
            }
            else if (format == FormatType.Bin)
            {
                string file = filename ;
                List<Node> resList = nodes.ToList();
                using (Stream stream = File.Open(file, FileMode.Create))
                {
                    BinaryFormatter bfWrite = new BinaryFormatter();
                    bfWrite.Serialize(stream, resList);
                    stream.Close();
                }
            }
            else
            {
                throw new Exception();
            }
        }

        public static List<Node> Deserialize(string filename, FormatType format)
        {
            if (format == FormatType.Json)
            {
                
                FileInfo f = new FileInfo(filename);
                string fullName = f.FullName;
                string jsonString = File.ReadAllText(fullName);
                List<Node> resList = JsonConvert.DeserializeObject<List<Node>>(jsonString);
                return resList;
            }
            else if (format == FormatType.Xml)
            {
                XmlSerializer formatter = new XmlSerializer(typeof(List<Node>));
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    List<Node> resList = (List<Node>)formatter.Deserialize(fs);
                    return resList;
                }
            }
            else if (format == FormatType.Bin)
            {
                BinaryFormatter bfWrite = new BinaryFormatter();
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    List<Node> resList = (List<Node>)bfWrite.Deserialize(fs);
                    return resList;
                }
            }
            else 
            {
                throw new Exception();
            }
        }
    }
}
