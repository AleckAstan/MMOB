namespace MiniProjet_M2.Helpers;
using System;

// une classe pour le generation de nombre entier aleatoire
public class RandomGenerator
{
    private static Random random = new Random();

    public int GetRandomInt(int min, int max)
    {
        return random.Next(min, max);
    }
}