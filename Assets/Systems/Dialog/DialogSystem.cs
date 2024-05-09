using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Properties;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogSystem : MonoBehaviour
{
    public bool playOnEnable;
    public int NextDialogIndex { get; set; }
    public dialog currentDialog { get; set; }

    [Space]
    public AudioSource source;
    public float GetDialogSamplesPercent()
    {
        if (source.clip)
        {
            return source.timeSamples / source.clip.samples;
        } else
        {
            return 1;
        }
    }
    public float GetDialogTimePercent()
    {
        if (source.clip)
        {
            return source.time / source.clip.length;
        } else
        {
            return 1;
        }
    }
    public float GetDialogTimeToEnd()
    {
        if (source.clip)
        {
            return source.clip.length - source.time;
        }
        else
        {
            return 0;
        }
    }

        [System.Serializable]
    public class dialog
    {
        [HideInInspector] public string name;
        [Tooltip("Play this dialog automatically?")] public bool PlayAutomatially;
        [Tooltip("Wait current dialog, then play this")] public bool WaitToPlay;
        public AudioClip clip;
        [TextArea()] public string Subtitle;
        [Tooltip("The next clip to play \n (-1 = Next clip on index | -2 = Play nothing after this)")]public int NxtClip = -1;
        public int RealNxtClip { get; set; }
    }
    [SerializeField]
    public List<dialog> Dialogs = new List<dialog>();

    public void Play()
    {
        Play(NextDialogIndex);
    }

    public void Play(int dialogIndex)
    {
        NextDialogIndex = dialogIndex;
        Play(Dialogs[dialogIndex]);
    }

    public void Play(int dialogIndex, bool wait)
    {
        NextDialogIndex = dialogIndex;
        Play(Dialogs[dialogIndex], wait);
    }

    public void Play(dialog dialog)
    {
        NextDialogIndex = Dialogs.IndexOf(dialog);
        Play(dialog, dialog.WaitToPlay);
    }

    public async void Play(dialog dialog, bool wait)
    {
        //Set next dialog
        NextDialogIndex = Dialogs.IndexOf(dialog);

        //Wait to play next
        if (wait)
        {
            await Task.Delay(((int)GetDialogTimeToEnd() * 1000));
            if (Application.isPlaying)
            {
                source.clip = dialog.clip;
                source.Play();
                currentDialog = dialog;
            }
        }
        else
        {
            //Or play next immediatly
            source.clip = dialog.clip;
            source.Play();
            currentDialog = dialog;
        }

        //Prepare to play next dialog
        if (dialog.RealNxtClip >= 0 && dialog.NxtClip != -2)
        {
            Play(Mathf.Clamp(dialog.RealNxtClip, 0 , Dialogs.Count));
        }
    }

    public void Pause()
    {
        source.Pause();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void PlayNext()
    {
        if (currentDialog.RealNxtClip > 0)
        {
            Play(Mathf.Clamp(currentDialog.RealNxtClip, 0, Dialogs.Count));
        }
    }

    public void RepeatCurrent()
    {
        Play(currentDialog, true);
    }

    private void OnEnable()
    {
        source.clip = null;

        if (playOnEnable)
        {
            Play(0,false);
        }

        for (int i = 0; i < Dialogs.Count; i++)
        {
            if (Dialogs[i].NxtClip == -1)
            {
                Dialogs[i].RealNxtClip = i + 1;
            }
        }
        NextDialogIndex = 1;
    }

    public void OnValidate()
    {
        if (!source)
        {
            source = GetComponent<AudioSource>();
        }

        foreach (var item in Dialogs)
        {
            item.name = item.clip.name;
        }
    }
}
