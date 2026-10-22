using UnityEngine;

public class SephirothAudioTest : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] attacks;

    public void Play(AudioClip[] clips)
    {
        if (clips.Length == 0) return;
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}