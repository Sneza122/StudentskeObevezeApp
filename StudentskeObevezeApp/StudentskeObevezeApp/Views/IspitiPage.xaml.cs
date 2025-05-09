using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using StudentskeObevezeApp.Models;
using StudentskeObevezeApp.Data;
using System.Threading.Tasks;

namespace StudentskeObevezeApp.Views
{
    public partial class IspitiPage : ContentPage
    {
        private ObservableCollection<Ispit> listaIspita = new ObservableCollection<Ispit>();
        private Ispit izabraniIspitZaIzmenu = null;

        public IspitiPage()
        {
            InitializeComponent();
            BindingContext = this;
            _ = InicijalizujIspite();
        }

        private async Task InicijalizujIspite()
        {
            var svi = await App.Database.GetIspitiAsync();

            
            // Dodaj samo tri osnovna ako nema ništa
            if ((await App.Database.GetIspitiAsync()).Count == 0)
            {
                var pocetni = new[]
                {
                    new Ispit { Predmet = "Microsoft tehnologije za pristup podacima", DatumIspita = new DateTime(2025, 5, 4, 17, 30, 0), IspitZavrsen = false },
                    new Ispit { Predmet = "Uvod u softversko inženjerstvo (USI)", DatumIspita = new DateTime(2025, 5, 5, 13, 15, 0), IspitZavrsen = false },
                    new Ispit { Predmet = "Informacione i računarske arhitekture (IRAC)", DatumIspita = new DateTime(2025, 5, 6, 9, 0, 0), IspitZavrsen = false }
                };

                foreach (var ispit in pocetni)
                {
                    await App.Database.SacuvajIspitAsync(ispit);
                }
            }

            await UcitajIspite();
        }

        private async Task UcitajIspite()
        {
            listaIspita.Clear();
            var ispiti = (await App.Database.GetIspitiAsync())
                         .OrderBy(x => x.DatumIspita)
                         .ToList();

            foreach (var ispit in ispiti)
            {
                if (!ispit.AlarmPostavljen)
                {
                    await ZakaziAlarm(ispit);
                    ispit.AlarmPostavljen = true;
                    await App.Database.SacuvajIspitAsync(ispit);
                }
                listaIspita.Add(ispit);
            }

            ispitiListView.ItemsSource = listaIspita;
        }

        private async Task ZakaziAlarm(Ispit ispit)
        {
            var vremeNotifikacije = ispit.DatumIspita.AddDays(-3);
            if (vremeNotifikacije > DateTime.Now)
            {
                // Simulacija notifikacije
                await Task.Delay(500);
                await DisplayAlert("Podsetnik", $"Ispit iz {ispit.Predmet} je za 3 dana ({ispit.DatumIspita:dd.MM.yyyy. HH:mm})", "OK");
            }
        }

        private void OnDodajIspitClicked(object sender, EventArgs e)
        {
            izabraniIspitZaIzmenu = null;
            addIspitForm.IsVisible = true;
            btnDodajIspit.IsVisible = false;
        }

        private async void OnSaveIspitClicked(object sender, EventArgs e)
        {
            var datumIVreme = datePickerIspit.Date + timePickerIspit.Time;

            if (izabraniIspitZaIzmenu != null)
            {
                izabraniIspitZaIzmenu.Predmet = entryPredmet.Text;
                izabraniIspitZaIzmenu.DatumIspita = datumIVreme;
                await App.Database.SacuvajIspitAsync(izabraniIspitZaIzmenu);
            }
            else
            {
                var novi = new Ispit
                {
                    Predmet = entryPredmet.Text,
                    DatumIspita = datumIVreme,
                    AlarmPostavljen = false,
                    IspitZavrsen = false
                };
                await App.Database.SacuvajIspitAsync(novi);
            }

            entryPredmet.Text = "";
            addIspitForm.IsVisible = false;
            btnDodajIspit.IsVisible = true;

            await UcitajIspite();
        }

        private async void OnZavrsiIspitChecked(object sender, CheckedChangedEventArgs e)
        {
            var cb = (CheckBox)sender;
            var ispit = (Ispit)cb.BindingContext;
            ispit.IspitZavrsen = e.Value;
            await App.Database.SacuvajIspitAsync(ispit);
        }

        private async void OnObrisiClicked(object sender, EventArgs e)
        {
            var dugme = (Button)sender;
            var ispit = (Ispit)dugme.BindingContext;

            bool potvrda = await DisplayAlert("Potvrda", $"Obrisati ispit: {ispit.Predmet}?", "Da", "Ne");
            if (potvrda)
            {
                await App.Database.ObrisiIspitAsync(ispit);
                listaIspita.Remove(ispit);
            }
        }

        private void OnIzmeniClicked(object sender, EventArgs e)
        {
            var dugme = (Button)sender;
            izabraniIspitZaIzmenu = (Ispit)dugme.BindingContext;

            if (izabraniIspitZaIzmenu != null)
            {
                entryPredmet.Text = izabraniIspitZaIzmenu.Predmet;
                datePickerIspit.Date = izabraniIspitZaIzmenu.DatumIspita;
                timePickerIspit.Time = izabraniIspitZaIzmenu.DatumIspita.TimeOfDay;

                addIspitForm.IsVisible = true;
                btnDodajIspit.IsVisible = false;
            }
        }
    }
}
