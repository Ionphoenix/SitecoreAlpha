using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Collections;
using Sitecore.Data.Comparers;
using Sitecore.Data.Items;

namespace SitecoreAlpha.Controllers
{
    public class SCMenuController : Controller
    {
        // GET: SCMenu
        public ActionResult Index()
        {

            Item homeItem = Sitecore.Configuration.Factory.GetDatabase("master").Items["/sitecore/content/Home"];
            ChildList children = new ChildList(homeItem);
            ItemComparer comparer = new ItemComparer();


            return View(children);
        }
    }
}