using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float damageInterval = 1f;
    float lastHitTime = -999f;

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastHitTime >= damageInterval)
            {
                lastHitTime = Time.time;
                //GameManager.cm.AddHits();
                Debug.Log("Player hit!");
            }
        }
    }
}