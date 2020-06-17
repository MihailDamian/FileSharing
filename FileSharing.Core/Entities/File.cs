using System;
using System.Collections.Generic;
using System.Text;

namespace FileSharing.Core.Entities
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateExpiredOn { get; set; }
        public Guid PublicId { get; set; }
    }
}
