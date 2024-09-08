﻿using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // Example dataset (1D points)
        List<double> dataPoints = new List<double>
        {
            1.0, 1.5, 5.0, 8.0, 1.0, 9.0, 8.0, 10.0, 9.0
        };

        // Number of clusters (k)
        int k = 4;

        // Instantiate the KMeans class and run the algorithm
        KMeans kMeans = new KMeans(k);
        var clusters = kMeans.Fit(dataPoints);

        // Output the cluster assignments
        for (int i = 0; i < clusters.Length; i++)
        {
            Console.WriteLine($"Data Point {dataPoints[i]} assigned to cluster {clusters[i]}");
        }
    }
}