
using Firebase.Auth;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoShooter.Utils
{
    public class FirebaseUtils
    {
        public async static void uploadSingleFile(string filePath,object[] credentials)
        {            
            string ApiKey = Convert.ToString(credentials[0]);
            string AuthEmail = Convert.ToString(credentials[1]);
            string AuthPassword = Convert.ToString(credentials[2]);    
            string Bucket = Convert.ToString(credentials[3]);
            string imageName = Path.GetFileName(filePath);



            var stream = File.Open(@filePath, FileMode.Open);
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                }
                )
                .Child("images")
                .Child(imageName)
                .PutAsync(stream, cancellation.Token);
            task.Progress.ProgressChanged += (s, e) => Trace.WriteLine($"Progress: {e.Percentage} %");
            var downloadUrl = await task;


        }
    }
}
