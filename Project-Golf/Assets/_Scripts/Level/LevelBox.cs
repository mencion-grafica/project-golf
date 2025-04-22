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

    public void OnCardInserted()
    {
        List<SOLevelData> levelData = ownInteractor.GetOldestInteractableSelected().transform.GetComponent<LevelCard>()
            .GetLevelData();
        foreach(SOLevelData data in levelData)
        {
            GameObject levelButton = Instantiate(Resources.Load<GameObject>("LevelButton"), levelScreen.transform);
            levelButton.GetComponent<LevelButton>().SetLevelData(data);
            //levelButton.GetComponent<LevelButton>().SetLevelName(data.levelName);
        }
    }
    
    public void OnCardRemoved()
    {
        foreach (Transform child in levelScreen.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
