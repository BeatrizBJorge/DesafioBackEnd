using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DesafioBackEnd.DTOs
{
    public class UploadCnhDto
    {
        public IFormFile File { get; set; } = null!;
    }
}