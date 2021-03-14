using AutoMapper;
using AdvertApi.Models.BS;
using WebAdvert_Web.Models;
using WebAdvert_Web.Models.AdvertManagement;
using WebAdvert_Web.Models.Home;

namespace WebAdvert_Web.ServiceClients
{
    public class AdvertApiProfile : Profile
    {
        public AdvertApiProfile()
        {
            CreateMap<CreateAdvertModel, CreateAdvertViewModel>().ReverseMap();
            CreateMap<AdvertModel, CreateAdvertModel>().ReverseMap();
            CreateMap<Advertisement, AdvertModel>().ReverseMap();
            CreateMap<Advertisement, IndexViewModel>().ReverseMap();
            CreateMap<AdvertType, SearchViewModel>().ReverseMap();
            CreateMap<AdvertResponse, CreateAdvertResponse>().ReverseMap();
            CreateMap<ConfirmAdvertRequest, ConfirmAdvertModel>().ReverseMap();    
        }
    }
}
