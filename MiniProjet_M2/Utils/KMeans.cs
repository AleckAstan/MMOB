using System;
using System.Collections.Generic;
using System.Linq;
using MiniProjet_M2.Models;

public class KMeans
{
    private int _k;
    private List<Employee> _employees; 
    private double[] _centroids;
    private List<Employee>[] _clusters;

    public KMeans(int k, List<Employee> employees)
    {
        _k = k;
        _employees = employees;
        _centroids = new double[_k];
        _clusters = new List<Employee>[_k];
        for (int i = 0; i < _k; i++)
        {
            _clusters[i] = new List<Employee>(); // creation des k clusters 
        }
    }

    public List<Employee>[]  getClusters()
    {
        return _clusters;
    }
    
    // le point d'entrer de la classe
    public void ClassifyEmployees()
    {
        InitializeCentroids();
        bool centroidsChanged;
        do
        {
            AssignToClusters();
            centroidsChanged = UpdateCentroids();
        } while (centroidsChanged);

        SortClusters();
    }

    private void InitializeCentroids()
    {
        // initialisation à des valeurs aleatoires de k centroids à partir de la valeur pour la classification: CurrentMotivation
        Random rand = new Random();
        HashSet<int> chosenIndexes = new HashSet<int>(); // pourqu'aucun centroids soit unique
        for (int i = 0; i < _k; i++)
        {
            int index;
            do
            {
                index = rand.Next(_employees.Count);
            } while (chosenIndexes.Contains(index));

            chosenIndexes.Add(index);
            _centroids[i] = _employees[index].CurrentMotivation;
        }
    }

    private void AssignToClusters()
    {
        // Nettoyage des precedentes clusters 
        for (int i = 0; i < _k; i++)
        {
            _clusters[i].Clear();
        }

        foreach (var employee in _employees)
        {
            int closestCentroidIndex = GetClosestCentroidIndex(employee.CurrentMotivation); // trouver le centroid le plus proche
            _clusters[closestCentroidIndex].Add(employee); // assigner l'employé au centroid plus proche
        }
    }

    //Recalcule des nouveaux centroids
    private bool UpdateCentroids()
    {
        bool centroidsChanged = false;

        for (int i = 0; i < _k; i++)
        {
            if (_clusters[i].Count == 0)
                continue; // Avoid division by zero

            double newCentroid = _clusters[i].Average(e => e.CurrentMotivation);
            if (Math.Abs(newCentroid - _centroids[i]) > 0.0001)
                centroidsChanged = true;

            _centroids[i] = newCentroid;
        }

        return centroidsChanged;
    }

    
    //Recherche du centroid le plus proche
    private int GetClosestCentroidIndex(double value)
    {
        double minDistance = double.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < _k; i++)
        {
            double distance = Math.Abs(value - _centroids[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    // recherche du centre du cluster
    private double GetClusterCenterIndex(List<Employee> cluster)
    {
        var firstElement = cluster[0].CurrentMotivation;
        var lastElement = cluster[cluster.Count - 1].CurrentMotivation;
        return (firstElement + lastElement) / 2;
    }
    
    
    // recherche de la classification d'un employé par sa valeur de motivation
    public int GetClusterClassification(double value)
    {
        var minDistance = Math.Abs(value - GetClusterCenterIndex(_clusters[0]));
        var clusterIndex = 0;
        for (int i = 1; i < _k; i++)
        {
            double distance = Math.Abs(value - GetClusterCenterIndex(_clusters[i]));
            if (distance < minDistance)
            {
                minDistance = distance;
                clusterIndex = i;
            }
        }

        return clusterIndex;
    }

    // triage des cluster pour suivre
    // demotivé C1 < C2 < C3 < C4 tres motivé 
    private void SortClusters()
    {
        // Ccreation de liste de cluster avec leur moyenne
        var clusterAverages = new List<(int Index, double AverageMotivation)>();
        for (int i = 0; i < _k; i++)
        {
            double averageMotivation = _clusters[i].Average(e => e.CurrentMotivation);
            clusterAverages.Add((i, averageMotivation));
        }

        // Trier les clusters en fonction de la moyenne
        clusterAverages = clusterAverages.OrderBy(c => c.AverageMotivation).ToList();

        var sortedClusters = new List<Employee>[_k];
        for (int i = 0; i < _k; i++)
        {
            sortedClusters[i] = _clusters[clusterAverages[i].Index];
        }
        var labels = new string[] { "Unmotivated", "Less Motivated", "Motivated", "Very Motivated" };
        _clusters = sortedClusters;
    }
}