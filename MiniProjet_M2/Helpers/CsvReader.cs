using System;
using System.Collections.Generic;
using System.IO;
using MiniProjet_M2.Models;

namespace MiniProjet_M2.Helpers;

public class CsvReader
{
    public static List<Employee> ReadCsvFile(string filePath)
    {
        var employees = new List<Employee>();

        try
        {
            using (var reader = new StreamReader(filePath))
            {
                // Skip the header line
                reader.ReadLine();

                // Read each line from the CSV file
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var employee = new Employee
                    {
                        Id = int.Parse(values[0]),
                        Name = values[1],
                        CurrentMotivation = double.Parse(values[4]),
                        MotivationA1 = double.Parse(values[5]),
                        MotivationA2 = double.Parse(values[6]),
                        MotivationA3 = double.Parse(values[7]),
                        MotivationA4 = double.Parse(values[9]),
                    };

                    employees.Add(employee);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading CSV file: {ex}");
        }

        return employees;
    }
}