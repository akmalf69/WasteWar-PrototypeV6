using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASyncLoader : MonoBehaviour
{
    [Header("Stage Select")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject stageSelect;

    [Header("Slider")]
    [SerializeField] private Slider loadSlider;

    public void LoadStageBtn(string levelToLoad)
    {
        stageSelect.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadStageASync(levelToLoad));
    }

    IEnumerator LoadStageASync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float proggresValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadSlider.value = proggresValue;
            yield return null;
        }
    }
}
