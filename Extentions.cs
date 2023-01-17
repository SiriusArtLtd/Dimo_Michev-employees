using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SirmaProject
{
    public static class Extention
    {
        public static DateTime ConvertDate(string date)
        {
            DateTime convertedDate = DateTime.Now;
            var formats = new string[]
            {
                "yyyy/MM/dd",
                "yyyy-MM-dd",
                "dd/MM/yyyy",
                "MM-dd-yyyy",
                "yyyyMMdd"
            };
            try
            {
                convertedDate = DateTime.ParseExact(date, formats, null);
            }
            catch
            {

            }
            return convertedDate;


        }
        public static DataTable ConvertToDataTable<T>(this IEnumerable<T> collection)
        {
            DataTable dt = new DataTable("DataTable");
            Type t = typeof(T);
            PropertyInfo[] pia = t.GetProperties();

            
            foreach (PropertyInfo pi in pia)
            {
                Type ColumnType = pi.PropertyType;
                if ((ColumnType.IsGenericType))
                {
                    ColumnType = ColumnType.GetGenericArguments()[0];
                }
                dt.Columns.Add(pi.Name, ColumnType);
            }

            
            foreach (T item in collection)
            {
                DataRow dr = dt.NewRow();
                dr.BeginEdit();
                foreach (PropertyInfo pi in pia)
                {
                    if (pi.GetValue(item, null) != null)
                    {
                        dr[pi.Name] = pi.GetValue(item, null);
                    }
                }
                dr.EndEdit();
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }



}
