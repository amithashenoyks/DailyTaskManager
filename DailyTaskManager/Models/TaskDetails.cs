using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

using System.Configuration;

namespace DailyTaskManager.Models
{
    public class TaskDetails
    {
        private SqlConnection con;
        //private List<SelectListItem> listSelectListItem = new List<SelectListItem>();
        private void connectionstring()
        {
            string _con = ConfigurationManager.ConnectionStrings["TaskManagerDB"].ToString();
            con = new SqlConnection(_con);
        }


        public string TaskName { get; set; }
        public string Tdate { get; set; }
        public bool Tchecked { get; set; }
        public bool TDelete { get; set; }


        public IEnumerable<SelectListItem> TaskNames { get; set; }
        public IEnumerable<int> TaskId { get; set; }



       

    }

    

}