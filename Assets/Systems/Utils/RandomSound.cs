using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    public AudioSource source;
    public List<AudioClip> clips;


    private void OnEnable()
    {
        source.clip = clips[Random.Range(0, clips.Count)];
        if (source.playOnAwake)
        {
            source.Play();
        }
    }

    private void OnValidate()
    {
        if (!source)
        {
            source = GetComponent<AudioSource>();
        }
    }
}
