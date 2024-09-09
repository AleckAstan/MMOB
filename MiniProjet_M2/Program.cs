using System;
using System.Collections.Generic;
using MiniProjet_M2.Helpers;
using MiniProjet_M2.Models;
using MiniProjet_M2.Utils;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;
using System.Windows.Forms;

class Program
{
    private static string DATA_PATH = "../../../Assets/Data/employee_data.csv";
    static Printer printer = new Printer();

    static List<Employee> getData()
    {
        List<Employee> employees = CsvReader.ReadCsvFile(DATA_PATH);
        return employees;
    }

    // Fonction pour calculer l'écart-type
    static double CalculateStandardDeviation(double[] probabilities, double mean)
    {
        double sumOfSquares = 0.0;
        foreach (double probability in probabilities)
        {
            sumOfSquares += Math.Pow(probability - mean, 2);
        }
        return Math.Sqrt(sumOfSquares / (probabilities.Length - 1));
    }

    // Fonction pour effectuer un test t sur les probabilités
    static bool PerformTTest(double observedProbability, double hypothesizedProbability, double stdDev, int sampleSize)
    {
        double tStatistic = (observedProbability - hypothesizedProbability) / (stdDev / Math.Sqrt(sampleSize));

        // Valeur critique pour un test t bilatéral (pour un certain degré de liberté et un niveau de signification)
        double criticalValue = 1.96; // Pour un niveau de confiance de 95%
        return Math.Abs(tStatistic) > criticalValue;
    }

    static void Main(string[] args)
    {
        List<Employee> employees = getData();

        // Nombre de clusters (k)
        int clusterCount = 4;
        // Nombre d'actions (a)
        int actionCount = 4;

        // Instancier la classe KMeans et exécuter l'algorithme
        KMeans kMeans = new KMeans(clusterCount, employees);
        kMeans.ClassifyEmployees();

        int sampleSize = 5;
        int samplingRate = 10;
        
        // Obtenir les clusters et calculer le score de silhouette
        var silhouetteClusters = kMeans.getClusters();
        double silhouetteScore = SilhouetteScore.CalculateSilhouetteScore(silhouetteClusters);

        Console.WriteLine($"Score de silhouette pour K = {clusterCount}: {silhouetteScore}");
        
        double hypothesizedProbability = 0.25; // Valeur de probabilité sous H0 (hypothèse nulle)
        
        for (int a = 0; a < actionCount; a++) // action
        {
            double[,] transitionMatrix = new double[actionCount, actionCount];
            for (int k = 0; k < clusterCount; k++) // classification
            {
                var randomGenerator = new RandomGenerator();
                var clusters = kMeans.getClusters();
                double[] classificationProbabilities = new double[clusterCount];
                double[,] newClassification = new double[clusterCount, samplingRate];
                for (int i = 0; i < samplingRate; i++) // itération de l'expérimentation
                {
                    var sampleClassification = new double[clusterCount];
                    for (int j = 0; j < sampleSize; j++) // nombre d'individus 
                    {
                        var randomIndex = randomGenerator.GetRandomInt(0, clusters[k].Count);
                        var employee = clusters[k][randomIndex];
                        var motivationTarget = $"MotivationA{a + 1}";
                        string newMotivationValue =$"{employee.GetType().GetProperty(motivationTarget).GetValue(employee, null)}";
                        var newEmployeeClassification =
                            kMeans.GetClusterClassification(double.Parse(newMotivationValue));
                        sampleClassification[newEmployeeClassification] += 1;
                    }

                    for (int l = 0; l < clusterCount; l++) // résultat de la classification de chaque personne
                    {
                        newClassification[l, i] = sampleClassification[l] / sampleSize;
                    }
                }
                for (int m = 0; m < clusterCount; m++) // conversion des résultats en probabilités
                {
                    double sum = 0.0;
                    double[] sampleProbabilities = new double[samplingRate];
                    for (int o = 0; o < samplingRate; o++)
                    {
                        sum += newClassification[m, o];
                        sampleProbabilities[o] = newClassification[m, o];
                    }

                    double observedProbability = Math.Round(sum / samplingRate, 2);
                    classificationProbabilities[m] = observedProbability;

                    // Calculer la moyenne des probabilités observées
                    double mean = observedProbability;

                    // Calculer l'écart-type en fonction des probabilités
                    double stdDev = CalculateStandardDeviation(sampleProbabilities, mean);

                    // Effectuer le test t pour vérifier si nous rejetons l'hypothèse nulle
                    bool rejectNull = PerformTTest(observedProbability, hypothesizedProbability, stdDev, sampleSize);
                    if (rejectNull)
                    {
                        transitionMatrix[k, m] = observedProbability; // Accepter la probabilité observée
                    }
                    else
                    {
                        transitionMatrix[k, m] = hypothesizedProbability; // Utiliser la probabilité hypothétique sous H0
                    }
                }

                // printer.printArray(classificationProbabilities);
            }

            printer.print2DArray(transitionMatrix);
            
            Kolmogorov kolmogorov = new Kolmogorov(transitionMatrix);
            // Obtenir la matrice de transition après 2 étapes
            double[,] forecastMatrix = kolmogorov.PowerMatrix(5);
        
            // Afficher la matrice forecastMatrix
            Console.WriteLine("Matrice de prévision : ");
            printer.print2DArray(forecastMatrix);
        
            // Vérifier la probabilité de transition de l'état 0 à l'état 1 en 2 étapes
            double multiStepsProbability = kolmogorov.GetTransitionProbability(0, 1, 5);
            Console.WriteLine($"Probabilité de transition de l'état 0 à l'état 1 en 5 étapes : {multiStepsProbability}");
            
            double verifiedProbability = kolmogorov.VerifyTransitionProbability(0, 1, 2, 3);
            Console.WriteLine($"Probabilité vérifiée de transition de l'état 0 à l'état 1 en 2 + 3 étapes : {verifiedProbability}");
        }
    }
}
