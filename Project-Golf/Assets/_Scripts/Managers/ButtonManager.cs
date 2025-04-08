using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject InteractionPanel;
    [SerializeField]
    private GameObject OptionsPanel;
    public void Exit()
    {
        Application.Quit();
    }

    public void ChangeToOptionsMenu()
    {
        InteractionPanel.SetActive(false);
        OptionsPanel.SetActive(false);
    }

    public void ChangeToStartMenu()
    {
        //TODO: Coger (con viudez) nivel del levelmanager
        //FileSystem.Save();
        OptionsPanel.SetActive(false);
        InteractionPanel.SetActive(true);
    }
}
