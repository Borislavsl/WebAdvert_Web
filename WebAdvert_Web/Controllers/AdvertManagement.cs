using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AdvertApi.Models.BS;
using WebAdvert_Web.Models.AdvertManagement;
using WebAdvert_Web.ServiceClients;
using WebAdvert_Web.Services;

namespace WebAdvert_Web.Controllers
{
    public class AdvertManagementController : Controller
    {
        private readonly IFileUploader _fileUploader;
        private readonly IAdvertApiClient _advertApiClient;
        private readonly IMapper _mapper;

        public AdvertManagementController(IFileUploader fileUploader, IAdvertApiClient advertApiClient, IMapper mapper)
        {
            _fileUploader = fileUploader;
            _advertApiClient = advertApiClient;
            _mapper = mapper;
        }

        public IActionResult Create(CreateAdvertViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertViewModel model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                CreateAdvertModel createAdvertModel = _mapper.Map<CreateAdvertModel>(model);
                createAdvertModel.UserName = User.Identity.Name;

                AdvertResponse apiCallResponse = await _advertApiClient.CreateAsync(createAdvertModel);
                string id = apiCallResponse.Id;

                bool isOkToConfirmAd = true;
                string filePath = string.Empty;
                if (imageFile != null)
                {
                    string fileName = !string.IsNullOrEmpty(imageFile.FileName) ? Path.GetFileName(imageFile.FileName) : id;
                    filePath = $"{id}/{fileName}";

                    try
                    {
                        using (Stream readStream = imageFile.OpenReadStream())
                        {
                            bool result = await _fileUploader.UploadFileAsync(filePath, readStream);
                            if (!result)
                                throw new Exception("Could not upload the image to file repository. Please see the logs for details.");
                        }
                    }
                    catch (Exception e)
                    {
                        isOkToConfirmAd = false;
                        Console.WriteLine(e);
                    }
                }

                var confirmModel = new ConfirmAdvertRequest()
                {
                    Id = id,
                    FilePath = filePath,
                    Status = isOkToConfirmAd ? AdvertStatus.Active : AdvertStatus.Pending
                };
                await _advertApiClient.ConfirmAsync(confirmModel);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}