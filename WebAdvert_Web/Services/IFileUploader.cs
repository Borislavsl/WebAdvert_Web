using System.IO;
using System.Threading.Tasks;

namespace WebAdvert_Web.Services
{
    public interface IFileUploader
    {
        Task<bool> UploadFileAsync(string fileName, Stream storageStream);
    }
}