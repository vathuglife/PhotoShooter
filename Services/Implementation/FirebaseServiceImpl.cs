using FolderBrowser.Utils;
using Microsoft.Extensions.Configuration;
using PhotoShooter.Constants;
using PhotoShooter.Enums;
using PhotoShooter.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoShooter.Services.Implementation
{
    public class FirebaseServiceImpl : FirebaseService
    {
        private IConfiguration configuration;
        public FirebaseServiceImpl() { InitializeObjects(); }
        public FirebaseUploadResult uploadToFirebaseBucket(string path)
        {
            string[] credentials = GetCredentials();
            string[] imagePaths = DirectoryUtils.getFilesByExtensions(path, ExtensionConstants.ImageExtensions);
            foreach (string imagePath in imagePaths)
            {
                FirebaseUtils.uploadSingleFile(
                    imagePath,credentials
                );
            }
            return FirebaseUploadResult.SUCCESS;
        }
        private void InitializeObjects()
        {
            configuration = ConfigurationUtils.GetConfiguration();

        }
        private string[] GetCredentials()
        {
            string apiKey = configuration["firebase:apiKey"];
            string email = configuration["firebase:email"];
            string password = configuration["firebase:password"];
            string bucketUrl = configuration["firebase:bucketUrl"];
            return new string[] { apiKey, email, password, bucketUrl };
        }       
    }
}
