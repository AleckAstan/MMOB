namespace MiniProjet_M2.Utils;

public class Genetic
{
    static Printer printer = new Printer();
    static PowerIteration powerIteration = new PowerIteration();
    static int PopulationSize = 5;
    static int StateCount = 4;
    int ActionCount = 4;
    List<int[]> population = new List<int[]>();

    double[,] costTable = new double[,]
    {
        // E1 (Démotivé), E2 (Plus ou moins motivé), E3 (Motivé), E4 (Très motivé)
        { 1000, 700, 560, 540 }, // A1: Ne rien faire
        { 500, 400, 250, 100 }, // A2: Augmenter le salaire 5%
        { 300, 200, 100, 50 }, // A3: Créer concurrence
        { 250, 200, 150, 75 } // A4: Heure de travail flexible
    };

    List<double[,]> transitionMatrixs = new List<double[,]>();

    public Genetic(List<double[,]> transitionMatrixs)
    {
        this.population = generatePopulation();
        this.transitionMatrixs = transitionMatrixs;
    }

    public void Evolve(int generations)
    {
        for (int gen = 0; gen < generations; gen++)
        {
            printer.printMatrixList(this.population,$"population ${gen}");
            List<double> fitnessValues = EvaluateFitness(this.population);
            List<int[]> parents = SelectParents(this.population, fitnessValues);
            this.population = Crossover(parents);
            Mutate(this.population);
        }

        // Afficher la meilleure politique après l'évolution
        double minCost = double.MaxValue;
        int bestIndex = -1;
        foreach (var policy in this.population)
        {
            double cost = CalculateCost(policy);
            if (cost < minCost)
            {
                minCost = cost;
                bestIndex = Array.IndexOf(this.population.ToArray(), policy);
            }
        }

        displayResult(bestIndex, minCost);
    }

    void displayResult(int index,double cost)
    {
        Console.WriteLine(
            $"Meilleure politique : {string.Join(", ", this.population[index])} avec un coût de {cost}");

        var best = this.population[index];
        for (int i = 0; i < best.Length; i++)
        {
            int matrixIndex = best[i]-1;
            Console.WriteLine($"Prendre l'action {best[i]} pour les personnes dans l'etat {i}");
            printer.print2DArray(transitionMatrixs[matrixIndex]);
            var matrix = transitionMatrixs[matrixIndex];
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                Console.WriteLine($"probabilité de passer de l'etat {i+1} à {j+1} = {matrix[i, j]}");
            }
        }
    }

    public List<int[]> generatePopulation() // un individu est une politique de decision, comme vu dans l'etape 4 
    {
        List<int[]> population = new List<int[]>();
        Random rand = new Random();

        for (int i = 0; i < PopulationSize; i++)
        {
            int[] ind = new int[StateCount];
            for (int j = 0; j < StateCount; j++)
            {
                ind[j] = rand.Next(1, 5);
            }

            population.Add(ind);
        }

        return population;
    }

    List<double> EvaluateFitness(List<int[]> population)
    {
        List<double> fitnessValues = new List<double>();
        foreach (var policy in population)
        {
            double cost = CalculateCost(policy);
            Console.WriteLine($"coût de la politique {cost}");
            fitnessValues.Add(cost);
        }

        return fitnessValues;
    }

    double CalculateCost(int[] policy)
    {
        double[,] politicTransitionMatrix =
            new double[this.transitionMatrixs[0].GetLength(0), this.transitionMatrixs[0].GetLength(1)];
        for (int j = 0; j < politicTransitionMatrix.GetLength(0); j++)
        {
            int politicIndex = policy[j] - 1;
            for (int k = 0; k < politicTransitionMatrix.GetLength(0); k++)
            {
                politicTransitionMatrix[j, k] = this.transitionMatrixs[politicIndex][j, k];
            }
        }

        var pi = powerIteration.resolveByPuissance(politicTransitionMatrix);
        List<double> costTableValue = new List<double>();
        for (int i = 0; i < policy.Length; i++)
        {
            int j = policy[i] - 1;
            double value = costTable[i, j];
            costTableValue.Add(value);
        }
        
        Console.WriteLine($"valeur de pi {string.Join(", " ,pi)}");

        return Cost(pi, costTableValue.ToArray());
    }

    double Cost(double[] pi, double[] cost)
    {
        double costMean = 0.0;
        for (int i = 0; i < pi.Length; i++)
        {
            costMean += cost[i] * pi[i];
        }

        return costMean;
    }

    List<int[]> SelectParents(List<int[]> population, List<double> fitnessValues)
    {
        List<int[]> parents = new List<int[]>();
        Random rand = new Random();

        // Sélection par tournoi
        for (int i = 0; i < PopulationSize; i++)
        {
            int parent1Index = rand.Next(PopulationSize);
            int parent2Index = rand.Next(PopulationSize);
            Console.WriteLine("TOURNOI-----------");
            Console.WriteLine($"Parent 1 fitness-----------{fitnessValues[parent1Index]}");
            Console.WriteLine($"Parent 2 fitness-----------{fitnessValues[parent2Index]}");
            // Choisir le meilleur parent
            if (fitnessValues[parent1Index] < fitnessValues[parent2Index])
            {
                parents.Add(population[parent1Index]);
            }
            else
            {
                parents.Add(population[parent2Index]);
            }
        }
        printer.printMatrixList(parents, "Nouvelle parents apres tournoi");
        return parents;
    }

    List<int[]> Crossover(List<int[]> parents)
    {
        List<int[]> newPopulation = new List<int[]>();
        Random rand = new Random();

        Console.WriteLine("CROSSOVER-----------");
        for (int i = 0; i < PopulationSize; i += 2)
        {
            int[] parent1 = parents[rand.Next(parents.Count)];
            int[] parent2 = parents[rand.Next(parents.Count)];
            printer.printArray(parent1, $"Parent 1");
            printer.printArray(parent2, $"Parent 2");
            int crossoverPoint = rand.Next(1, StateCount);
            Console.WriteLine($"crossover point {crossoverPoint}");

            int[] child1 = new int[StateCount];
            int[] child2 = new int[StateCount];

            // Crossover
            for (int j = 0; j < StateCount; j++)
            {
                if (j < crossoverPoint)
                {
                    child1[j] = parent1[j];
                    child2[j] = parent2[j];
                }
                else
                {
                    child1[j] = parent2[j];
                    child2[j] = parent1[j];
                }
            }
            printer.printArray(child1, $"child1 1");
            printer.printArray(child2, $"child2 2");
            newPopulation.Add(child1);
            newPopulation.Add(child2);
        }
        Console.WriteLine("APRES CROSSOVER------------");
        printer.printMatrixList(newPopulation, "Parent apres crossover");
        return newPopulation;
    }

    void Mutate(List<int[]> population)
    {
        Random rand = new Random();
        Console.WriteLine("MUTATION---------------");
        for (int i = 0; i < PopulationSize; i++)
        {
            if (rand.NextDouble() < 0.1) // 10% de chance de mutation
            {
                int mutationPoint = rand.Next(StateCount);
                Console.WriteLine($"Indice de la valeur à muter---------------{mutationPoint}");
                var value = rand.Next(1, ActionCount + 1);
                Console.WriteLine($"Nouvelle valeur {value}");
                population[i][mutationPoint] = value; // Modifier l'action
            }
        }
    }
}