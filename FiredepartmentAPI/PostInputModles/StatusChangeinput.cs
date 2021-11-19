using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiredepartmentAPI.PostInputModles
{
    public class StatusChangeinput
    {
    
  
        public string ItemId { get; set; }
     
     public int PlaceId { get; set; }
        public int Beforechange { get; set; }
        public int StatusCode { get; set; }
        public string Postscript { get; set; }
        public string UserId { get; set; }
      
    }
}
