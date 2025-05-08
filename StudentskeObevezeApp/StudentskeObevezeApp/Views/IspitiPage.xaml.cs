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

            // Proveravamo da li već postoje ispiti u listi
            if (!ispiti.Any())
            {
                // Fiksni ispiti sa tačnim datumom i vremenom
                ispiti.Add(new Ispit { Id = 1, Predmet = "RVAS", DatumIspita = new DateTime(2025, 6, 20, 10, 30, 0) });
                ispiti.Add(new Ispit { Id = 2, Predmet = "IRAC", DatumIspita = new DateTime(2025, 6, 15, 9, 0, 0) });
                ispiti.Add(new Ispit { Id = 3, Predmet = "USI", DatumIspita = new DateTime(2025, 6, 10, 8, 0, 0) });
            }

            // Sortiranje od najdaljeg ka najbližem
            ispiti = ispiti.OrderByDescending(i => i.DatumIspita).ToList();
            ispitiListView.ItemsSource = ispiti;
        }

        private void OnDodajIspitClicked(object sender, EventArgs e)
        {
            // Kada klikneš na dugme, forma za unos ispita postaje vidljiva
            addIspitForm.IsVisible = true;
        }

        private void OnSaveIspitClicked(object sender, EventArgs e)
        {
            // Dodavanje ispita u listu
            var datum = datePickerIspit.Date;
            var vreme = timePickerIspit.Time;
            var datumIVreme = datum + vreme;

            // Novi ispit sa unetim podacima
            var noviIspit = new Ispit
            {
                Id = ispiti.Count + 1,
                Predmet = entryPredmet.Text,
                DatumIspita = datumIVreme,
                AlarmPostavljen = false
            };

            // Dodavanje novog ispita na početak liste
            ispiti.Insert(0, noviIspit);
            ispitiListView.ItemsSource = null;
            ispitiListView.ItemsSource = ispiti;

            // Sakrivanje forme za unos
            addIspitForm.IsVisible = false;
        }
    }
}