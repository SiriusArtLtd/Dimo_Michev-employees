using Itenso.TimePeriod;
using SimpleCsv.Reader;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SirmaProject
{
    public partial class Form1 : Form
    {
        private OpenFileDialog openFileDialog1;
  


        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog()
            {
                FileName = "Select a csv file",
                Filter = "Text files (*.csv)|*.csv",
                Title = "Open data file"
            };
        }

        public static IEnumerable<(T, T)> ValuePairs<T>(IList<T> source)
        {
            return source.SelectMany((_, i) => source.Where((_, j) => i < j),
                (x, y) => (x, y));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> lines = new List<string>();
            List<ProjectData> data = new List<ProjectData>();
            List<DaysWorked> daysListMax = new List<DaysWorked>();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var filePath = openFileDialog1.FileName;
                    CsvReader cr11 = new CsvReader(filePath, ',', "\r\n", '\"');
                    var header = cr11.ReadRow();

                    while ((lines = cr11.ReadRow()) != null)
                    {
                        if (lines.Count == 4)
                        {
                            var dataArr = lines.ToArray();
                            ProjectData pd = new ProjectData(dataArr[0].Trim(), dataArr[1].Trim(), dataArr[2].Trim(), dataArr[3].Trim());
                            data.Add(pd);
                        }

                    }
                    List<int> projectList = data.Select(x => x.ProjectID).Distinct().ToList();

                    foreach (int project in projectList)
                    {
                        List<DaysWorked> daysList = new List<DaysWorked>();
                        foreach ((ProjectData x, ProjectData y) in ValuePairs(data.Where(p => p.ProjectID == project).ToList()))
                        {

                            Console.WriteLine($"Pair: {x}, {y}");
                            TimePeriodCollection periods = new TimePeriodCollection();
                            periods.Add(new TimeRange(x.DateFrom, x.DateTo));
                            periods.Add(new TimeRange(y.DateFrom, y.DateTo));

                            TimePeriodIntersector<TimeRange> periodIntersector =
                                                      new TimePeriodIntersector<TimeRange>();
                            ITimePeriodCollection intersectedPeriods = periodIntersector.IntersectPeriods(periods);

                            foreach (ITimePeriod intersectedPeriod in intersectedPeriods)
                            {

                                daysList.Add(new DaysWorked(x.EmpID, y.EmpID, project, intersectedPeriod.Duration.Days));
                            }

                        }
                        if (daysList!.Count > 0)
                        {
                            DaysWorked dw = daysList.MaxBy(x => x.Days);
                            daysListMax.Add(dw);
                        }



                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }

                DataTable dt = Extention.ConvertToDataTable(daysListMax.OrderByDescending(x => x.Days));

                dt.Columns[0].ColumnName = "Employee ID #1";
                dt.Columns[1].ColumnName = "Employee ID #2";
                dt.Columns[2].ColumnName = "Project ID";
                dt.Columns[3].ColumnName = " Days worked";

                dataGridView1.DataSource = dt;
                dataGridView1.AutoResizeColumns(
                  DataGridViewAutoSizeColumnsMode.AllCells);
            }
        }
    }
}