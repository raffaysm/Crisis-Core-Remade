using UnityEngine;

public class CloudAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] braver;
    public AudioClip[] focused;
    public AudioClip[] punisher;
    public AudioClip[] cure;
    public AudioClip[] ascension;

    public void Play(AudioClip[] clips)
    {
        if (clips.Length == 0) return;
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}