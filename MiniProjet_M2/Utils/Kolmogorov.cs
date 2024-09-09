namespace MiniProjet_M2.Utils;
using System;


public class Kolmogorov
{
    private double[,] transitionMatrix;

    public Kolmogorov(double[,] matrix)
    {
        this.transitionMatrix = matrix;
    }

    public double[,] GetTransitionMatrix()
    {
        return this.transitionMatrix;
    }

    // Multiply two matrices
    public static double[,] MultiplyMatrices(double[,] A, double[,] B)
    {
        int rowA = A.GetLength(0);
        int colA = A.GetLength(1);
        int colB = B.GetLength(1);
        
        double[,] result = new double[rowA, colB];
        
        for (int i = 0; i < rowA; i++)
        {
            for (int j = 0; j < colB; j++)
            {
                result[i, j] = 0;
                for (int k = 0; k < colA; k++)
                {
                    result[i, j] += A[i, k] * B[k, j];
                }
            }
        }
        
        return result;
    }

    // Raise matrix to the power of n
    public double[,] PowerMatrix(int power)
    {
        int size = this.transitionMatrix.GetLength(0);
        double[,] result = new double[size, size];
        
        // Initialize result as identity matrix
        for (int i = 0; i < size; i++)
        {
            result[i, i] = 1;
        }
        
        // Matrix exponentiation
        double[,] baseMatrix = this.transitionMatrix;
        while (power > 0)
        {
            if (power % 2 == 1)
            {
                result = MultiplyMatrices(result, baseMatrix);
            }
            baseMatrix = MultiplyMatrices(baseMatrix, baseMatrix);
            power /= 2;
        }
        
        return result;
    }

    // Calculate transition probability from i to j in n steps
    public double GetTransitionProbability(int i, int j, int steps)
    {
        double[,] poweredMatrix = PowerMatrix(steps);
        return poweredMatrix[i, j];
    }

    // Verify transition probability using Chapman-Kolmogorov
    public double VerifyTransitionProbability(int i, int j, int n, int m)
    {
        double[,] P_n = PowerMatrix(n);
        double[,] P_m = PowerMatrix(m);
        
        double sum = 0.0;
        for (int k = 0; k < P_n.GetLength(0); k++)
        {
            sum += P_n[i, k] * P_m[k, j];
        }
        
        return sum;
    }
}
