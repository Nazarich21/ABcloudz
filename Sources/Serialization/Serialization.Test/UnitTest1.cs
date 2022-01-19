using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Serialization.Test
{
    public class Tests
    {
        const string TestPath = "Test";

        List<Node> _expect = CreateEtalon();

        [OneTimeSetUp]
        public static void Create()
        {
            CreateDirectory();
        }

        [OneTimeTearDown]
        public static void DeleteDirectory()
        {
            File.Delete("ser.json");
            File.Delete("ser.xml");
            File.Delete("ser.bin");
            Directory.Delete(TestPath, true);
        }

        [Test]
        [TestCase("ser.json", "json")]
        [TestCase("ser.xml", "xml")]
        [TestCase("ser.bin", "binary")]
        public void TestSerialization(string filename, string fileformat)
        {
            ProgramOptions option1 = new ProgramOptions(new string[] { TestPath, filename, fileformat });
            Serialization.Serialize(_expect, option1.FileName, option1.Format);

            List<Node> resList = Serialization.Deserialize(option1.FileName, option1.Format);
            foreach (var res in resList)
            {
                res.Time = default;
            }

            Assert.AreEqual(_expect, resList);
        }

        public static void CreateDirectory()
        {
            string subpath = "Folder1";
            string subpath1 = "Folder2";
            DirectoryInfo dirInfo = new DirectoryInfo(TestPath);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            dirInfo.CreateSubdirectory(subpath);
            dirInfo.CreateSubdirectory(subpath1);

            DirectoryInfo dirInfo1 = new DirectoryInfo(TestPath + "\\" + subpath);
            if (!dirInfo1.Exists)
            {
                dirInfo1.Create();
            }
            dirInfo1.CreateSubdirectory("Folder3");

            using (FileStream fs = File.Create(TestPath + "\\File1.txt"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");

                fs.Write(info, 0, info.Length);
            }

            using (FileStream fs = File.Create(TestPath + "\\" + subpath + "\\File2.txt"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");

                fs.Write(info, 0, info.Length);
            }
        }
  
        public static List<Node> CreateEtalon()
        {
            List<Node> expectlist = new List<Node>();
            Node node = new Node("Test", 0, 0, default, 0);
            Node node1 = new Node("Folder1", 1, 0, default, 0);
            Node node2 = new Node("Folder3", 2, 0, default, 0);
            Node node3 = new Node("File2.txt", 2, 0, default, 30);
            Node node4 = new Node("Folder2", 1, 0, default, 0);
            Node node5 = new Node("File1.txt", 1, 0, default, 30);

            expectlist.Add(node1);
            expectlist.Add(node2);
            expectlist.Add(node3);
            expectlist.Add(node4);
            expectlist.Add(node5);

            return expectlist;
        }
    }
}


