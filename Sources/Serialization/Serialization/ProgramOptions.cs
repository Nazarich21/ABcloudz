using System;
using System.IO;

namespace Serialization
{
    public enum FormatType
    {
        Bin,
        Json,
        Xml
    }

    public class ProgramOptions
    {
        public bool Serialize { get; private set; }
        public FormatType Format { get; private set; }
        public string Catalog { get; private set; }
        public string FileName { get; private set; }

        public ProgramOptions(string[] args)
        {
            if (args.Length == 3)
            {
                Serialize = true;

                Catalog = args[0];
                FileName = args[1];
                string format = args[2];

                if (format == "json")
                {
                    Format = FormatType.Json;

                }
                else if (format == "xml")
                {
                    Format = FormatType.Xml;
                }
                else if (format == "binary")
                {
                    Format = FormatType.Bin;
                }
                else
                    throw new ArgumentException("Unsupported file format");
            }
            else if (args.Length == 1)
            {
                Serialize = false;

                FileName = args[0];
                string format = File.ReadAllText(FileName);
                if (format.StartsWith("["))
                {
                    Format = FormatType.Json;

                }
                else if (format.StartsWith("<"))
                {
                    Format = FormatType.Xml;
                }
                else if (format.StartsWith(@"\0"))
                {
                    Format = FormatType.Bin;
                }
                else
                    throw new ArgumentException("Unsupported file format");
            }
            else
                throw new ApplicationException("Invalid number of arguments.");
        }
    }
}
