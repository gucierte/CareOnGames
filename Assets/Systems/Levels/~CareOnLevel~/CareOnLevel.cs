using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CareOnLevel : MonoBehaviour
{
    static CareOnLevel instance;
    public static CareOnLevel main
    {
        get
        {
            if (!instance)
            {
                instance = FindFirstObjectByType<CareOnLevel>();
            }
            return instance;
        } }

    public Animator anim;

    public Material EmissionMaterial;
    [HideInInspector] public Color defaultEmissionColor;
    static Color currentEmissionColor;
    [Space]
    public AudioClip LowMusic;
    public AudioClip HighMusic;

    public static void ResetLevelMusic()
    {
        MusicFlow.main.SetMusic(main.HighMusic, main.LowMusic, 0.3f);
        MusicFlow.main.pitch = 1;
    }

    public static void SetEmissionColorOnThisFrame(Color newColor)
    {
        if (Application.isPlaying)
        {
            currentEmissionColor = newColor;
        }
    }

    private void Start()
    {
        Invoke(nameof(ResetLevelMusic), 1);
    }

    public void FixedUpdate()
    {
        currentEmissionColor = Color.Lerp(currentEmissionColor, defaultEmissionColor, 3 * Time.deltaTime);
        EmissionMaterial.SetColor("_EmissionColor", currentEmissionColor);

    }
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            defaultEmissionColor = EmissionMaterial.GetColor("_EmissionColor");
        }
    }
}
