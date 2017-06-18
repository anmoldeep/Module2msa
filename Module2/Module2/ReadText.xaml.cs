using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Module2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReadText : ContentPage
    {
        const string subscriptionKey = "fce13dd0420d486792458e1b3309d5c8";
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze";
       

        public ReadText()
        {
            InitializeComponent();
            // Set Picker Default value to Chinese
            langpicker.SelectedIndex = 0;
        }



        private async void loadCamera(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();
            // check camera permissions and availibility
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }
            // take a photo
            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Full,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });
            // display image
            if (file == null)
                return;

            //     image.Source = ImageSource.FromStream(() =>
            //   {
            //        return file.GetStream();
            //    });

            OcrResults text;

            var client = new VisionServiceClient("fce13dd0420d486792458e1b3309d5c8", "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0");

            // initiate call to Custom Vision API
            using (var stream = file.GetStream())
                text = await client.RecognizeTextAsync(stream);

            // create object for retrieving text
            ReadText x = new ReadText();
            // store recognized string in variable
            var RecognizedText = x.retrieveocr(text);
            // Display on screen
            ocrprint.Text = RecognizedText;
            //Start Translate Text
            // First Get the token
            string token = await x.FetchTokenAsync("https://api.cognitive.microsoft.com/sts/v1.0", "6c67eee6ba7e4ff094dab7d1ff498797");
            // Get language Input and Generate Language Code
            var LangCode = LanguageInput();
            // Invoke Translation method with API
            string translated = await TranslateTextAsync(RecognizedText, LangCode);
            // Print Translated Text
            translateprint.Text = translated;
        }

        // Fucntion to retrieve OCR text
        public string retrieveocr(OcrResults text)
        {
            /// <summary>
            /// Retrieve text from the given OCR results object.
            /// </summary>
            /// <param name="results">The OCR results.</param>
            /// <returns>Return the text.</returns>
            OcrResults results = text;
            StringBuilder stringBuilder = new StringBuilder();
            if (results != null && results.Regions != null)
            {
                stringBuilder.Append("Hello how are you");
                stringBuilder.AppendLine();
                foreach (var item in results.Regions)
                {
                    foreach (var line in item.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            stringBuilder.Append(word.Text);
                            stringBuilder.Append(" ");
                        }

                        stringBuilder.AppendLine();
                    }

                    stringBuilder.AppendLine();
                }
            }
            return stringBuilder.ToString();
        }
        // invoke translation
        public async Task<string> TranslateTextAsync(string text, string lang)
        {

            string requestUri = GenerateRequestUri("https://api.microsofttranslator.com/v2/http.svc/Translate", text, lang);
            string accessToken = await FetchTokenAsync("https://api.cognitive.microsoft.com/sts/v1.0", "6c67eee6ba7e4ff094dab7d1ff498797");
            var response = await SendRequestAsync(requestUri, accessToken);
            var xml = XDocument.Parse(response);
            return xml.Root.Value;
        }
        //Generate URL Request
        string GenerateRequestUri(string endpoint, string text, string to)
        {
            string requestUri = endpoint;
            requestUri += string.Format("?text={0}", Uri.EscapeUriString(text));
            requestUri += string.Format("&to={0}", to);
            return requestUri;
        }
        // Get Token
        async Task<string> FetchTokenAsync(string fetchUri, string apiKey)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
                UriBuilder uriBuilder = new UriBuilder(fetchUri);
                uriBuilder.Path += "/issueToken";

                var result = await client.PostAsync(uriBuilder.Uri.AbsoluteUri, null);
                return await result.Content.ReadAsStringAsync();
            }
        }
        //Sent Request
        async Task<string> SendRequestAsync(string url, string bearerToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                var response = await httpClient.GetAsync(url);
                return await response.Content.ReadAsStringAsync();
            }
        }
        // Generate Language code
        public string LanguageInput()
        {
            var lang_code = "";
            // get current selection of language
            var selected_lang = langpicker.SelectedIndex;
            switch (selected_lang)
            {
                case 0:
                    lang_code = "zh-CHS";
                    return lang_code;
                case 1:
                    lang_code = "hi";
                    return lang_code;
                case 3:
                    lang_code = "fr";
                    return lang_code;
                case 4:
                    lang_code = "en";
                    return lang_code;
                case 5:
                    lang_code = "fr";
                    return lang_code;
                case 6:
                    lang_code = "de";
                    return lang_code;
                case 7:
                    lang_code = "es";
                    return lang_code;
                    }
            return lang_code;
        }

        
    }
}

