using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XMLGUI.DataClass;
using XMLGUI.EventsLib;


namespace XMLTEST
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {
            Controller controller = new Controller();
            Assert.AreEqual(0, controller.LoadFile("reader.xml"));
            Assert.AreEqual("a123d", controller.readers[0].number);
            Assert.AreEqual("Семенов", controller.readers[0].sname);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Controller controller = new Controller();
            Assert.AreEqual(2, controller.LoadFile("reader1.xml"));
        }

        [TestMethod]
        public void TestMethod3()
        {
            Controller controller = new Controller();
            Assert.AreEqual(1, controller.LoadFile("reader2.xml"));
        }

        [TestMethod]
        public void TestMethod4()
        {
            List<Reader> testReaders = new List<Reader>();
            testReaders.Add(new Reader() { number = "a123d", sname = "Семенов", books = new List<Book>() });
            Controller controller = new Controller();
            Assert.AreEqual(0, controller.LoadFile("reader.xml"));
            Assert.IsTrue(testReaders.SequenceEqual(controller.Filter(Field.IZDAT, "TST")));
        }

        [TestMethod]
        public void TestMethod5()
        {
            List<Reader> testReaders = new List<Reader>();
            testReaders.Add(new Reader() { number = "a123d", sname = "Семенов", books = new List<Book>() });
            testReaders[0].books.Add(new Book()
            {
                dateOut = DateTime.Parse("01/01/2019 00:00:00"),
                days = 10,
                author = "TST",
                name = "tst",
                year = 2015,
                izdat = "123",
                price = 1
            });
            Controller controller = new Controller();
            Assert.AreEqual(0, controller.LoadFile("reader.xml"));
            Assert.IsTrue(testReaders.SequenceEqual(controller.Filter(Field.IZDAT, "123")));
        }

        [TestMethod]
        public void TestMethod6()
        {
            List<Reader> testReaders = new List<Reader>();
            testReaders.Add(new Reader() { number = "a123d", sname = "Семенов", books = new List<Book>() });
            Reader reader = new Reader() { number = "b123d", sname = "Семенов", books = new List<Book>() };
            testReaders.Add(reader);
            Book book = new Book()
            {
                dateOut = DateTime.Parse("01/02/2019 00:00:00"),
                days = 10,
                author = "TST",
                name = "tst",
                year = 2015,
                izdat = "123",
                price = 1
            };
            testReaders[0].books.Add(new Book()
            {
                dateOut = DateTime.Parse("01/01/2019 00:00:00"),
                days = 10,
                author = "TST",
                name = "tst",
                year = 2015,
                izdat = "123",
                price = 1
            });
            testReaders[1].books.Add(book);
            Controller controller = new Controller();
            Assert.AreEqual(0, controller.LoadFile("reader.xml"));
            controller.AddReader(reader);
            controller.AddBook(reader, book);
            Assert.IsTrue(testReaders.SequenceEqual(controller.readers));
        }

        [TestMethod]
        public void TestMethod7()
        {
            List<Reader> testReaders = new List<Reader>();
            testReaders.Add(new Reader() { number = "a123d", sname = "Семенов", books = new List<Book>() });
            Reader reader = new Reader() { number = "b123d", sname = "Семенов", books = new List<Book>() };
            testReaders.Add(reader);
            Book book = new Book()
            {
                dateOut = DateTime.Parse("01/02/2019 00:00:00"),
                days = 10,
                author = "TST",
                name = "tst",
                year = 2015,
                izdat = "123",
                price = 1
            };
            testReaders[0].books.Add(new Book()
            {
                dateOut = DateTime.Parse("01/01/2019 00:00:00"),
                days = 10,
                author = "TST",
                name = "tst",
                year = 2015,
                izdat = "123",
                price = 1
            });
            testReaders[1].books.Add(book);
            Controller controller = new Controller();
            Assert.AreEqual(0, controller.LoadFile("reader.xml"));
            controller.AddReader(reader);
            controller.AddBook(reader, book);
            Assert.IsTrue(testReaders.SequenceEqual(controller.readers));
            controller.Save("test.xml");
            Controller comp = new Controller();
            Assert.AreEqual(0, comp.LoadFile("test.xml"));

            testReaders = new List<Reader>();
            testReaders.Add(new Reader() { number = "a123d", sname = "Семенов", books = new List<Book>() });
            reader = new Reader() { number = "b123d", sname = "Семенов", books = new List<Book>() };
            testReaders.Add(reader);
            testReaders[0].books.Add(new Book()
            {
                dateOut = DateTime.Parse("01/01/2019 00:00:00"),
                days = 10,
                author = "TST",
                name = "tst",
                year = 2015,
                izdat = "123",
                price = 1
            });
            testReaders[1].books.Add(book);

            Assert.IsTrue(testReaders.SequenceEqual(comp.readers));
        }

        [TestMethod]
        public void TestMethod8()
        {
            List<Reader> testReaders = new List<Reader>();
            testReaders.Add(new Reader() { number = "a123d", sname = "Семенов", books = new List<Book>() });
            Reader reader = new Reader() { number = "b123d", sname = "Семенов", books = new List<Book>() };
            testReaders.Add(reader);
            Book book = new Book()
            {
                dateOut = DateTime.Parse("01/02/2019 00:00:00"),
                days = 10,
                author = "TST",
                name = "tst",
                year = 2015,
                izdat = "123",
                price = 1
            };
            testReaders[0].books.Add(new Book()
            {
                dateOut = DateTime.Parse("01/01/2019 00:00:00"),
                days = 10,
                author = "TST",
                name = "tst",
                year = 2015,
                izdat = "123",
                price = 1
            });
            testReaders[1].books.Add(book);
            Controller controller = new Controller();
            Assert.AreEqual(0, controller.LoadFile("reader.xml"));
            controller.AddReader(reader);
            controller.AddBook(reader, book);
            Assert.IsTrue(testReaders.SequenceEqual(controller.readers));
            controller.DeleteNod(book);
            Assert.IsTrue(testReaders.SequenceEqual(controller.readers));
            controller.DeleteNod(reader);
            Assert.IsFalse(testReaders.SequenceEqual(controller.readers));
        }

        [TestMethod]
        public void TestMethod9()
        {
            Controller controller = new Controller();
            Assert.AreEqual(0, controller.LoadFile("reader.xml"));
            controller.DeleteNod(controller.readers[0]);
            Assert.AreEqual(0, controller.readers.Count);
        }

        [TestMethod]
        public void TestMethod10()
        {
            List<Reader> testReaders = new List<Reader>();
            testReaders.Add(new Reader() { number = "a123d", sname = "Семенов", books = new List<Book>() });
            testReaders.Add(new Reader() { number = "b123d", sname = "Семенов", books = new List<Book>() });
            testReaders[0].books.Add(new Book()
            {
                dateOut = DateTime.Parse("01/01/2019 00:00:00"),
                days = 10,
                author = "TST",
                name = "tst",
                year = 2015,
                izdat = "123",
                price = 1
            });
            Controller controller = new Controller();
            Assert.AreEqual(0, controller.LoadFile("test.xml"));
            Assert.IsTrue(testReaders.SequenceEqual(controller.Filter(Field.DATE, "20.01.2019")));
        }
    }
}
