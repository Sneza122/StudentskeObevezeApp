using System;

namespace StudentskeObevezeApp.Models
{
    public class Ispit
    {
        public int Id { get; set; }
        public string Predmet { get; set; }
        public DateTime DatumIspita { get; set; }
        public bool AlarmPostavljen { get; set; }
        public bool IspitZavrsen { get; set; }

        public string PrikazDatuma
        {
            get
            {
                return DatumIspita.ToString("dd.MM.yyyy. HH:mm");
            }
        }
    }
}
