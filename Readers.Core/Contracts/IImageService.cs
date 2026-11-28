using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Readers.Core.Contracts
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
