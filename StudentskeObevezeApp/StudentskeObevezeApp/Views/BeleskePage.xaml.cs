using System;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Forms;
using StudentskeObevezeApp.Models;

namespace StudentskeObevezeApp.Views
{
    public partial class BeleskePage : ContentPage
    {
        private ObservableCollection<Beleska> listaBeleski = new ObservableCollection<Beleska>();
        private readonly string fajlPutanja = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "beleske.json"
        );

        public BeleskePage()
        {
            InitializeComponent();
            UcitajBeleske(); // prvo učitamo podatke
            beleškeList.ItemsSource = listaBeleski; // zatim vežemo listu
        }

        private void UcitajBeleske()
        {
            if (File.Exists(fajlPutanja))
            {
                var json = File.ReadAllText(fajlPutanja);
                var ucitane = JsonConvert.DeserializeObject<ObservableCollection<Beleska>>(json);
                if (ucitane != null)
                {
                    listaBeleski.Clear();
                    foreach (var beleska in ucitane)
                    {
                        listaBeleski.Add(beleska);
                    }
                }
            }
            else
            {
                // Dodajemo unapred definisane beleške
                listaBeleski.Insert(0, new Beleska
                {
                    Tekst = "Završiti projekat iz Microsoft tehnologija za pristup podacima.",
                    Datum = new DateTime(2025, 5, 4, 17, 30, 0)
                });

                listaBeleski.Insert(0, new Beleska
                {
                    Tekst = "Napraviti projekat iz USI-a.",
                    Datum = new DateTime(2025, 5, 5, 13, 15, 0)
                });

                listaBeleski.Insert(0, new Beleska
                {
                    Tekst = "Preći teoriju iz IRAC-a.",
                    Datum = new DateTime(2025, 5, 6, 9, 0, 0)
                });

                SacuvajBeleske();
            }
        }

        private void SacuvajBeleske()
        {
            var json = JsonConvert.SerializeObject(listaBeleski);
            File.WriteAllText(fajlPutanja, json);
        }

        private void OnDodajClicked(object sender, EventArgs e)
        {
            unosStack.IsVisible = true;
            btnDodaj.IsVisible = false;
        }

        private void OnSacuvajNovuClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(noviUnosEditor.Text))
            {
                var novaBeleska = new Beleska
                {
                    Tekst = noviUnosEditor.Text,
                    Datum = DateTime.Now
                };

                listaBeleski.Insert(0, novaBeleska);
                noviUnosEditor.Text = string.Empty;
                unosStack.IsVisible = false;
                btnDodaj.IsVisible = true;

                SacuvajBeleske();
            }
            else
            {
                DisplayAlert("Greška", "Unos ne može biti prazan.", "OK");
            }
        }

        private async void OnObrisiClicked(object sender, EventArgs e)
        {
            var dugme = sender as Button;
            var beleska = dugme?.BindingContext as Beleska;

            if (beleska != null)
            {
                // Prikazujemo dijalog za potvrdu
                bool odgovor = await DisplayAlert("Potvrda", "Da li ste sigurni da želite da obrišete ovu belešku?", "Da", "Ne");

                if (odgovor)
                {
                    listaBeleski.Remove(beleska);
                    SacuvajBeleske();
                }
            }
        }

        private async void OnUrediClicked(object sender, EventArgs e)
        {
            var dugme = sender as Button;
            var beleska = dugme?.BindingContext as Beleska;

            if (beleska != null)
            {
                // Unos novog teksta
                string noviTekst = await DisplayPromptAsync("Izmeni belešku", "Izmeni tekst:", initialValue: beleska.Tekst);
                if (!string.IsNullOrWhiteSpace(noviTekst))
                {
                    beleska.Tekst = noviTekst;
                    beleska.Datum = DateTime.Now; // Ažuriramo datum kada je izmena izvršena
                    SacuvajBeleske();

                    // Osvežavanje prikaza
                    beleškeList.ItemsSource = null;
                    beleškeList.ItemsSource = listaBeleski;
                }
            }
        }
    }
}
