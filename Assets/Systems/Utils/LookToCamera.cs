using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToCamera : MonoBehaviour
{
    public float Lerp = -1;
    public bool invert;

    private void FixedUpdate()
    {
        Quaternion quat = Quaternion.LookRotation(CameraManager.CurrentCam.transform.position - transform.position);

        if (invert)
        {
            quat = Quaternion.LookRotation(transform.position - CameraManager.CurrentCam.transform.position);
        }

        if (Lerp > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, Lerp * Time.deltaTime);
        } else
        {
            transform.rotation = quat;
        }

    }
}
