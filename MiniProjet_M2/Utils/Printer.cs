namespace MiniProjet_M2.Utils;
 
// une classe pour faciliter l'affichage des matrices 1D et  2D
public class Printer
{
    public void print2DArray(double[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        Console.WriteLine($"----------------------------------------------------------");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(array[i, j] + "\t");
            }

            Console.WriteLine();
        }
    }

    public void printArray(double[] array)
    {
        int rows = array.Length;
        for (int i = 0; i < rows; i++)
        {
            Console.Write(array[i] + "\t");
        }

        Console.WriteLine();
    }
}