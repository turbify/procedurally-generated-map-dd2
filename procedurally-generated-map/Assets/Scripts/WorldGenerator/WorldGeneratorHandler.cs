using log4net.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class WorldGeneratorHandler : MonoBehaviour
{
    [SerializeField] private int seed = 0;
    public WorldSO _worldData;

    [Header("World size")]
    [Range(0, 300)]
    public int worldSize = 25;

    [Header("Chance to generate 2 or 3 points in line")]
    [Range(0, 100)]
    public int PointsAmountChance = 50;
    static public int pointsAmountChance = 50;

    public static WorldGeneratorHandler Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _worldData.worldLevels.Clear();

        SetRandomSeed();

        for (int i = 0; i < worldSize; i++)
        {
            GenerateWorldLevel();
        }

        SetRoutesOnLevels();
        FillRoutesOnLevels();

    }

    //generating points on each level
    private void GenerateWorldLevel()
    {
        WorldLevel level = new WorldLevel();

        level.SetSize(Coinflip(pointsAmountChance) ? 2 : 3);

        _worldData.worldLevels.Add(level);
    }

    private void SetRoutesOnLevels()
    {
        WorldLevel previousLevel = null;

        foreach(WorldLevel level in _worldData.worldLevels)
        {
            if(previousLevel != null)
            {
                for (int i = 0; i < previousLevel.size; i++) 
                {
                    previousLevel.worldPoints[i].CreateRoutes(level.size);
                    previousLevel.worldPoints[i].index = i;
                }
            }

            previousLevel = level;
        }
    }

    private void FillRoutesOnLevels()
    {
        WorldLevel previousLevel = null;

        foreach (WorldLevel level in _worldData.worldLevels)
        {
            if (previousLevel != null)
            {
                List<int> _routeList = SetupRouteList(previousLevel, level);

                previousLevel.SetupRoutes(_routeList, level.size);

            }

            previousLevel = level;
        }

        
    }

    private List<int> SetupRouteList(WorldLevel previousLevel, WorldLevel level)
    {
        List<int> _routeList = new List<int>();

        int points = level.size - 1;

        for (int i = 0; i < previousLevel.size; i++)
        {
            _routeList.Add(1);
        }

        for(int i = 0; i < points; i++)
        {
            _routeList[Random.Range(0,_routeList.Count)]++;
        }

        _routeList = _routeList.OrderBy(x => Random.value).ToList();

        return _routeList;
    }


    static public bool Coinflip(int chances)
    {
        int roll = Random.Range(0, 101);

        return roll <= chances;
    }

    #region setting seed
    public void SetRandomSeed()
    {
        seed = Random.Range(1,1000000);
        Random.InitState(seed);
    }

    public void ChangeSeed(int newSeed)
    {
        seed = newSeed;
        Random.InitState(seed);
    }
    #endregion
}
