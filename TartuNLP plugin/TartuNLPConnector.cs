using System;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdl.LanguagePlatform.Core;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace TartuNLP
{
    public class TartuNLPConnector
    {
        private readonly string _url;
        private readonly string _domain;
        private readonly string _auth;

        public TartuNLPConnector(string url, string auth, string domain)
        {
            _url = url;
            _domain = domain;
            _auth = auth;
        }

        public List<string> GetTranslation(LanguagePair languageDirection, List<string> sourceString)
        {
            var targetLanguage = languageDirection.TargetCulture.TwoLetterISOLanguageName;
            try
            {
                var client = new HttpClient();
                var content = new BatchInput {text = sourceString};
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(content);
                var response = client.PostAsync(_url + "?auth=" + _auth + "&olang=" + targetLanguage + "&odomain=" + _domain, new StringContent(json, Encoding.UTF8, "application/json")).Result;
                var translatedObject = new JavaScriptSerializer().Deserialize<JSONResponseBatch>(response.Content.ReadAsStringAsync().Result);

                var translatedText = translatedObject.result;
                
                return translatedText;
            } 
            catch (Exception ex)
            {
                if (!(ex.InnerException is HttpRequestException) ||
                    !(ex.InnerException.InnerException is WebException response)) return null;
                var message = $"HTTP error message: {response.Message}";
                throw new Exception(message);
            }
        }

        public static IDictionary<string, string[]> GetConfig(string url, string auth)
        {
            var client = new HttpClient();
            var content = new StringContent("application/json");
            var response = client.PostAsync(url + "/support?auth=" + auth, content).Result;
            var jsonResponse = new JavaScriptSerializer().Deserialize<ConfigJSON>(response.Content.ReadAsStringAsync().Result);
            IDictionary<string, string[]> config = new Dictionary<string, string[]>();
            foreach (var option in jsonResponse.options)
            {
                var languages = (option.name + "," + string.Join(",", option.lang)).Split(',');
                config.Add(option.odomain, languages);
            }
            return config;

        }
    }

    class ConfigJSON
    {
        public string domain { get; set; }
        public OptionsJSON[] options { get; set; }
    }

    class OptionsJSON
    {
        public string odomain { get; set; }
        public string name { get; set; }
        public string[] lang { get; set; }
    }

    class BatchInput
    {
        public List<string> text;
    }

    public class JSONResponseBatch
    {
        public List<string> result { get; set; }
        public string status { get; set; }
        public List<string> input { get; set; }
        public string message { get; set; }
    }
}
