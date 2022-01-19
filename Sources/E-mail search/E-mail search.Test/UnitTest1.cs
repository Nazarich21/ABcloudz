using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace E_mail_search.Test
{
    public class Tests
    {
        public SortedSet<string> expectEmail = new SortedSet<string>()
        {
            "artemnaz@gmail.com",
            "email@uk.ua",
            "mail@megacom.com",
        };
        public SortedSet<string> expectLink = new SortedSet<string>()
        {
            "https://www.codeproject.com",
            "https://www.google.com.ua",
            "https://www.codeproject.com",
            "http://my.spec.link/fred/pa?dir=ref"
        };

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Email()
        {
            string data = File.ReadAllText("Resources/TextFile1.txt");
            Search serchemail = Search.Email(data);
            SortedSet<string> resList = new SortedSet<string>();

            serchemail.NewResult += delegate (object sender, CustomEventArgs args)
            {
                resList.Add(args.Valueev);
            };

            Thread myThreadEmail = new Thread(new ParameterizedThreadStart(serchemail.SearchObj));
            myThreadEmail.Start(serchemail);
            myThreadEmail.Join();

            CollectionAssert.AreEquivalent(expectEmail, resList);
        }

        [Test]
        public void Email_Empty_File()
        {
            string data = File.ReadAllText("Resources/TextFileEmpty.txt");
            Search serchemail = Search.Email(data);
            SortedSet<string> resList = new SortedSet<string>();

            serchemail.NewResult += delegate (object sender, CustomEventArgs args)
            {
                resList.Add(args.Valueev);
            };

            Thread myThreadEmail = new Thread(new ParameterizedThreadStart(serchemail.SearchObj));
            myThreadEmail.Start(serchemail);
            myThreadEmail.Join();

            Assert.Zero(serchemail.Length);
        }

        [Test]
        public void Link()
        {
            string data = File.ReadAllText("Resources/TextFile1.txt");
            Search serchlink = Search.Link(data);
            SortedSet<string> resList = new SortedSet<string>();

            serchlink.NewResult += delegate (object sender, CustomEventArgs args)
            {
                resList.Add(args.Valueev);
            };

            Thread myThreadEmail = new Thread(new ParameterizedThreadStart(serchlink.SearchObj));
            myThreadEmail.Start(serchlink);
            myThreadEmail.Join();

            CollectionAssert.AreEquivalent(expectLink, resList);
        }

        [Test]
        public static void Link_Empty_File()
        {
            string data = File.ReadAllText("Resources/TextFileEmpty.txt");
            Search serchlink = Search.Link(data);
            SortedSet<string> resList = new SortedSet<string>();

            serchlink.NewResult += delegate (object sender, CustomEventArgs args)
            {
                resList.Add(args.Valueev);
            };

            Thread myThreadEmail = new Thread(new ParameterizedThreadStart(serchlink.SearchObj));
            myThreadEmail.Start(serchlink);
            myThreadEmail.Join();

            Assert.Zero(serchlink.Length);
        }

    }
}
