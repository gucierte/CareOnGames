using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;

/// <summary>
/// The visuals of controller
/// </summary>
public class ControllerVisuals : MonoBehaviour
{
    public XRController Controller;

    public Renderer Body;
    public SkinnedMeshRenderer Thumbstick;
    [Space]
    public SkinnedMeshRenderer A_Button;
    public SkinnedMeshRenderer B_Button;
    public SkinnedMeshRenderer Oculus;
    [Space]
    public SkinnedMeshRenderer Grip;
    public SkinnedMeshRenderer Trigger;


    public void SetVisualShape(SkinnedMeshRenderer btn, float value, Vector2 axis)
    {
        if (btn != null)
        {
            btn.SetBlendShapeWeight(0, value * 100);
            btn.SetBlendShapeWeight(1, Mathf.Clamp01(axis.y) * 100);
            btn.SetBlendShapeWeight(2, Mathf.Clamp01(-axis.y) * 100);
            btn.SetBlendShapeWeight(3, Mathf.Clamp01(axis.x) * 100);
            btn.SetBlendShapeWeight(4, Mathf.Clamp01(-axis.x) * 100);
        }
    }
    public void SetVisualShape(SkinnedMeshRenderer btn, float value)
    {
        if (btn != null)
        {
            btn.SetBlendShapeWeight(0, value * 100);
        }
    }
    public void SetVisualShape(SkinnedMeshRenderer btn, bool value)
    {
        SetVisualShape(btn, (value ? 1 : 0));
    }
    public void SetVisualShape(SkinnedMeshRenderer btn, bool value, Vector2 axis)
    {
        SetVisualShape(btn, value ? 1 : 0, axis);
    }

    private void Update()
    {
        SetVisualShape(A_Button, Controller.Controller.GetButton(WebXRController.ButtonTypes.ButtonA));
        SetVisualShape(B_Button, Controller.Controller.GetButton(WebXRController.ButtonTypes.ButtonB));
        SetVisualShape(Trigger, Controller.Controller.GetAxis(WebXRController.AxisTypes.Trigger));
        SetVisualShape(Grip, Controller.Controller.GetAxis(WebXRController.AxisTypes.Grip));
        SetVisualShape(Thumbstick, Controller.Controller.GetButton(WebXRController.ButtonTypes.Thumbstick), Controller.Controller.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick));
    }
}
