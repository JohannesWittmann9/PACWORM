using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip introClip;
    [SerializeField] private AudioClip gameMusicClip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = introClip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // If the Intro has finished, the game music is played
        if (!audioSource.isPlaying)
        {
            audioSource.clip = gameMusicClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        
    }
}
