using StudentskeObevezeApp.Models;
using System;
using Xamarin.Forms;

namespace StudentskeObevezeApp.Views
{
    public partial class AddTaskPage : ContentPage
    {
        public event EventHandler TaskSaved;
        private Zadaci _task;

        public AddTaskPage(Zadaci task)
        {
            InitializeComponent();

            _task = task ?? new Zadaci(); // Ako je null, kreiraj novi

            // Ako je postojeći zadatak, popuni polja
            if (_task.Id != 0)
            {
                nazivEntry.Text = _task.Naziv;
                opisEditor.Text = _task.Opis;
                datumPicker.Date = _task.Datum;
                prioritetPicker.SelectedItem = _task.Prioritet.ToString();
                podsetnikSwitch.IsToggled = _task.PodsetnikUkljucen;
            }
        }

        private async void OnSaveTaskClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nazivEntry.Text))
            {
                await DisplayAlert("Greška", "Naziv zadatka je obavezan.", "U redu");
                return;
            }

            _task.Naziv = nazivEntry.Text;
            _task.Opis = opisEditor.Text;
            _task.Opis = opisEditor.Text;
            _task.Datum = datumPicker.Date;
            _task.Prioritet = (PrioritetZadatka)Enum.Parse(typeof(PrioritetZadatka), prioritetPicker.SelectedItem?.ToString() ?? "Nizak");
            _task.PodsetnikUkljucen = podsetnikSwitch.IsToggled;

            TaskSaved?.Invoke(this, EventArgs.Empty);
            await Navigation.PopAsync();

        }
        public Zadaci Zadatak => _task;


    }
}
