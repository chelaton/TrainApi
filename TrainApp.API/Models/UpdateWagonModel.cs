﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainApp.API.Models
{
    public class UpdateWagonModel
    {
        public int WagonId { get; set; }
        public int NumberOfChairs { get; set; }
        public ICollection<UpdateChairModel> Chairs { get; set; }
    }
}
