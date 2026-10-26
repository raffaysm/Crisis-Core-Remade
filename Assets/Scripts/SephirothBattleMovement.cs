using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

public class SephirothBattleMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody rb;
    Animator animator;
    GameObject limitSpawned = null;
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
    public GameObject limitAura;
    public GameObject aura;
    public Vector3 magicVfxPositionOffset;
    public Vector3 magicVfxRotationOffset;
    public Transform sword;
    public Transform hand;
    public Transform fakeCloud;
    public Transform fakeCloud2;
    public Transform fakeSephi;
    public Transform fakeSephi3;
    public bool limit = false;
    public bool hasUsedLimit = false;

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
        // 1. HIGHEST PRIORITY: If charging, he MUST finish the Limit strike.
        if (phase1 && limit == true)
        {
            Limit2();
            return; // Stop reading so he doesn't phase change this turn
        }

        // 2. THE 60% TRIGGER: If HP reaches 60% or lower, and he hasn't used Limit yet.
        if (phase1 && cm.sephiHP <= cm.sephiMaxHP * 0.6f && hasUsedLimit == false)
        {
            hasUsedLimit = true; // Lock it so he doesn't loop this forever
            Limit1();
            return; // Stop reading so he doesn't phase change this turn
        }

        // 3. PHASE CHANGE: If he isn't charging, Limit is done, and HP is 50% or lower.
        if (phase1 && hasUsedLimit == true && cm.sephiHP <= cm.sephiMaxHP / 2f)
        {
            StartCoroutine(PhaseChange());
            return;
        }

        // 4. NORMAL PHASE 1 LOGIC: Standard attacks above 60% HP
        if (phase1)
        {
            int randomAction = Random.Range(1, 4);

            if (randomAction == 1)
            {
                SephiAttack1();
            }
            else if (randomAction == 2)
            {
                SephiAttack2();
            }
            else if (randomAction == 3)
            {
                SephiAttack3();
            }
        }

        // 5. PHASE 2 LOGIC
        if (phase2)
        {
            int randomAction = Random.Range(1, 4);

            if (randomAction == 1)
            {
                AngelAttack1();
            }
            else if (randomAction == 2)
            {
                AngelAttack2();
            }
            else if (randomAction == 3)
            {
                AngelAttack3();
            }
        }
    }

    public void Limit1()
    {
        animator.SetTrigger("Limit");
        limit = true;
        StartCoroutine(Turn());
    }
    public void Limit2()
    {
        StartCoroutine(LimitHit());
    }
    IEnumerator LimitHit()
    {
        animator.SetTrigger("LimitHit");
        sounds.Limit();
        yield return new WaitForSeconds(1.5f);

        // NOW safely destroy the aura exactly when the hit connects
        if (limitSpawned != null)
        {
            Destroy(limitSpawned);
            limitSpawned = null;
        }
        EnemyDamageTaken(true);
        cloudDamage.PlayOneShot(aargh, 0.5f);
        Vector3 direction = (enemy.position - sephi.position).normalized;
        sephi.position = enemy.position - direction * distance;
        yield return new WaitForSeconds(5f);
        limit = false;
        MoveBack(350);
    }

    IEnumerator PhaseChange()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        animator.SetTrigger("Phase");
        sounds.Phase2();
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
        cloudDamage.PlayOneShot(aargh, 0.5f);
        Vector3 direction = (enemy.position - sephi.position).normalized;
        sephi.position = enemy.position - direction * distance;
        yield return new WaitForSeconds(3f);
        MoveBack(damage);
    }

    public void SephiAttack2()
    {
        StartCoroutine(SephiAttack2Hit());
    }

    IEnumerator SephiAttack2Hit()
    {
        animator.SetTrigger("Attack 2");
        sounds.Play(sounds.attacks);
        sounds.Attack2();
        yield return new WaitForSeconds(0.916f);
        EnemyDamageTaken(true);
        cloudDamage.PlayOneShot(aargh, 0.5f);
        Vector3 direction = (enemy.position - sephi.position).normalized;
        sephi.position = enemy.position - direction * distance;
        yield return new WaitForSeconds(3.083f);
        MoveBack(damage);
    }

    public void SephiAttack3()
    {
        StartCoroutine(SephiAttack3Hit());
    }

    IEnumerator SephiAttack3Hit()
    {
        animator.SetTrigger("Attack 3");
        sounds.Play(sounds.attacks);
        sounds.Attack3();
        EnemyDamageTaken(true);
        cloudDamage.PlayOneShot(aargh, 0.5f);
        Vector3 direction = (enemy.position - sephi.position).normalized;
        sephi.position = enemy.position - direction * distance;
        yield return new WaitForSeconds(3.25f);
        MoveBack(damage);
    }
    public void AngelAttack1()
    {
        StartCoroutine(AngelAttack1Hit());
    }

    IEnumerator AngelAttack1Hit()
    {
        animator.SetTrigger("Angel 1");
        sounds.Play(sounds.attacks);
        sounds.Angel1();
        yield return new WaitForSeconds(3.333f);
        EnemyDamageTaken(true);
        cloudDamage.PlayOneShot(aargh, 0.5f);
        yield return new WaitForSeconds(3f);
        MoveBack(damage);
    }
    public void AngelAttack2()
    {
        StartCoroutine(AngelAttack2Hit());
    }

    IEnumerator AngelAttack2Hit()
    {
        animator.SetTrigger("Angel 2");
        sounds.Play(sounds.attacks);
        sounds.Angel2();
        yield return new WaitForSeconds(4.875f);
        EnemyDamageTaken(true);
        cloudDamage.PlayOneShot(aargh, 0.5f);
        yield return new WaitForSeconds(3f);
        MoveBack(damage);
    }
    public void AngelAttack3()
    {
        StartCoroutine(AngelAttack3Hit());
    }

    IEnumerator AngelAttack3Hit()
    {
        animator.SetTrigger("Angel 3");
        sounds.Play(sounds.attacks);
        sounds.Angel3();
        yield return new WaitForSeconds(3.875f);
        EnemyDamageTaken(true);
        cloudDamage.PlayOneShot(aargh, 0.5f);
        yield return new WaitForSeconds(4.5f);
        MoveBack(damage);
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
    public void Aura()
    {
        // Check for Cloud instead of the Buster Sword
        if (aura != null)
        {
            // Instantiate and immediately parent it to Cloud
            GameObject spawned = Instantiate(aura, fakeSephi);

            // Apply position offset (e.g., Y = 0 for feet, Y = 1 for chest)
            spawned.transform.position = fakeSephi.position + vfxPositionOffset;

            // Apply rotation, staying aligned with Cloud
            spawned.transform.rotation = fakeSephi.rotation * Quaternion.Euler(vfxRotationOffset);

            Destroy(spawned, 6f);
        }
    }
    public void LimitAura()
    {
        if (limitAura != null && limitSpawned == null)
        {
            limitSpawned = Instantiate(limitAura, fakeSephi3);
            limitSpawned.transform.position = fakeSephi3.position + vfxPositionOffset;
            limitSpawned.transform.rotation = fakeSephi3.rotation * Quaternion.Euler(vfxRotationOffset);
        }
    }
    void MoveBack(int damageDealt)
    {
        
        cm.cloudHP = cm.cloudHP - damageDealt;
        cm.cloudDamageTaken = cm.cloudDamageTaken + damageDealt;
        sephi.localPosition = Vector3.zero;
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
