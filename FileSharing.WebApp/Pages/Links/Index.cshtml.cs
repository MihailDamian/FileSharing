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
    public class IndexModel : PageModel
    {
        public List<Link> Links { get; set; }
        public int FileId { get; set; }
        public string BaseURL;
        private IConfiguration configuration { get; set; }
        private string connectionString
        {
            get
            {
                return configuration.GetConnectionString("FileSharingDatabase");
            }
        }
        public IndexModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void OnGet(int id)
        {
            FileId = id;
            Refresh();
        }

        public void OnPostSave()
        {
            this.FileId = FileId;
            Refresh();

        }

        public void OnPostAdd(int FileId)
        {
            this.FileId = FileId;
            if (FileId > 0)
            {
                LinkServices services = new LinkServices();
                services.GenerateNewLink(new LinkRepository(connectionString), FileId);
            }
            Refresh();
        }

        public void OnPostRemove(int FileId, int selectedLinkId)
        {
             
            this.FileId = FileId;
            if (selectedLinkId > 0)
            {
                LinkRepository lRepository = new LinkRepository(connectionString);
                lRepository.Delete(selectedLinkId);
            }
            Refresh();
        }

        private void Refresh()
        {
            BaseURL = $"{Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            if (FileId > 0)
            {
                LinkRepository lRepository = new LinkRepository(connectionString);
                Links = lRepository.GetAll(FileId).ToList();
            }
        }
    }
}