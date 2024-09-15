using MiniProjet_M2.Helpers;
using MiniProjet_M2.Models;
using MiniProjet_M2.Utils;

class Program
{
    private static string DATA_PATH = "../../../Assets/Data/employee_data.csv";
    static Printer printer = new Printer();
    static PowerIteration powerIteration = new PowerIteration();

    static List<Employee> getData()
    {
        List<Employee> employees = CsvReader.ReadCsvFile(DATA_PATH);
        return employees;
    }

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

        // Number of clusters (k)
        int clusterCount = 4;
        // Number of actions (a)
        int actionCount = 4;

        // Instantiate the KMeans class and run the algorithm
        KMeans kMeans = new KMeans(clusterCount, employees);
        kMeans.ClassifyEmployees();

        int sampleSize = 5;
        int samplingRate = 10;

        // Get clusters and calculate the silhouette score
        var silhouetteClusters = kMeans.getClusters();
        double silhouetteScore = SilhouetteScore.CalculateSilhouetteScore(silhouetteClusters);

        Console.WriteLine($"Silhouette Score for K = {clusterCount}: {silhouetteScore}");

        double hypothesizedProbability = 0.25; // Valeur de probabilité sous H0 (hypothèse nulle)
        List<double[,]> transitionMatrixs = new List<double[,]>();

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

                    double probability = Math.Round(sum / samplingRate, 2);
                    classificationProbabilities[m] = probability;
                    transitionMatrix[k, m] = probability;
                    double observedProbability = Math.Round(sum / samplingRate, 2);
                    classificationProbabilities[m] = observedProbability;
                    double stdDev = 0.1; // Exemple d'écart-type (à calculer en fonction des données)

                    // Effectuer le test t pour vérifier si nous rejetons l'hypothèse nulle
                    bool rejectNull = PerformTTest(observedProbability, hypothesizedProbability, stdDev, sampleSize);
                    if (rejectNull)
                    {
                        transitionMatrix[k, m] = observedProbability; // Accepter la probabilité observée
                    }
                    else
                    {
                        transitionMatrix[k, m] =
                            hypothesizedProbability; // Utiliser la probabilité hypothétique sous H0
                    }
                }
                // printer.printArray(classificationProbabilities);
            }

            printer.print2DArray(transitionMatrix);
            transitionMatrixs.Add(transitionMatrix);

            Kolmogorov kolmogorov = new Kolmogorov(transitionMatrix);
            // Obtenir la matrice de transition après 2 étapes
            double[,] forecastMatrix = kolmogorov.PowerMatrix(5);

            // Afficher la matrice forecastMatrix
            Console.WriteLine("Forecast Matrix : ");
            printer.print2DArray(forecastMatrix);

            // Vérifier la probabilité de transition de l'état 0 à l'état 1 en 2 étapes
            double multiStepsProbability = kolmogorov.GetTransitionProbability(0, 1, 5);
            Console.WriteLine($"Probability of transition from 0 to 1 in 5 steps: {multiStepsProbability}");

            double verifiedProbability = kolmogorov.VerifyTransitionProbability(0, 1, 2, 3);
            Console.WriteLine($"Verified probability of transition from 0 to 1 in 2 + 3 steps: {verifiedProbability}");
        }


        /// etape 4
        Console.WriteLine("=====================DEBUT=============================");
        int[,] politics =
        {
            { 1, 1, 1, 1 },
            { 1, 2, 2, 3 },
            { 2, 2, 3, 3 },
            { 1, 2, 3, 4 },
        };

        List<double[,]> politicMatrixs = new List<double[,]>();
        for (int i = 0; i < transitionMatrixs.Count; i++)
        {
            printer.print2DArray(transitionMatrixs[i], $"transition matrix #{i}");
        }

        for (int i = 0; i < politics.GetLength(0); i++)
        {
            double[,] politic = new double[transitionMatrixs[0].GetLength(0), transitionMatrixs[0].GetLength(1)];
            for (int j = 0; j < politic.GetLength(0); j++)
            {
                int politicIndex = politics[i, j] - 1;
                for (int k = 0; k < politic.GetLength(0); k++)
                {
                    politic[j, k] = transitionMatrixs[politicIndex][j, k];
                }
            }

            printer.print2DArray(politic, $"politic-{i}");
            politicMatrixs.Add(politic);
        }

        // Tableau des coûts (A1, A2, A3, A4) en fonction des états (E1, E2, E3, E4)
        double[,] costTable = new double[,]
        {
            // E1 (Démotivé), E2 (Plus ou moins motivé), E3 (Motivé), E4 (Très motivé)
            { 0, 0, 0, 0 }, // A1: Ne rien faire
            { 500, 400, 250, 100 }, // A2: Augmenter le salaire 5%
            { 300, 200, 100, 50 }, // A3: Créer concurrence
            { 250, 200, 150, 75 } // A4: Heure de travail flexible
        };

        Console.WriteLine("=====================CONCLUSION=============================");
        // Conclusion sur le coût minimum politique de décision
        double minCost = double.MaxValue;
        int bestPoliticIndex = -1;
        for (int i = 0; i < politicMatrixs.Count; i++)
        {
            var pi = powerIteration.resolveByPuissance(politicMatrixs[i]);
            double cost = CostMean(pi, costTable);
            Console.WriteLine($"Politic {i} Cost = {CostMean(pi, costTable)}");
            if (cost < minCost)
            {
                minCost = cost;
                bestPoliticIndex = i;
            }
        }

        Console.WriteLine($"The best decision policy is Politic {bestPoliticIndex} with a minimum cost of {minCost}");
    }


    static double CostMean(double[] pi, double[,] costTable)
    {
        double costMean = 0.0;
        int actionCount = costTable.GetLength(0);
        int stateCount = costTable.GetLength(1);

        // Vérifiez si la longueur de pi correspond au nombre d'états
        if (pi.Length != stateCount)
        {
            throw new ArgumentException(
                "La longueur du vecteur des probabilités d'état ne correspond pas au nombre d'états dans le tableau des coûts.");
        }

        // Calculer le coût moyen pour chaque action
        for (int action = 0; action < actionCount; action++)
        {
            double actionCost = 0.0;
            for (int state = 0; state < stateCount; state++)
            {
                actionCost += pi[state] * costTable[action, state];
            }

            // Ajouter le coût moyen de cette action à costMean
            costMean += actionCost;
        }

        return costMean;
    }
}