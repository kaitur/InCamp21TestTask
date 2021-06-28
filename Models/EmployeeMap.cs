using CsvHelper.Configuration;

namespace InCamp21TestTask.Models{
    sealed class EmployeeMap : ClassMap<Employee>{
       public EmployeeMap(){
            Map(m => m.Name).Name("Employee Name");
            Map(m => m.Date).Name("Date");
            Map(m => m.Hours).Name("Work Hours");
        }
    }
}