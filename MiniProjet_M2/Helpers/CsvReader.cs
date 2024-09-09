using System;
using System.Collections.Generic;
using System.IO;
using MiniProjet_M2.Models;

namespace MiniProjet_M2.Helpers;

using System.Globalization;

public class CsvReader
{
    public static List<Employee> ReadCsvFile(string filePath)
    {
        var employees = new List<Employee>();
        try
        {
            using (var reader = new StreamReader(filePath))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var employee = new Employee
                    {
                        Id = int.Parse(values[0]),
                        CurrentMotivation = double.Parse(values[4].Replace('.', ',')), 
                        MotivationA1 = double.Parse(values[5].Replace('.', ',')),
                        MotivationA2 = double.Parse(values[6].Replace('.', ',')),
                        MotivationA3 = double.Parse(values[7].Replace('.', ',')),
                        MotivationA4 = double.Parse(values[9].Replace('.', ',')),
                    };
                    employees.Add(employee);
                    Console.WriteLine(employee);
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