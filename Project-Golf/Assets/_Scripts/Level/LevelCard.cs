using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCard : MonoBehaviour
{
    [SerializeField] private List<SOLevelData> levelData;

    public List<SOLevelData> GetLevelData()
    {
        return levelData;
    }
}
