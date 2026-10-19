using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleScreenManager : MonoBehaviour
{
    public string scene;
    public string bossScene;
    public GameObject any;
    public GameObject menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            any.SetActive(false);
            menu.SetActive(true);
        }
    }

    public void Continue()
    {
        SceneManager.LoadScene(scene);
    }

    public void BossBattle()
    {
        SceneManager.LoadScene(bossScene);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
