using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace fms.ViewModels
{
    public class FileViewModel
    {
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public List<FileViewModel> Files { get; set; } = new List<FileViewModel>();
    }
}