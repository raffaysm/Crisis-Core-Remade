using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager cm;
    public AudioSource sounds;
    public AudioClip start;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject attackMenu;
    public GameObject magicMenu;
    public GameObject limitMenu;
    public Button attackButton;
    public Button magicButton;
    public Button limitButton;
    public TMP_Text HP;
    public TMP_Text MP;
    public TMP_Text Limit;
    public CloudBattleMovement cloudBattle;
    public SephirothBattleMovement sephiBattle;
    public bool cloud = true;
    public bool sephi = false;  
    private int maxCloudHP = 777;
    public int cloudHP = 777;
    public int cloudMP = 77;

    public int limitRequirement = 1000;
    public int cloudLimit = 0;
    public int cloudDamageTaken = 0;
    public int sephiHP = 1000;
    public int sephiMaxHP = 1000;
    public int sephiMP = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        cm = this;
    }
    void Start()
    {
        cloudBattle = FindAnyObjectByType<CloudBattleMovement>();
        Invoke("ActivatingCombatMode", 0.5f);
    }

    void ActivatingCombatMode()
    {
        if(sounds != null)
        {
            sounds.PlayOneShot(start);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused();
        }

        // minimum HP to 0
        if (cloudHP >= 0)
        {
            HP.text = cloudHP.ToString();
        }
        else
        {
            HP.text = "0";
        }

        // No HP overcapping
        if (cloudHP > maxCloudHP)
        {
            cloudHP = maxCloudHP;
        }

        // Minimum MP to 0
        if (cloudMP >= 0)
        {
            MP.text = cloudMP.ToString();
        }
        else
        {
            MP.text = "0";
        }

        //No Limit Overcapping
        cloudLimit = (int)(((float)cloudDamageTaken / limitRequirement) * 100);

        if (cloudLimit <= 100)
        {
            Limit.text = cloudLimit.ToString() + "%";
        }
        else
        {
            Limit.text = "100%";
        }

        // Game Over
        if (cloudHP <= 0)
        {
            gameOverMenu.SetActive(true);
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

    public void CloudAttack()
    {
        attackMenu.SetActive(true);
        magicMenu.SetActive(false);
        limitMenu.SetActive(false);
    }

    public void CloudMagic()
    {
        magicMenu.SetActive(true);
        attackMenu.SetActive(false);
        limitMenu.SetActive(false);
    }

    public void CloudLimit()
    {
        limitMenu.SetActive(true);
        attackMenu.SetActive(false);
        magicMenu.SetActive(false);
    }
    public void Combat()
    {
        if (cloud)
        {
            attackButton.interactable = true;
            magicButton.interactable = true;
            limitButton.interactable = true;
        }

        if (sephi)
        {
            attackButton.interactable = false;
            magicButton.interactable = false;
            limitButton.interactable = false;
            sephiBattle.SephiAttack();
        }
    }
}
