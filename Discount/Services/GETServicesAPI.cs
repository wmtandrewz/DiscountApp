using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Discount.Models;
using Discount.Helpers;
using Newtonsoft.Json.Linq;

namespace Discount.Services
{
    public static class GETServicesAPI
    {

        public static async Task<string> GetPendingDiscountList(DateTime startDate, DateTime endDate)
        {
            string sDate = startDate.ToString("yyyy-MM-dd");
            string eDate = endDate.ToString("yyyy-MM-dd");

            string url = $"beta/v1/pendingdiscountapprovals/?$filter=ImHotelId eq '{Constants._hotel_number}' and ImChargDtTo eq datetime'{sDate}T00:00:00' and ImChargDtFrom eq datetime'{eDate}T00:00:00'&$expand=DiscountDetailsSet,DiscountGroupSet";
            return await GetODataService(url);
        }

        /// <summary>
        /// Gets the OD ata service.
        /// </summary>
        /// <returns>The OD ata service.</returns>
        /// <param name="url">URL.</param>
        public static async Task<string> GetODataService(String url)
        {
            try
            {
                //Refresh Token if expires
                if (Convert.ToDateTime(Settings.ExpiresTime) <= DateTime.Now)
                {
                    //Authenticate against ADFS and NW Gateway
                    oAuthLogin oauthlogin = new oAuthLogin();
                    String access_token = await oauthlogin.LoginUserAsync(Constants._user);
                    if (access_token == "" && access_token == Constants._userNotExistInNWGateway)
                    {
                        Console.WriteLine("Logout");
                    }
                }
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constants._azureAPIMDEVBase + url);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants._access_token);
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "4f4af597f5814a989297601aaa276fe4");
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(Constants._azureAPIMDEVBase + url);
                        using (HttpContent content = response.Content)
                        {
                            string result = await content.ReadAsStringAsync();
                            return result;
                        }
                    }
                    catch (Exception)
                    {
                        return "Error";
                    }
                }
            }
            catch (Exception)
            {
                return "Error";
            }
        }
    }
}
