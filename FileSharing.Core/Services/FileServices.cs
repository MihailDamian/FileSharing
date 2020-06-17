using FileSharing.Core.Entities;
using FileSharing.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileSharing.Core.Services
{
    public class FileServices
    {
        public File SaveFile(FileRepository fRepository, string fileName, int expMinutes)
        {
            if (fRepository != null)
            {
                File dbFile = new File();
                dbFile.DateCreated = DateTime.Now;
                dbFile.DateExpiredOn = dbFile.DateCreated.AddMinutes(expMinutes);
                dbFile.Name = fileName;
                dbFile.Path = "";

                dbFile.Id = fRepository.Add(dbFile);
                dbFile.Path = GetPath(dbFile);
                fRepository.Update(dbFile);

                new LinkServices().GenerateNewLink(new LinkSharing.Core.Repositories.LinkRepository(fRepository.connectionString), dbFile.Id);
                return dbFile;
            }
            else
                return null;

        }

        public string GetPath(File file)
        {
            return $"/DataFiles/{ file.DateCreated.Year }/{file.DateCreated.Month }/{file.Id}";
        }

        public bool RemoveFile(string serverPath, FileRepository fRepository, int fileId)
        {
            File file = fRepository.Get(fileId);
            if (file != null)
            {
                try
                {
                    if (System.IO.File.Exists(System.IO.Path.Combine(serverPath + file.Path, file.Name)))
                    {
                        System.IO.File.Delete(System.IO.Path.Combine(serverPath + file.Path, file.Name));
                        fRepository.Delete(fileId);
                        return true;
                    }
                    else return false;
                }
                catch (System.IO.IOException ioExp)
                {

                }
            }
            return false;
        }
    }
}
