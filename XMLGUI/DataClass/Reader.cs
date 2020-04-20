using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XMLGUI.EventsLib;

namespace XMLGUI.DataClass
{
    public class Controller
    {
        public XDocument xdoc;
        private string path;
        public List<Reader> readers = new List<Reader>();
        private delegate bool flmb(XElement o, string v);

        public List<Reader> Filter(Field name, string val)
        {
            if (name == Field.NUM)
                return new List<Reader>(from r in this.xdoc.Element("library").Elements("reader")
                                            where r.Attribute("num").Value == val
                                            select new Reader() {
                                                number = r.Attribute("num").Value,
                                                sname = r.Attribute("sname").Value,
                                                books = new List<Book>(from b in r.Elements("book")
                                                                       select new Book {
                                                                           dateOut = DateTime.ParseExact(b.Attribute("date").Value, "d.M.yyyy", System.Globalization.CultureInfo.InvariantCulture),
                                                                           days = Convert.ToInt32(b.Attribute("days").Value),
                                                                           author = b.Attribute("author").Value,
                                                                           name = b.Attribute("name").Value,
                                                                           year = Convert.ToInt32(b.Attribute("year").Value),
                                                                           izdat = b.Attribute("izdat").Value,
                                                                           price = Convert.ToInt32(b.Attribute("price").Value)
                                                                       })
                                            });
            else
            {
                flmb filter = (o, v) => true;
                switch (name)
                {
                    case Field.AUTHOR:
                        filter = (o, v) => o.Attribute("author").Value == v;
                        break;
                    case Field.IZDAT:
                        filter = (o, v) => o.Attribute("izdat").Value == v;
                        break;
                    case Field.DATE:
                        filter = (o, v) =>
                        (DateTime.ParseExact(o.Attribute("date").Value, "d.M.yyyy", System.Globalization.CultureInfo.InvariantCulture).AddDays(Convert.ToInt32(o.Attribute("days").Value)))
                                < DateTime.ParseExact(v, "d.M.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        break;
                }
                return new List<Reader>(from r in this.xdoc.Element("library").Elements("reader")
                                            select new Reader() {
                                                number = r.Attribute("num").Value,
                                                sname = r.Attribute("sname").Value,
                                                books = new List<Book>(from b in r.Elements("book")
                                                                       where filter(b, val)
                                                                       select new Book {
                                                                           dateOut = DateTime.ParseExact(b.Attribute("date").Value, "d.M.yyyy", System.Globalization.CultureInfo.InvariantCulture),
                                                                           days = Convert.ToInt32(b.Attribute("days").Value),
                                                                           author = b.Attribute("author").Value,
                                                                           name = b.Attribute("name").Value,
                                                                           year = Convert.ToInt32(b.Attribute("year").Value),
                                                                           izdat = b.Attribute("izdat").Value,
                                                                           price = Convert.ToInt32(b.Attribute("price").Value)
                                                                       })
                                            });
            }
        }

        public void Save() => xdoc.Save(this.path);

        public void Save(string path) => xdoc.Save(path);

        public int LoadFile(string path)
        {
            this.xdoc = XDocument.Load(path);
            this.path = path;
            try
            {
                this.readers = new List<Reader>(from r in this.xdoc.Element("library").Elements("reader")
                                                select new Reader() {
                                                    number = r.Attribute("num").Value,
                                                    sname = r.Attribute("sname").Value,
                                                    books = new List<Book>(from b in r.Elements("book")
                                                                           select new Book {
                                                                               dateOut = DateTime.ParseExact(b.Attribute("date").Value, "d.M.yyyy", System.Globalization.CultureInfo.InvariantCulture),
                                                                               days = Convert.ToInt32(b.Attribute("days").Value),
                                                                               author = b.Attribute("author").Value,
                                                                               name = b.Attribute("name").Value,
                                                                               year = Convert.ToInt32(b.Attribute("year").Value),
                                                                               izdat = b.Attribute("izdat").Value,
                                                                               price = Convert.ToInt32(b.Attribute("price").Value)
                                                                           })
                                                });
            } catch (NullReferenceException)
            {
                return 1;
            } catch (System.FormatException)
            {
                return 2;
            }
            return 0;
        }

        public void AddReader(Reader reader)
        {
            readers.Add(reader);
            xdoc.Element("library").Add(new XElement("reader",
                new XAttribute("num", reader.number),
                new XAttribute("sname", reader.sname)));
        }

        public void AddBook(Reader reader, Book book)
        {
            reader.books.Add(book);
            (from r in xdoc.Element("library").Elements("reader")
             where r.Attribute("num").Value == reader.number
             select r).Single().Add(new XElement("book",
                new XAttribute("name", book.name),
                new XAttribute("author", book.author),
                new XAttribute("date", book.dateOut.ToString("dd.MM.yyyy")),
                new XAttribute("days", book.days),
                new XAttribute("izdat", book.izdat),
                new XAttribute("price", book.price),
                new XAttribute("year", book.year)
             ));
        }

        public void DeleteNod(object tag)
        {
            if (tag is Reader)
            {
                Reader reader = (Reader)tag;
                readers.Remove(reader);
                (from r in xdoc.Element("library").Elements("reader")
                 where r.Attribute("num").Value == reader.number && r.Attribute("sname").Value == reader.sname
                 select r).ToList().ForEach(r => r.Remove());
            } else if (tag is Book)
            {
                Reader reader = (from r in readers
                                 where r.books.Contains((Book)tag)
                                 select r).Single();
                (from b in (from r in xdoc.Element("library").Elements("reader")
                            where r.Attribute("num").Value == reader.number && r.Attribute("sname").Value == reader.sname
                            select r).Elements("book")
                 where new Book {
                     dateOut = DateTime.ParseExact(b.Attribute("date").Value, "d.M.yyyy", System.Globalization.CultureInfo.InvariantCulture),
                     days = Convert.ToInt32(b.Attribute("days").Value),
                     author = b.Attribute("author").Value,
                     name = b.Attribute("name").Value,
                     year = Convert.ToInt32(b.Attribute("year").Value),
                     izdat = b.Attribute("izdat").Value,
                     price = Convert.ToInt32(b.Attribute("price").Value)
                 } == (Book) tag
                 select b).ToList().ForEach(r => r.Remove());

                reader.books.Remove((Book)tag);
            }
        }
    }

