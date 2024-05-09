using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitNinjaSaber : MonoBehaviour
{
    public XRController controller;

    public Transform oldRay;
    public Transform newRay;

    public AudioSource Idle;
    public AudioSource Move;
    public AudioSource Cut;

    public float oldDistance = -1;
    public float newDistance = -1;


    public void SetNewRayStart()
    {
        controller.ray.start = newRay;
        controller.ray.distance = newDistance;
    }
    public void ResetRayStart()
    {
        controller.ray.start = oldRay;
        controller.ray.distance = oldDistance;
    }
    private void OnEnable()
    {
        SetNewRayStart();
    }
    private void OnDestroy()
    {
        ResetRayStart();
    }
    private void OnDisable()
    {
        ResetRayStart();
    }
    private void OnValidate()
    {
        if (!oldRay)
        {
            oldRay = controller.ray.start;
        }

        if (oldDistance < 0)
        {
            oldDistance = controller.ray.distance;
        }
    }
    private void FixedUpdate()
    {
        Move.volume = Mathf.Lerp(Move.volume, Mathf.Abs((controller.Velocity.magnitude) * 20), 5 * Time.deltaTime);

        Cut.gameObject.SetActive(controller.ray.hit.collider);
        Cut.transform.position = controller.ray.hit.point;
        //Move.volume = Mathf.Lerp(Move.pitch, .5f + Mathf.Abs((controller.Velocity.magnitude) * 20), 5 * Time.deltaTime);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(newRay.position, newRay.position + newRay.forward * newDistance);
    }
}
