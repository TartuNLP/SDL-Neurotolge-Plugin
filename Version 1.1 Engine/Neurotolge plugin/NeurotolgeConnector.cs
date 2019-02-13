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

namespace Neurotolge_plugin
{
    public class NeurotolgeConnector
    {
        private string uri;
        private List<string> _feats;

        private enum httpMethod
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public NeurotolgeConnector(string serverAddress, int port, List<string> features)
        {
            uri = "https://" + serverAddress;
            _feats = features;
        }

        public string getTranslation(LanguagePair languageDirection, string sourceString)
        {
            var translatedText = String.Empty;

            try
            {
                var client = new RestClient(uri);
                var request = new RestRequest("translate?engine=etenlv&auth=public&conf=fml,et", Method.POST);
                
                request.AddParameter("src", sourceString);
                //request.AddUrlSegment("src", sourceString);

                var response = client.Execute(request).Content;
                Test translatedObject = JsonConvert.DeserializeObject<Test>(response);
                while (translatedObject.tgt == "(Some error, sorry)")
                {
                    string[] sourceStringList = sourceString.Split('|');
                    int sourceStringLen = sourceStringList.Length;
                    var sourceStringNew = String.Empty;
                    for (var i = 0; i < 2; i++)
                    {
                        if (i == 0)
                        {
                            sourceStringNew = String.Join("|", sourceStringList.Take(sourceStringLen - 2));
                        }
                        else
                        {
                            sourceStringNew = String.Join("|", sourceStringList.Skip(sourceStringLen - 2).Take(1));
                        }

                        request.AddOrUpdateParameter("src", sourceStringNew);

                        response = client.Execute(request).Content;
                        translatedObject = JsonConvert.DeserializeObject<Test>(response);

                        if (i == 1)
                        {
                            translatedText += translatedObject.tgt;
                        }
                        else
                        {
                            translatedText += translatedObject.tgt + "|";
                        }
                    }
                }

                if (translatedObject != null && translatedText == String.Empty)
                {
                    translatedText = translatedObject.tgt;
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
}
