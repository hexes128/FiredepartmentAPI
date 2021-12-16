using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FiredepartmentAPI.DbModels
{

    public class editinforecord
    {
        [Key]
        public int editid { get; set; }

        public string itemid { get; set; }
        public string oldname { get; set; }
        public string newname { get; set; }

        public int oldstore { get; set; }
        public int newstore { get; set; }
        public string UserId { get; set; }
        public DateTime ChangeDate { get; set; }


    }
}
