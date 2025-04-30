using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public static Color completeColor = new Color(0f, 1f, 0f, 1f);
    [SerializeField] private SOLevelData levelData;
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private Image buttonImage;
    
    // Start is called before the first frame update


    public void SetLevelName(string levelName)
    {
        levelNameText.text = levelName;
    }
    
    public void SetLevelData(SOLevelData levelData)
    {
        this.levelData = levelData;
    }

    public void OnButtonPress()
    {
        LevelManager.Instance.LoadLevel(levelData);
    }

    public void CompleteLevel()
    {
        buttonImage.color = completeColor;
    }
}
