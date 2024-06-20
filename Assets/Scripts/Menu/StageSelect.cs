using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    public Button[] stageButtons;

    void Awake()
    {
        //Optionally reset PlayerPrefs for testing
        //PlayerPrefs.DeleteAll(); 

        if (!PlayerPrefs.HasKey("HighestUnlockedStage"))
        {
            PlayerPrefs.SetInt("HighestUnlockedStage", 0);
            PlayerPrefs.Save();
        }

        SetButtonInteractivity();
    }

    private void SetButtonInteractivity()
    {
        int highestUnlockedStage = PlayerPrefs.GetInt("HighestUnlockedStage", 0);

        // Set tombol pertama sebagai interaktif
        if (stageButtons.Length > 0)
        {
            stageButtons[0].interactable = true;
        }

        for (int i = 1; i < stageButtons.Length; i++) // Mulai dari indeks 1
        {
            if (i < highestUnlockedStage) // Ubah dari <= menjadi <
            {
                stageButtons[i].interactable = true;
            }
            else
            {
                stageButtons[i].interactable = false;
            }
        }
    }


    public void OpenStage(int stageId)
    {
        string stageName = "Stage " + stageId;
        SceneManager.LoadScene(stageName);
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
