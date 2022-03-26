using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N01521496_C1_w22.Models
{
    public class TeacherObject
    {
        // this will be used to get and set our properties and also mention what will be the datatypes
        public int TeacherId { get; set; }
        public string TeacherFname { get; set; }
        public string TeacherLname { get; set; }
        public string EmployeeNumber { get; set; }
        public string HireDate { get; set; }
        public double Salary { get; set; }
    }
}