﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo2.Models.ViewModels
{
    public class WalkerProfileViewModel
    {
       public Walker Walker { get; set; }
        public List<Walk> Walks { get; set; }
    }
}
