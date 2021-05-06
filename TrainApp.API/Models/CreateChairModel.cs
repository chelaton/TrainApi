using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainApp.API.Models
{
    public class CreateChairModel
    {
        public bool NearWindow { get; set; }
        public int Number { get; set; }
        public bool Reserved { get; set; }
        public int WagonId { get; set; }
        public CreateWagonModel CreateWagon { get; set; }
    }
}
