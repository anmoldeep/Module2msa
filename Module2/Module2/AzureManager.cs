using Microsoft.WindowsAzure.MobileServices;
using Module2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module2
{
    public class AzureManager
    {

        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<TranslatorModel> translatorTable;

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://microsoftapp1.azurewebsites.net");
            this.translatorTable = this.client.GetTable<TranslatorModel>();

        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }
        public async Task<List<TranslatorModel>> GetTranslatorInformation()
        {
            return await this.translatorTable.ToListAsync();
        }
        public async Task PostTranslatorInformation(TranslatorModel TranslatorModel)
        {
            await this.translatorTable.InsertAsync(TranslatorModel);
        }
    }
}