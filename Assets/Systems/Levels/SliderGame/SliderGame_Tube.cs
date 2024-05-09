using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderGame_Tube : MonoBehaviour
{
    public Animator anim;
    [Range(0,1)] public float RawValue;
    [Range(0, 1)] public float DeltaValue;
    public float LastValue { get; set; }
    public float value { get; set; }
    public bool isFilling;

    public void SetValue(float v)
    {
        RawValue = v;
        RawValue = Mathf.Clamp01(v);
    }

    private void LateUpdate()
    {
        value = Mathf.Lerp(value, RawValue, 5 * Time.deltaTime);

        anim.SetFloat("Value", value);
    }

}
