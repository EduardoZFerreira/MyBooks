using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBooks
{
    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
    }
}
