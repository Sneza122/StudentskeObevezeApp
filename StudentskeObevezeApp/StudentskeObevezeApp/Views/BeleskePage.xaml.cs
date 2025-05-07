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
        private readonly string fajlPutanja = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "beleske.json");

        public BeleskePage()
        {
            InitializeComponent();

            UcitajBeleske();
            beleškeList.ItemsSource = listaBeleski;
        }

        private void UcitajBeleske()
        {
            if (File.Exists(fajlPutanja))
            {
                var json = File.ReadAllText(fajlPutanja);
                var ucitane = JsonConvert.DeserializeObject<ObservableCollection<Beleska>>(json);
                if (ucitane != null)
                    listaBeleski = new ObservableCollection<Beleska>(ucitane);
            }
            else
            {
                // Prvo pokretanje – ubacujemo 3 beleške
                listaBeleski.Add(new Beleska { Tekst = "Preći teoriju iz IRAC-a.", Datum = DateTime.Now.AddDays(-2) });
                listaBeleski.Add(new Beleska { Tekst = "Napraviti projekat iz USI-a.", Datum = DateTime.Now.AddDays(-1).AddHours(-3) });
                listaBeleski.Add(new Beleska { Tekst = "Završiti projekat iz Microsoft tehnologija za pristup podacima.", Datum = DateTime.Now.AddHours(-5) });
                SacuvajBeleske(); // Sačuvamo ih odmah
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

                listaBeleski.Insert(0, novaBeleska); // dodaje na vrh liste
                noviUnosEditor.Text = string.Empty;
                unosStack.IsVisible = false;
                btnDodaj.IsVisible = true;

                SacuvajBeleske(); // čuvamo lokalno
            }
            else
            {
                DisplayAlert("Greška", "Unos ne može biti prazan.", "OK");
            }
        }
    }
}
