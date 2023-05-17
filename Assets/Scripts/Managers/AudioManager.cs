using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;

    [Header("Shooting Audio")]
    public AudioClip gunSmall;
    public AudioClip gunMedium;
    public AudioClip gunHeavy;
    public AudioClip shotGun;

    [Header("Grapple Audio")]
    public List<AudioClip> grappleStart = new List<AudioClip>();

    [Header("Bullet Impacts")]
    public List<AudioClip> bulletImpact = new List<AudioClip>();

    [Header("Player Damage Reaction")]
    public List<AudioClip> playerDmgReact = new List<AudioClip>();

    [Header("Enemy Damage Reaction")]
    public List<AudioClip> enemyDmgReact = new List<AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        if (audioManager == null)
        {
            audioManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (audioManager != this && audioManager != null)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(AudioSource source, AudioClip clip, bool playOnce)
    {
        if (source.isPlaying && playOnce) return;

        source.clip = clip;
        // volume stuff here
        source.Play();
        //Debug.Log("Sound played");
    }

    public void StopSound(AudioSource source)
    {
        source.Stop();
        source.clip = null;
    }
}
