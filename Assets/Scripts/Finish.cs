using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public GameObject player;
    public GameObject uiManager;
    public GameObject manager;
    public GameObject stageClearPanel;

    void Start()
    {
        if (stageClearPanel != null)
        {
            stageClearPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShowStageClearPanel();
        }
    }

    private void ShowStageClearPanel()
    {
        if (stageClearPanel != null)
        {
            stageClearPanel.SetActive(true);
        }
    }

    private void DestroyObject()
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

    public void LoadMainMenu()
    {
        DestroyObject();
        SceneManager.LoadScene("MainMenu");
    }

    public void UnlockNewStage()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentStageNumber = int.Parse(currentSceneName.Split(' ')[1]);

        int nextStageNumber = currentStageNumber + 1;
        int highestUnlockedStage = PlayerPrefs.GetInt("HighestUnlockedStage", 0);

        // Check if the next stage is not already unlocked
        if (nextStageNumber > highestUnlockedStage)
        {
            PlayerPrefs.SetInt("HighestUnlockedStage", nextStageNumber);
            PlayerPrefs.Save();
        }

        DestroyObject();
        SceneManager.LoadScene("StageSelect");
    }

}
