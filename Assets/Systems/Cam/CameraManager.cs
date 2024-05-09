using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;
using static WebXR.WebXRSubsystem;

/// <summary>
/// The custom camera manger of XR interaction
/// </summary>
public class CameraManager : MonoBehaviour
{
    /// <summary>
    /// The main instance, use "Main()" to get this
    /// </summary>
    static CameraManager instance { get; set; }
    /// <summary>
    /// Returns the main instance
    /// </summary>
    /// <returns></returns>
    public static CameraManager Main()
    {
        if (!instance)
        {
            instance = FindFirstObjectByType<CameraManager>();
        }

        return instance;
    }

    /// <summary>
    /// The WebXR camera handle
    /// </summary>
    public WebXRCamera XRCamera;
    /// <summary>
    /// Get's the current camera
    /// </summary>
    public static Camera CurrentCam;
    /// <summary>
    /// Current WebXR state
    /// </summary>
    public static WebXRState State;
    public Camera MainCamera;


    //Behaviour
    private void Awake()
    {
        MainCamera.enabled = false;

        WebXRManager.OnXRChange += OnXRChange;
        WebXRManager.OnHeadsetUpdate += OnHeadsetUpdate;
        OnXRChange(WebXRManager.Instance.XRState,
                    WebXRManager.Instance.ViewsCount,
                    WebXRManager.Instance.ViewsLeftRect,
                    WebXRManager.Instance.ViewsRightRect);

        MainCamera.enabled = true;
        CurrentCam = MainCamera;
    }
    private void OnHeadsetUpdate(Matrix4x4 leftProjectionMatrix, Matrix4x4 rightProjectionMatrix, Quaternion leftRotation, Quaternion rightRotation, Vector3 leftPosition, Vector3 rightPosition){}
    private void OnXRChange(WebXRState state, int viewsCount, Rect leftRect, Rect rightRect)
    {
        State = state;
        switch (state)
        {
            case WebXRState.VR:
                CurrentCam = XRCamera.GetCamera(WebXRCamera.CameraID.LeftVR);
                break;
            case WebXRState.AR:
                CurrentCam = XRCamera.GetCamera(WebXRCamera.CameraID.LeftAR);
                break;
            case WebXRState.NORMAL:
                CurrentCam = XRCamera.GetCamera(WebXRCamera.CameraID.Main);
                break;
            default:
                break;
        }
    }

    private void OnValidate()
    {
        if (!XRCamera)
        {
            XRCamera = FindFirstObjectByType<WebXRCamera>();
        }
    }
}
