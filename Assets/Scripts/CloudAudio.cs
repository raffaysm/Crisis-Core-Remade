using UnityEngine;

public class CloudAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] braver;
    public AudioClip[] focused;
    public AudioClip[] punisher;
    public AudioClip[] cure;
    public AudioClip[] ascension;
    public AudioClip braverAttack;
    public AudioClip focusedAttack;
    public AudioClip punisherAttack;
    public AudioClip cureHeal;
    public AudioClip limitBreak;

    public void Play(AudioClip[] clips)
    {
        if (clips.Length == 0) return;
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    public void Braver()
    {
        audioSource.PlayOneShot(braverAttack);
    }

    public void Focused()
    {
        audioSource.PlayOneShot(focusedAttack);
    }
    public void Punisher()
    {
        audioSource.PlayOneShot(punisherAttack);
    }
    public void Cure()
    {
        audioSource.PlayOneShot(cureHeal);
    }
    public void Limit_Break()
    {
        audioSource.PlayOneShot(limitBreak);
    }
}