namespace MiniProjet_M2.Helpers;
using System;


public class RandomGenerator
{
    private static Random random = new Random();

    public int GetRandomInt(int min, int max)
    {
        return random.Next(min, max); // max is exclusive
    }
}