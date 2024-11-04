using UnityEngine;

public class RouteCreator : MonoBehaviour
{
    [SerializeField] private WorldSO _worldData;

    public int roadLength = 5;

    private GameObject roadStraightPrefab;
    private GameObject roadForkPrefab;
    private GameObject roadRoundaboutPrefab;

    private void Start()
    {
        _worldData = WorldGeneratorHandler.Instance._worldData;


    }
}
