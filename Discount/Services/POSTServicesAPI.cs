using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Discount.Helpers;
using Discount.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Discount.Services
{
    public static class POSTServicesAPI
    {
        public static async Task <bool> LDAPAuthenticateUser(string userName,string password)
        {
            try
            {

                string userJson = "{\"UserName\":\"" + "KEELLS\\\\" + userName + "\"," +
                                  "\"Password\":\"" + password + "\"," +
                                  "\"ADGroup\":\"" + "BI_APP_Discount" + "\"" +
                                  "}";

                using (var client = new WebClient())
                {

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string responce = await client.UploadStringTaskAsync(new Uri(Settings.BaseAPIUrl + "CinnamonDirectoryServices/api/Directory/AuthGroup"), "POST", userJson);
                    Console.WriteLine(responce);

                    if (responce.Contains("BI_APP_Discount"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch(Exception)
            {
                return false;
            }
        }

        public static async Task <string> ApproveDiscount(DiscountPayload payload)
        {
                
            string url = "beta/v1/approvadiscount/";
            string result = await GetODataService(url, JsonConvert.SerializeObject(payload));

            //If result is success
            if (result == "success")
            {
                return "Success";
            }
            else if (result == "Error")
            {
                return "Sorry";
            }
            else
            {
                return "Sorry";
            }
        }

        public async static Task<string> GetODataService(string url, string postBody)
        {
            string xcsrf_token = "";
            string cookie_value = "";

            try
            {
                //Refresh Token if expires
                if (Convert.ToDateTime(Settings.ExpiresTime) <= DateTime.Now)
                {
                    //Authenticate against ADFS and NW Gateway
                    oAuthLogin oauthlogin = new oAuthLogin();

                    string access_token = await oauthlogin.LoginUserAsync(Constants._user);
                    if (access_token == "" && access_token == Constants._userNotExistInNWGateway)
                    {
                        Console.WriteLine("Logout");
                    }
                }

                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = cookies;

                using (var client = new HttpClient(handler))
                {
                    //Get X-CSRF-TOKEN
                    client.BaseAddress = new Uri(Constants._azureAPIMDEVBase + url);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants._access_token);
                    client.DefaultRequestHeaders.Add("X-CSRF-Token", "fetch");
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "4f4af597f5814a989297601aaa276fe4");

                    //GET data
                    HttpResponseMessage response = await client.GetAsync(Constants._azureAPIMDEVBase + url);
                    xcsrf_token = response.Headers.GetValues("X-CSRF-Token").FirstOrDefault();

                    Uri uri = new Uri(Constants._azureAPIMDEVBase + url);
                    IEnumerable <Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
                    foreach (Cookie cookie in responseCookies)
                    {
                        if (Constants._cookie == cookie.Name)
                            cookie_value = cookie.Value;
                    }
                }

                if (xcsrf_token != "" && cookie_value != "")
                {
                    //Post Method
                    Uri baseUri = new Uri(Constants._azureAPIMDEVBase + url);
                    HttpClientHandler clientHandler = new HttpClientHandler();
                    //Set Cookie in Post
                    clientHandler.CookieContainer.Add(baseUri, new Cookie(Constants._cookie, cookie_value));

                    using (var client_post = new HttpClient(clientHandler))
                    {
                        client_post.BaseAddress = new Uri(Constants._azureAPIMDEVBase + url);
                        client_post.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants._access_token);
                        client_post.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //Set Token in Post
                        client_post.DefaultRequestHeaders.Add("X-CSRF-Token", xcsrf_token);
                        client_post.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "4f4af597f5814a989297601aaa276fe4");

                        //Post json content
                        var response = client_post.PostAsync(Constants._azureAPIMDEVBase + url, new StringContent(postBody, Encoding.UTF8, "application/json")).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Debug.WriteLine(response);
                            return "success";
                        }
                        else
                        {
                            return await response.Content.ReadAsStringAsync();
                        }
                    }
                }
                else
                {
                    return "Token or cookie is not available";
                }
            }
            catch (Exception e)
            {
                return "Error";
            }
        }
    }
}

