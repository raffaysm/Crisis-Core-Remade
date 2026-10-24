using UnityEngine;

public class SephirothAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] attacks;
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip attack3;
    public AudioClip phase2;
    public AudioClip angel1;
    public AudioClip angel2;
    public AudioClip angel3;    
    public void Play(AudioClip[] clips)
    {
        if (clips.Length == 0) return;
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
    public void Attack1()
    {
        audioSource.PlayOneShot(attack1);
    }
    public void Attack2()
    {
        audioSource.PlayOneShot(attack2);
    }
    public void Attack3()
    {
        audioSource.PlayOneShot(attack3);
    }
    public void Phase2()
    {
        audioSource.PlayOneShot(phase2, 2f);
    }
    public void Angel1()
    {
        audioSource.PlayOneShot(angel1);
    }
    public void Angel2()
    {
        audioSource.PlayOneShot(angel2);
    }
    public void Angel3()
    {
        audioSource.PlayOneShot(angel3);
    }
}