using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private AudioSource source;
    [SerializeField] AudioClip intro;
    [SerializeField] AudioClip gameNormal;
    [SerializeField] AudioClip ghostsScared;
    [SerializeField] AudioClip ghostDead;

    private static GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        
        DontDestroyOnLoad(gameObject);

        if (player == null)
        {
            player = gameObject;
        }

        else
        {
            Destroy(gameObject);
            return;
        }

        source = GetComponent<AudioSource>();
        PlayIntro();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Stop()
    {
        source.Stop();
    }

    public void PlayIntro()
    {
        source.clip = intro;
        source.Play();
    }

    public void PlayGame()
    {
        source.clip = gameNormal;
        source.Play();
    }

    public void PlayScared()
    {
        source.clip = ghostsScared;
        source.Play();
    }

    public void PlayGhostDead()
    {
        source.clip = ghostDead;
        source.Play();
    }
}
