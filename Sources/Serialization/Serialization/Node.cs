using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Serialization
{
    [Serializable]
    public class Node
    {
        public string Name { get; set; }
        public int Deepth { get; set; }
        public int Type { get; set; }
        public DateTime Time { get; set; }
        public long Size { get; set; }

        public Node() { }
        public Node(string name, int deepth, int type, DateTime time, long size)
        {
            Name = name;
            Deepth = deepth;
            Type = type;
            Time = time;
           Size = size;
        }
        public override string ToString()
        {
            if (Type == 0)
            {
                return string.Concat(Enumerable.Repeat('-', Deepth)) + Name;
            }
            else
            {
                return string.Concat(Enumerable.Repeat('-', Deepth)) + Name + "  Creation time:" + Time + " Size:" +Size + "MB";
            }
        }

        public override bool Equals(object other)
        {
            return Equals(other as Node);
        }

        public bool Equals(Node other)
        {
            return other != null &&
                   Name == other.Name &&
                   Deepth == other.Deepth &&
                  Size == other.Size &&
                   Time == other.Time &&
                   Type == other.Type;
        }
        public static IEnumerable<Node> SearchDirectory(DirectoryInfo directory, int deep = 0)
        {
            yield return new Node(directory.Name, deep, 0, default, 0);
            foreach (var subdirectory in directory.GetDirectories())
                foreach (var item in SearchDirectory(subdirectory, deep + 1))
                    yield return item;

            foreach (var file in directory.GetFiles())
            {
                FileInfo fileInfo = new FileInfo(directory + "/" + file.Name);
                yield return new Node(file.Name, deep + 1, 1, File.GetCreationTime(directory + "/" + file.Name), fileInfo.Length);
            }

        }

    }
}



