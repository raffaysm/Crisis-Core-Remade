using UnityEngine;

public class testanimation : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    public SephirothTest parent;
    public string attack;
    GameObject vfx;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
        parent = GetComponentInParent<SephirothTest>();
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