using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Some interactive example
/// </summary>
public class InteractionExample : Interactive
{
    public override void OnPlayerAim(XRController controller)
    {
        base.OnPlayerAim(controller);
        if (controller.Controller.GetButtonDown(WebXR.WebXRController.ButtonTypes.Trigger))
        {
            GetComponent<Renderer>().material.color = Random.ColorHSV();
        }
    }

    public override void OnPlayerAim(MobileControll controller)
    {
        if (!interactible)
            return;
        if (Vector3.Distance(transform.position, controller.cam.transform.position) > interactionRange)
            return;
        controller.Load();
        if (controller.loaded >= 1)
        {
            if (!MobileControll.interact)
            {
                GetComponent<Renderer>().material.color = Random.ColorHSV();
                MobileControll.interact = true;
            }
        }
    }

    public override void OnMouseOver()
    {
        base.OnMouseOver();
        if (Vector3.Distance(transform.position, CameraManager.CurrentCam.transform.position) > interactionRange)
            return;
        if (Input.GetButtonDown("Fire1"))
        {
            GetComponent<Renderer>().material.color = Random.ColorHSV();
        }
    }
}
