using System;
using System.IO;
using Xamarin.Forms;
using StudentskeObevezeApp.Data;
using StudentskeObevezeApp.Views;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StudentskeObevezeApp
{
    public partial class App : Application
    {
        static Database database;
        public static string DbFileName = "tasks.db3";

        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbFileName);
                    database = new Database(dbPath);
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();


            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbFileName);

            // 🔴 Brisanje stare baze (SAMO ZA TESTIRANJE)
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
                database = null;
            }

            var baza = Database;

            // ✅ Dodaj testne zadatke
            Task.Run(async () =>
            {
                var zadaci = new List<Models.Zadaci>
        {
            new Models.Zadaci
            {
                Naziv = "Učiti za ispit",
                Opis = "Spremiti SQL upite",
                Datum = DateTime.Now.AddDays(1),
                Prioritet = Models.PrioritetZadatka.Visok,
                Zavrsen = false,
                PodsetnikUkljucen = false
            },
            new Models.Zadaci
            {
                Naziv = "Napraviti prezentaciju",
                Opis = "Završiti slajdove za seminar",
                Datum = DateTime.Now.AddDays(2),
                Prioritet = Models.PrioritetZadatka.Srednji,
                Zavrsen = false,
                PodsetnikUkljucen = true
            },
            new Models.Zadaci
            {
                Naziv = "Vežbati kodiranje",
                Opis = "Rad na zadacima iz OOP-a",
                Datum = DateTime.Now.AddDays(3),
                Prioritet = Models.PrioritetZadatka.Nizak,
                Zavrsen = true,
                PodsetnikUkljucen = false
            }
        };

                foreach (var zad in zadaci)
                {
                    await baza.SaveItemAsync(zad);
                }

            }).Wait(); // čekamo da se ubace svi pre nego što se stranica prikaže

            // 🔵 Prikaz glavne stranice
            MainPage = new MainPage();
        }
    }
}
