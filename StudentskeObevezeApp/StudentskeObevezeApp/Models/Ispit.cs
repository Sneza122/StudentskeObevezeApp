using System;
using System.Collections.Generic;
using System.Text;

namespace StudentskeObevezeApp.Models
{
    public class Ispit
    {
        public int Id { get; set; }
        public string Predmet { get; set; }
        public DateTime DatumIspita { get; set; }
        public bool AlarmPostavljen { get; set; }
    }
}
