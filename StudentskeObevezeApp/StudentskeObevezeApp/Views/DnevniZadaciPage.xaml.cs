using Xamarin.Forms;
using StudentskeObevezeApp.Data;
using StudentskeObevezeApp.Models;
using System.Collections.ObjectModel;
using System;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace StudentskeObevezeApp.Views
{
    public partial class DnevniZadaciPage : ContentPage
    {
        private Database _database;

        // Konstruktor sa parametrima za prosleđivanje baze podataka
        public DnevniZadaciPage()
        {
            InitializeComponent();
            _database = App.Database;
            LoadTasks();
        }
        // Metoda za učitavanje zadataka
        private async void LoadTasks()
        {
            var tasks = await _database.GetItemsAsync();
            zadaciListView.ItemsSource = new ObservableCollection<Zadaci>(tasks);
        }
        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox?.BindingContext is Zadaci zadatak)
            {
                zadatak.Zavrsen = e.Value; // ažurira se vrednost
                await _database.SaveItemAsync(zadatak); // čuva se u bazu
            }
        }
        private async void OnDeleteTaskClicked(object sender, EventArgs e)
        {
            // Prvo dobijamo dugme koje je pritisnuto
            var button = sender as Button;

            // Zadatak kojem pripada dugme
            var zadatak = button?.BindingContext as Zadaci;

            if (zadatak != null)
            {
                // Brisanje zadatka iz baze podataka
                await _database.DeleteItemAsync(zadatak);

                // Ponovno učitavanje liste zadataka
                LoadTasks();
            }
        }
        private async void OnEditTaskClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var zadatak = button?.BindingContext as Zadaci;

            if (zadatak != null)
            {
                var editPage = new AddTaskPage(zadatak);
                editPage.TaskSaved += async (s, args) =>
                {
                    await _database.SaveItemAsync(zadatak);
                    LoadTasks();
                };
                await Navigation.PushAsync(editPage);
            }
        }
        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            var novaStrana = new AddTaskPage(null); // null znači kreiramo novi zadatak

            novaStrana.TaskSaved += async (s, args) =>
            {
                await _database.SaveItemAsync(novaStrana.Zadatak); // čuvamo novi zadatak
                LoadTasks(); // osvežavamo prikaz liste
            };

            await Navigation.PushAsync(novaStrana); // otvaramo formu
        }









    }
}