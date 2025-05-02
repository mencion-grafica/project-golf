using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public static Color completeColor = new Color(0f, 1f, 0f, 1f);
    public static Color unlockedColor = new Color(1f, 1f, 0f, 1f);
    [SerializeField] private SOLevelData levelData;
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private Image buttonImage;
    
    // Start is called before the first frame update

    public SOLevelData GetLevelData()
    {
        return levelData;
    }
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
        if(CardManager.Instance.isLevelPlayable(levelData))
            LevelManager.Instance.LoadLevel(levelData);
    }

    public void SetLevelState(LevelCompletionStage stage)
    {
        switch (stage)
        {
            case LevelCompletionStage.Complete:
                buttonImage.color = completeColor;
                break;
            case LevelCompletionStage.Unlocked:
                buttonImage.color = unlockedColor;
                break;
        }
    }
}
