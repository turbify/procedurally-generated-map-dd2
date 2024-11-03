using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

public class UIMapGenerator : MonoBehaviour
{
    public static UIMapGenerator Instance { get; private set; }

    public float offsetX = 1;
    public float offsetY = 1;

    public Color lineColor = Color.red;
    public float lineWidth = 0.1f;
    public Material lineMaterial;

    public GameObject worldPointPrefab;

    [SerializeField] private WorldSO _worldData;

    private void Awake()
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
    }

    private void Start()
    {
        _worldData = WorldGeneratorHandler.Instance._worldData;
    }

    public void GenerateMap()
    {
        GeneratePoints();
        GenerateRoutes();
    }

    private void GeneratePoints()
    {
        List<WorldLevel> worldLevels = _worldData.worldLevels;
        float height = 0;

        foreach (WorldLevel level in worldLevels)
        {
            for (int i = 0; i < level.worldPoints.Count; i++)
            {
                Vector3 position = new Vector3(height, 0, (i * offsetX) + (level.worldPoints.Count == 3 ? 0f : 0.5f));

                GameObject newPointObject = Instantiate(worldPointPrefab, position, Quaternion.Euler(90, 0, 0));
                level.worldPoints[i].gpsPoint = newPointObject.transform;
            }

            height += offsetY;
        }
    }

    private void GenerateRoutes()
    {
        List<WorldLevel> worldLevels = _worldData.worldLevels;
        WorldLevel previousLevel = null;

        foreach (WorldLevel level in worldLevels)
        {
            if (previousLevel != null)
            {
                for (int i = 0; i < previousLevel.worldPoints.Count; i++)
                {
                    for (int j = 0; j < previousLevel.worldPoints[i].routes.Count; j++)
                    {
                        if (previousLevel.worldPoints[i].routes[j] == true)
                        {
                            // Create a new LineRenderer for each endpoint
                            GameObject lineObj = new GameObject("LineRenderer_" + j);
                            lineObj.transform.parent = previousLevel.worldPoints[i].gpsPoint;
                            LineRenderer lineRenderer =  lineObj.AddComponent<LineRenderer>();

                            // Set up the line's appearance
                            lineRenderer.positionCount = 2; // Start and end points
                            lineRenderer.startWidth = lineWidth;
                            lineRenderer.endWidth = lineWidth;
                            lineRenderer.startColor = lineColor;
                            lineRenderer.endColor = lineColor;

                            // Set material
                            lineRenderer.material = lineMaterial != null ? lineMaterial : new Material(Shader.Find("Sprites/Default"));

                            // Assign to the "GPS" layer
                            lineObj.layer = LayerMask.NameToLayer("GPS");

                            lineRenderer.SetPosition(0, previousLevel.worldPoints[i].gpsPoint.position);
                            lineRenderer.SetPosition(1, level.worldPoints[j].gpsPoint.position);
                        }
                    }
                }
            }

            previousLevel = level;
        }
    }
}
