using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainApp.Core.Models
{
    public class WagonModel
    {
        public int WagonId { get; set; }
        public int WagonPosition { get; set; }
        public int NumberOfChairs { get; set; }
        public int TrainId { get; set; }
        public List<ChairModel> Chairs { get; set; }
    }
}
