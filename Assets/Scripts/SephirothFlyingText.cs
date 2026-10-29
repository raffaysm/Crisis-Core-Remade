using UnityEngine;
using TMPro;

public class SephirothFlyingText : MonoBehaviour
{
    public float moveSpeed = 50f; // Set higher for Canvas UI (adjust in editor if too fast/slow)
    public float destroyTime = 1.5f; // How long it takes to fade and disappear

    private TMP_Text textMesh;
    private Color alphaColor;

    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        alphaColor = textMesh.color;

        // Automatically destroy this clone after 'destroyTime' seconds
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // Float upwards
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Fade out the transparency
        alphaColor.a -= Time.deltaTime / destroyTime;
        textMesh.color = alphaColor;
    }
}