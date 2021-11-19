using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiredepartmentAPI.DbModels;
using static FiredepartmentAPI.Dbcontext.FiredepartmentDbContext;

namespace FiredepartmentAPI.PostInputModles
{
    public class InventoryInput
    {

        public string UserId { get; set; }
        public int PlaceId { get; set; }

        public List<InventoryItemModel> InventoryItemList { get; set; }
    }
}
