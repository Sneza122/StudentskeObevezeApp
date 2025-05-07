using System;
using Xamarin.Forms;

namespace StudentskeObevezeApp.Views
{
    public partial class BeleskePage : ContentPage
    {
        private string poslednjaBeleška = "";
         
        public BeleskePage()
        {
            InitializeComponent();
        }

        private void OnSacuvajClicked(object sender, EventArgs e)
        {
            poslednjaBeleška = editorBeleška.Text;
            labelPrikazBeleške.Text = poslednjaBeleška;
            DisplayAlert("Uspešno", "Beleška je sačuvana.", "OK");
        }
    }
}
