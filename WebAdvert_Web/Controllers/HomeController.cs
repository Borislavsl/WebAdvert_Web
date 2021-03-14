using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebAdvert_Web.Models;
using WebAdvert_Web.Models.Home;
using WebAdvert_Web.ServiceClients;

namespace WebAdvert.Web.Controllers
{
    public class HomeController : Controller
    {
        public ISearchApiClient SearchApiClient { get; }
        public IMapper Mapper { get; }
        public IAdvertApiClient ApiClient { get; }

        public HomeController(ISearchApiClient searchApiClient, IMapper mapper, IAdvertApiClient apiClient)
        {
            SearchApiClient = searchApiClient;
            Mapper = mapper;
            ApiClient = apiClient;
        }

        [Authorize]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> Index()
        {
            List<Advertisement> allAds = await ApiClient.GetAllAsync();
            IEnumerable<IndexViewModel> allViewModels = allAds.Select(x => Mapper.Map<IndexViewModel>(x));

            return View(allViewModels);
        }
        
        public async Task<IActionResult> AdvertDetail(string id)
        {
            Advertisement record = await ApiClient.GetAsync(id);

            IndexViewModel model = Mapper.Map<IndexViewModel>(record);

            return View("AdvertDetail", model);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string keyword)
        {
            var viewModel = new List<SearchViewModel>();

            List<AdvertType> searchResult = await SearchApiClient.Search(keyword);
            searchResult.ForEach(advertDoc =>
            {
                SearchViewModel viewModelItem = Mapper.Map<SearchViewModel>(advertDoc);
                viewModel.Add(viewModelItem);
            });

            return View("Search", viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}