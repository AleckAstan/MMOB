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

}