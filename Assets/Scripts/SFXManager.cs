using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; set; }
    private AudioSource audioSource;
    public AudioClip cageHitClip;
    public AudioClip cardInsertedClip;
    public AudioClip winningSong;

    private void Awake()
    {
        Instance = this;    
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCageHitEffect()
    {
        audioSource.PlayOneShot(cageHitClip);
    }

    public void PlayCardInsertedCorrectly()
    {
        audioSource.PlayOneShot(cardInsertedClip);
    }

    public void PlayWinningSong()
    {
        audioSource.clip = winningSong;
        audioSource.Play();
    }
}