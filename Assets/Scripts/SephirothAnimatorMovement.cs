using UnityEngine;

public class SephirothAnimatorMovement : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
    }

    void OnAnimatorMove()
    {
        if (rb && animator)
        {
            rb.MovePosition(rb.position + animator.deltaPosition);
        }
    }
}