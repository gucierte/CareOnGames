using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class memoryGame : MonoBehaviour, iPlayerInteraction
{
    static memoryGame instance;
    public static memoryGame main
    {
        get
        {
            if (!instance)
            {
                instance = FindFirstObjectByType<memoryGame>();
            }
            return instance;
        }
    }

    public Animator anim;
    public List<Transform> positions = new List<Transform>();
    public List<memoryGameCard> cards = new List<memoryGameCard>();
    public bool get;
    public static bool Ready;
    public static bool Shuffled;
    [Space]
    public Camera MobileCam;
    public Ped player;
    [Space]
    public AudioSource DoneSound;
    public AudioClip Music_Low;
    public AudioClip Music_High;

    [Space]
    public int InitialTime = 120;
    public int CurrentTime { get; set; }
    public TextMeshProUGUI TimeLabel;
    public AudioSource TimeSFX;
    public GameObject GameOver;
    public int currentLevel { get; set; }

    public bool isPlaying { get; set; }

    public void OnDrawGizmos()
    {
        if (get)
        {
            cards = GetComponentsInChildren<memoryGameCard>().ToList();
            foreach (var c in cards)
            {
                positions.Add(c.transform.parent);
            }
            get = false;
        }
    }
    public void Start()
    {
        InvokeRepeating(nameof(TimerBehaviour), 1, 1);
    }
    private void Update()
    {
        Ready = Shuffled;

        if (CheatCode.CheatCheck("p,l,a,y,m,e,m,o,r,y"))
        {
            //Play();
        }

        if (CheatCode.CheatCheck("s,t,o,p"))
        {
            //Stop();
        }

        if (CheatCode.CheatCheck("f,l,i,p,a,l,l"))
        {
            //FlipAll();
        }
    }

    void FlipAll()
    {
        foreach (var c in cards)
        {
            c.Flip();
        }
    }

    public void Shuffle()
    {
        List<Transform> tmp = positions.OrderBy(x => Guid.NewGuid()).ToList();
        positions = tmp;

        for (int i = 0; i < positions.Count; i++)
        {
            cards[i].transform.parent = positions[i];
            gameTime = 0;
        }

        Shuffled = true;
    }
    public static float gameTime;

    private void FixedUpdate()
    {
        if (Shuffled)
        {
            gameTime += Time.fixedDeltaTime;
        } else
        {
            gameTime = 0;
        }

        //sceneAnim.SetBool("Start", isPlaying);

        MobileCam.enabled = false;
        anim.SetBool("IsMobile", false);
        //player.gameObject.SetActive(true);
    }

    public void Play()
    {
        CurrentTime = Mathf.Clamp(InitialTime - (currentLevel * 30), 30, 9999);
        anim.SetBool("GameOver", false);
        isPlaying = true;
        this.gameObject.SetActive(true);
        Debug.Log("Play");
        anim.SetBool("Start", true);
        //sceneAnim.gameObject.SetActive(true);
        gameTime = 0;
        CareOnLevel.main.anim.SetBool("MemoryGame", true);
        MusicFlow.main.SetMusic(Music_High, Music_Low, .5f);
    }

    public void Stop()
    {
        Debug.Log("Stop");
        anim.SetBool("GameOver", false);
        currentLevel = 0;
        gameTime = 0;
        foreach (var item in cards)
        {
            item.UnFlip();
            item.Done = false;
            item.flipped = false;
        }
        anim.SetBool("Start", false);
        //sceneAnim.SetBool("Start", false);
        Shuffled = false;
        Ready = false;
        isPlaying = false;
        CareOnLevel.main.anim.SetBool("MemoryGame", false);
        Invoke(nameof(DisableThisObject), 3);
        CareOnLevel.ResetLevelMusic();
        //this.gameObject.SetActive(false);
    }

    public void TimerBehaviour()
    {
        if (isPlaying && Shuffled)
        {
            CurrentTime -= 1;
            TimeSFX.Play();

            TimeLabel.text = $"{TimeSpan.FromSeconds(CurrentTime).ToString(@"mm\:ss")}";

            if (CurrentTime <= 0)
            {
                anim.SetBool("GameOver", true);
                GameOver.SetActive(true);
            }
        }
    }

    public void DisableThisObject()
    {
        this.gameObject.SetActive(false);
    }

    public void Replay()
    {
        gameTime = 0;
        foreach (var item in cards)
        {
            item.UnFlip();
            item.Done = false;
            item.flipped = false;
        }
        anim.SetBool("Start", false);
        Shuffled = false;
        Ready = false;
        currentLevel += 1;

        Invoke(nameof(Play), .3f);
    }

    public void CheckFinish()
    {
        bool finished = true;
        foreach (var item in cards)
        {
            if (!item.Done)
            {
                finished = false;
            }
        }

        Debug.Log("Finished!");

        if (finished == true)
        {
            FinishGame();
        }
    }

    public void FinishGame()
    {
        Debug.Log("130");
        foreach (var item in cards)
        {
            item.anim.SetBool("Done", false);
        }
        Debug.Log("136");
        DoneSound.PlayDelayed(1.5f);
        Invoke(nameof(Replay), 3);
    }

    public void OnPlayerAim(XRController controller){}

    public void OnPlayerTriggerDown(XRController controller)
    {
        Ready = Shuffled;
    }

    public void OnPlayerAim(MobileControll controller)
    {
    }
}
