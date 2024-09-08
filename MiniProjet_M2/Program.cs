using System;
using System.Collections.Generic;
using MiniProjet_M2.Helpers;
using MiniProjet_M2.Models;
using MiniProjet_M2.Utils;

class Program
{
    private static string DATA_PATH = "../../../Assets/Data/employee_data.csv";
    static Printer printer = new Printer();

    static List<Employee> getData()
    {
        List<Employee> employees = CsvReader.ReadCsvFile(DATA_PATH);
        return employees;
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
        for (int a = 0; a < actionCount; a++) // action
        {
            double[,] transitionMatrix = new double[actionCount, actionCount];
            for (int k = 0; k < clusterCount; k++) // classification
            {
                var randomGenerator = new RandomGenerator();
                var clusters = kMeans.getClusters();
                double[] classificationProbabilities = new double[clusterCount];
                double[,] newClassification = new double[clusterCount, samplingRate];
                for (int i = 0; i < samplingRate; i++) // iteration de l'experimentation
                {
                    var sampleClassification = new double[clusterCount];
                    for (int j = 0; j < sampleSize; j++) // nombre d'individus 
                    {
                        var randomIndex = randomGenerator.GetRandomInt(0, clusters[k].Count);
                        var employee = clusters[k][randomIndex];
                        var motivationTarget = $"MotivationA{a + 1}";
                        string newMotivationValue =
                            $"{employee.GetType().GetProperty(motivationTarget).GetValue(employee, null)}";
                        var newEmployeeClassification =
                            kMeans.GetClusterClassification(double.Parse(newMotivationValue));
                        sampleClassification[newEmployeeClassification] += 1;
                    }

                    for (int l = 0; l < clusterCount; l++) // resultat de la classification de chaque personne
                    {
                        newClassification[l, i] = sampleClassification[l] / sampleSize;
                    }
                }
                for (int m = 0; m < clusterCount; m++) // conversion des resultats en probabilités
                {
                    double sum = 0.0;
                    for (int o = 0; o < samplingRate; o++)
                    {
                        sum += newClassification[m, o];
                    }
                    double probability = Math.Round(sum / samplingRate,2);
                    classificationProbabilities[m] = probability;
                    transitionMatrix[k, m] = probability;
                }
                // printer.printArray(classificationProbabilities);
            }
            printer.print2DArray(transitionMatrix);
        }
    }
}