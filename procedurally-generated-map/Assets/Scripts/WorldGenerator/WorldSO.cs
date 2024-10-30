using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WorldGenerator/WorldData")]
public class WorldSO : ScriptableObject
{
    [SerializeField] public List<WorldLevel> worldLevels = new List<WorldLevel>();
}