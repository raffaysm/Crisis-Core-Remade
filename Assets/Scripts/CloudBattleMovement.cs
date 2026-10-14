using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CloudBattleMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator animator;
    Rigidbody rb;
    GameManager gm;
    public Button braver;
    public Button focused;
    public Button punisher;
    public Button cure;
    public Button ascension;
    public Transform enemy;
    public Transform cloud;
    public CloudAudio sounds;
    public float distance = 2f;
    private int damage = 100;
    private int heal = 300;
    private int limitDamage = 500;
    private int mpCost = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        gm = GameManager.gm;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RootMovement()
    {
        if(animator && rb)
        {
            Vector3 newPosition = rb.position + animator.deltaPosition;
            rb.MovePosition(newPosition);
        }
    }

    public void Braver()
    {
        StartCoroutine(BraverHit());
    }

    IEnumerator BraverHit()
    {
        gm.attackMenu.SetActive(false);
        gm.attackButton.interactable = false;
        animator.SetTrigger("Braver");
        sounds.Play(sounds.braver);

        yield return new WaitForSeconds(0.9f);
        Vector3 direction = (enemy.position - cloud.position).normalized;
        cloud.position = enemy.position - direction * distance;
        gm.sephiHP = gm.sephiHP - damage;
        yield return new WaitForSeconds(1.3f);
        MoveBack();
    }

    public void Focused()
    {
        StartCoroutine(FocusedHit());
    }

    IEnumerator FocusedHit()
    {
        gm.attackMenu.SetActive(false);
        gm.attackButton.interactable = false;
        animator.SetTrigger("Focused");
        sounds.Play(sounds.focused);

        yield return new WaitForSeconds(0.9f);
        Vector3 direction = (enemy.position - cloud.position).normalized;
        cloud.position = enemy.position - direction * distance;
        gm.sephiHP = gm.sephiHP - damage;
        yield return new WaitForSeconds(1.3f);
        MoveBack();
    }
    public void Punisher()
    {
        StartCoroutine(PunisherHit());
    }

    IEnumerator PunisherHit()
    {
        gm.attackMenu.SetActive(false);
        gm.attackButton.interactable = false;
        animator.SetTrigger("Punisher");
        yield return new WaitForSeconds(0.4583f);
        Vector3 direction = (enemy.position - cloud.position).normalized;
        cloud.position = enemy.position - direction * distance;
        gm.sephiHP = gm.sephiHP - damage;
        yield return new WaitForSeconds(1.7f);
        MoveBack();
    }
    public void Cure()
    {
        if(gm.cloudMP >= mpCost)
        {
            gm.magicMenu.SetActive(false);
            gm.magicButton.interactable = false;
            animator.SetTrigger("Cure");
            gm.cloudHP = gm.cloudHP + heal;
            gm.cloudMP = gm.cloudMP - mpCost;
            StartCoroutine(Turn());
        }
    }

    public void Ascension()
    {
        if(gm.cloudDamageTaken >= 1000)
        {
            gm.limitMenu.SetActive(false);
            gm.limitButton.interactable = false;
            StartCoroutine(AscensionHit());
        }
    }

    IEnumerator AscensionHit()
    {
        gm.attackMenu.SetActive(false);
        gm.attackButton.interactable = false;
        animator.SetTrigger("Ascension");
        sounds.Play(sounds.ascension);

        yield return new WaitForSeconds(2.5f);
        Vector3 direction = (enemy.position - cloud.position).normalized;
        cloud.position = enemy.position - direction * distance;
        gm.sephiHP = gm.sephiHP - limitDamage;
        yield return new WaitForSeconds(3.833f);
        MoveBack();
    }
    void MoveBack()
    {
        cloud.localPosition = Vector3.zero;
        StartCoroutine(Turn());
    }

    IEnumerator Turn()
    {
        yield return new WaitForSeconds(1.5f);
        gm.cloud = false;
        gm.sephi = true;
        gm.Combat();
    }
}
