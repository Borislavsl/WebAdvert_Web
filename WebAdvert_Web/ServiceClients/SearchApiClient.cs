using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebAdvert_Web.Models;

namespace WebAdvert_Web.ServiceClients
{
    public class SearchApiClient : ISearchApiClient
    {
        private readonly HttpClient _client;
        private readonly string _baseAddress = string.Empty;
        public SearchApiClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _baseAddress = configuration.GetSection("SearchApi").GetValue<string>("url");
        }

        public async Task<List<AdvertType>> Search(string keyword)
        {
            string callUrl = $"{_baseAddress}/search/v1/{keyword}";
            HttpResponseMessage httpResponse = await _client.GetAsync(new Uri(callUrl));

            List<AdvertType> result;
            if (httpResponse.StatusCode == HttpStatusCode.OK)
                result =  await httpResponse.Content.ReadAsAsync<List<AdvertType>>();
            else
                result = new List<AdvertType>();

            return result;
        }
    }
}
