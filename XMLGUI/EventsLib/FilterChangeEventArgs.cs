using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLGUI.EventsLib
{
    public enum Field { DEF, NUM, AUTHOR, IZDAT, DATE }
    public class FilterChangeEventArgs : EventArgs
    {
        public Field Name { get; internal set; }
        public string Param { get; internal set; }

        public FilterChangeEventArgs(Field nameValue, string paramValue)
        {
            Name = nameValue;
            Param = paramValue;
        }
    }

    public class InsertReaderEventArgs : EventArgs
    {
        public string Num { get; internal set; }
        public string SName { get; internal set; }

        public InsertReaderEventArgs(string numValue, string snameValue)
        {
            this.Num = numValue;
            this.SName = snameValue;
        }
    }

    public class InsertBookEventArgs : EventArgs
    {
        public InsertBookEventArgs(DateTime dateOut, int days, string author, string name, int year, string izdat, int price)
        {
            DateOut = dateOut;
            Days = days;
            Author = author;
            Name = name;
            Year = year;
            Izdat = izdat;
            Price = price;
        }

        public DateTime DateOut { get; internal set; }
        public int Days { get; internal set; }
        public string Author { get; internal set; }
        public string Name { get; internal set; }
        public int Year { get; internal set; }
        public string Izdat { get; internal set; }
        public int Price { get; internal set; }
        
    }
}