    public class Reader
    {
        public string number { get; set; }
        public string sname { get; set; }
        public List<Book> books { get; set; }
        public Reader()
        {
            this.books = new List<Book>();
        }

        public override string ToString() => number + " - " + sname;

        public override bool Equals(object obj)
        {
            var reader = obj as Reader;
            return reader != null &&
                   number == reader.number &&
                   sname == reader.sname &&
                   books.SequenceEqual(reader.books);
        }

        public override int GetHashCode()
        {
            var hashCode = -2107053726;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(number);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(sname);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Book>>.Default.GetHashCode(books);
            return hashCode;
        }

        public static bool operator ==(Reader reader1, Reader reader2) => EqualityComparer<Reader>.Default.Equals(reader1, reader2);
        public static bool operator !=(Reader reader1, Reader reader2) => !(reader1 == reader2);
    }

    public class Book
    {
        public DateTime dateOut { get; set; }
        public int days { get; set; }
        public string author { get; set; }
        public string name { get; set; }
        public int year { get; set; }
        public string izdat { get; set; }
        public int price { get; set; }

        public override bool Equals(object obj)
        {
            var book = obj as Book;
            return book != null &&
                   dateOut == book.dateOut &&
                   days == book.days &&
                   author == book.author &&
                   name == book.name &&
                   year == book.year &&
                   izdat == book.izdat &&
                   price == book.price;
        }

        public static bool operator ==(Book book1, Book book2) => EqualityComparer<Book>.Default.Equals(book1, book2);
        public static bool operator !=(Book book1, Book book2) => !(book1 == book2);

        public override string ToString() => "\"" + name + "\" " + author + ", " + year;

        public override int GetHashCode()
        {
            var hashCode = 126658358;
            hashCode = hashCode * -1521134295 + dateOut.GetHashCode();
            hashCode = hashCode * -1521134295 + days.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(author);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + year.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(izdat);
            hashCode = hashCode * -1521134295 + price.GetHashCode();
            return hashCode;
        }
    }
}