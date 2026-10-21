using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.VFX;

public class CloudBattleMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator animator;
    public Animator sephi;
    public SephirothBattleMovement sephiBattle;
    public CloudAnimationMove anim;
    public GameObject braverVfx;
    public GameObject punisherVfx;
    public Vector3 vfxRotationOffset;
    public Vector3 vfxPositionOffset;
    public Transform busterSword;
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
        anim.attack = "braver";
        cm.attackMenu.SetActive(false);
        cm.attackButton.interactable = false;
        animator.SetTrigger("Braver");
        sounds.Play(sounds.braver);
        sounds.Braver();

        yield return new WaitForSeconds(0.9f);
        
        Vector3 direction = (enemy.position - cloud.position).normalized;
        cloud.position = enemy.position - direction * distance;
        cm.sephiHP = cm.sephiHP - damage;
        yield return new WaitForSeconds(0.3f);
        EnemyDamageTaken(true);
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
        sounds.Focused();

        yield return new WaitForSeconds(0.9f);
        EnemyDamageTaken(true);
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
        anim.attack = "punisher";
        cm.attackMenu.SetActive(false);
        cm.attackButton.interactable = false;
        animator.SetTrigger("Punisher");
        sounds.Play(sounds.punisher);
        sounds.Punisher();
        yield return new WaitForSeconds(0.4583f);
        EnemyDamageTaken(true);
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
            sounds.Cure();
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
        sounds.Limit_Break();

        yield return new WaitForSeconds(2.5f);
        EnemyDamageTaken(true);
        Vector3 direction = (enemy.position - cloud.position).normalized;
        cloud.position = enemy.position - direction * distance;
        cm.sephiHP = cm.sephiHP - limitDamage;
        yield return new WaitForSeconds(3.833f);
        MoveBack();
    }
    void MoveBack()
    {
        cloud.localPosition = Vector3.zero;
        EnemyDamageTaken(false);
        StartCoroutine(Turn());
    }

    void EnemyDamageTaken(bool stagger)
    {
        if(sephiBattle.phase2)
        {
            sephi.SetBool("Stagger 2", stagger);
        }

        if(sephiBattle.phase1)
        {
            sephi.SetBool("Stagger", stagger);
        }
    }

    public void PlayVFX(string attack)
    {
        GameObject vfx = null;
        if (attack == "braver") vfx = braverVfx;
        if (attack == "punisher") vfx = punisherVfx;
        if (vfx != null && busterSword != null)
        {
            GameObject spawnedVFX = Instantiate(vfx, busterSword.position, busterSword.rotation, busterSword);

            spawnedVFX.transform.localPosition = vfxPositionOffset;
            spawnedVFX.transform.localRotation = Quaternion.Euler(vfxRotationOffset);

            Destroy(spawnedVFX, 2f);
        }
    }

    IEnumerator Turn()
    {
        yield return new WaitForSeconds(2.5f);
        
        cm.cloud = false;
        cm.sephi = true;
        cm.Combat();
    }
}
