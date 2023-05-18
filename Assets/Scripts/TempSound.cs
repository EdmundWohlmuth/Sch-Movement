using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSound : MonoBehaviour
{
    float maxtime = 2f;
    float currentTime = 0f;
    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        AudioManager.audioManager.PlaySound(source, AudioManager.audioManager.bulletImpact[Random.Range(0, AudioManager.audioManager.bulletImpact.Count)], true);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime < maxtime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
