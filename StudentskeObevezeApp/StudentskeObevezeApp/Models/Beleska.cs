using SQLite;
using System;

namespace StudentskeObevezeApp.Models
{
    public class Beleska
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Tekst { get; set; }

        public DateTime Datum { get; set; }
    }
}

