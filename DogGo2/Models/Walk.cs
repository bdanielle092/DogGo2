﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo2.Models
{
    public class Walk
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public int WalkerId { get; set; }
        public int DogId { get; set; }
        public int WalkStatusId { get; set; }
        public Walker Walker { get; set; }
        public Dog Dog { get; set; }
        public WalkStatus WalkStatus { get; set; }
     
    }
}
