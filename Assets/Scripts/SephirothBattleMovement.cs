using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

public class SephirothBattleMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody rb;
    Animator animator;
    CombatManager cm;
    public AudioSource cloudDamage;
    public AudioClip aargh;
    public CloudBattleMovement cloudBattle;
    public Animator cloudAnimator;
    public SephirothAudio sounds;
    public Transform enemy;
    public Transform sephi;
    public float distance = 2f;
    public float tilt = 10f;
    private int damage = 150;
    public bool phase1 = true;
    public bool phase2 = false;
    public GameObject vfx;
    public Vector3 vfxPositionOffset;
    public Vector3 vfxRotationOffset;
    public Transform sword;

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
        if(rb && animator)
        {
            Vector3 newPosition = rb.position + animator.deltaPosition;
            rb.MovePosition(newPosition);
        }
    }

    public void SephiAttack()
    {
            if (phase1 && cm.sephiHP <= cm.sephiMaxHP / 2)
            {
                StartCoroutine(PhaseChange());
                return;
            }

            if (phase1)
            {
                int randomAction = Random.Range(1, 4);

                if (randomAction == 1)
                {
                    SephiAttack1();
                }
                if (randomAction == 2)
                {
                    SephiAttack2();
                }
                if (randomAction == 3)
                {
                    SephiAttack3();
                }

            }

            if (phase2)
            {
                AngelAttack1();
            }
    }

    IEnumerator PhaseChange()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        animator.SetTrigger("Phase");
        yield return new WaitForSeconds(5.833f);
        Vector3 endPosition = sephi.position;
        phase1 = false;
        phase2 = true;
        sephi.localRotation = Quaternion.Euler(0f, 0f, tilt);
        SephiAttack();
    }
    public void SephiAttack1()
    {
        StartCoroutine(SephiAttack1Hit());
    }

    IEnumerator SephiAttack1Hit()
    {
        animator.SetTrigger("Attack 1");
        sounds.Play(sounds.attacks);
        yield return new WaitForSeconds(1.5f);
        EnemyDamageTaken(true);
        Vector3 direction = (enemy.position - sephi.position).normalized;
        sephi.position = enemy.position - direction * distance;
        yield return new WaitForSeconds(2f);
        MoveBack();
    }

    public void SephiAttack2()
    {
        StartCoroutine(SephiAttack2Hit());
    }

    IEnumerator SephiAttack2Hit()
    {
        animator.SetTrigger("Attack 2");
        sounds.Play(sounds.attacks);
        yield return new WaitForSeconds(0.916f);
        EnemyDamageTaken(true);
        Vector3 direction = (enemy.position - sephi.position).normalized;
        sephi.position = enemy.position - direction * distance;
        yield return new WaitForSeconds(3.083f);
        MoveBack();
    }

    public void SephiAttack3()
    {
        StartCoroutine(SephiAttack3Hit());
    }

    IEnumerator SephiAttack3Hit()
    {
        animator.SetTrigger("Attack 3");
        sounds.Play(sounds.attacks);
        EnemyDamageTaken(true);
        Vector3 direction = (enemy.position - sephi.position).normalized;
        sephi.position = enemy.position - direction * distance;
        yield return new WaitForSeconds(3.25f);
        MoveBack();
    }
    public void AngelAttack1()
    {
        StartCoroutine(AngelAttack1Hit());
    }

    IEnumerator AngelAttack1Hit()
    {
        animator.SetTrigger("Angel 1");
        sounds.Play(sounds.attacks);
        yield return new WaitForSeconds(3.333f);
        EnemyDamageTaken(true);
        MoveBack();
    }
    public void PlayAttackVFX(string attack)
    {
        if (vfx != null && sword != null)
        {
            Vector3 pos = enemy.position + vfxPositionOffset;
            Quaternion rot = sword.rotation * Quaternion.Euler(vfxRotationOffset);
            GameObject spawned = Instantiate(vfx, pos, rot);
            Destroy(spawned, 2f);
        }
    }
    void MoveBack()
    {
        cm.cloudHP = cm.cloudHP - damage;
        cm.cloudDamageTaken = cm.cloudDamageTaken + damage;
        sephi.localPosition = Vector3.zero;
        cloudDamage.PlayOneShot(aargh);
        EnemyDamageTaken(false);
        if (cm.cloudHP <= 0)
        {
            cm.SephirothWins();
        }
        else
        {
            StartCoroutine(Turn());
        }  
    }
    void EnemyDamageTaken(bool stagger)
    {
        cloudAnimator.SetBool("Stagger", stagger);
    }

    IEnumerator Turn()
    {
        yield return new WaitForSeconds(3f);
        cm.Combat(true, false);
    }
}
