using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloadObject : MonoBehaviour
{
    public bool initialActivation { get; set; }
    [System.Serializable]
    public enum peloadType
    {
        InstantiateNew, JustEnable
    }
    [SerializeField]
    public peloadType PeloadType;

    public void GetInitialActivation()
    {
        initialActivation = gameObject.activeSelf;
    }

    private void Awake()
    {

    }
}
