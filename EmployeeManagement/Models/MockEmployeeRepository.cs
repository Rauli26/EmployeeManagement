using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee(){Id=1, Name="Rauli",Department=Dept.HR,Email="rauli4u@gmail.com"},
                new Employee() { Id = 2, Name = "Sony", Department = Dept.IT, Email = "sony4u@gmail.com" },
                new Employee(){Id=3, Name="Varsha",Department=Dept.Payroll,Email="varsha@gmail.com"}
            };
        }
        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }

       public IEnumerable<Employee> GetAllEmployee()
        {
            return _employeeList;
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id )+ 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Update(Employee employeeChanges)
        {
           Employee employee = _employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);

            if(employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }

        public Employee Delete(int Id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == Id);
            if(employee != null)
            {
                _employeeList.Remove(employee);
            }

            return employee;
        }
    }
}
