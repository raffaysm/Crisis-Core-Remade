using UnityEngine;

public class CloudAnimationMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private CloudBattleMovement parent;
    public string attack;
    GameObject vfx;
    void Start()
    {
        parent = GetComponentInParent<CloudBattleMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnAnimatorMove()
    {
        if(parent != null)
        {
            parent.RootMovement();
        }
    }
    public void TriggerVFX()
    {
        if (parent != null)
        {
            parent.PlayVFX(attack);
        }
    }
}
