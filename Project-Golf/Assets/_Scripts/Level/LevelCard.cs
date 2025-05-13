using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelCompletionStage
{
    Locked,
    Unlocked,
    Complete
}
public class LevelCard : MonoBehaviour
{
    [SerializeField] private List<SOLevelData> levelData;
    [SerializeField] private List<LevelCompletionStage> isComplete;

    public List<SOLevelData> GetLevelData()
    {
        return levelData;
    }
    
    public List<LevelCompletionStage> GetLevelComplete()
    {
        return isComplete;
    }

    public void SetLevelStage(int level, LevelCompletionStage stage)
    {
        isComplete[level] = stage;
        CardManager.Instance.GetLevelBox().SetLevelDataOnButton(level, LevelCompletionStage.Complete);
        if (stage == LevelCompletionStage.Complete && level + 1 < isComplete.Count)
        {
            isComplete[level + 1] = LevelCompletionStage.Unlocked;
            CardManager.Instance.GetLevelBox().SetLevelDataOnButton(level+1, LevelCompletionStage.Unlocked);
        }
            
        foreach(LevelCompletionStage checkStage in isComplete)
            if (checkStage != LevelCompletionStage.Complete)
                return;
        CardManager.Instance.SpawnNext(gameObject);
    }
    
    public void SetLevelStage(SOLevelData levelDataIn, LevelCompletionStage stage)
    {
        int level = levelData.IndexOf(levelDataIn);
        SetLevelStage(level, stage);
    }

    public bool isLevelInCard(SOLevelData level)
    {
        foreach(SOLevelData levelCheck in levelData)
        {
            if (levelCheck.name == level.name)
            {
                
                return true;
            }
                
        }
        return false;
    }
    
}
