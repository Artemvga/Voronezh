using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject panelLoad;
    public GameObject sliderLoad;
    public Slider sliderUI;
    public GameObject buttonStartLoad;
    public Button buttonStart;
    public int indexScene;

    public void Awake()
    {
        buttonStart.onClick.AddListener(() => SceneLoad(indexScene));
    }

    public void SceneLoad(int sceneIndex)
    {
        panelLoad.SetActive(true);
        menuUI.SetActive(false);
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        buttonStartLoad.SetActive(false);
        sliderLoad.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            sliderUI.value = progress;
            yield return null;
        }
    }
}
