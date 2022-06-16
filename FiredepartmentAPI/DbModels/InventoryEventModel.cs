using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FiredepartmentAPI.DbModels
{
    public class InventoryEventModel
    {
        [Key]
        public int EventId { get; set; }
        public string UserName { get; set; }
        [JsonIgnore]
        public int PlaceId { get; set; }
        public DateTime InventoryDate { get; set; }
        [JsonIgnore]
        public PlaceModel PlaceRef { get; set; }
        public IList<InventoryItemModel> InventoryItemList { get; set; } 
    }
}
