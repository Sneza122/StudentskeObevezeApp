using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using StudentskeObevezeApp.Models;
using StudentskeObevezeApp.Data;
using System.Threading.Tasks;

namespace StudentskeObevezeApp.Views
{
    public partial class BeleskePage : ContentPage
    {
        private ObservableCollection<Beleska> listaBeleski = new ObservableCollection<Beleska>();

        public BeleskePage()
        {
            InitializeComponent();
            BindingContext = this;
            _ = UcitajBeleske();
            beleškeList.ItemsSource = listaBeleski;
        }

        private async Task UcitajBeleske()
        {
            listaBeleski.Clear();
            var sveBeleske = await App.Database.GetBeleskeAsync();

            if (sveBeleske.Count == 0)
            {
                var pocetne = new[]
                {
                    new Beleska { Tekst = "Završiti projekat iz Microsoft tehnologija za pristup podacima.", Datum = new DateTime(2025, 5, 4, 17, 30, 0) },
                    new Beleska { Tekst = "Napraviti projekat iz USI-a.", Datum = new DateTime(2025, 5, 5, 13, 15, 0) },
                    new Beleska { Tekst = "Preći teoriju iz IRAC-a.", Datum = new DateTime(2025, 5, 6, 9, 0, 0) }
                };

                foreach (var beleska in pocetne)
                {
                    await App.Database.SacuvajBeleskuAsync(beleska);
                    listaBeleski.Add(beleska);
                }
            }
            else
            {
                foreach (var beleska in sveBeleske)
                    listaBeleski.Add(beleska);
            }
        }

        private void OnDodajClicked(object sender, EventArgs e)
        {
            unosStack.IsVisible = true;
            btnDodaj.IsVisible = false;
        }

        private async void OnSacuvajNovuClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(noviUnosEditor.Text))
            {
                var novaBeleska = new Beleska
                {
                    Tekst = noviUnosEditor.Text,
                    Datum = DateTime.Now
                };

                await App.Database.SacuvajBeleskuAsync(novaBeleska);
                listaBeleski.Insert(0, novaBeleska);

                noviUnosEditor.Text = string.Empty;
                unosStack.IsVisible = false;
                btnDodaj.IsVisible = true;
            }
            else
            {
                await DisplayAlert("Greška", "Unos ne može biti prazan.", "OK");
            }
        }

        private async void OnObrisiClicked(object sender, EventArgs e)
        {
            var dugme = sender as Button;
            var beleska = dugme?.BindingContext as Beleska;

            if (beleska != null)
            {
                bool potvrda = await DisplayAlert("Potvrda", "Da li ste sigurni da želite da obrišete ovu belešku?", "Da", "Ne");
                if (potvrda)
                {
                    await App.Database.ObrisiBeleskuAsync(beleska);
                    listaBeleski.Remove(beleska);
                }
            }
        }

        private async void OnUrediClicked(object sender, EventArgs e)
        {
            var dugme = sender as Button;
            var beleska = dugme?.BindingContext as Beleska;

            if (beleska != null)
            {
                string noviTekst = await DisplayPromptAsync("Izmeni belešku", "Izmeni tekst:", initialValue: beleska.Tekst);
                if (!string.IsNullOrWhiteSpace(noviTekst))
                {
                    beleska.Tekst = noviTekst;
                    beleska.Datum = DateTime.Now;
                    await App.Database.SacuvajBeleskuAsync(beleska);

                    var index = listaBeleski.IndexOf(beleska);
                    if (index >= 0)
                    {
                        listaBeleski.RemoveAt(index);
                        listaBeleski.Insert(index, beleska);
                    }
                }
            }
        }
    }
}
