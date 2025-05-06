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
        Debug.Log("Saliendo de la aplicacion");
        Application.Quit();
    }

    public void ChangeToOptionsMenu()
    {
        InteractionPanel.SetActive(false);
        OptionsPanel.SetActive(true);
    }

    public void ChangeToStartMenu()
    {
        
        OptionsPanel.SetActive(false);
        InteractionPanel.SetActive(true);
    }
}
