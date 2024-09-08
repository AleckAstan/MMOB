using System;
using System.Collections.Generic;
using System.Linq;

public class KMeans
{
    private int _k; // Number of clusters
    private double[] _centroids; // Centroids of the clusters

    public KMeans(int k)
    {
        _k = k;
        _centroids = new double[k];
    }

    // Euclidean distance calculation for 1D
    private double EuclideanDistance(double point1, double point2)
    {
        return Math.Abs(point1 - point2);
    }

    // Fit the model to the dataset
    public int[] Fit(List<double> dataPoints, int maxIterations = 100)
    {
        int n = dataPoints.Count;
        int[] labels = new int[n]; // Cluster labels for each point
        Random random = new Random();

        // Randomly initialize centroids
        for (int i = 0; i < _k; i++)
        {
            _centroids[i] = dataPoints[random.Next(n)];
        }

        bool hasConverged = false;
        int iteration = 0;

        while (!hasConverged && iteration < maxIterations)
        {
            hasConverged = true;

            // Assign each point to the nearest centroid
            for (int i = 0; i < n; i++)
            {
                double minDist = double.MaxValue;
                int closestCentroid = 0;

                for (int j = 0; j < _k; j++)
                {
                    double dist = EuclideanDistance(dataPoints[i], _centroids[j]);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closestCentroid = j;
                    }
                }

                // Update cluster assignment if necessary
                if (labels[i] != closestCentroid)
                {
                    labels[i] = closestCentroid;
                    hasConverged = false;
                }
            }

            // Update centroids
            for (int j = 0; j < _k; j++)
            {
                var pointsInCluster = dataPoints.Where((p, index) => labels[index] == j).ToList();

                if (pointsInCluster.Any())
                {
                    _centroids[j] = pointsInCluster.Average();
                }
            }

            iteration++;
        }

        return labels;
    }
}