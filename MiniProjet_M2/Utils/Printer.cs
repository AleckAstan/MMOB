namespace MiniProjet_M2.Utils;

public class Printer
{
    public void print2DArray(double[,] array, string title = "")
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        Console.WriteLine($"----------------------------{title}------------------------------");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(Math.Round(array[i, j], 2) + "\t");
            }

            Console.WriteLine();
        }
    }

    public void printArray(double[] array, string title = "")
    {
        Console.WriteLine($"{title}-----------------------{title}-----------------------------------");
        int rows = array.Length;
        for (int i = 0; i < rows; i++)
        {
            Console.Write(array[i] + "\t");
        }

        Console.WriteLine();
    }
}