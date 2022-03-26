﻿using MySql.Data.MySqlClient;
using N01521496_C1_w22.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace N01521496_C1_w22.Controllers
{
    public class TeacherDataController : ApiController
    {
        // connceting api with database
        private SchoolDbConnection School = new SchoolDbConnection();

        // This method will access Teachers table from school database
        /// <summary>
        /// Returns a list of teachers in school OR
        /// a list of teacher which is searched
        /// </summary>
        /// 
        /// <example> api/TeacherData/ListTeachers/</example>
        /// <returns>
        /// list of teachers with first name and last name
        /// </returns>
        /// 
        /// <example> api/TeacherData/ListTeachers/linda</example>
        /// <returns> Linda Chan</returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{searchKey?}")]

        public List<TeacherObject> ListTeachers(string searchKey=null)
        {
            // creating a instance of connection with schooldb
            MySqlConnection Conn = School.AccessDatabase();

            // opening connection
            Conn.Open();

            // creating command which will be used to create query
            MySqlCommand cmd = Conn.CreateCommand();

            // query
            string query = "SELECT * FROM teachers";


            // =======x==x===x==x===x===x== Search functionality ===x====x====x====x=====x=====x====x==

            // it will change query if there is something in search input , also it will skip "if" if search is entered without any input
            if (searchKey != null && searchKey.Trim() != "")
            {
                query = query + " WHERE LOWER(teacherfname) LIKE @key OR LOWER(teacherlname) LIKE @key OR LOWER(CONCAT(teacherfname,teacherlname)) LIKE @key";

                // search will work with any name input also with fullname having space between fname and lname lower or upper case
                // how to remove space Ref. https://stackoverflow.com/questions/3905180/how-to-trim-whitespace-between-characters#:~:text=How%20to%20remove%20whitespaces%20between,()%20results%20%22C%20Sharp%22%20.
                
                searchKey = Regex.Replace(searchKey, @"\s", "");
                cmd.Parameters.AddWithValue("@key", "%"+searchKey+"%");
                cmd.Prepare();
            }

            cmd.CommandText = query;

            
            // =====x====x====x====x===x= Search logic ends =x=====x======x=======x========x=============

            // storing result of query in a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            // empty list of teacher names
            List<TeacherObject> Teachers = new List<TeacherObject> { };

            // looping through ResultSet
            // and getting each teachers names one by one
            while (ResultSet.Read())
            {
                // creating instance of Teacher object
                TeacherObject NewTeacher = new TeacherObject
                {
                    TeacherId = Convert.ToInt32(ResultSet["teacherid"]),
                    TeacherFname = ResultSet["teacherfname"].ToString(),
                    TeacherLname = ResultSet["teacherlname"].ToString(),
                    EmployeeNumber = ResultSet["employeenumber"].ToString(),
                    HireDate = ResultSet["hiredate"].ToString().Replace("00:00:00",""), // will remove 0s if not specifc time 
                    Salary = Convert.ToDouble(ResultSet["salary"])

                };

                Teachers.Add(NewTeacher);
            }

            return Teachers;

        }



        /// <summary>
        /// This method returns TeacherObject with detailed info of teacher with matching teacherId
        /// </summary>
        /// <param name="teacherId"></param>
        /// <example>api/TeacherData/FindOne/2</example>
        /// <returns>
        /// Detailed info of teacher with teacherId = 2
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData/FindOne/{teacherId}")]
        public TeacherObject FindOne(int teacherId)
        {
            // creating an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            // opening connection
            Conn.Open();

            // creating command which will be used to create query
            MySqlCommand cmd = Conn.CreateCommand();

            // query
            cmd.CommandText = "SELECT * FROM teachers WHERE teacherid = @key";
            cmd.Parameters.AddWithValue("@key", teacherId);
            cmd.Prepare();

            // storing result of query in a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            TeacherObject TeacherInfo = new TeacherObject();

            // looping through ResultSet
            // and getting each teachers names one by one
            while (ResultSet.Read())
            {
                TeacherInfo.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                TeacherInfo.TeacherFname = ResultSet["teacherfname"].ToString();
                TeacherInfo.TeacherLname = ResultSet["teacherlname"].ToString();
                TeacherInfo.EmployeeNumber = ResultSet["employeenumber"].ToString();
                TeacherInfo.HireDate = ResultSet["hiredate"].ToString().Replace("00:00:00", "");
                TeacherInfo.Salary = Convert.ToDouble(ResultSet["salary"]);
            }

            Conn.Close();

            return TeacherInfo;
        }

    }
}