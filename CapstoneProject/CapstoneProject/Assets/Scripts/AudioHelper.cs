using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static AudioSource PlayClip2D(AudioClip clip, float volume)
    {
        // create
        GameObject audioObject = new GameObject("Audio2D");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        // configure
        audioSource.clip = clip;
        audioSource.volume = volume;
        // activate
        audioSource.Play();
        Object.Destroy(audioObject, clip.length);
        // return in case other things need it
        return audioSource;
    }

}
