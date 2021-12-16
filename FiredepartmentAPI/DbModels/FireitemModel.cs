using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FiredepartmentAPI.DbModels
{
    public class FireitemModel
    {
        [Key]
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public int PresentStatus { get; set; }
        public int InventoryStatus { get; set; }
        [JsonIgnore]
        public int StoreId { get; set; }
        public string postscript { get; set; }
        [JsonIgnore]
        public PriorityModel PriorityRef { get; set; }
        [JsonIgnore]
        public IList<InventoryItemModel> InventoryItemList { get; set; }
        [JsonIgnore]
        public IList<StatusChangeModel> lendFixList { get; set; }

    }
}
