using Sdl.LanguagePlatform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Neurotolge_Plugin
{
    class NLPConnector
    {
        private string uri;
        private List<string> _feats;

        public NLPConnector(string serverAddress, int port, LanguagePair languages, List<string> features)
        {
            var sourceLang = languages.SourceCulture.TwoLetterISOLanguageName;
            var targetLang = languages.TargetCulture.TwoLetterISOLanguageName;
            uri = "https://" + serverAddress + "/" + sourceLang + targetLang + ":" +
                port.ToString() + "/translator";

            _feats = features;
        }

        public string getTranslation(LanguagePair languageDirection, string sourceString)
        {
            var translatedText = String.Empty;

            try
            {
                var client = new RestClient(uri);
                var request = new RestRequest("translate", Method.POST);

                List<JsonTranslationResponse> ListJson = new List<JsonTranslationResponse>();
                JsonTranslationResponse JsonTranslation = new JsonTranslationResponse
                {
                    src = sourceString,
                    feats = _feats
                };

                ListJson.Add(JsonTranslation);
                string serializedSourceString = JsonConvert.SerializeObject(ListJson);

                request.AddParameter("application/json; charset=utf-8", serializedSourceString, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                var response = client.Execute(request).Content;
                JArray translatedObject = JArray.Parse(response);
                if (translatedObject != null)
                {
                    translatedText = translatedObject[0][0].SelectToken("tgt").ToString();
                    Console.WriteLine(translatedText);
                }

            }
            catch (WebException e)
            {
                var response = (HttpWebResponse)e.Response;
                string message = string.Format("Http status code={0}, error message= {1}", response.StatusCode, e.Message);
                throw new Exception(message);
            }

            return translatedText;
        }
    }

    public class JsonTranslationResponse
    {
        [JsonIgnore]
        public string tgt { get; set; }

        public string src { get; set; }

        public List<string> feats { get; set; }

        [JsonIgnore]
        public float pred_score { get; set; }
        [JsonIgnore]
        public int n_best { get; set; }
    }
}
