using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FiredepartmentAPI.DbModels
{

    public class StatusChangeModel
    {
        [Key]
        public int StatusChangId { get; set; }



        [JsonIgnore]
        public int PlaceId { get; set; }
        [JsonIgnore]
        public PlaceModel PlaceRef { get; set; }

        public string ItemId { get; set; }

        public FireitemModel FireitemRef { get; set; }
        
        public int Beforechange { get; set; }
        public int StatusCode { get; set; }
        public string Postscript { get; set; }
        public string UserId { get; set; }
        public DateTime ChangeDate { get; set; }


    }
}
