using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainApp.Data.Entities
{
    public class Train
    {
        [Key]
        public int TrainId { get; set; }
        public string TrainName { get; set; }
        public List<Wagon> Wagons { get; set; }
    }
}
