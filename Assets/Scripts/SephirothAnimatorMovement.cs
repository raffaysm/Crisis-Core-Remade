using UnityEngine;

public class SephirothAnimatorMovement : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    public SephirothBattleMovement parent;
    public string attack;
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
            parent.PlayAttackVFX(attack);
        }
    }
    public void TriggerCureVFX()
    {
        if (parent != null)
        {
            //parent.PlayCureVFX();
        }
    }
}