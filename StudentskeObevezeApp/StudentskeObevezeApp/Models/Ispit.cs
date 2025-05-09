using SQLite;
using System;

namespace StudentskeObevezeApp.Models
{
    public class Ispit
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Predmet { get; set; }

        public DateTime DatumIspita { get; set; }

        public bool AlarmPostavljen { get; set; }

        public bool IspitZavrsen { get; set; }

        [Ignore]
        public string PrikazDatuma => DatumIspita.ToString("dd.MM.yyyy. HH:mm");
    }
}
