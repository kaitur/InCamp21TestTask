using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using InCamp21TestTask.Models;

namespace InCamp21TestTask.processors
{
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
    }
}
