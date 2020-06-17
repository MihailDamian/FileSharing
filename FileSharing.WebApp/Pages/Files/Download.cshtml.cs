using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileSharing.Core.Entities;
using FileSharing.Core.Repositories;
using FileSharing.Core.Services;
using LinkSharing.Core.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace FileSharing.WebApp.Pages.Files
{
    public class DownloadModel : PageModel
    {
        public File File { get; set; }
        public Link Link { get; set; }
        private IConfiguration configuration { get; set; }
        private string connectionString
        {
            get
            {
                return configuration.GetConnectionString("FileSharingDatabase");
            }
        }
        private IHostingEnvironment env { get; set; }
        public DownloadModel(IConfiguration configuration, IHostingEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }
        public void OnGet(Guid id)
        {
            if (id != null)
            {
                LinkRepository linkRepository = new LinkRepository(connectionString);
                Link = linkRepository.Get(id);
                if (Link != null)
                {
                    FileRepository fileRepository = new FileRepository(connectionString);
                    File = fileRepository.Get(Link.FileId);
                }
            }
        }

        public IActionResult OnPost(Guid id)
        {
            if (id != null)
            {
                LinkRepository linkRepository = new LinkRepository(connectionString);
                Link = linkRepository.Get(id);
                if (Link != null)
                {
                    FileRepository fileRepository = new FileRepository(connectionString);
                    File = fileRepository.Get(Link.FileId);
                    string filePath = env.WebRootPath + File.Path + @"/" + File.Name;
                    string fileName = File.Name;

                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    Link.Count++;
                    linkRepository.Update(Link);
                    return File(fileBytes, "application/force-download", fileName);
                }
            }
            return NotFound();
        }
    }
}