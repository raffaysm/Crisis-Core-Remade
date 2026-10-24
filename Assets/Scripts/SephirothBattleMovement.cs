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
    public GameObject magicVfx;
    public GameObject magicHitVfx;
    public GameObject explosion;
    public Vector3 magicVfxPositionOffset;
    public Vector3 magicVfxRotationOffset;
    public Transform sword;
    public Transform hand;
    public Transform fakeCloud;
    public Transform fakeCloud2;

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

            /*if (phase1)
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

            }*/

            if (phase1)
            {
                SephiAttack1();
            }

            if (phase2)
            {
                AngelAttack3();
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
        sounds.Attack1();
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
        yield return new WaitForSeconds(3f);
        MoveBack();
    }
    public void AngelAttack2()
    {
        StartCoroutine(AngelAttack2Hit());
    }

    IEnumerator AngelAttack2Hit()
    {
        animator.SetTrigger("Angel 2");
        sounds.Play(sounds.attacks);
        yield return new WaitForSeconds(4.875f);
        EnemyDamageTaken(true);
        yield return new WaitForSeconds(3f);
        MoveBack();
    }
    public void AngelAttack3()
    {
        StartCoroutine(AngelAttack3Hit());
    }

    IEnumerator AngelAttack3Hit()
    {
        animator.SetTrigger("Angel 3");
        sounds.Play(sounds.attacks);
        yield return new WaitForSeconds(3.875f);
        EnemyDamageTaken(true);
        yield return new WaitForSeconds(4.5f);
        MoveBack();
    }
    public void PlayAttackVFX()
    {
        if (vfx != null && sword != null)
        {
            Vector3 pos = enemy.position + vfxPositionOffset;
            Quaternion rot = sword.rotation * Quaternion.Euler(vfxRotationOffset);
            GameObject spawned = Instantiate(vfx, pos, rot);
            Destroy(spawned, 2f);
        }
    }
    public void PlayMagicVFX()
    {
        if (magicVfx != null && hand != null)
        {
            Vector3 pos = hand.position + magicVfxPositionOffset;
            Quaternion rot = hand.rotation * Quaternion.Euler(magicVfxRotationOffset);

            GameObject spawned = Instantiate(magicVfx, pos, rot);
            spawned.transform.SetParent(hand); // Locks the VFX to the moving hand

            Destroy(spawned, 2f);
        }
    }
    public void PlayMagicVFX2()
    {
        // Check for Cloud instead of the Buster Sword
        if (magicHitVfx != null)
        {
            // Instantiate and immediately parent it to Cloud
            GameObject spawned = Instantiate(magicHitVfx, fakeCloud);

            // Apply position offset (e.g., Y = 0 for feet, Y = 1 for chest)
            spawned.transform.position = fakeCloud.position + vfxPositionOffset;

            // Apply rotation, staying aligned with Cloud
            spawned.transform.rotation = fakeCloud.rotation * Quaternion.Euler(vfxRotationOffset);

            Destroy(spawned, 2f);
        }
    }
    public void Explosion()
    {
        // Check for Cloud instead of the Buster Sword
        if (explosion != null)
        {
            // Instantiate and immediately parent it to Cloud
            GameObject spawned = Instantiate(explosion, fakeCloud2);

            // Apply position offset (e.g., Y = 0 for feet, Y = 1 for chest)
            spawned.transform.position = fakeCloud2.position + vfxPositionOffset;

            // Apply rotation, staying aligned with Cloud
            spawned.transform.rotation = fakeCloud2.rotation * Quaternion.Euler(vfxRotationOffset);

            Destroy(spawned, 5f);
        }
    }
    void MoveBack()
    {
        cm.cloudHP = cm.cloudHP - damage;
        cm.cloudDamageTaken = cm.cloudDamageTaken + damage;
        sephi.localPosition = Vector3.zero;
        EnemyDamageTaken(false);
        if (cm.cloudHP <= 0)
        {
            cm.SephirothWins();
        }
        else
        {
            cloudDamage.PlayOneShot(aargh);
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
