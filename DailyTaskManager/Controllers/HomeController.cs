using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DailyTaskManager.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DailyTaskManager.Controllers
{
    public class HomeController : Controller
    {


        private SqlConnection con;
        private List<SelectListItem> listSelectListItem = new List<SelectListItem>();
        private void connectionstring()
        {
            string _con = ConfigurationManager.ConnectionStrings["TaskManagerDB"].ToString();
            con = new SqlConnection(_con);
        }
        // GET: Home

        public ActionResult Index()
        {
            TaskDetails t = new TaskDetails();
            GetTaskName();
            if (listSelectListItem.Count > 0)
            {


                t.TaskNames = listSelectListItem;
            }

            return View(t);
        }

        [HttpPost]
        public ActionResult Index(TaskDetails tmodel)
        {
            if (AddToTaskList(tmodel))
            {
                ViewBag.Message = "Task added in db";

                GetTaskName();
                tmodel.TaskNames = listSelectListItem;

            }

            return View(tmodel);
        }


        public ActionResult Reminder()
        {
            return PartialView("Reminder");
        }

        [HttpPost]
        public ActionResult UpdateOrDelete(string ID, bool ischecked, string command)
        {
            TaskDetails tmodel = new TaskDetails();

            if (UpdateOrDeleteTaskList(ID, ischecked, command))
            {

                if (command == "delete")
                {
                    return Json("/Home/Index", JsonRequestBehavior.AllowGet);

                }
            }

            return View();
        }






        //Add TaskName to Table
        public bool AddToTaskList(TaskDetails t)
        {
            connectionstring();
            SqlCommand com = new SqlCommand("AddtoTaskList", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@TaskName", t.TaskName);
            com.Parameters.AddWithValue("@Tdate", t.Tdate);
            com.Parameters.AddWithValue("@checked", 0);
            com.Parameters.AddWithValue("@Isdelete", 0);
            con.Open();
            int _success = com.ExecuteNonQuery();
            con.Close();
            if (_success > 0)
            {
                return true;
            }

            return false;
        }


        public void GetTaskName()
        {

            connectionstring();
            SqlCommand com = new SqlCommand("Select id,TaskName,checked from TaskList where  isdelete!=1", con);
            com.CommandType = CommandType.Text;
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                SelectListItem s = new SelectListItem()
                {
                    Text = reader["TaskName"].ToString(),
                    Value = reader["id"].ToString(),
                    Selected = (bool)reader["checked"]

                };
                listSelectListItem.Add(s);

            }

        }
        public bool UpdateOrDeleteTaskList(string ID, bool ischecked, string command)
        {
            connectionstring();
            SqlCommand com = new SqlCommand("UpdateOrDeleteTaskList", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id", ID);
            com.Parameters.AddWithValue("@ischeckdel ", ischecked);
            com.Parameters.AddWithValue("@command", command);
            con.Open();
            int _success = 1;
            com.ExecuteNonQuery();
            con.Close();
            if (_success > 0)
            {
                return true;
            }

            return false;


        }


    }
}