using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FiredepartmentAPI.DbModels
{
   
    public class PlaceModel
    {
        [Key]      
        public int PlaceId { get; set; } 
        public string PlaceName { get; set; } 
        public bool todaysend { get; set; } 
        public IList<PriorityModel> PriorityList { get; set; }  
        public IList<InventoryEventModel> InventoryEventList { get; set; } 
        public IList<StatusChangeModel> StatusChangeList { get; set; } 
    }
}
