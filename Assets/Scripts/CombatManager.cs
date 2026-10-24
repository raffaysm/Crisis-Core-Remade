using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    public static CombatManager cm;
    public string scene;
    public AudioSource sounds;
    public AudioClip start;
    public AudioClip end;
    public AudioClip dead;
    public AudioClip gongaga;
    public AudioClip metalPipe;
    public AudioClip wellx3;
    public AudioClip sephiWin;
    public AudioClip limitFill;
    public bool limitFillPlayed = false;
    public GameObject mainCamera;
    public GameObject zackWinCamera;
    public GameObject sephiWinCamera;
    public GameObject battleMenu;
    public GameObject gameOverMenu;
    public GameObject attackMenu;
    public GameObject magicMenu;
    public GameObject limitMenu;
    public GameObject attackButtonArrow;
    public GameObject magicButtonArrow;
    public GameObject limitButtonArrow;
    public GameObject exit;
    public GameObject win;
    public Image gameOverImage;
    public Image winImage;
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
            sounds.PlayOneShot(start, 0.5f);
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(scene);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            attackMenu.SetActive(false);
            magicMenu.SetActive(false);
            limitMenu.SetActive(false);
            attackButtonArrow.SetActive(false);
            magicButtonArrow.SetActive(false);
            limitButtonArrow.SetActive(false);
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

        if (cloudLimit < 100)
        {
            Limit.text = cloudLimit.ToString() + "%";
            limitFillPlayed = false;
        }
        else
        {
            Limit.text = "100%";
            if(limitFillPlayed == false)
            {
                sounds.PlayOneShot(limitFill, 0.5f);
                limitFillPlayed = true;
            }
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
        sounds.PlayOneShot(metalPipe, 0.2f);
        Invoke("Win", 3.5f);
    }

    void Win()
    {
        mainCamera.SetActive(false);
        zackWinCamera.SetActive(true);

        cloudAnimator.SetBool("Win", true);
        sounds.PlayOneShot(gongaga, 2f);

        battleMenu.SetActive(false);
        StartCoroutine(WinImage());
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
        sounds.PlayOneShot(wellx3, 2f);
        sounds.PlayOneShot(sephiWin, 2f);

        battleMenu.SetActive(false);
        gameOverMenu.SetActive(true);

        StartCoroutine(GameOverImage());
    }
    IEnumerator GameOverImage()
    {
        // 1. Wait 3 seconds for Sephiroth's animation to finish
        yield return new WaitForSeconds(3f);

        // 2. Slowly increase the image alpha (transparency) from 0 to 1
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * 0.5f; // Change 0.5f to make it fade faster or slower
            gameOverImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        exit.SetActive(true);
    }
    IEnumerator WinImage()
    {
        // 1. Wait 3 seconds for Sephiroth's animation to finish
        yield return new WaitForSeconds(3f);

        // 2. Slowly increase the image alpha (transparency) from 0 to 1
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * 0.5f; // Change 0.5f to make it fade faster or slower
            winImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        win.SetActive(true);
    }
}
