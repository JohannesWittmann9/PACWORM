using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip intro;
    [SerializeField] AudioClip gameNormal;
    [SerializeField] AudioClip ghostsScared;
    [SerializeField] AudioClip ghostDead;

    // Start is called before the first frame update
    void Start()
    {
        GameObject audio = GameObject.Find("BackgroundMusic");
        DontDestroyOnLoad(audio);
        PlayIntro();
    }

    // Update is called once per frame
    void Update()
    {
        
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
