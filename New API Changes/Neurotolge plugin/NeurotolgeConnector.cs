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
    public class NeurotolgeConnector
    {
        private string uri;
        private string _domain;
        private string _auth;

        private enum httpMethod
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public NeurotolgeConnector(string URL, string Auth, string domain)
        {
            uri =  URL;
            _domain = domain;
            _auth = Auth;
        }

        public string getTranslation(LanguagePair languageDirection, string sourceString)
        {
            string[] sourceStringList = null;
            if (sourceString.Contains('|'))
                sourceStringList = sourceString.Split('|');
            else
                sourceStringList = new string[] {sourceString};

            var translatedText = String.Empty;
            var targetLanguage = languageDirection.TargetCultureName.Split('-')[0];
            try
            {
                HttpClient client = new HttpClient();
                var content = new BatchInput();
                content.text = sourceStringList.ToList<string>();
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(content);
                HttpResponseMessage response = client.PostAsync(uri + "?auth=" + _auth + "&olang=" + targetLanguage + "&odomain=" + _domain, new StringContent(json, Encoding.UTF8, "application/json")).Result;
                var translatedObject = new JavaScriptSerializer().Deserialize<JSONResponseBatch>(response.Content.ReadAsStringAsync().Result);

                if (translatedObject != null && translatedText == String.Empty) { 

                    if (translatedObject.result.Count == 1)
                        translatedText = translatedObject.result[0];
                    else
                        translatedText = String.Join("|", translatedObject.result);
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

        public static IDictionary<string, string[]> getConfig(string url, string auth)
        {
            HttpClient client = new HttpClient();
            HttpContent inputContent = new StringContent("application/json");
            HttpResponseMessage response = client.PostAsync(url + "/support?auth=" + auth, inputContent).Result;
            var JSONResponse = new JavaScriptSerializer().Deserialize<ConfigJSON>(response.Content.ReadAsStringAsync().Result);
            IDictionary<string, string[]> testing = new Dictionary<string, string[]>();
            foreach (OptionsJSON option in JSONResponse.options)
            {
                string[] langs = (option.name + "," + string.Join(",", option.lang)).Split(',');
                testing.Add(option.odomain, langs);
            }
            return testing;

        }

        public static string StringReplace(string text, string oldString, string newString)
        {

            return text.Replace(oldString, newString);
        }
    }

    public class JsonTranslationResponse
    {
        [JsonIgnore]
        public string tgt { get; set; }

        public string src { get; set; }

        public string engine { get; set; }

        public string conf { get; set; }

        public string auth { get; set; }

        //public List<string> feats { get; set; }

        [JsonIgnore]
        public float pred_score { get; set; }
        [JsonIgnore]
        public int n_best { get; set; }
    }

    public class Test
    {
        public string tgt { get; set; }
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

    class Input
    {
        public string text;
    }
    class BatchInput
    {
        public List<string> text;
    }

    public class JSONResponse
    {

        public string result { get; set; }
        public string status { get; set; }
        public string input { get; set; }
        public string message { get; set; }
    }

    public class JSONResponseBatch
    {
        public List<string> result { get; set; }
        public string status { get; set; }
        public List<string> input { get; set; }
        public string message { get; set; }
    }
}
