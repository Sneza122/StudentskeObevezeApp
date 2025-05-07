using System;
using System.Collections.Generic;
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
            ispitiListView.ItemsSource = ispiti;
        }

        private void OnDodajIspitClicked(object sender, EventArgs e)
        {
            var noviIspit = new Ispit
            {
                Id = ispiti.Count + 1,
                Predmet = entryPredmet.Text,
                DatumIspita = datePickerIspit.Date,
                AlarmPostavljen = false
            };

            ispiti.Add(noviIspit);
            ispitiListView.ItemsSource = null;
            ispitiListView.ItemsSource = ispiti;

            ZakaziAlarm(noviIspit);
        }

        private async void ZakaziAlarm(Ispit ispit)
        {
            var vreme = ispit.DatumIspita - DateTime.Now;
            if (vreme.TotalSeconds > 0)
            {
                await System.Threading.Tasks.Task.Delay((int)vreme.TotalMilliseconds);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Podsetnik", $"Ispit iz {ispit.Predmet} je danas!", "OK");
                });
            }
        }
    }
}
