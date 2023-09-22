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
        Debug.Log("�ε�");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("�ε�");
        StartCoroutine(LoadCoroutine());
    }


    public void ClickExit()
    {
        Debug.Log("����");
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
