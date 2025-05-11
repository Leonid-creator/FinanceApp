using System.Text;
using FinanceApp.Lib.Dtos;
using System.Text.Json;
using System.Net.Http.Json;
using CommunityToolkit.Maui.Views;
using FinanceApp.Client.Api;

namespace FinanceApp.Client.Pages
{
    public partial class MainPage : ContentPage
    {        
        public MainPage()
        {
            InitializeComponent();
        }
        
        private async void OnNewReceptClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewReceiptPage());
        }
        private async void OnReportsClicked(object sender, EventArgs e)
        {
        }
    }
}