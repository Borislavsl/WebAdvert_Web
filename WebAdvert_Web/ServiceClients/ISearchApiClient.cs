using System.Collections.Generic;
using System.Threading.Tasks;
using WebAdvert_Web.Models;

namespace WebAdvert_Web.ServiceClients
{
    public interface ISearchApiClient
    {
        Task<List<AdvertType>> Search(string keyword);
    }
}
