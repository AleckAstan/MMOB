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

    static void Print2DArray(double[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        Console.WriteLine($"--------------------");

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(array[i, j] + "\t"); // Print each element with a tab for formatting
            }

            Console.WriteLine(); // New line after each row
        }
    }

    static void printArray(double[] array)
    {
        int rows = array.Length;
        for (int i = 0; i < rows; i++)
        {
            Console.Write(array[i] + "\t");
        }

        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        List<Employee> employees = getData();

        // Number of clusters (k)
        int clusterCount = 4;
        // Number of actions (a)
        int actionCount = 4;

        // Instantiate the KMeans class and run the algorithm
        KMeans kMeans = new KMeans(clusterCount, employees);
        kMeans.ClassifyEmployees();

        int sampleSize = 5;
        int samplingRate = 10;
        // action
        double[,] transitionMatrix = new double[actionCount, actionCount];
        for (int k = 0; k < clusterCount; k++) // classification
        {
            var randomGenerator = new RandomGenerator();
            var clusters = kMeans.getClusters();
            int[,] array = new int[1, k];
            double[] classificationProbabilities = new double[clusterCount];
            double[,] newClassification = new double[clusterCount, samplingRate];
            for (int i = 0; i < samplingRate; i++)
            {
                var sampleClassification = new double[clusterCount];
                for (int j = 0; j < sampleSize; j++)
                {
                    var randomIndex = randomGenerator.GetRandomInt(0, clusters[k].Count);
                    var employee = clusters[k][randomIndex];
                    var motivationTarget = $"MotivationA{k}";
                    var propertyInfo = employee.GetType().GetProperty(motivationTarget);
                    var newEmployeeClassification =
                        kMeans.GetClusterClassification(employee.MotivationA1); //follow action
                    sampleClassification[newEmployeeClassification] += 1;
                }

                for (int l = 0; l < clusterCount; l++)
                {
                    newClassification[l, i] = sampleClassification[l] / 5;
                }
            }

            // Print2DArray(newClassification);
            for (int m = 0; m < clusterCount; m++)
            {
                double sum = 0.0;
                for (int o = 0; o < samplingRate; o++)
                {
                    sum += newClassification[m, o];
                }

                classificationProbabilities[m] = sum / samplingRate;
                transitionMatrix[k, m] = sum / samplingRate;
            }

            // printArray(classificationProbabilities);
        }
        
        Print2DArray(transitionMatrix);


        // TO DO:  for each action
        // get random people from each classification
        // check what are they new classification,
        // sum and calculate probabilities
        // build matrix
    }
}