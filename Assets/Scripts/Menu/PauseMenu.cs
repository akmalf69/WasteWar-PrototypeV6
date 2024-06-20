using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    private bool isPaused = false;

    public GameObject player;
    public GameObject uiManager;
    public GameObject manager;

    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        DestroyPersistentObjects();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
        Debug.Log("Player Quit");
    }

    private void DestroyPersistentObjects()
    {
        if (player != null)
        {
            Destroy(player);
        }
        if (uiManager != null)
        {
            Destroy(uiManager);
        }
        if (manager != null)
        {
            Destroy(manager);
        }
    }
}
