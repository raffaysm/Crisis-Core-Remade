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
            parent.PlayCureVFX();
        }
    }
    public void TriggerLimitVFX()
    {
        if (parent != null)
        {
            parent.PlayLimitVFX();
        }
    }


}
