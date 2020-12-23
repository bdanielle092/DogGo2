﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo2.Models.ViewModels
{
    public class ScheduleAWalkFormViewModel
    {
        public Walk Walk { get; set; }
        public List<Dog> Dogs { get; set; }
        public Walker Walker { get; set; }
        public int OwnerId { get; set; }
        public string ErrorMessage { get; internal set; }
    }
}
