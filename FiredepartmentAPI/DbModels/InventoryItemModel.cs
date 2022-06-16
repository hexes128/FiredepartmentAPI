using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FiredepartmentAPI.DbModels
{
   
    public class InventoryItemModel
    {
        [Key]
        public int InventoryItemId { get; set; }
        public string ItemId { get; set; }
        public int StatusBefore { get; set; } 
        public int StatusAfter { get; set; } 
        public FireitemModel FireitemsRef { get; set; }
        [JsonIgnore]
        public int EventId { get; set; } 
        [JsonIgnore]
        public InventoryEventModel InventoryEventRef { get; set; } 
    }
}
