using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager cm;
    public AudioSource sounds;
    public AudioClip start;
    public AudioClip end;
    public AudioClip dead;
    public AudioClip gongaga;
    public GameObject mainCamera;
    public GameObject zackWinCamera;
    public GameObject sephiWinCamera;
    public GameObject battleMenu;
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
    public Animator cloudAnimator;
    public Animator sephiAnimator;
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
        cm.mainCamera.SetActive(true);
        cm.zackWinCamera.SetActive(false);
        cm.sephiWinCamera.SetActive(false);
    }
    void Start()
    {
        cloudBattle = FindAnyObjectByType<CloudBattleMovement>();
        Invoke("ActivatingCombatMode", 0.5f);
        Invoke("BattleMenu", 4.375f);
    }

    void BattleMenu()
    {
        battleMenu.SetActive(true);
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
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            attackMenu.SetActive(false);
            magicMenu.SetActive(false);
            limitMenu.SetActive(false);
            attackButton.GetComponent<EventTrigger>().enabled = true;
            magicButton.GetComponent<EventTrigger>().enabled = true;
            limitButton.GetComponent<EventTrigger>().enabled = true;
            attackButton.interactable = true;
            magicButton.interactable = true;
            limitButton.interactable = true;
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
    }

    public void close()
    {
        Application.Quit();
    }

    public void CloudAttack()
    {
        attackMenu.SetActive(true);
        magicMenu.SetActive(false);
        limitMenu.SetActive(false);
        attackButton.interactable = false;
        magicButton.interactable = false;
        limitButton.interactable = false;
        attackButton.GetComponent<EventTrigger>().enabled = false;
        magicButton.GetComponent<EventTrigger>().enabled = false;
        limitButton.GetComponent<EventTrigger>().enabled = false;

    }

    public void CloudMagic()
    {
        magicMenu.SetActive(true);
        attackMenu.SetActive(false);
        limitMenu.SetActive(false);
        attackButton.interactable = false;
        magicButton.interactable = false;
        limitButton.interactable = false;
        attackButton.GetComponent<EventTrigger>().enabled = false;
        magicButton.GetComponent<EventTrigger>().enabled = false;
        limitButton.GetComponent<EventTrigger>().enabled = false;
    }

    public void CloudLimit()
    {
        limitMenu.SetActive(true);
        attackMenu.SetActive(false);
        magicMenu.SetActive(false);
        attackButton.interactable = false;
        magicButton.interactable = false;
        limitButton.interactable = false;
        attackButton.GetComponent<EventTrigger>().enabled = false;
        magicButton.GetComponent<EventTrigger>().enabled = false;
        limitButton.GetComponent<EventTrigger>().enabled = false;
    }
    public void LockMenuForAction()
    {
        // Close all sub-menus
        attackMenu.SetActive(false);
        magicMenu.SetActive(false);
        limitMenu.SetActive(false);

        // Force pointers to turn off

        // Disable main buttons and triggers completely
        attackButton.interactable = false;
        magicButton.interactable = false;
        limitButton.interactable = false;

        attackButton.GetComponent<EventTrigger>().enabled = false;
        magicButton.GetComponent<EventTrigger>().enabled = false;
        limitButton.GetComponent<EventTrigger>().enabled = false;
    }
    public void Combat(bool cloud = true, bool sephi = false)
    {
        if (cloud)
        {
                attackButton.interactable = true;
                magicButton.interactable = true;
                limitButton.interactable = true;
                attackButton.GetComponent<EventTrigger>().enabled = true;
                magicButton.GetComponent<EventTrigger>().enabled = true;
                limitButton.GetComponent<EventTrigger>().enabled = true;
        }

        if (sephi)
        {
            attackButton.interactable = false;
            magicButton.interactable = false;
            limitButton.interactable = false;
            attackButton.GetComponent<EventTrigger>().enabled = false;
            magicButton.GetComponent<EventTrigger>().enabled = false;
            limitButton.GetComponent<EventTrigger>().enabled = false;
        }
    }

    public void CloudWins()
    {
        sephiAnimator.SetBool("Dead", true);
        sounds.PlayOneShot(end);
        Invoke("Win", 3.5f);
    }

    void Win()
    {
        mainCamera.SetActive(false);
        zackWinCamera.SetActive(true);

        cloudAnimator.SetBool("Win", true);
        sounds.PlayOneShot(gongaga, 2f);

        battleMenu.SetActive(false);
    }

    public void SephirothWins()
    {
        cloudAnimator.SetBool("Dead", true);
        sounds.PlayOneShot(dead);
        Invoke("GameOver", 3.5f);
    }

    void GameOver()
    {
        mainCamera.SetActive(false);
        sephiWinCamera.SetActive(true);

        sephiAnimator.SetBool("Win", true);

        battleMenu.SetActive(false);
        gameOverMenu.SetActive(true);
    }
}
