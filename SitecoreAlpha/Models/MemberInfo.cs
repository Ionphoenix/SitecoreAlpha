using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;

namespace SitecoreAlpha.Models
{
    public class MemberInfo 
    {


        [Key]
        public string Cust_ID  { get; set; }
        public string Display_Nm { get; set; }
        public string Last_NM { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string cityname { get; set; }
        public string state_cd { get; set; }
        public string phonenum { get; set; }
        public string phoneext { get; set; }
        public string email { get; set; }
        public string certtext { get; set; }
        public string specialtydesc { get; set; }

        public string attribute_cd { get; set; }

        public string country_nm { get; set; }

        
        // public string language { get; set; }

    }

}