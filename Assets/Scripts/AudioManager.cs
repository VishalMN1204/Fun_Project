using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip cardFlipSound;
    [SerializeField] AudioClip correctSound;
    [SerializeField] AudioClip incorrectSound;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayCardFlipSound()
    {
        sfxSource.clip = cardFlipSound;
        sfxSource.Play();
    }

    public void PlayWinLoseSound(bool result)
    {
        sfxSource.clip = result ? correctSound : incorrectSound;
        sfxSource.Play();
    }
}
