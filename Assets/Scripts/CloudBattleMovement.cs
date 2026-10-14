using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CloudBattleMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator animator;
    Rigidbody rb;
    CombatManager cm;
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
        cm = CombatManager.cm;
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
        cm.attackMenu.SetActive(false);
        cm.attackButton.interactable = false;
        animator.SetTrigger("Braver");
        sounds.Play(sounds.braver);

        yield return new WaitForSeconds(0.9f);
        Vector3 direction = (enemy.position - cloud.position).normalized;
        cloud.position = enemy.position - direction * distance;
        cm.sephiHP = cm.sephiHP - damage;
        yield return new WaitForSeconds(1.3f);
        MoveBack();
    }

    public void Focused()
    {
        StartCoroutine(FocusedHit());
    }

    IEnumerator FocusedHit()
    {
        cm.attackMenu.SetActive(false);
        cm.attackButton.interactable = false;
        animator.SetTrigger("Focused");
        sounds.Play(sounds.focused);

        yield return new WaitForSeconds(0.9f);
        Vector3 direction = (enemy.position - cloud.position).normalized;
        cloud.position = enemy.position - direction * distance;
        cm.sephiHP = cm.sephiHP - damage;
        yield return new WaitForSeconds(1.3f);
        MoveBack();
    }
    public void Punisher()
    {
        StartCoroutine(PunisherHit());
    }

    IEnumerator PunisherHit()
    {
        cm.attackMenu.SetActive(false);
        cm.attackButton.interactable = false;
        animator.SetTrigger("Punisher");
        yield return new WaitForSeconds(0.4583f);
        Vector3 direction = (enemy.position - cloud.position).normalized;
        cloud.position = enemy.position - direction * distance;
        cm.sephiHP = cm.sephiHP - damage;
        yield return new WaitForSeconds(1.7f);
        MoveBack();
    }
    public void Cure()
    {
        if(cm.cloudMP >= mpCost)
        {
            cm.magicMenu.SetActive(false);
            cm.magicButton.interactable = false;
            animator.SetTrigger("Cure");
            cm.cloudHP = cm.cloudHP + heal;
            cm.cloudMP = cm.cloudMP - mpCost;
            StartCoroutine(Turn());
        }
    }

    public void Ascension()
    {
        if(cm.cloudDamageTaken >= 1000)
        {
            cm.limitMenu.SetActive(false);
            cm.limitButton.interactable = false;
            StartCoroutine(AscensionHit());
        }
    }

    IEnumerator AscensionHit()
    {
        cm.attackMenu.SetActive(false);
        cm.attackButton.interactable = false;
        animator.SetTrigger("Ascension");
        sounds.Play(sounds.ascension);

        yield return new WaitForSeconds(2.5f);
        Vector3 direction = (enemy.position - cloud.position).normalized;
        cloud.position = enemy.position - direction * distance;
        cm.sephiHP = cm.sephiHP - limitDamage;
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
        cm.cloud = false;
        cm.sephi = true;
        cm.Combat();
    }
}
