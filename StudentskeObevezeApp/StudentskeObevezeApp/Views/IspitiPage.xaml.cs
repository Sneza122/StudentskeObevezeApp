using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using StudentskeObevezeApp.Models;

namespace StudentskeObevezeApp.Views
{
    public partial class IspitiPage : ContentPage
    {
        private List<Ispit> ispiti = new List<Ispit>();

        public IspitiPage()
        {
            InitializeComponent();

            // Fiksni ispiti sa tačnim datumom i vremenom
            ispiti.Add(new Ispit { Id = 1, Predmet = "RVAS", DatumIspita = new DateTime(2025, 6, 20, 10, 30, 0) });
            ispiti.Add(new Ispit { Id = 2, Predmet = "IRAC", DatumIspita = new DateTime(2025, 6, 15, 9, 0, 0) });
            ispiti.Add(new Ispit { Id = 3, Predmet = "USI", DatumIspita = new DateTime(2025, 6, 10, 8, 0, 0) });

            // Sortiranje od najdaljeg ka najbližem
            ispiti = ispiti.OrderByDescending(i => i.DatumIspita).ToList();
            ispitiListView.ItemsSource = ispiti;

            // Zakazivanje alarma
            foreach (var ispit in ispiti)
            {
                if (!ispit.AlarmPostavljen)
                {
                    ZakaziAlarm(ispit);
                    ispit.AlarmPostavljen = true;
                }
            }
        }

        private void OnDodajIspitClicked(object sender, EventArgs e)
        {
            addIspitForm.IsVisible = true;
            btnDodajIspit.IsVisible = false;
        }

        private void OnSaveIspitClicked(object sender, EventArgs e)
        {
            var datum = datePickerIspit.Date;
            var vreme = timePickerIspit.Time;
            var datumIVreme = datum + vreme;

            var noviIspit = new Ispit
            {
                Id = ispiti.Count + 1,
                Predmet = entryPredmet.Text,
                DatumIspita = datumIVreme,
                AlarmPostavljen = false
            };

            ispiti.Add(noviIspit);
            ispiti = ispiti.OrderByDescending(i => i.DatumIspita).ToList();

            ispitiListView.ItemsSource = null;
            ispitiListView.ItemsSource = ispiti;

            ZakaziAlarm(noviIspit);

            // Reset forme
            entryPredmet.Text = string.Empty;
            datePickerIspit.Date = DateTime.Today;
            timePickerIspit.Time = new TimeSpan(9, 0, 0);
            addIspitForm.IsVisible = false;
            btnDodajIspit.IsVisible = true;
        }

        private async void ZakaziAlarm(Ispit ispit)
        {
            var vreme = ispit.DatumIspita - DateTime.Now;

            if (vreme.TotalSeconds > 0)
            {
                await DisplayAlert("Podsetnik", $"Ispit iz {ispit.Predmet} je {ispit.DatumIspita:dd.MM.yyyy. HH:mm}.", "OK");
            }
        }
    }
}
