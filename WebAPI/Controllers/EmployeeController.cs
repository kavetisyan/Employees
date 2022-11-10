using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/Employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        static string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\Intern_1\\source\\repos\\Employees\\Employees\\Employees.mdf; Integrated Security = True";

        // GET: api/Employee
        [HttpGet]
        public ActionResult<List<Employee>> Get()
        {
            List<Employee> list = new List<Employee>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(@"select * from Employees", sqlConnection);
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    list.Add(new Employee()
                    {
                        Id = (int)dataReader["Id"],
                        FirstName = dataReader["First Name"].ToString(),
                        LastName = dataReader["Last Name"].ToString(),
                        Speciality = dataReader["Speciality"].ToString(),
                        Salary = (int)dataReader["Salary"],
                        BirthDate = DateTime.Parse((dataReader["Birth Date"]).ToString()),
                        EmployementDate = DateTime.Parse((dataReader["Employement Date"]).ToString())
                    });
                }
            }
            return Ok(list);
        }

        // GET: api/Employee/5/Hayk/Manukyan/Driver
        [HttpGet("search")]
        public ActionResult<List<Employee>> Get([FromBody] Employee employee)
        {
            List<Employee> list = new List<Employee>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT * FROM Employees WHERE (@id='0' OR Id = @id)
                                                AND (@fn='' OR [First Name] = @fn)
                                                AND (@ln='' OR [Last Name] = @ln)
                                                AND (@sp='' OR [Speciality] = @sp)", sqlConnection);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = employee.Id.ToString() == "" ? 0 : employee.Id;
                cmd.Parameters.Add("@fn", SqlDbType.VarChar, 50).Value = employee.FirstName;
                cmd.Parameters.Add("@ln", SqlDbType.VarChar, 50).Value = employee.LastName;
                cmd.Parameters.Add("@sp", SqlDbType.VarChar, 50).Value = employee.Speciality;
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    list.Add(new Employee()
                    {
                        Id = (int)dataReader["Id"],
                        FirstName = dataReader["First Name"].ToString(),
                        LastName = dataReader["Last Name"].ToString(),
                        Speciality = dataReader["Speciality"].ToString(),
                        Salary = (int)dataReader["Salary"],
                        BirthDate = DateTime.Parse((dataReader["Birth Date"]).ToString()),
                        EmployementDate = DateTime.Parse((dataReader["Employement Date"]).ToString())
                    });
                }
                sqlConnection.Close();
            }
            return Ok(list);
        }


        // GET api/Employee/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }



        


        // POST api/Employee
        [HttpPost]
        public void Post([FromBody] Employee employee)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlDataAdapter adap = new SqlDataAdapter();
                string cmdtext = "insert into Employees([First Name], [Last Name],[Birth Date],[Speciality] ,[Employement Date],[Salary] )" +
                                " values(@fn,@ln,@bd,@sp,@ed,@sl)";
                using (SqlCommand cmd = new SqlCommand(cmdtext, sqlConnection))
                {
                    cmd.Parameters.Add("@fn", SqlDbType.VarChar, 50).Value = employee.FirstName;
                    cmd.Parameters.Add("@ln", SqlDbType.VarChar, 50).Value = employee.LastName;
                    cmd.Parameters.Add("@bd", SqlDbType.Date).Value = employee.BirthDate;
                    cmd.Parameters.Add("@sp", SqlDbType.VarChar, 50).Value = employee.Speciality;
                    cmd.Parameters.Add("@ed", SqlDbType.Date).Value = employee.EmployementDate;
                    cmd.Parameters.Add("@sl", SqlDbType.Int).Value = employee.Salary;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
        }

        // PUT api/Employee/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Employee employee)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Employees SET [First Name]=@fn," +
                                                "[Last Name]=@ln, [Salary]=@sl, " +
                                                "[Speciality]=@sp, [Birth Date]=@bd, " +
                                                "[Employement Date]=@ed" +
                                                " WHERE Id='" + id + "'", sqlConnection);
                cmd.Parameters.Add("@fn", SqlDbType.VarChar, 50).Value = employee.FirstName;
                cmd.Parameters.Add("@ln", SqlDbType.VarChar, 50).Value = employee.LastName;
                cmd.Parameters.Add("@bd", SqlDbType.Date).Value = employee.BirthDate;
                cmd.Parameters.Add("@sp", SqlDbType.VarChar, 50).Value = employee.Speciality;
                cmd.Parameters.Add("@ed", SqlDbType.Date).Value = employee.EmployementDate;
                cmd.Parameters.Add("@sl", SqlDbType.Int).Value = employee.Salary;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        // DELETE api/Employee/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(@"DELETE FROM Employees WHERE Id='" + id + "'", sqlConnection);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
