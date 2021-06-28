using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using InCamp21TestTask.Models;

namespace InCamp21TestTask.processors
{
    // Читайте ReadMe !!!
    class TransformTable
    {
        private readonly string Path;
        private Lazy<SortedSet<DateTime>> DateTimes = new Lazy<SortedSet<DateTime>>(() => new SortedSet<DateTime>());
        private List<Employee> employees;
        public TransformTable(string path) => Path = path;
        private List<Employee> GetEmployees()
        {
            using var reader = new StreamReader(Path);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvReader.Configuration.Delimiter = ",";
            csvReader.Configuration.RegisterClassMap<EmployeeMap>();
            return csvReader.GetRecords<Employee>().OrderBy(name => name.Name).ToList();
        }

        public void TableFormation()
        {
            using var writer = new StreamWriter("OutputFile.csv");
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            employees = GetEmployees();
            SetDates();
            FillingHeader(csv);
            FillingFields(csv);
        }

        private void SetDates() => employees.ForEach(employee => DateTimes.Value.Add(employee.Date));

        private void FillingHeader(CsvWriter csvWriter)
        {
            csvWriter.WriteField("Name/Date");
            foreach (DateTime date in DateTimes.Value)
            {
                csvWriter.WriteField(date.ToString("yyyy-MM-dd"));
            }
            csvWriter.NextRecord();
        }

        private void FillingFields(CsvWriter csvWriter)
        {
            List<Employee> currentEmpoyee = new List<Employee>();

            foreach (Employee employee in employees)
            {
                currentEmpoyee.Add(employee);
                if ((!currentEmpoyee.ElementAt(0).Name.Equals(employee.Name))
                    || employees.IndexOf(employee).Equals(employees.Count - 1))
                {
                    csvWriter.WriteField(currentEmpoyee.ElementAt(0).Name);
                    foreach (DateTime date in DateTimes.Value)
                    {
                        if (currentEmpoyee.Count > 0 && DateTime.Equals(date, currentEmpoyee.ElementAt(0).Date))
                        {
                            csvWriter.WriteField(currentEmpoyee.ElementAt(0).Hours);
                            currentEmpoyee.RemoveAt(0);
                        }
                        else
                        {
                            csvWriter.WriteField("0");
                        }
                    }
                    csvWriter.NextRecord();
                    currentEmpoyee.Clear();
                    currentEmpoyee.Add(employee);
                }
            }
        }
    }
}
