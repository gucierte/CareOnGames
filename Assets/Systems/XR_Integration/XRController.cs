using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;

/// <summary>
/// The basic controller tooltip
/// </summary>
public class XRController : MonoBehaviour
{
    //Basic vars
    [Tooltip("The controller to get the infos (here you get info if some button is pressed)")]
    public WebXRController Controller;

    //Ray vars
    /// <summary>
    /// (Internal use only) returns: Ray.use
    /// </summary>
    /// <returns></returns>
    bool isUsingRay()
    {
        return ray.use;
    }
    [System.Serializable]
    public class interactionRay
    {
        [Tooltip("Use Ray to interact (most indicated when use controller to interact instead hands)")]
        public bool use = true;
        [Space]
        [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(isUsingRay))]
        [Tooltip("The ray start point and direction")]
        public Transform start;
        [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(isUsingRay))]
        [Tooltip("The max distance of ray")]
        public float distance;
        [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(isUsingRay))]
        [Tooltip("The layers to interact with ray")]
        public LayerMask Layers;
        [Space]
        [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(isUsingRay))]
        [Tooltip("The default ray colour (using when 'ResetRayColour' is called)")]
        public Color defaultColor;
        [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(isUsingRay))]
        [Tooltip("The line renderer of ray")]
        public LineRenderer renderer;
        public Color currentColour { get; set; }
        public RaycastHit hit;
    }
    [SerializeField]
    [Tooltip("Get the interaction ray")] public interactionRay ray;

    Vector3 lastPosition;

    public Vector3 Velocity;

    //Ray Voids
    /// <summary>
    /// Set Ray colour on single frame and reset after
    /// </summary>
    /// <param name="newColor">the colour to apply on ray</param>
    public void SetRayColourAtThisFrame(Color newColor)
    {
        CancelInvoke(nameof(ResetRayColour));
        ray.currentColour = newColor;
        Invoke(nameof(ResetRayColour), (Time.deltaTime + Time.fixedDeltaTime) * 2);
    }
    /// <summary>
    /// Reset the ray colour at frame end
    /// </summary>
    public void ResetRayColour()
    {
        ray.currentColour = ray.defaultColor;
    }
    /// <summary>
    /// The ray behaviour when ray is used
    /// </summary>
    public void RayBehaviour()
    {
        //Set Ray colour
        Color c1 = ray.currentColour;
        Color c2 = ray.currentColour;
        c1.a = 0;

        //Set ray positons
        if (ray.start)
        {
            ray.renderer.SetPosition(0, ray.start.position);
            ray.renderer.SetPosition(1, ray.start.position + ray.start.forward * ray.distance);

            if (Physics.Linecast(ray.start.position, ray.start.position + ray.start.forward * ray.distance, out ray.hit, ray.Layers))
            {
                ray.renderer.SetPosition(1, ray.hit.point);
                ray.renderer.startColor = c1;
                ray.renderer.endColor = c2;
                ray.hit.collider.SendMessage("OnPlayerAim", this, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                ray.renderer.startColor = c1;
                ray.renderer.endColor = c2;
            }
        }
        else
        {
            Debug.LogError("[XR Controller] Ray.Start is not defined!");
        }
    }

    //Mono
    private void Start()
    {
        ray.renderer.positionCount = 2;
        switch (Controller.hand)
        {
            case WebXRControllerHand.NONE:
                break;
            case WebXRControllerHand.LEFT:
                WebXR_Manager.leftHand = this;
                break;
            case WebXRControllerHand.RIGHT:
                WebXR_Manager.rightHand = this;
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (ray.use) { RayBehaviour(); }

        if (Controller.GetButtonDown(WebXRController.ButtonTypes.Trigger)) { MessageSender.Send("OnPlayerTriggerDown", this); }
        if (Controller.GetButton(WebXRController.ButtonTypes.Trigger)) { MessageSender.Send("OnPlayerTrigger", this); }
    }
    private void FixedUpdate()
    {
        Vector3 rayEnd = ray.start.position + ray.start.forward * ray.distance;
        if (lastPosition != rayEnd)
        {
            Velocity = (lastPosition - rayEnd) * Time.fixedDeltaTime;
            lastPosition = rayEnd;
        }
    }
    private void OnDrawGizmos()
    {
        if (ray.start)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(ray.start.position, ray.start.position + ray.start.forward * ray.distance);
        }
    }
}

public interface iPlayerInteraction
{
    /// <summary>
    /// Called when player controller aim to this object
    /// </summary>
    /// <param name="controller"></param>
    public void OnPlayerAim(XRController controller);

    public void OnPlayerAim(MobileControll controller);

    public void OnPlayerTriggerDown(XRController controller);
}
