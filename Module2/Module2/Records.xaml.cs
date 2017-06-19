using Microsoft.WindowsAzure.MobileServices;
using Module2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Module2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Records : ContentPage
    {
        MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;

        public Records()
        {
            InitializeComponent();
            Populate();
        }

        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            loading.IsRunning = true;
            List<TranslatorModel> info = await AzureManager.AzureManagerInstance.GetTranslatorInformation();

            TList.ItemsSource = info;
            loading.IsRunning = false;

        }

        public async void Populate()
        {
            loading.IsRunning = true;
            List<TranslatorModel> info = await AzureManager.AzureManagerInstance.GetTranslatorInformation();

            TList.ItemsSource = info;
            loading.IsRunning = false;

        }
    }
}