using AdvertApi.Models.BS;

namespace WebAdvert_Web.ServiceClients
{
    public class ConfirmAdvertRequest
    {
        public string Id { get; set; }
        public string FilePath { get; set; }
        public AdvertStatus Status { get; set; }
    }
}