using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainApp.API.Models
{
    public class UpdateTrainModel
    {
        public int TrainId { get; set; }
        public string TrainName { get; set; }
    }
}
