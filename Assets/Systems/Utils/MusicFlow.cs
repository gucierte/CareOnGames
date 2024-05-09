using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFlow : MonoBehaviour
{
    static MusicFlow instance;
    public static MusicFlow main { get { if (!instance) { instance = FindFirstObjectByType<MusicFlow>(); } return instance; } }

    [SerializeField] AudioClip HighMusic;
    [SerializeField] AudioClip LowMusic;
    public float Flow { get; set; }


    public List<AudioSource> Sources = new List<AudioSource>();

    public AudioSource highSource { get; set; }
    public AudioSource lowSource { get; set; }

    public AudioSource GetHighSource()
    {
        AudioSource r = null;

        foreach (var s in Sources)
        {
            if (s.gameObject.name == HighMusic.name + "_High")
            {
                r = s;
            }
        }

        highSource = r;

        return r;
    }
    public AudioSource GetLowSource()
    {
        AudioSource r = null;

        foreach (var s in Sources)
        {
            if (s.gameObject.name == LowMusic.name + "_Low")
            {
                r = s;
            }
        }

        lowSource = r;

        return r;
    }

    public float volume = 1;
    public float pitch = 1;

    public void SetMusic(AudioClip high, AudioClip low, float _volume)
    {
        HighMusic = high;
        LowMusic = low;

        volume = _volume;

        if (GetHighSource() == null)
        {
            AudioSource h = new GameObject(high.name + "_High").AddComponent<AudioSource>();
            h.transform.parent = transform;
            h.clip = high;
            h.volume = 0;
            h.loop= true;
            h.Play();
            Sources.Add(h);
            highSource = h;
        }

        if (GetLowSource() == null)
        {
            AudioSource h = new GameObject(low.name + "_Low").AddComponent<AudioSource>();
            h.transform.parent = transform;
            h.clip = low;
            h.volume = volume;
            h.loop= true;
            h.Play();
            Sources.Add(h);
            lowSource= h;
        }
    }

    private void LateUpdate()
    {
        //Mute other sources
        foreach (var s in Sources)
        {
            if (s.gameObject.name != HighMusic.name && s.gameObject.name != HighMusic.name)
            {
                s.volume = Mathf.Lerp(s.volume, 0, 5 * Time.deltaTime);
            }
        }

        Flow = Mathf.Lerp(Flow, 0, .5f*Time.deltaTime);

        highSource.volume = Mathf.Lerp(highSource.volume, Mathf.Clamp(Flow, 0, volume), 3 * Time.deltaTime);
        lowSource.volume = Mathf.Lerp(lowSource.volume, volume - Mathf.Clamp(Flow, 0, volume), 3 * Time.deltaTime);

        highSource.pitch = pitch;
        lowSource.pitch = pitch;
    }
}
