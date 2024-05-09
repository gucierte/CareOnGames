using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// The master interactive module
/// </summary>
public class Interactive : MonoBehaviour, iPlayerInteraction
{
    public bool interactible = true;
    public float interactionRange = 10;
    public Button.ButtonClickedEvent OnTriggerEvent;
    [Header("Opitional")]
    public Animator transition_anim;

    public virtual void OnPlayerAim(XRController controller)
    {
        if (!interactible)
            return;
        if (Vector3.Distance(transform.position, controller.transform.position) > interactionRange)
            return;
        controller.SetRayColourAtThisFrame(new Color(0,1,1,1));

        if (transition_anim)
        {
            CancelInvoke(nameof(BackAnimToNormal));
            transition_anim.SetBool("Hover", true);
            Invoke(nameof(BackAnimToNormal), Time.deltaTime + Time.fixedDeltaTime);
        }

        if (controller.Controller.GetButton(WebXR.WebXRController.ButtonTypes.Trigger))
        {
            if (transition_anim)
            {
                transition_anim.SetTrigger("Click");
            }
            OnInteraction();
        }

        if (controller.Controller.GetButtonDown(WebXR.WebXRController.ButtonTypes.Trigger))
        {
            if (transition_anim)
            {
                transition_anim.SetTrigger("Click");
            }
            OnInteractionEnter();
            OnTriggerEvent.Invoke();
        }
    }

    public virtual void OnPlayerAim(MobileControll controller)
    {
        if (!interactible)
            return;
        if (Vector3.Distance(transform.position, controller.cam.transform.position) > interactionRange)
            return;
        if (transition_anim)
        {
            CancelInvoke(nameof(BackAnimToNormal));
            transition_anim.SetBool("Hover", true);
            Invoke(nameof(BackAnimToNormal), Time.deltaTime + Time.fixedDeltaTime);
        }
        controller.Load();
        if (controller.loaded >= 1)
        {
            OnInteraction();

            if (!MobileControll.interact)
            {
                if (transition_anim)
                {
                    transition_anim.SetTrigger("Click");
                }
                OnTriggerEvent.Invoke();
                OnInteractionEnter();
                MobileControll.interact = true;
            }
        }
    }

    void BackAnimToNormal()
    {
        transition_anim.SetBool("Hover", false);
    }

    public virtual void OnPlayerTriggerDown(XRController controller) { }

    public virtual void OnInteractionEnter()
    {

    }

    public virtual void OnInteraction()
    {
        if (transition_anim)
        {
            transition_anim.SetTrigger("Click");
        }
    }

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0,1,1,0.3f);
        Gizmos.DrawSphere(transform.position, -interactionRange);
    }
    public virtual void OnMouseOver()
    {
        if (Vector3.Distance(transform.position, CameraManager.CurrentCam.transform.position) > interactionRange)
            return;
        Debug.Log("Mouse over: " + this.gameObject.name);
        if (transition_anim)
        {
            transition_anim.SetTrigger("Hover");
        }
        if (Input.GetButton("Fire1"))
        {
            if (transition_anim)
            {
                transition_anim.SetTrigger("Click");
            }
            OnInteraction();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Clicked on: " + this.gameObject.name);
            OnInteractionEnter();
            OnTriggerEvent.Invoke();
        }
    }
}
