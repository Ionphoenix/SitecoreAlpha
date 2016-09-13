using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Globalization;
using System.Data.Entity;

namespace SitecoreAlpha.Models
{
    public class MemberDataContext : DbContext
    {

        public MemberDataContext() : base("AAWeb")
        {
            
        }
        public DbSet<MemberInfo> MemberData { get; set; }




    }
}