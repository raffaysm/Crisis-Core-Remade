using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.VFX;

public class CloudBattleMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator cloudAnimator;
    public Animator sephiAnimator;
    public SephirothBattleMovement sephiBattle;
    public CloudAnimationMove anim;
    public GameObject vfx;
    public GameObject cureVfx;
    public GameObject limitVfx;
    public Transform fakeCloud;
    public Vector3 vfxPositionOffset;
    public Vector3 vfxRotationOffset;    public Transform busterSword;
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
        cloudAnimator = GetComponentInChildren<Animator>();
        cm = CombatManager.cm;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RootMovement()
    {
        if(cloudAnimator && rb)
        {
            Vector3 newPosition = rb.position + cloudAnimator.deltaPosition;
            rb.MovePosition(newPosition);
        }
    }

    public void Braver()
    {
        cm.Combat(false, true);
        cm.LockMenuForAction();
        StartCoroutine(BraverHit());
    }

    IEnumerator BraverHit()
    {
        anim.attack = "braver";
        cm.attackMenu.SetActive(false);
        cm.attackButton.interactable = false;
        cloudAnimator.SetTrigger("Braver");
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
        cm.Combat(false, true);
        cm.LockMenuForAction();
        StartCoroutine(FocusedHit());
    }

    IEnumerator FocusedHit()
    {
        anim.attack = "focused";
        cm.attackMenu.SetActive(false);
        cm.attackButton.interactable = false;
        cloudAnimator.SetTrigger("Focused");
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
        cm.Combat(false, true);
        cm.LockMenuForAction();
        StartCoroutine(PunisherHit());
    }

    IEnumerator PunisherHit()
    {
        anim.attack = "punisher";
        cm.attackMenu.SetActive(false);
        cm.attackButton.interactable = false;
        cloudAnimator.SetTrigger("Punisher");
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
            cm.LockMenuForAction();
            cm.Combat(false, true);
            cm.magicMenu.SetActive(false);
            cm.magicButton.interactable = false;
            cloudAnimator.SetTrigger("Cure");
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
            cm.LockMenuForAction();
            cm.Combat(false, true);
            cm.limitMenu.SetActive(false);
            cm.limitButton.interactable = false;
            StartCoroutine(AscensionHit());
        }
    }

    IEnumerator AscensionHit()
    {
        anim.attack = "ascension";
        cm.attackMenu.SetActive(false);
        cm.attackButton.interactable = false;
        cloudAnimator.SetTrigger("Ascension");
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
        if(cm.sephiHP <= 0)
        {
            cm.CloudWins();
        }
        else
        {
            EnemyDamageTaken(false);
            StartCoroutine(Turn());
        }
    }

    void EnemyDamageTaken(bool stagger)
    {
        if(sephiBattle.phase2)
        {
            sephiAnimator.SetBool("Stagger 2", stagger);
        }

        if(sephiBattle.phase1)
        {
            sephiAnimator.SetBool("Stagger", stagger);
        }
    }

    public void PlayAttackVFX(string attack)
    {

        if (vfx != null && busterSword != null)
        {
            Vector3 pos = enemy.position + vfxPositionOffset;
            Quaternion rot = busterSword.rotation * Quaternion.Euler(vfxRotationOffset);
            GameObject spawned = Instantiate(vfx, pos, rot);
            Destroy(spawned, 2f);
        }
    }
    public void PlayCureVFX()
    {

        // Check for Cloud instead of the Buster Sword
        if (cureVfx != null && cloud != null)
        {
            // Instantiate and immediately parent it to Cloud
            GameObject spawned = Instantiate(cureVfx, fakeCloud);

            // Apply position offset (e.g., Y = 0 for feet, Y = 1 for chest)
            spawned.transform.position = fakeCloud.position + vfxPositionOffset;

            // Apply rotation, staying aligned with Cloud
            spawned.transform.rotation = fakeCloud.rotation * Quaternion.Euler(vfxRotationOffset);

            Destroy(spawned, 2f);
        }
    }
    public void PlayLimitVFX()
    {

        // Check for Cloud instead of the Buster Sword
        if (limitVfx != null && cloud != null)
        {
            // Instantiate and immediately parent it to Cloud
            GameObject spawned = Instantiate(limitVfx, fakeCloud);

            // Apply position offset (e.g., Y = 0 for feet, Y = 1 for chest)
            spawned.transform.position = fakeCloud.position + vfxPositionOffset;

            // Apply rotation, staying aligned with Cloud
            spawned.transform.rotation = fakeCloud.rotation * Quaternion.Euler(vfxRotationOffset);

            Destroy(spawned, 2f);
        }
    }

    IEnumerator Turn()
    {
        yield return new WaitForSeconds(2.5f);
        sephiBattle.SephiAttack();
    }
}
