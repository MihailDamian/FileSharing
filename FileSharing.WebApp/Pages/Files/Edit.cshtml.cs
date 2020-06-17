using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileSharing.Core.Entities;
using FileSharing.Core.Repositories;
using FileSharing.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace FileSharing.WebApp.Pages.Files
{
    public class EditModel : PageModel
    {
        public File File { get; set; }
        [BindProperty]
        public IFormFile InputFile { get; set; }
        public int Expiration { get; set; }

        private IConfiguration configuration { get; set; }
        private string connectionString
        {
            get
            {
                return configuration.GetConnectionString("FileSharingDatabase");
            }
        }
        private readonly IHostingEnvironment webHostEnvironment;


        public EditModel(IConfiguration configuration, IHostingEnvironment webHostEnvironment)
        {
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult OnGet()
        {
            //if(id>0)
            //{
            //    FileRepository fRepository = new FileRepository(connectionString);
            //    File = fRepository.Get(id);
            //    if(File == null)
            //    {
            //        return RedirectToPage("/NotFound");
            //    }
            //}
            //else
            //{
            File = new File();
            //}
            return Page();
        }

        public async Task<IActionResult> OnPost(int Expiration)
        {
            if (InputFile != null)
                if (InputFile.Length > 0)
                {
                    FileServices fileServices = new FileServices();
                    FileRepository fileRepository = new FileRepository(connectionString);
                    File = fileServices.SaveFile(fileRepository, InputFile.FileName, Expiration);
                    string filePath = webHostEnvironment.WebRootPath + File.Path;

                    System.IO.Directory.CreateDirectory(filePath);
                    filePath += @"/" + InputFile.FileName;
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await InputFile.CopyToAsync(stream);
                    }
                    return RedirectToPage("/Links/Index",new { id = File.Id });
                }
            return Page();
        }

    }
}