using EzySlice;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class FruitNinja : MonoBehaviour
{
    static FruitNinja instance;

    public static FruitNinja Main()
    {
        if (!instance)
        {
            instance = FindFirstObjectByType<FruitNinja>();
        }

        return instance;
    }

    [Header("Main")]
    public List<Transform> positions = new List<Transform>();
    [Space]
    public List<FruitNinjaFruit> Fruits = new List<FruitNinjaFruit>();
    public float Range = 1;
    public float MaxSpeed = 1;
    public float SpawnDelay = 1;
    [Space]
    public FruitNinjaSaber LeftSaber;
    public FruitNinjaSaber RightSaber;
    [Header("Score")]
    public float ScoreMultipiler = 1;
    public int CurrentScore;
    public TextMeshProUGUI ScoreLabel;
    [Header("Music/SFX")]
    public AudioSource SlowMotionSFX;
    [Space]
    public float MaxMusicVolume = 0.5f;
    //public AudioSource MusicHigh;
    //public AudioSource MusicLow;

    public AudioClip MusicHigh;
    public AudioClip MusicLow;

    float mFlow;
    public float PlayTime { get; set; }
    public bool isPlaying { get; set; }

    
    public void Play(int delayMs)
    {
        Debug.Log("Playing Fruit Ninja in " + delayMs + "ms");
        CareOnLevel.main.anim.SetBool("FruitNinja", true);
        Invoke("Play", (delayMs / 1000));
    }

    public FruitNinjaFruit SpawnFruit(FruitNinjaFruit spawn, Vector3 speedMultipiler, Vector3 pos)
    {
        FruitNinjaFruit r = Instantiate(spawn.gameObject).GetComponent<FruitNinjaFruit>();
        
        r.transform.position = pos;

        r.rb.velocity = speedMultipiler * r.DefaultSpeed + (Vector3.down * r.DefaultSpeedDown);

        r.transform.eulerAngles = new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));

        return r;
    }

    public void SpawnSpawnRandomFruit()
    {
        Transform poses = positions[Random.Range(0, positions.Count)];
        Vector3 pos = poses.transform.position + (Random.insideUnitSphere * Range);

        SpawnFruit(Fruits[Random.Range(0, Fruits.Count)], (poses.transform.forward * MaxSpeed), pos);
        if (enabled && gameObject.activeInHierarchy)
        {
            Invoke(nameof(SpawnSpawnRandomFruit), (int)((Random.Range(1000, 5000) * SpawnDelay) / ((Mathf.Clamp(CurrentScore, 0, 30) * ScoreMultipiler) + 1)) / 800);
        }
    }

    [Header("Fruit Missed")]
    public TextMeshProUGUI FruitMissedLabel;
    public int FruitsMissed { get; set; }
    public void MissFruit(FruitNinjaFruit fruit)
    {
        FruitsMissed += 1;
        //FruitMissedLabel.text = $"Erros: <b>{FruitsMissed}</b>";
        CurrentScore -= 1;
        CurrentScore = Mathf.Clamp(CurrentScore, 0, int.MaxValue);
        ScoreLabel.color = Color.red;
        Destroy(fruit.gameObject);
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        //Play();
#endif
    }


    private void Play()
    {
        this.gameObject.SetActive(true);
        Debug.Log("Playing Fruit Ninja");

        LeftSaber.gameObject.SetActive(true);
        RightSaber.gameObject.SetActive(true);
        SaveScore();
        ComputeHighscore();
        Invoke(nameof(SpawnSpawnRandomFruit), 3);

        Lifes = MaxLifes;
        LifesLabel.text = "";
        for (int i = 0; i < MaxLifes; i++)
        {
            LifesLabel.text += "⚫";
        }

        StopTimeLabel.text = "";
        ScoreLabel.text = "";
        FruitMissedLabel.text = "";

        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        isPlaying = true;

        MusicFlow.main.SetMusic(MusicHigh, MusicLow, .8f);
    }
    public void Stop()
    {
        CurrentScore = 0;
        Lifes = MaxLifes;
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        isPlaying = false;
        LeftSaber.gameObject.SetActive(false);
        RightSaber.gameObject.SetActive(false);
        GameOver_Label.gameObject.SetActive(false);
        mFlow = 0;
        MusicFlow.main.pitch = 1;

        PlayTime = 0;
        FruitsMissed = 0;
        stopTime = 0;
        CareOnLevel.ResetLevelMusic(); Clear();

        this.gameObject.SetActive(false);
    }

    public void Clear()
    {
        foreach (var item in FruitNinjaFruit.fruits)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in FruitNinjaFruit.allParts)
        {
            Destroy(item.gameObject);
        }

        FruitNinjaFruit.fruits.Clear();
        FruitNinjaFruit.allParts.Clear();
    }

    [Header("GameOver")]
    public GameObject GameOver_Label;
    public TextMeshProUGUI GameOver_Score_Label;
    public TextMeshProUGUI GameOver_MaxScore_Label;
    public TextMeshProUGUI GameOver_Time_Label;
    public void GameOver()
    {
        GameOver_Score_Label.text = "Score:" + CurrentScore;
        GameOver_MaxScore_Label.text = "Highscore: " + PlayerPrefs.GetInt("Fruitninja_Highscore");
        GameOver_Time_Label.text = "Tempo de jogo: " + System.TimeSpan.FromSeconds(PlayTime).ToString("mm\\:ss");;
        isPlaying = false;

        slowmoTimeSpan = 0;
        Time.timeScale = 0.04f;
        Time.fixedDeltaTime = 0.0004f;

        mFlow = 0;
        MusicFlow.main.pitch = 0.3f;

        GameOver_Label.gameObject.SetActive(true);

        foreach (var f in FruitNinjaFruit.fruits)
        {
            f.Explode(transform.position, Vector3.zero);
        }
        CancelInvoke(nameof(SpawnSpawnRandomFruit));
    }

    public void OnDisable()
    {
        LeftSaber.gameObject.SetActive(false);
        RightSaber.gameObject.SetActive(false);

        CareOnLevel.main.anim.SetBool("FruitNinja", false);
        SaveScore();
    }
    [Header("Highscore")]
    public TextMeshProUGUI HighScoreLabel;
    public void ComputeHighscore()
    {
        SaveScore();
        if (CurrentScore > PlayerPrefs.GetInt("Fruitninja_Highscore"))
        {
            HighScoreLabel.text = $"<Color=#F1DCFF>*New   High Score: <b>{PlayerPrefs.GetInt("Fruitninja_Highscore")}</b></Color>";
        }
        else
        {
            HighScoreLabel.text = $"<Color=#F1DCFF>High Score: <b>{PlayerPrefs.GetInt("Fruitninja_Highscore")}</b>";
        }
    }

    public void SaveScore()
    {
        if (!PlayerPrefs.HasKey("Fruitninja_Highscore"))
        {
            PlayerPrefs.SetInt("Fruitninja_Highscore", -1);
        }

        if (PlayerPrefs.GetInt("Fruitninja_Highscore") < CurrentScore)
        {
            PlayerPrefs.SetInt("Fruitninja_Highscore", CurrentScore);
        }
    }

    //The master void
    [Header("Lifes")]
    public int MaxLifes = 4;
    public int Lifes { get; set; }
    public TextMeshProUGUI LifesLabel;

    public void SaberBehaviour(XRController controller)
    {
        if (controller.ray.hit.collider)
        {
            if (controller.ray.hit.collider.GetComponent<FruitNinjaFruit>())
            {
                FruitNinjaFruit fruit = controller.ray.hit.collider.GetComponent<FruitNinjaFruit>();
                //MusicLow.timeSamples = MusicHigh.timeSamples;

                if (fruit != null)
                {
                    fruit.Health -= 50;
                    if (fruit.col)
                    {
                        fruit.col.radius = Mathf.Clamp(fruit.col.radius - 0.3f, 0.2f, fruit.col.radius);
                    }

                    if (fruit.Health <= 0)
                    {
                        /*Vector3 cutDir = -controller.Velocity;
                        float angle = Mathf.Atan2(cutDir.x, cutDir.z) + 1.5708f;

                        cutDir.x = Mathf.Sin(angle);
                        cutDir.y = Mathf.Cos(angle);
                        cutDir.z = Mathf.Tan(angle);*/

                        //GameObject[] parts = fruit.Slice(controller.ray.hit.point, cutDir);

                        if (fruit.isBomb)
                        {
                            fruit.Explode(controller.ray.hit.point, -controller.Velocity);
                            CurrentScore -= Mathf.Abs(fruit.Score);
                            mFlow = 0;

                            CareOnLevel.SetEmissionColorOnThisFrame(new Color(1, 0, 0, 1));

                            Color c;
                            ColorUtility.TryParseHtmlString("#FF6F5B", out c);
                            ScoreLabel.color = c;

                            foreach (var f in FruitNinjaFruit.fruits)
                            {
                                f.Explode(transform.position, Vector3.zero);
                            }

                            Lifes -= 1;
                            LifesLabel.text = "";
                            for (int i = 0; i < MaxLifes - Lifes; i++)
                            {
                                LifesLabel.text += "⚪";
                            }

                            for (int i = 0; i < Lifes; i++)
                            {
                                LifesLabel.text += "⚫";
                            }

                            if (Lifes <= 0)
                            {
                                GameOver();
                            }
                        }
                        else
                        {
                            if (controller.Velocity.magnitude >= 2)
                            {
                                CurrentScore += fruit.Score * 2;
                            } else
                            {
                                CurrentScore += fruit.Score;
                            }

                            mFlow += .04f;

                            Color c;
                            ColorUtility.TryParseHtmlString("#DAFFA5", out c);
                            ScoreLabel.color = c;
                            CareOnLevel.SetEmissionColorOnThisFrame(new Color(0, 1, .5f, 1));

                            GameObject[] parts = fruit.Slice(controller.ray.hit.point, -controller.Velocity);

                            foreach (var p in parts)
                            {
                                //p.transform.position += (fruit.transform.InverseTransformPoint(p.transform.position).normalized) * 0.1f;
                                MeshCollider m = p.AddComponent<MeshCollider>();
                                Rigidbody rb = p.AddComponent<Rigidbody>();
                                m.sharedMesh = p.GetComponent<MeshFilter>().mesh;
                                m.convex = true;
                                rb.velocity = fruit.rb.velocity * 0.3f;
                                rb.AddExplosionForce(500, fruit.transform.position, 10);
                                rb.mass = 0.3f;
                                rb.angularVelocity *= 10;
                                //FruitNinjaFruit f = p.AddComponent<FruitNinjaFruit>();
                                //f.Health = 1;
                            }

                            //StartCoroutine(addParts(parts));
                            FruitNinjaFruit.allParts.AddRange(parts);
                            Destroy(fruit.gameObject);
                        }
                        CurrentScore = Mathf.Clamp(CurrentScore, 0, int.MaxValue);
                        mFlow = Mathf.Clamp(mFlow, 0, 3);
                        ScoreLabel.text = CurrentScore.ToString("00");
                        ComputeHighscore();
                        Destroy(controller.ray.hit.collider.GetComponent<FruitNinjaFruit>());
                    }
                }
            }
            if (FruitNinjaFruit.allParts.Contains(controller.ray.hit.collider.gameObject) && (stopTime > 0 || controller.Velocity.magnitude >= 2))
            {
                GameObject go = controller.ray.hit.collider.gameObject;

                Vector3 v = new Vector3(controller.Velocity.y, controller.Velocity.x, controller.Velocity.z);
                GameObject[] parts = go.SliceInstantiate(controller.ray.hit.point, -v, SliceMaterial);
                foreach (var p in parts)
                {
                    p.layer = go.layer;
                    MeshCollider m = p.AddComponent<MeshCollider>();
                    Rigidbody rb = p.AddComponent<Rigidbody>();
                    m.sharedMesh = p.GetComponent<MeshFilter>().mesh;
                    m.convex = true;
                    rb.velocity = go.GetComponent<Rigidbody>().velocity;
                    rb.AddExplosionForce(500, controller.ray.hit.point, 10);
                    rb.mass = 0.3f;
                    rb.angularVelocity *= 10;
                    StartCoroutine(DestryPart(p));
                }
                FruitNinjaFruit.allParts.AddRange(parts);
                FruitNinjaFruit.allParts.Remove(go);
                Destroy(go);
            }
        }
    }

    IEnumerator DestryPart(GameObject part)
    {
        yield return new WaitForSeconds(5);
        FruitNinjaFruit.allParts.Remove(part);
        Destroy(part);
    }

    [Header("Slowmotion")]
    public TextMeshProUGUI StopTimeLabel;
    public float stopTime { get; set; }
    float slowmoTimeSpan;

    void TimeCountdown()
    {
        stopTime -= 1;
        if (stopTime <= 0)
        {
            ResetTime();
        }
    }

    public void StopTime(int time)
    {
        stopTime = time;
    }

    public void ResetTime()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }

    private void Update()
    {
        if (CheatCode.CheatCheck("c,u,t,a,l,l"))
        {
            foreach (var item in FruitNinjaFruit.fruits)
            {
                item.Slice(item.transform.position, new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)));
            }
        }

        if (!isPlaying)
            return;

        SaberBehaviour(WebXR_Manager.leftHand);
        SaberBehaviour(WebXR_Manager.rightHand);
        
        PlayTime += Time.deltaTime;
        
        if (stopTime > 0)
        {
            SetGameSpeed(0.04f);
            SlowMotionSFX.enabled = true;
            MusicFlow.main.pitch = 0.5f;
            mFlow = 0;
            StopTimeLabel.text = $"🕓  {stopTime}s";
        }
        else
        {
            StopTimeLabel.text = "";
            if (SlowMotionSFX.enabled)
            {
                MusicFlow.main.pitch = 1;
                mFlow = .5f;
                SlowMotionSFX.enabled = false;
            }
            ResetGameSpeed();
            mFlow = Mathf.Lerp(mFlow, 0, 0.2f * Time.deltaTime);
        }
    }
    public float gameSpeed { get; set; }

    public void SetGameSpeed(float speed)
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, speed, 0.3f);
        Time.fixedDeltaTime = Mathf.Lerp(Time.fixedDeltaTime, Time.timeScale * 0.02f, 0.3f);
    }
    public void ResetGameSpeed()
    {
        Time.timeScale = gameSpeed;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void FixedUpdate()
    {
        ScoreLabel.text = CurrentScore.ToString();
        gameSpeed = Mathf.Clamp01(0.7f + (CurrentScore / 50));
        if (Time.timeScale != 1)
        {
            slowmoTimeSpan += Time.fixedUnscaledDeltaTime;
        }
        else
        {
            slowmoTimeSpan = 0;
        }


        if (Lifes <= 0)
        {
            if (slowmoTimeSpan >= 3)
            {
                ResetGameSpeed();
                /*
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02f;*/
            }
        }

        if (!isPlaying)
            return;
        SaberBehaviour(WebXR_Manager.leftHand);
        SaberBehaviour(WebXR_Manager.rightHand);

        stopTime -= Time.fixedUnscaledDeltaTime;

        //MusicHigh.volume = Mathf.Lerp(MusicHigh.volume, Mathf.Clamp(mFlow, 0, MaxMusicVolume), 3 * Time.deltaTime);
        //MusicLow.volume = Mathf.Lerp(MusicLow.volume, MaxMusicVolume - Mathf.Clamp(mFlow, 0, MaxMusicVolume), 3 * Time.deltaTime);
        MusicFlow.main.Flow = mFlow;

        Color c;
        ColorUtility.TryParseHtmlString("#636FDA", out c);
        ScoreLabel.color = Color.Lerp(ScoreLabel.color, c, 5 * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        foreach (var poses in positions)
        {
            Gizmos.DrawWireSphere(poses.transform.position, Range);
            Gizmos.DrawLine(poses.transform.position, poses.transform.position + (poses.transform.forward * 10));
        }
    }


    [Header("Extra")]
    public Material SliceMaterial;
    public Collider FruitKiller;
}
