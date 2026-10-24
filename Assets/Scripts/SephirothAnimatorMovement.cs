using UnityEngine;

public class SephirothAnimatorMovement : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    public SephirothBattleMovement parent;
    GameObject vfx;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
        parent = GetComponentInParent<SephirothBattleMovement>();
    }

    void OnAnimatorMove()
    {
        if (rb && animator)
        {
            rb.MovePosition(rb.position + animator.deltaPosition);
        }
    }
    public void TriggerAttackVFX()
    {
        if (parent != null)
        {
            parent.PlayAttackVFX();
        }
    }
    public void TriggerMagicVFX()
    {
        if (parent != null)
        {
            parent.PlayMagicVFX();
        }
    }
    public void TriggerMagicVFX2()
    {
        if (parent != null)
        {
            parent.PlayMagicVFX2();
        }
    }
    public void Explosion()
    {
        if (parent != null)
        {
            parent.Explosion();
        }
    }
    public void Aura()
    {
        if (parent != null)
        {
            parent.Aura();
        }
    }
    public void LimitAura()
    {
        if (parent != null)
        {
            parent.LimitAura();
        }
    }
}