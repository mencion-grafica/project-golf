using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor.Build.Content;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }
    
    [SerializeField] private List<GameObject> cards;
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] private GameObject cardParent;
    [SerializeField] private LevelBox levelBox;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    
    //Loads all keys up to the specified level
    public void LoadKeysUpToLevel(int level)
    {
        SOLevelData levelData = LevelManager.Instance.GetLevelData(level);
        
        for (int i = 0; i < cards.Count; i++)
        {
            Spawn(cards[i]);
            LevelCard levelCard = cards[i].GetComponent<LevelCard>();
            
            if (doesCardContainLevel(levelCard, levelData))
            {
                int j;
                for(j = 0; j <= levelCard.GetLevelData().IndexOf(levelData); j++)
                {
                    levelCard.SetLevelStage(j, LevelCompletionStage.Complete);
                }
                levelCard.SetLevelStage(j + 1, LevelCompletionStage.Unlocked);
                break;
            }
            
            for(int j = 0; j < levelCard.GetLevelData().Count; j++)
            {
                levelCard.SetLevelStage(j, LevelCompletionStage.Complete);
            }
        }
    }

    public void Spawn(GameObject card)
    {
        card.SetActive(true);
        card.transform.position = cardSpawnPoint.position;
    }

    public void SpawnNext(GameObject card)
    {
        GameObject nextCard = cards[Mathf.Clamp(cards.IndexOf(card) + 1, 0, cards.Count -1)];
        if(nextCard == card)
            return;
        nextCard.SetActive(true);
        nextCard.transform.position = cardSpawnPoint.position;
    }

    public void TeleportAllKeys()
    {
        for(int i = 0; i < cardParent.transform.childCount; i++)
        {
            Transform child = cardParent.transform.GetChild(i);
            if(child.gameObject.activeInHierarchy)
                child.position = cardSpawnPoint.position;
        }
    }

    public bool doesCardContainLevel(LevelCard levelCard, SOLevelData levelData)
    {
        return levelCard.GetLevelData().Contains(levelData);
    }
    
    public bool isLevelPlayable(SOLevelData level)
    {
        foreach (GameObject card in cards)
        {
            LevelCard levelCard = card.GetComponent<LevelCard>();
            if (levelCard.GetLevelData().Contains(level))
            {
                return levelCard.GetLevelComplete()[levelCard.GetLevelData().IndexOf(level)] == LevelCompletionStage.Unlocked ||
                       levelCard.GetLevelComplete()[levelCard.GetLevelData().IndexOf(level)] == LevelCompletionStage.Complete;
            }
        }
        return false;
    }

    public LevelCard GetCurrentCard()
    {
        return levelBox.GetCard();
    }
    
    public LevelBox GetLevelBox()
    {
        return levelBox;
    }
    
}
