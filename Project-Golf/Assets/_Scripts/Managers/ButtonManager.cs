using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject InteractionPanel;
    [SerializeField]
    private GameObject ButtonPanel;
    public void Exit()
    {
        Application.Quit();
    }

    public void ChangeToOptions()
    {
        InteractionPanel.SetActive(false);
        ButtonPanel.SetActive(false);

    }

    public void ChangeToMenu()
    {
        //TODO: Coger (con viudez) nivel del levelmanager
        //FileSystem.Save();
        ButtonPanel.SetActive(false);
        InteractionPanel.SetActive(true);
    }
}
