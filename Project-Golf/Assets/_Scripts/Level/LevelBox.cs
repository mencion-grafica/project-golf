using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LevelBox : MonoBehaviour
{
    [SerializeField]
    private XRSocketInteractor ownInteractor;

    [SerializeField] private GameObject levelScreen;
    [SerializeField] private List<LevelButton> levelButtons;

    public void OnCardInserted()
    {
        List<SOLevelData> levelData = ownInteractor.GetOldestInteractableSelected().transform.GetComponent<LevelCard>()
            .GetLevelData();
        foreach(SOLevelData data in levelData)
        {
            LevelButton levelButton = Instantiate(Resources.Load<GameObject>("LevelButton"), levelScreen.transform).GetComponent<LevelButton>();
            levelButton.SetLevelData(data);
            levelButton.SetLevelState(
                ownInteractor.GetOldestInteractableSelected().transform.GetComponent<LevelCard>().GetLevelComplete()[levelData.IndexOf(data)]);
            
            levelButton.SetLevelName(data.name);
            levelButtons.Add(levelButton);
        }
    }
    
    public void OnCardRemoved()
    {
        foreach (Transform child in levelScreen.transform)
        {
            Destroy(child.gameObject);
        }
        levelButtons.Clear();
    }
    
    public LevelCard GetCard()
    {
        return ownInteractor.GetOldestInteractableSelected().transform.GetComponent<LevelCard>();
    }
    
    public LevelButton GetLevelButtonFromLevelData(SOLevelData levelData)
    {
        foreach(LevelButton button in levelButtons)
        {
            if (button.GetLevelData() == levelData)
                return button;
        }
        return null;
    }

    public void SetLevelDataOnButton(int level, LevelCompletionStage stage)
    {
        levelButtons[level].SetLevelState(stage);
    }
}
