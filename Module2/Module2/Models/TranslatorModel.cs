using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module2.Models
{
   public class TranslatorModel
    {

      
        
            [JsonProperty(PropertyName = "Id")]
            public string ID { get; set; }

            [JsonProperty(PropertyName = "Recognized")]
            public string Recognized { get; set; }

            [JsonProperty(PropertyName = "LanguageFrom")]
            public string LanguageFrom { get; set; }

            [JsonProperty(PropertyName = "LanguageTo")]
            public string LanguageTo { get; set; }

            [JsonProperty(PropertyName = "Translated")]
            public string Translated { get; set; }

        }
    }


