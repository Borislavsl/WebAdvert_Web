using AdvertApi.Models.BS;
using AutoMapper;

namespace WebAdvert_Web.ServiceClients
{
    public class AdvertApiProfile : Profile
    {
        public AdvertApiProfile()
        {
            CreateMap<AdvertModel, CreateAdvertModel>().ReverseMap();
            CreateMap<CreateAdvertResponse, AdvertResponse>().ReverseMap();
            CreateMap<ConfirmAdvertRequest, ConfirmAdvertModel>().ReverseMap();
        }
    }
}
