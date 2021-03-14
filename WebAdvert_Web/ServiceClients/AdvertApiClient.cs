using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using AutoMapper;
using AdvertApi.Models.BS;

namespace WebAdvert_Web.ServiceClients
{
    public class AdvertApiClient : IAdvertApiClient
    {
        private readonly string _baseAddress;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;

        public AdvertApiClient(IConfiguration configuration, HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;

            _baseAddress = configuration.GetSection("AdvertApi").GetValue<string>("BaseUrl");
        }

        public async Task<AdvertResponse> CreateAsync(CreateAdvertModel model)
        {
            AdvertModel advertApiModel = _mapper.Map<AdvertModel>(model);

            string jsonModel = JsonConvert.SerializeObject(advertApiModel);
            HttpResponseMessage response = await _client.PostAsync(new Uri($"{_baseAddress}/create"),
                                                                   new StringContent(jsonModel, Encoding.UTF8, "application/json"));
            jsonModel = await response.Content.ReadAsStringAsync();
            CreateAdvertResponse createAdvertResponse = JsonConvert.DeserializeObject<CreateAdvertResponse>(jsonModel);
            AdvertResponse advertResponse = _mapper.Map<AdvertResponse>(createAdvertResponse);

            return advertResponse;
        }

        public async Task<bool> ConfirmAsync(ConfirmAdvertRequest model)
        {
            ConfirmAdvertModel advertModel = _mapper.Map<ConfirmAdvertModel>(model);
            string jsonModel = JsonConvert.SerializeObject(advertModel);
            HttpResponseMessage response = await _client.PutAsync(new Uri($"{_baseAddress}/confirm"),
                                                                  new StringContent(jsonModel, Encoding.UTF8, "application/json"));
                
            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<List<Advertisement>> GetAllAsync()
        {
            HttpResponseMessage apiCallResponse = await _client.GetAsync(new Uri($"{_baseAddress}/all"));
            List<AdvertModel> allAdvertModels = await apiCallResponse.Content.ReadAsAsync<List<AdvertModel>>();

            List<Advertisement> allAdvertisements = allAdvertModels.Select(x => _mapper.Map<Advertisement>(x)).ToList();
            return allAdvertisements;
        }

        public async Task<Advertisement> GetAsync(string advertId)
        {
            HttpResponseMessage apiCallResponse = await _client.GetAsync(new Uri($"{_baseAddress}/{advertId}"));
            AdvertModel fullAdvert = await apiCallResponse.Content.ReadAsAsync<AdvertModel>();

            Advertisement advertisement = _mapper.Map<Advertisement>(fullAdvert);
            return advertisement;
        }
    }
}