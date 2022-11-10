using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Employees
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        static string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\Intern_1\\source\\repos\\Employees\\Employees\\Employees.mdf; Integrated Security = True";

        HttpClient client = new HttpClient();

        public AddWindow()
        {
            client.BaseAddress = new Uri("https://localhost:44384/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            InitializeComponent();
        }
        private async void Add_Button_Click(object sender, RoutedEventArgs e)
        {

            Employee employee = new Employee();
            employee.FirstName = FirstName.Text;
            employee.LastName = LastName.Text;
            employee.Speciality = Speciality.Text;
            employee.Salary = int.Parse(Salary.Text);
            employee.BirthDate = DateTime.Parse(BirthDate.Text);
            employee.EmployementDate = DateTime.Parse(EmployementDate.Text);

            await client.PostAsJsonAsync("employee", employee);

            MessageBox.Show("New Employee Added");

            //using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            //{
            //    sqlConnection.Open();
            //    SqlDataAdapter adap = new SqlDataAdapter();
            //    string cmdtext = "insert into Employees([First Name], [Last Name],[Birth Date],[Speciality] ,[Employement Date],[Salary] )" +
            //                    " values(@fn,@ln,@bd,@sp,@ed,@sl)";
            //    using (SqlCommand cmd = new SqlCommand(cmdtext, sqlConnection))
            //    {
            //        cmd.Parameters.Add("@fn", SqlDbType.VarChar, 50).Value = FirstName.Text;
            //        cmd.Parameters.Add("@ln", SqlDbType.VarChar, 50).Value = LastName.Text;
            //        cmd.Parameters.Add("@bd", SqlDbType.Date).Value = BirthDate.Text;
            //        cmd.Parameters.Add("@sp", SqlDbType.VarChar, 50).Value = Speciality.Text;
            //        cmd.Parameters.Add("@ed", SqlDbType.Date).Value = EmployementDate.Text;
            //        cmd.Parameters.Add("@sl", SqlDbType.Int).Value = Salary.Text;
            //        cmd.CommandType = CommandType.Text;
            //        cmd.ExecuteNonQuery();
            //        MessageBox.Show("New Employee Added");
            //    }
            //    sqlConnection.Close();
            //}
        }
    }
}