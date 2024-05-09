using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This code manage the object handle to grab on controll
/// </summary>
public class GrabObject : MonoBehaviour, iPlayerInteraction
{
    public float interactionRange = 10;
    [Tooltip("The rigidbody of object")]
    public Rigidbody rb;
    [Tooltip("Make the mass change handle")]
    public bool MassEffect = true;

    /// <summary>
    /// The controller handling this
    /// </summary>
    public XRController currentController { get; set; }
    /// <summary>
    /// The object speed when grabbing
    /// </summary>
    public Vector3 speed { get; set; }

    //Private
    /// <summary>
    /// The last position of object (used to get speed)
    /// </summary>
    Vector3 lastPos;
    /// <summary>
    /// The poisiton of this object relative to controller
    /// </summary>
    Vector3 controllerRelativeLocalPosition;

    //voids
    /// <summary>
    /// Grab this object on this frame
    /// </summary>
    public void GrabOnThisFrame()
    {
        CancelInvoke(nameof(Leave));
        if (MassEffect)
        {
            transform.position = Vector3.Lerp(transform.position, currentController.transform.TransformPoint(controllerRelativeLocalPosition), 
                (20 / rb.mass) * Time.deltaTime);
        }
        else
        {
            transform.parent = currentController.transform;
        }
        //Rigidbody manage
        if (rb)
        {
            rb.velocity= Vector3.zero;
            rb.useGravity = false;
        }
        Invoke(nameof(Leave), Time.deltaTime + Time.fixedDeltaTime);
    }
    /// <summary>
    /// Leave this object free
    /// </summary>
    void Leave()
    {
        transform.parent = null;
        currentController = null;
        if (rb)
        {
            rb.useGravity = true;
            rb.velocity = speed;
        }
    }


    //Callbacks
    public virtual void OnPlayerAim(XRController controller)
    {
        if (Vector3.Distance(transform.position, controller.transform.position) > interactionRange)
            return;
        if (MassEffect)
        {
            if (controller.Controller.GetButtonDown(WebXR.WebXRController.ButtonTypes.Grip))
            {
                controllerRelativeLocalPosition = controller.transform.InverseTransformPoint(transform.position);
            }
        }

        if (controller.Controller.GetButton(WebXR.WebXRController.ButtonTypes.Grip))
        {
            currentController = controller;
            GrabOnThisFrame();
        }
    }

    //Mono
    void FixedUpdate()
    {
        if (!currentController)
            return;
        if (lastPos != transform.position)
        {
            speed = ((transform.position - lastPos) / Time.fixedDeltaTime) * 0.65f;
            lastPos = transform.position;
        }
    }
    private void OnValidate()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
    }


    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.15f);
        Gizmos.DrawSphere(transform.position, -interactionRange);
    }

    public void OnPlayerTriggerDown(XRController controller){}

    public void OnPlayerAim(MobileControll controller){}
}
