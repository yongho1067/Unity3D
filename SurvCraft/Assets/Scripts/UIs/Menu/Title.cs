using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "Main Game";

    public static Title instance;

    private SaveNLoad saveNLoad;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ClickStart()
    {
        Debug.Log("로딩");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("로드");
        StartCoroutine(LoadCoroutine());
    }


    public void ClickExit()
    {
        Debug.Log("종료");
        Application.Quit();
    }

    IEnumerator LoadCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone)
        {
            yield return null;
        }

        saveNLoad = FindObjectOfType<SaveNLoad>();
        saveNLoad.LoadData();
        gameObject.SetActive(false);
    }
}
