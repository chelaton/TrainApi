using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainApp.Core.Models
{
    public class TrainModel
    {
        public int TrainId { get; set; }
        public string TrainName { get; set; }
        public List<WagonModel> Wagons { get; set; }
    }
}
