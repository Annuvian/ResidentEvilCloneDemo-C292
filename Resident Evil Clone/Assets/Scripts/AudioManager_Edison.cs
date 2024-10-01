using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Edison : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    [SerializeField] public AudioClip zombieDamage;

    public static AudioManager_Edison instance;

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
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]); // Play random audio clip on start
    }

    public void PlayOneShot(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
