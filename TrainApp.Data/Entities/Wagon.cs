using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TrainApp.Data.Entities
{
    public class Wagon
    {
        [Key]
        public int WagonId { get; set; }
        public int WagonPosition { get; set; }
        public int TrainId { get; set; }
        public List<Chair> Chairs { get; set; }
        public Train Train { get; set; }
    }
}
