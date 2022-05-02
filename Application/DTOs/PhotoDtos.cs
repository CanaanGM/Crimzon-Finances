using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PhotoReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageBase64 { get; set; }
        public string Description { get; set; }
        public string FileExtension { get; set; }
        public decimal Size { get; set; }
    }

    public class PhotoWriteDto
    {
        public ICollection<IFormFile> Files { get; set; }

    }
}
