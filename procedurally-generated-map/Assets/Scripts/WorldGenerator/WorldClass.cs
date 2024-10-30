using System;
using System.Collections.Generic;
using UnityEngine;
using static PlasticGui.Help.GuiHelp;

[System.Serializable]
public class WorldLevel
{
    public bool generatedRoutes = false;
    public int size = 3;
    public List<WorldPoint> worldPoints = new List<WorldPoint>();

    public void SetSize(int newSize)
    {
        size = newSize;

        for (int i = 0; i < size; i++)
        {
            worldPoints.Add(new WorldPoint());
        }
    }

    public void SetupRoutes(List<int> _routelist, int nextLevelSize)
    {
        for (int i = 0; i < _routelist[0]; i++)
        {
            worldPoints[0].routes[i] = true;
        }

        if (size == 3)
        {
            for (int i = 0; i < _routelist[1]; i++)
            {
                worldPoints[1].routes[(i + (_routelist[0] - 1))] = true;
            }
        }

        for (int i = 0; i < _routelist[size - 1]; i++)
        {
            worldPoints[size - 1].routes[(worldPoints[0].routes.Count - i - 1)] = true;
        }
    }
}

[System.Serializable]
public class WorldPoint
{
    public int index;
    public List<bool> routes = new List<bool>();

    public void CreateRoutes(int amount)
    {
        for (int i = 0; i < amount; i++) 
        { 
            routes.Add(false);
        }
    }
}
