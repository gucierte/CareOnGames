using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using WebXR;

public class WebXR_Manager : MonoBehaviour
{
    [System.Serializable]
    public enum plataform
    {
        Null, Android, IOS, PC, Oculus, Linux
    }
    public static plataform currentPlatform;
    [SerializeField] public plataform SimulateDevice;
    public ScreenOrientation Orientation;

    public static WebXRManager manager;
    public static bool VR_Supported;
    public static bool AR_Supported;

    public static XRController leftHand;
    public static XRController rightHand;

    public static bool OnVR { get { return manager.XRState == WebXRState.VR; } }
    public static bool OnAR { get { return manager.XRState == WebXRState.AR; } }

#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void CheckXRSupport();

    [DllImport("__Internal")]
    private static extern void GetPlataformInfo();
#endif

    private void Start()
    {
        Debug.Log("Initializing");
        manager = FindFirstObjectByType<WebXRManager>();

#if UNITY_EDITOR
        currentPlatform = SimulateDevice;
#else
    #if UNITY_WEBGL
        CheckXRSupport();
        GetPlataformInfo();
    #endif
#endif

        if (currentPlatform == plataform.Android)
        {
            Screen.orientation = Orientation;
        }

        DontDestroyOnLoad(this.gameObject);
        InvokeRepeating(nameof(EnableXRButtons), 1, 3);
    }

    public void EnableXRButtons()
    {
        Debug.Log("VR Support: " + manager.isSupportedVR + "\n" + "AR Support: " + manager.isSupportedAR);

        skin.main().AR_BUTTON.gameObject.SetActive(manager.isSupportedAR && skin.main().Enable_AR_btn);
        skin.main().VR_BUTTON.gameObject.SetActive(manager.isSupportedVR && skin.main().Enable_VR_btn);
    }

    public void ToggleToVR()
    {
        manager.ToggleVR();
    }
    public void ToggleToAR()
    {
        manager.ToggleAR();
    }

    //Callbacks
    void ARSupport()
    {
        AR_Supported = true;
        //skin.main().AR_BUTTON.gameObject.SetActive(AR_Supported);
    }
    void VRSupport()
    {
        VR_Supported = true;
        //skin.main().VR_BUTTON.gameObject.SetActive(VR_Supported);
    }
    public void OnPlataformInfo(string plataform)
    {
        string pl = plataform.ToLower();
        if (pl.Contains("oculus"))
        {
            currentPlatform = WebXR_Manager.plataform.Oculus;
        }
        else
        {
            if (pl.Contains("android"))
            {
                currentPlatform = WebXR_Manager.plataform.Android;
            }
            else
            {
                if (pl.Contains("ios"))
                {
                    currentPlatform = WebXR_Manager.plataform.IOS;
                }
                else
                {
                    if (pl.Contains("windows"))
                    {
                        currentPlatform = WebXR_Manager.plataform.PC;
                    }
                    else
                    {
                        if (pl.Contains("linux"))
                        {
                            currentPlatform = WebXR_Manager.plataform.Linux;
                        }
                    }
                }
            }
        }

        Debug.Log("Plataform: " + currentPlatform);
        Debug.Log("Plataform Info: " + plataform);
    }
}
