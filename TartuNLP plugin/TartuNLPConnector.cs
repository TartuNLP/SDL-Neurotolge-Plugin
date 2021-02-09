using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using Sdl.LanguagePlatform.Core;
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
        
        public static EngineConf GetConfig(string url, string auth)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-api-key", auth);
            client.DefaultRequestHeaders.Add("application", "SDL");
            var response = client.GetAsync(url).Result;
            var engineConf = new JavaScriptSerializer().Deserialize<EngineConf>(response.Content.ReadAsStringAsync().Result);
            return engineConf;
        }

        public List<string> GetTranslation(LanguagePair languageDirection, List<string> sourceString)
        {
            var sourceLanguage = languageDirection.SourceCulture.TwoLetterISOLanguageName;
            var targetLanguage = languageDirection.TargetCulture.TwoLetterISOLanguageName;
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", _auth);
                client.DefaultRequestHeaders.Add("application", "SDL");
                var content = new BatchInput
                {
                    text = sourceString,
                    src = sourceLanguage,
                    tgt = targetLanguage,
                    domain = _domain
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(content);
                var strContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync(_url, strContent).Result;
                var jsonResponse = new JavaScriptSerializer().Deserialize<JSONResponseBatch>(response.Content.ReadAsStringAsync().Result);
                return jsonResponse.result;
            } 
            catch (Exception ex)
            {
                if (!(ex.InnerException is HttpRequestException) ||
                    !(ex.InnerException.InnerException is WebException response)) return null;
                var message = $"HTTP error message: {response.Message}";
                throw new Exception(message);
            }
        }
    }
    
    public class EngineConf
    {
        public bool xml_support { get; set; }
        public DomainConf[] domains { get; set; }
    }
    
    public class DomainConf
    {
        public string name { get; set; }
        public string code { get; set; }
        public string[] languages { get; set; }
    }

    internal class BatchInput
    {
        public List<string> text;
        public string src;
        public string tgt;
        public string domain;
    }

    public class JSONResponseBatch
    {
        public List<string> result { get; set; }
    }
}
