using AutoMapper;
using System.IO;

namespace WorldNews.Mappings
{
    abstract class ProfileBase : Profile
    {
        protected string GetServerPhotoPath(string photoPath)
        {
            return Path.Combine("~/Images/Uploads", Path.GetFileName(photoPath));
        }
    }
}