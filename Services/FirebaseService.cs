﻿using PhotoShooter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoShooter.Services
{
    public interface FirebaseService
    {
        FirebaseUploadResult uploadToFirebaseBucket(string path);       
        
    }
}
