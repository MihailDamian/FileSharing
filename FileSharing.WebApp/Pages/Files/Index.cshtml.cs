using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using FileSharing.Core.Entities;
using FileSharing.Core.Repositories;
using FileSharing.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace FileSharing.WebApp.Pages.Files
{
    public class IndexModel : PageModel
    {
        public List<File> Files { get; set; }
        private IConfiguration configuration { get; set; }
        private string connectionString
        {
            get
            {
                return configuration.GetConnectionString("FileSharingDatabase");
            }
        }
        private readonly IHostingEnvironment webHostEnvironment;
        public IndexModel(IConfiguration configuration, IHostingEnvironment webHostEnvironment)
        {
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }
        public void OnGet()
        {
            Refresh();
        }
        public void OnPostRemove( int selectedFileId)
        {
            if (selectedFileId > 0)
            {
                new FileServices().RemoveFile(webHostEnvironment.WebRootPath, new FileRepository(connectionString), selectedFileId);
            }
            Refresh();
        }

        private void Refresh()
        {
            FileRepository fRepository = new FileRepository(connectionString);
            Files = fRepository.GetAll().ToList();
        }
    }
}