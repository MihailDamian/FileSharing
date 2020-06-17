using FileSharing.Core.Entities;
using LinkSharing.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileSharing.Core.Services
{
    public class LinkServices
    {
        public Link GenerateNewLink(LinkRepository lRepository, int fileId)
        {
            if (lRepository != null)
            {
                Link l = new Link();
                l.FileId = fileId;
                l.Email = "";
                l.Count = 0;
                l.PublicId = Guid.NewGuid();
                while (lRepository.Get(l.PublicId) != null)
                {
                    l.PublicId = Guid.NewGuid();
                }
                l.Url = "/Files/Download/"+l.PublicId;
                l.Id = lRepository.Add(l);
                return l;
            }
            else
                return null;
        }
    }
}
