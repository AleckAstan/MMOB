namespace MiniProjet_M2.Utils;

public class PowerIteration
{
    static Matrix matrixMethod = new Matrix();
    public  void resolveByPuissance(double[,] P)
    {
        // Vecteur initial π (valeurs égales ou aléatoires)
        int M = P.GetLength(0); // Nombre d'états (taille de la matrice)
        double[] pi = new double[M];
        var tP = matrixMethod.transposeMatrix(P); // transposée de P

        // Remplir le vecteur avec 1/M
        for (int i = 0; i < M; i++)
        {
            pi[i] = 1.0 / M;
        }

        double tolerance = 1e-9;
        int maxIterations = 10000;
        // Résolution de tPπ = π par la méthode des puissances
        for (int iteration = 0; iteration < maxIterations; iteration++)
        {
            // Nouveau vecteur π
            double[] newPi = new double[pi.Length];
            // Calcul de tPπ
            for (int i = 0; i < tP.GetLength(0); i++)
            {
                newPi[i] = 0;
                for (int j = 0; j < tP.GetLength(1); j++)
                {
                    newPi[i] += tP[i, j] * pi[j]; // Utilisation de la matrice transposée tP
                }
            }
            // Normalisation pour que la somme soit égale à 1

            double sum = 0;
            for (int i = 0; i < newPi.Length; i++)
            {
                sum += newPi[i];
            }

            for (int i = 0; i < newPi.Length; i++)
            {
                newPi[i] /= sum;
            }

            // Vérifier la convergence
            double error = 0;
            for (int i = 0; i < pi.Length; i++)
            {
                error += Math.Abs(newPi[i] - pi[i]);
            }

            if (error < tolerance)
            {
                Console.WriteLine("Convergence atteinte après " + iteration + " itérations.");
                break;
            }

            // Mise à jour de π pour la prochaine itération
            Array.Copy(newPi, pi, pi.Length);
        }

        // Afficher les valeurs du vecteur propre π
        Console.WriteLine("Le vecteur propre π est :");
        for (int i = 0; i < pi.Length; i++)
        {
            Console.WriteLine("π" + (i + 1) + " = " + pi[i]);
        }

        // Vérifier la condition de somme égale à 1
        double piSum = 0;
        foreach (var value in pi)
        {
            piSum += value;
        }

        Console.WriteLine("Somme des éléments de π : " + piSum);
    }
}