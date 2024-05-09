using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skin : MonoBehaviour
{
    static skin instance;
    public Canvas canvas;
    public CanvasScaler canvasScaler;

    public float DefaultScaleFactor = 1;
    public float MobileScaleFactor = 1;

    public static skin main()
    {
        if (!instance)
        {
            instance = FindFirstObjectByType<skin>();
        }

        return instance;
    }

    public Button AR_BUTTON;
    public Button VR_BUTTON;
    public bool Enable_AR_btn = true;
    public bool Enable_VR_btn = true;

    private void Awake()
    {
        AR_BUTTON.gameObject.SetActive(Enable_AR_btn);
        VR_BUTTON.gameObject.SetActive(Enable_VR_btn);
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //if (WebXR_Manager.currentPlatform == WebXR_Manager.plataform.Android || WebXR_Manager.currentPlatform == WebXR_Manager.plataform.IOS)
        //{
            //canvasScaler.scaleFactor = MobileScaleFactor;
        //}
    }

    private void OnValidate()
    {
        if (!canvas)
        {
            canvas = GetComponent<Canvas>();
        }

        if (canvas)
        {
            canvasScaler = canvas.GetComponent<CanvasScaler>();
        }
    }
}
