using Xamarin.Forms;
using StudentskeObevezeApp.Data;
using StudentskeObevezeApp.Models;
using System.Collections.ObjectModel;

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
    }
}
