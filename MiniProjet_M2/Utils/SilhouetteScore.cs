using System;
using System.Collections.Generic;
using MiniProjet_M2.Models;

public class SilhouetteScore
{
    public static double CalculateSilhouetteScore(List<Employee>[] clusters)
    {
        double totalScore = 0.0;
        int totalEmployees = 0;

        for (int i = 0; i < clusters.Length; i++)
        {
            foreach (var employee in clusters[i])
            {
                double a = CalculateAverageIntraClusterDistance(employee, clusters[i]);
                double b = CalculateAverageInterClusterDistance(employee, clusters, i);

                double silhouette = (b - a) / Math.Max(a, b);
                totalScore += silhouette;
                totalEmployees++;
            }
        }

        return totalScore / totalEmployees;
    }

    private static double CalculateAverageIntraClusterDistance(Employee employee, List<Employee> cluster)
    {
        double totalDistance = 0.0;
        foreach (var other in cluster)
        {
            totalDistance += Math.Abs(employee.CurrentMotivation - other.CurrentMotivation);
        }

        return totalDistance / (cluster.Count - 1);
    }

    private static double CalculateAverageInterClusterDistance(Employee employee, List<Employee>[] clusters, int currentClusterIndex)
    {
        double minAverageDistance = double.MaxValue;

        for (int i = 0; i < clusters.Length; i++)
        {
            if (i == currentClusterIndex) continue;

            double totalDistance = 0.0;
            foreach (var other in clusters[i])
            {
                totalDistance += Math.Abs(employee.CurrentMotivation - other.CurrentMotivation);
            }

            double averageDistance = totalDistance / clusters[i].Count;
            if (averageDistance < minAverageDistance)
            {
                minAverageDistance = averageDistance;
            }
        }
        return minAverageDistance;
    }
}