using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileControll : MonoBehaviour
{
    static MobileControll instance;
    public static MobileControll main()
    {
        if (!instance)
        {
            instance = FindFirstObjectByType<MobileControll>();
        }
        return instance;
    }

    public Camera cam;
    public float loaded { get; set; }
    public float loadSpeed = 1;
    public LayerMask RayLayers;
    public RaycastHit hit;
    public static bool interact;
    public static bool use;

    private void FixedUpdate()
    {
        if (WebXR_Manager.currentPlatform == WebXR_Manager.plataform.Android || WebXR_Manager.currentPlatform == WebXR_Manager.plataform.IOS)
        {
            if (WebXR_Manager.manager.XRState == WebXR.WebXRState.VR || WebXR_Manager.manager.XRState == WebXR.WebXRState.AR)
            {
                use = true;
            } else
            {
                use = false;
            }
        } else
        {
            use = false;
        }

        if (!use)
            return;
        cam = CameraManager.CurrentCam;
        if (Physics.Linecast(cam.transform.position, cam.transform.position + (cam.transform.forward * 100), out hit, RayLayers))
        {
            hit.collider.SendMessage("OnPlayerAim", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void Load()
    {
        if (WebXR_Manager.currentPlatform != WebXR_Manager.plataform.Android && WebXR_Manager.currentPlatform != WebXR_Manager.plataform.IOS)
            return;
        CancelInvoke(nameof(ResetLoad));
        loaded += loadSpeed * Time.fixedDeltaTime;
        Invoke(nameof(ResetLoad), Time.deltaTime + Time.fixedDeltaTime);
    }
    void ResetLoad()
    {
        loaded = 0;
        interact = false;
    }
}
