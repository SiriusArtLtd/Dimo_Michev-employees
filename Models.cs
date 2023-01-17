using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmaProject
{
    //EmpID,ProjectID,DateFrom,DateTo
    public class ProjectData
    {
        public int EmpID { get; set; }
        public int ProjectID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ProjectData(string empID, string projectID, string dateFrom, string dateTo)
        {
            EmpID = Convert.ToInt32(empID);
            ProjectID = Convert.ToInt32(projectID);
            DateFrom = Extention.ConvertDate(dateFrom);
            DateTo = Extention.ConvertDate(dateTo);
        }
    }

    public class DaysWorked
    {
        public int EmpID1 { get; set; }
        public int EmpID2 { get; set; }
        public int ProjectID { get; set; }
        public int Days { get; set; }
        public DaysWorked(int empID1, int empID2, int projectID, int days)
        {
            EmpID1 =  empID1;
            EmpID2 = empID2;
            ProjectID =   projectID;
            Days = days;
        }
    }

}
