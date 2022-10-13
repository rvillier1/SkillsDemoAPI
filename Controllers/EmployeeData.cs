using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SkillsDemoAPI.Data;
using SkillsDemoAPI.Model;
using System;

namespace SkillsDemoAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class EmployeeData : ControllerBase
    {       
        private readonly ILogger<EmployeeData> _logger;
        private readonly IConfiguration Configuration;
        private string connString;

        public EmployeeData(ILogger<EmployeeData> logger, IConfiguration _configuration)
        {
            _logger = logger;
            Configuration = _configuration;
            connString = Configuration.GetConnectionString("AzureTestDB");
        }

        [HttpGet(Name = "GetEmployeeForDisplay")]
        public IActionResult Get(string SortOrder, string SortType, string EmployeeID = "0")
        {
            List<EmployeeForDisplay> emp = new List<EmployeeForDisplay>();
            try
            {
                dbContect context = new dbContect(connString);
                string sql = "";
                if (EmployeeID.Length == 0 || EmployeeID == "0")                
                    sql = $"EXECUTE dbo.GetEmployeeForDisplay '" + SortOrder + "', '" + SortType + "'";                 
                
                else
                    sql = $"EXECUTE dbo.GetEmployeeByID " + EmployeeID;
                
                emp = context.EmployeeData.FromSqlRaw(sql).ToList(); 

                if (!emp.Any())                
                    return NotFound("Not Found");

                return Ok(emp);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost(Name = "UpdateEmployee")]
        public IActionResult Post([FromBody] EmployeeForDisplay EmpResults)
        {
            List<EmployeeForDisplay> emp = new List<EmployeeForDisplay>();
            try
            {
                dbContect context = new dbContect(connString);
                string sql = "";
                if (EmpResults.EmployeeId == 0)
                    sql = "EXECUTE dbo.InsertEmployee '" + EmpResults.LastName + "', '" + EmpResults.FirstName + "', '" + EmpResults.Title + "', '" + EmpResults.BirthDate + "', '" + EmpResults.HireDate + "', '" + EmpResults.Address + "', '" + EmpResults.City + "', '" + EmpResults.Region + "', '" + EmpResults.PostalCode + "', '" + EmpResults.HomePhone + "', '" + EmpResults.Extension + "', '" + EmpResults.Notes + "'";
                else
                    sql = "EXECUTE dbo.UpdateEmployeeByID '" + EmpResults.EmployeeId + "', '" + EmpResults.LastName + "', '" + EmpResults.FirstName + "', '" + EmpResults.Title + "', '" + EmpResults.BirthDate + "', '" + EmpResults.HireDate + "', '" + EmpResults.Address + "', '" + EmpResults.City + "', '" + EmpResults.Region + "', '" + EmpResults.PostalCode + "', '" + EmpResults.HomePhone + "', '" + EmpResults.Extension + "', '" + EmpResults.Notes + "'";
                emp = context.EmployeeData.FromSqlRaw(sql).ToList();
                if (!emp.Any())
                    return NotFound("Not Found");

                return Ok(emp);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }


        [HttpDelete(Name = "DeleteEmployee")]
        public IActionResult Delete(string EmployeeID)
        {
            List<EmployeeForDisplay> emp = new List<EmployeeForDisplay>();
            try
            {
                dbContect context = new dbContect(connString);                
                string sql = "EXECUTE dbo.DeleteEmployeeByID " + EmployeeID;
                emp = context.EmployeeData.FromSqlRaw(sql).ToList();
                if (!emp.Any())
                    return NotFound("Not Found");

                return Ok(emp);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}