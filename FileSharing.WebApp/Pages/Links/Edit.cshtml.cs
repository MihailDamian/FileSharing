using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileSharing.Core.Entities;
using FileSharing.Core.Services;
using LinkSharing.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace FileSharing.WebApp.Pages.Links
{
    public class EditModel : PageModel
    {
        public Link Link { get; set; }
        private IConfiguration configuration { get; set; }
        private string connectionString
        {
            get
            {
                return configuration.GetConnectionString("FileSharingDatabase");
            }
        }
        public EditModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void OnGet(int id)
        {
            if (id > 0)
            {
                LinkRepository lRepository = new LinkRepository(connectionString);
                Link = lRepository.Get(id);
            }
        }

        public IActionResult OnPostSave(int id, string email)
        {
            if (id > 0)
            {
                LinkRepository lRepository = new LinkRepository(connectionString);
                Link = lRepository.Get(id);
                Link.Email = email;
                lRepository.Update(Link);
                if (!String.IsNullOrEmpty(email))
                {
                    EmailServices eServices = new EmailServices();
                    eServices.SendLink(email, $"{Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/{Link.Url}");
                }
                return RedirectToPage("Index", new { Id = Link.FileId });
            }
            return RedirectToPage("/Error");
        }
    }
}