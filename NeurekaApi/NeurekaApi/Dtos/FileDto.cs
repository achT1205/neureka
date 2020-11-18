using System;
using Microsoft.AspNetCore.Http;

namespace NeurekaApi.Dtos
{
    public class FileDto
    {
        public IFormFile File { get; set; }
    }

    public class DeleteFileDto
    {
        public string FileName { get; set; }
    }
}
