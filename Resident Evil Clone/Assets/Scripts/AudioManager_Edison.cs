using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Edison : MonoBehaviour
{
    public static AudioManager_Edison instance;

    [SerializeField] AudioSource audioSource;

    [SerializeField] public AudioClip[] zombieDamage;
    [SerializeField] public AudioClip rayGun;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]); // Play random audio clip on start
    }

    /// <summary>
    /// Play a single audio clip
    /// </summary>
    /// <param name="audioClip"></param>
    public void PlayOneShot(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    /// <summary>
    /// Plays a random audio clip from an array of audio clips
    /// </summary>
    /// <param name="audioClipArray"></param>
    public void PlayRandomOneShot(AudioClip[] audioClipArray)
    {
        audioSource.PlayOneShot(audioClipArray[Random.Range(0, audioClipArray.Length)]);
    }
}
