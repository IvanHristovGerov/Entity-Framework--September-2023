using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();


            Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));

        }


        //3
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                }).ToList();



            string result = string.Join(Environment.NewLine, employees.Select(e => $"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}"));

            return result;
        }

        //4
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                }).Where(e => e.Salary > 50000)
                  .OrderBy(e => e.FirstName)
                  .ToList();

            return string.Join(Environment.NewLine, employees.Select(e => $"{e.FirstName} - {e.Salary:f2}"));
        }

        //5
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {

            var rndEmployees = context.Employees
                .Where(e => e.Department.Name == "Research and Development ")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Department.Name,
                    e.Salary

                }).OrderBy(e => e.Salary)
                  .ThenByDescending(e => e.FirstName)
                ;

            //Console.WriteLine(rndEmployees.ToQueryString());

            return string.Join(Environment.NewLine, rndEmployees.Select(e => $"{e.FirstName} {e.LastName} from {e.Name} - ${e.Salary:f2}"));
        }

        //6
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var employee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");
            //Console.WriteLine($"{employees.FirstName} {employees.LastName}");

            employee.Address = address;

            context.SaveChanges();

            var employees = context.Employees
                .Select(e => new
                {
                    e.AddressId,
                    e.Address.AddressText
                }).OrderByDescending(e => e.AddressId)
                  .Take(10)
                  .ToList();

            return string.Join(Environment.NewLine, employees.Select(e => $"{e.AddressText}"));

            
        }


        //13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();


            return string.Join(Environment.NewLine, employees.Select(
                e => $"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})"));
        }
    }

}