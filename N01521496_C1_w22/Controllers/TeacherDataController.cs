using MySql.Data.MySqlClient;
using N01521496_C1_w22.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        // ================xxxxxx=xxxxx===== Delete function =============x==========x============x=======x
        /// <summary>
        /// This will delete teacher with given id from database
        /// </summary>
        /// <param name="id"></param>
        public void DeleteTeacher(int id)
        {
            // creating an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            // opening connection
            Conn.Open();

            // creating command which will be used to create query
            MySqlCommand cmd = Conn.CreateCommand();

            Debug.WriteLine("teacher is is " + id);

            // "DELETE t1, c1 FROM teachers t1 INNER JOIN classes c1 ON t1.teacherid = c1.teacherid WHERE t1.teacherid = @key;" + 
            // query
            string query = "DELETE t1, c1 FROM teachers t1 INNER JOIN classes c1 ON t1.teacherid = c1.teacherid WHERE t1.teacherid = @key;";


            // deleting a teacher will also remove all classes that selected teacher teaches 
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@key", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            // if a teacher do not have any classes 1st query will not run as there will be no t1.teacherid = c1.teacherid so inner join will not work
            string query2 = "DELETE FROM teachers WHERE teacherid = @key2;";
            cmd.CommandText = query2;
            cmd.Parameters.AddWithValue("@key2", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            
            Conn.Close();

        }

        // =============x==============x==========x== Add new teacher ================x==========x=======x=======x
        /// <summary>
        /// this method adds new author to database 
        /// </summary>
        /// <param name="newTeacher"></param>
        public void AddTeacher(TeacherObject newTeacher)
        {
            // creating an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            // opening connection
            Conn.Open();

            // creating command which will be used to create query
            MySqlCommand cmd = Conn.CreateCommand();

            string query = "insert into teachers (teacherfname,teacherlname,employeenumber,hiredate,salary) values (@tFname,@tLname,@empNum,CURRENT_DATE(),@tSalary)";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@tFname", newTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@tLname", newTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@empNum", newTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@tSalary", newTeacher.Salary);

            cmd.Prepare();

            cmd.ExecuteNonQuery();
            Conn.Close();



        }

        // ==x=======x===========x==========x====== Update teacher ====x=x==========x===============x============x
        public void UpdateTeacher(int teacherId, TeacherObject teacherInfo)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            string query = "update teachers set teacherfname=@t_fname, teacherlname=@t_lname, employeenumber=@t_empNum, salary=@t_salary WHERE teacherid=@t_id";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@t_fname", teacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@t_lname", teacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@t_empNum", teacherInfo.EmployeeNumber);
            cmd.Parameters.AddWithValue("@t_salary", teacherInfo.Salary);
            cmd.Parameters.AddWithValue("@t_id", teacherId);

            cmd.Prepare();
            cmd.ExecuteNonQuery();


            Conn.Close();


        }
    }
}
