using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Media.Animation;
using static System.Net.WebRequestMethods;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace Employees
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\Intern_1\\source\\repos\\Employees\\Employees\\Employees.mdf; Integrated Security = True";

        HttpClient client = new HttpClient();
        public MainWindow()
        {
            client.BaseAddress = new Uri("https://localhost:44384/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            
            InitializeComponent();

            DataGridTextColumn col1 = new DataGridTextColumn() { Width = 30, Header = "ID", Binding = new Binding("Id") };
            DataGridTextColumn col2 = new DataGridTextColumn() { Width = 100, Header = "First Name", Binding = new Binding("FirstName") };
            DataGridTextColumn col3 = new DataGridTextColumn() { Width = 100, Header = "Last Name", Binding = new Binding("LastName") };
            DataGridTextColumn col4 = new DataGridTextColumn() { Width = 120, Header = "Birth Date", Binding = new Binding("BirthDate") };
            DataGridTextColumn col5 = new DataGridTextColumn() { Width = 100, Header = "Speciality", Binding = new Binding("Speciality") };
            DataGridTextColumn col6 = new DataGridTextColumn() { Width = 120, Header = "Employment Date", Binding = new Binding("EmployementDate") };
            DataGridTextColumn col7 = new DataGridTextColumn() { Width = 80, Header = "Salary", Binding = new Binding("Salary") };

            Table.Columns.Add(col1);
            Table.Columns.Add(col2);
            Table.Columns.Add(col3);
            Table.Columns.Add(col4);
            Table.Columns.Add(col5);
            Table.Columns.Add(col6);
            Table.Columns.Add(col7);
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            SearchId.Text="";
            SearchFirstName.Text = "";
            SearchLastName.Text = "";
            SearchSpeciality.Text = "";
            //LoadTable();

            Table.Items.Clear();
            this.GetEmployees();

        }
        private async void GetEmployees()
        {
            var response = await client.GetStringAsync("employee");
            var employees = JsonConvert.DeserializeObject<List<Employee>>(response);
            foreach (var employee in employees)
            {
                Table.Items.Add(employee);
            }
        }
        //public void LoadTable()
        //{
        //    Table.Items.Clear();
        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        sqlConnection.Open();
        //        SqlCommand cmd = new SqlCommand(@"SELECT * FROM Employees", sqlConnection);
        //        var dataReader = cmd.ExecuteReader();
        //        var employees = GetList<Employee>(dataReader);

        //        foreach (var employee in employees)
        //        {
        //            Table.Items.Add(employee);
        //        }
        //        sqlConnection.Close();
        //    }
        //}

        private List<Employee> GetList<T>(IDataReader reader)
        {
            List<Employee> list = new List<Employee>();
            while (reader.Read())
            {
                System.Console.WriteLine(reader.ToString());
                list.Add(new Employee()
                {
                    Id = (int)reader["Id"],
                    FirstName = reader["First Name"].ToString(),
                    LastName = reader["Last Name"].ToString(),
                    Speciality = reader["Speciality"].ToString(),
                    Salary = (int)reader["Salary"],
                    BirthDate = ((DateTime)reader["Birth Date"]),
                    EmployementDate = ((DateTime)reader["Employement Date"])
                });
            }
            return list;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddWindow add = new AddWindow();
            add.Show();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditWindow edit = new EditWindow();
            Employee editingEmployee = ((FrameworkElement)sender).DataContext as Employee;
            edit.Id.Content = editingEmployee.Id;
            edit.FirstName.Text = editingEmployee.FirstName;
            edit.LastName.Text = editingEmployee.LastName;
            edit.BirthDate.Text = editingEmployee.BirthDate.ToString();
            edit.Speciality.Text = editingEmployee.Speciality;
            edit.EmployementDate.Text = editingEmployee.EmployementDate.ToString();
            edit.Salary.Text = editingEmployee.Salary.ToString();
            edit.Show();
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            Table.Items.Clear();
            //Employee searchEmployee = new Employee()
            //{
            //    Id = int.Parse(SearchId.Text),
            //    FirstName = SearchFirstName.Text,
            //    LastName = SearchLastName.Text,
            //    Speciality = SearchSpeciality.Text
            //};
            //var response = await client.GetFromJsonAsync<Employee>("employee/search");
            //var employees = JsonConvert.DeserializeObject<List<Employee>>(response);
            //foreach (var employee in employees)
            //{
            //    Table.Items.Add(employee);
            //}
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT * FROM Employees WHERE (@id='0' OR Id = @id)
                                                AND (@fn='' OR [First Name] = @fn)
                                                AND (@ln='' OR [Last Name] = @ln)
                                                AND (@sp='' OR [Speciality] = @sp)", sqlConnection);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SearchId.Text == "" ? 0 : SearchId.Text;
                cmd.Parameters.Add("@fn", SqlDbType.VarChar, 50).Value = SearchFirstName.Text;
                cmd.Parameters.Add("@ln", SqlDbType.VarChar, 50).Value = SearchLastName.Text;
                cmd.Parameters.Add("@sp", SqlDbType.VarChar, 50).Value = SearchSpeciality.Text;
                var dataReader = cmd.ExecuteReader();
                var employees = GetList<Employee>(dataReader);

                foreach (var employee in employees)
                {
                    Table.Items.Add(employee);
                }
                sqlConnection.Close();
            }
        }
    }
}
