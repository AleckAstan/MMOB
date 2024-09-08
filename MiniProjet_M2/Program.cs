using System;
using System.Collections.Generic;
using MiniProjet_M2.Helpers;
using MiniProjet_M2.Models;

class Program
{
    private static string DATA_PATH = "../../../Assets/Data/employee_data.csv";

    static List<Employee> getData()
    {
        List<Employee> employees = CsvReader.ReadCsvFile(DATA_PATH);
        return employees;
    }

    static void Main(string[] args)
    {
        List<Employee> employees = getData();

        // Number of clusters (k)
        int k = 4;

        // Instantiate the KMeans class and run the algorithm
        KMeans kMeans = new KMeans(4, employees);
        kMeans.ClassifyEmployees();
        Console.WriteLine($"0.4655 {kMeans.GetClusterClassification(0.4655)}");
        Console.WriteLine($"0.505 {kMeans.GetClusterClassification(0.505)}");
        Console.WriteLine($"0.707 {kMeans.GetClusterClassification(0.707)}");
        Console.WriteLine($"0.7715 {kMeans.GetClusterClassification(0.7715)}");
    }
}