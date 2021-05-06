using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainApp.API.Models
{
    public class CreateWagonModel
    {
        public int TrainId { get; set; }
        public int NumberOfChairs { get; set; }
    }
}
