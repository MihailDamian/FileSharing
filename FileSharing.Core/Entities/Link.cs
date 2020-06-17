using System;
using System.Collections.Generic;
using System.Text;

namespace FileSharing.Core.Entities
{
    public class Link
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public File File { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
        public int Count { get; set; }
        public Guid PublicId { get; set; }
    }
}
