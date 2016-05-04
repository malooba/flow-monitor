//Copyright 2016 Malooba Ltd

//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at

//    http://www.apache.org/licenses/LICENSE-2.0

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Remote
{
    public static class RestClient
    {
        private static readonly Uri baseUrl;
        static RestClient()
        {
            var appSettings = ConfigurationManager.AppSettings;
            baseUrl = new Uri(appSettings["BaseUrl"]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url)
        {
            try
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = baseUrl;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.GetAsync(url).Result;
                    if(response.StatusCode == HttpStatusCode.OK)
                        return response.Content.ReadAsStringAsync().Result;

                    if(response.StatusCode == HttpStatusCode.NoContent)
                        return null;

                    // logger.WarnFormat("HTTP Status code = {0} ({1})", response.ReasonPhrase, (int)response.StatusCode);
                    return null;
                }
            }
            catch(Exception)
            {
                return null;
            }
        }

        public static string Put(string url, string json)
        {
            try
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = baseUrl;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PutAsync(url, new StringContent(json, Encoding.UTF8)).Result;
                    if(response.StatusCode == HttpStatusCode.OK)
                        return response.Content.ReadAsStringAsync().Result;

                    if(response.StatusCode == HttpStatusCode.NoContent)
                        return null;

                    // logger.WarnFormat("HTTP Status code = {0} ({1})", response.ReasonPhrase, (int)response.StatusCode);
                    return null;
                }
            }
            catch(Exception)
            {
                return null;
            }
        }

        public static bool Delete(string url)
        {
            try
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = baseUrl;
                    client.DefaultRequestHeaders.Accept.Clear();

                    var response = client.DeleteAsync(url).Result;
                    return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            {
                // ignore
                return false;
            }
        }
    }
}
