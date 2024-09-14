namespace MiniProjet_M2.Utils;

public class Matrix
{
    public double[,] transposeMatrix(double[,] matrix)
    {
        // Get the dimensions of the matrix
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        // Create a new matrix to store the transposed result
        double[,] transposedMatrix = new double[cols, rows];

        // Perform the transposition
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                transposedMatrix[j, i] = matrix[i, j];
            }
        }

        return transposedMatrix;
    }

    public double[,] adaptToGauss(double[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        double[,] transposedMatrix = new double[cols + 1, rows];
        for (int i = 0; i < cols - 1; i++)
        {
            transposedMatrix[i, i] = matrix[i, i] - 1;
        }

        for (int i = 0; i < rows; i++)
        {
            transposedMatrix[rows, i] = 1;
        }

        return transposedMatrix;
    }

    public double[] generateInitialResult(int length)
    {
        double[] result = new double[length];

        for (int i = 0; i < length; i++)
        {
            if (i < length-1)
            {
                result[i] = 0;
            }
            else
            {
                result[i] = 1;
            }
        }

        return result;
    }
}