namespace MiniProjet_M2.Utils;

public class Genetic
{
    static Printer printer = new Printer();
    public double[,] generatePopulation(int populationSize,int size) // une population est une polique de decision, comme vu dans l'etape 4 
    {
        double[,] population = new double[populationSize, size];
        Random rand = new Random();

        for (int i = 0; i < populationSize; i++)
        {
            for (int j = 0; j < size; j++)
            {
                population[i, j] = rand.Next(1, 6); 
            }
        }
        printer.print2DArray(population);
        return population;
    } 
}