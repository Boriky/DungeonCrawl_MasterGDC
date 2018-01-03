using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    GameObject m_loadingText;
    [SerializeField]
    GameObject m_menuPanel;
    [SerializeField]
    GameObject m_menuPanelBackground;

    public void StartGame()
    {
        Application.LoadLevel("MainGame");
        m_menuPanel.SetActive(false);
        m_menuPanelBackground.SetActive(false);
        m_loadingText.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    } 
}
