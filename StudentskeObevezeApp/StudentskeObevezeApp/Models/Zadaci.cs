using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace StudentskeObevezeApp.Models
{
    public class Zadaci
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Naziv { get; set;}    
        public string Opis { get; set; }

        public DateTime Datum{ get; set; }
        public bool Zavrsen { get; set; }
        public PrioritetZadatka Prioritet { get; set; }
        public bool PodsetnikUkljucen { get; set; }
    }
    public enum PrioritetZadatka
    {
        Nizak,
        Srednji,
        Visok
    }
}
