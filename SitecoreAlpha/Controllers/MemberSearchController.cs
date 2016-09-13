using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SitecoreAlpha.Models;
using System.Threading.Tasks;
using PagedList;
using PagedList.Mvc;
using Sitecore.StringExtensions;
using Sitecore.Mvc.Controllers;

namespace SitecoreAlpha.Controllers
{
    public class MemberSearchController : Controller
    {
        private MemberDataContext _db = new MemberDataContext();
        // GET: MemberSearch
        public ActionResult Index()
        {


            return View();
        }

      [HttpGet]
        public ActionResult Display(string lname, string sortorder, string statecd,
                                                int? page,string country,string areacode,string city,string zip,string spec,string language,string filteroption)
        {





           

          if (page == null)
          {
              ViewBag.CurrentFilter = lname + ";" + statecd + ";" + country + ";" + areacode + ";" + city + ";" + zip +
                                      ";" + spec + ";" + language;
          }
          else
          {
              string[] filters = filteroption.Split(';');
              lname       = string.IsNullOrEmpty(filters[0]) ? "" : filters[0];
              statecd     = string.IsNullOrEmpty(filters[1]) ? "" : filters[1];
              country     = string.IsNullOrEmpty(filters[2]) ? "" : filters[2];
              areacode    = string.IsNullOrEmpty(filters[3]) ? "" : filters[3];
              city        = string.IsNullOrEmpty(filters[4]) ? "" : filters[4];
              zip         = string.IsNullOrEmpty(filters[5]) ? "" : filters[5];
              spec        = string.IsNullOrEmpty(filters[6]) ? "" : filters[6];
              language    = string.IsNullOrEmpty(filters[7]) ? "" : filters[7];


              ViewBag.CurrentFilter = filteroption;

          }


            string statequery = "SELECT CUST_ID , mem.DISPLAY_NM, LAST_NM, Country_NM, Streetaddress1, streetaddress2, cityname," +
                               "state_cd, phonenum, phoneext,email,CERTTEXT,SPECIALTYDESC,ATTRIBUTE_CD " +
                               "FROM AANSMembers mem " +
                               "LEFT JOIN " +
                               "(SELECT * FROM ( SELECT *, ROW_NUMBER() OVER (PARTITION BY memberid ORDER BY attribute_TY DESC) " +
                               "AS rn FROM aansmemberattrib where ATTRIBUTE_TY = 'LANGUAGE') temptable " +
                               "WHERE rn = '1') attrib " +
                               "ON mem.CUST_ID = attrib.MEMBERID " +
                               "ORDER BY " +
                               "LAST_NM, FIRST_NM";

            var MemberData =  _db.Database.SqlQuery<MemberInfo>(statequery).ToList();

            
            
            //var test = from s in _db.MemberData
            //           orderby s.country_nm
            //           select s;

            ViewBag.Country = MemberData.OrderBy(s=>s.country_nm)
                            .Select(s=>s.country_nm)
                            .Distinct();

            ViewBag.Specialty = MemberData.OrderBy(s => s.specialtydesc)
                            .Select(s => s.specialtydesc)
                            .Distinct();

            ViewBag.Language = MemberData.OrderBy(s => s.attribute_cd)
                            .Select(s => s.attribute_cd)
                            .Distinct();

          ViewBag.lname = lname;
          ViewBag.area = areacode;
          ViewBag.citycode = city;
          ViewBag.state = statecd;
          ViewBag.zip = zip;
          ViewBag.selcountry = !string.IsNullOrEmpty(country) ? country : "";
          ViewBag.spec = spec;
          ViewBag.langtest = !string.IsNullOrEmpty(language) ? language : "";

            string query =
                "SELECT  CUST_ID, mem.DISPLAY_NM, LAST_NM, Country_NM, Streetaddress1, streetaddress2,cityname," +
                "state_cd, phonenum, phoneext,email,CERTTEXT,SPECIALTYDESC,ATTRIBUTE_CD " +
                "FROM AANSMembers mem " +
                "LEFT JOIN " +
                "(SELECT * FROM ( SELECT *, ROW_NUMBER() OVER (PARTITION BY memberid ORDER BY attribute_TY DESC) " +
                "AS rn FROM aansmemberattrib where ATTRIBUTE_TY = 'LANGUAGE') temptable " +
                "WHERE rn = '1') attrib " +
                "ON mem.CUST_ID = attrib.MEMBERID " +
                "WHERE " +
                "(mem.POSTAL_CD LIKE @zip  or @zip is null) " +
                "AND " +
                "(mem.LAST_NM LIKE @LastName or @LastName is null) " +
                "AND " +
                "(mem.PHONENUM LIKE @PhoneCd or @PhoneCd is null) " +
                "AND " +
                "(mem.CITYNAME = @city or @city is null) " +
                "AND " +
                "(mem.STATE_CD = @State or @State is null) " +
                "AND " +
                "(mem.COUNTRY_NM = @country or @Country is null) " +
                "AND " +
                "(mem.specialtydesc = @spec or @spec is null) " +
                "AND " +
                "(attrib.ATTRIBUTE_CD = @Language or @Language is null) " +
                "Order By " +
                "mem.FIRST_NM,mem.LAST_NM";



            string callSP = "exec ListDirectoryMember @Country = {0},@Lastname = {1},@State = {2},@Phonecd = {3},@City = {4},@Spec = {5},@Zip = {6},@Language = {7}";

            //  var param = new SqlParameter {ParameterName = "viUserId", Value = userId};
            var Member = _db.Database.SqlQuery<MemberInfo>(query,

                    new SqlParameter("Lastname", string.IsNullOrEmpty(lname) ? (object)DBNull.Value : lname + "%"),
                    new SqlParameter("State", string.IsNullOrEmpty(statecd) ? (object)DBNull.Value : statecd),
                    new SqlParameter("Phonecd", string.IsNullOrEmpty(areacode) ? (object)DBNull.Value : "(" + areacode + ")%"),
                    new SqlParameter("City", string.IsNullOrEmpty(city) ? (object)DBNull.Value : city),
                    new SqlParameter("Zip", string.IsNullOrEmpty(zip) ? (object)DBNull.Value : zip),
                    new SqlParameter("country", string.IsNullOrEmpty(country) ? (object)DBNull.Value : country),
                    new SqlParameter("Language", string.IsNullOrEmpty(language) ? (object)DBNull.Value : language),
                    new SqlParameter("Spec", string.IsNullOrEmpty(spec) ? (object)DBNull.Value : spec)


        //new object[]
        //{
        //    new SqlParameter("Lastname", string.IsNullOrEmpty(lname) ? (object)DBNull.Value : lname),
        //    new SqlParameter("State",string.IsNullOrEmpty(statecd) ? (object)DBNull.Value : statecd),
        //    new SqlParameter("Phonecd", string.IsNullOrEmpty(areacode) ? (object)DBNull.Value : areacode),
        //    new SqlParameter("City",string.IsNullOrEmpty(city) ? (object)DBNull.Value : city),
        //    new SqlParameter("Zip",string.IsNullOrEmpty(zip) ? (object)DBNull.Value : zip),
        //    new SqlParameter("country",string.IsNullOrEmpty(country) ? (object)DBNull.Value : country),
        //    new SqlParameter("Language",string.IsNullOrEmpty(language) ? (object)DBNull.Value : language),
        //    new SqlParameter("Spec",string.IsNullOrEmpty(spec) ? (object)DBNull.Value : spec)


        //}
        ).ToList();

            //var Member = await _db.Database.SqlQuery<MemberInfo>(callSP,
            //    string.IsNullOrEmpty(country) ? (object)DBNull.Value : country,
            //    string.IsNullOrEmpty(lname) ? (object)DBNull.Value : lname + "%",
            //    string.IsNullOrEmpty(statecd) ? (object)DBNull.Value : statecd,
            //    string.IsNullOrEmpty(areacode) ? (object)DBNull.Value : "(" + areacode + ")%",
            //    string.IsNullOrEmpty(city) ? (object)DBNull.Value : city,
            //    string.IsNullOrEmpty(spec) ? (object)DBNull.Value : spec,
            //    string.IsNullOrEmpty(zip) ? (object)DBNull.Value : zip,
            //    string.IsNullOrEmpty(language) ? (object)DBNull.Value : language



            //    ).ToListAsync();

            //       new SqlParameter("p",id +"%")).ToListAsync();

            //var Member = MemberData
            //    .Where(d => d.Last_NM.StartsWith(Lname))
            //    .Where(d => (d.phonenum.Contains(areacode) || (string.IsNullOrEmpty(d.phonenum) ?? "")))
            //    .OrderBy(s => s.Last_NM)
            //    .ToList();
            //string lastname = "";
            //lastname = string.IsNullOrEmpty(Lname) ? "" : Lname;
            //areacode = string.IsNullOrEmpty(areacode) ? "" : areacode;
            //var Memberdatatest = from d in MemberData
            //                     where d.Last_NM.StartsWith(lastname)
            //                     where d.phonenum.Contains(areacode)

            //                     select d;
            //List<MemberInfo> Member = new List<MemberInfo>(Memberdatatest);






            switch (sortorder)
            {
                case "desc":
                    ViewBag.SortOrder = "desc";
                    Member = Member.OrderByDescending(s =>s.Last_NM).ToList();
                    break;

                case "fname":
                    ViewBag.SortOrder = "fname";
                    Member = Member = Member.OrderByDescending(s => s.Display_Nm).ToList();
                    break;

                default:
                    ViewBag.SortOrder = "";
                    Member = Member.OrderBy(s => s.Last_NM).ToList();
                    break;
            }

            var memberpaged = Member.ToPagedList(1, 10); 
           // var memlist = new List<MemberInfo> {Member};
            //var PagedMember = Member.
            int pageNumber = (page ?? 1);
           Member.ToPagedList(pageNumber, 10);
            return View(Member.ToPagedList(pageNumber, 10));
        }



        public JsonResult AjaxLoader()
        {
            System.Threading.Thread.Sleep(2000);
            return Json("Ok!", JsonRequestBehavior.AllowGet);
        }


    }
}