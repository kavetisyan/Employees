using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
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

namespace Employees
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        static string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\Intern_1\\source\\repos\\Employees\\Employees\\Employees.mdf; Integrated Security = True";
        
        HttpClient client = new HttpClient();

        public EditWindow()
        {
            client.BaseAddress = new Uri("https://localhost:44384/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            InitializeComponent();
        }

        private async void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            await client.DeleteAsync("employee/" + Id.Content);
            MessageBox.Show("Employee Data Deleted");

            //using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            //{
            //    sqlConnection.Open();
            //    SqlCommand cmd = new SqlCommand(@"DELETE FROM Employees WHERE Id='" + Id.Content + "'", sqlConnection);
            //    cmd.ExecuteNonQuery();
            //    MessageBox.Show("Data deleted successfully");
            //    sqlConnection.Close();
            //}
        }
        private async void Edit_Button_Click(object sender, RoutedEventArgs e)
        {

            Employee employee = new Employee();
            employee.FirstName = FirstName.Text;
            employee.LastName = LastName.Text;
            employee.Speciality = Speciality.Text;
            employee.Salary = int.Parse(Salary.Text);
            employee.BirthDate = DateTime.Parse(BirthDate.Text);
            employee.EmployementDate = DateTime.Parse(EmployementDate.Text);

            await client.PutAsJsonAsync("employee/" + Id.Content, employee);
            MessageBox.Show("Employee Data Deleted");


            //using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            //{
            //    sqlConnection.Open();
            //    SqlCommand cmd = new SqlCommand("UPDATE Employees SET [First Name]=@fn," +
            //                                    "[Last Name]=@ln, [Salary]=@sl, " +
            //                                    "[Speciality]=@sp, [Birth Date]=@bd, " +
            //                                    "[Employement Date]=@ed" +
            //                                    " WHERE Id='" + Id.Content + "'", sqlConnection);
            //    cmd.Parameters.Add("@fn", SqlDbType.VarChar, 50).Value = FirstName.Text;
            //    cmd.Parameters.Add("@ln", SqlDbType.VarChar, 50).Value = LastName.Text;
            //    cmd.Parameters.Add("@bd", SqlDbType.Date).Value = BirthDate.Text;
            //    cmd.Parameters.Add("@sp", SqlDbType.VarChar, 50).Value = Speciality.Text;
            //    cmd.Parameters.Add("@ed", SqlDbType.Date).Value = EmployementDate.Text;
            //    cmd.Parameters.Add("@sl", SqlDbType.Int).Value = Salary.Text;
            //    cmd.CommandType = CommandType.Text;
            //    cmd.ExecuteNonQuery();
            //    MessageBox.Show("Employee info edited");
            //    sqlConnection.Close();
            //}
        }
    }
}
