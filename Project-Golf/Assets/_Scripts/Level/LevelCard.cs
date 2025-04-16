using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCard : MonoBehaviour
{
    [SerializeField] private SOLevelData levelData;

    public SOLevelData GetLevelData()
    {
        return levelData;
    }
}
