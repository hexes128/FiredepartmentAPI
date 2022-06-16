using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FiredepartmentAPI.DbModels
{
   
    public class PriorityModel
    {
        [Key]
        public int StoreId { get; set; }
        [JsonIgnore]
        public int PlaceId { get; set; } 
        public string SubArea { get; set; } 
        public int PriorityNum { get; set; } 
        [JsonIgnore]
        public PlaceModel PlaceRef { get; set; } 
        public IList<FireitemModel> FireitemList { get; set; } 
  
    }
}
