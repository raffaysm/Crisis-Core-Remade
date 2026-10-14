using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager cm;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        cm = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused();
        }

    }    
    public void isPaused()
    {
        Time.timeScale = 0f;
        Pause();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
    }

    public void isResumed()
    {
        Time.timeScale = 1f;
        Resume();
    }

    public void close()
    {
        Application.Quit();
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
    }
}
