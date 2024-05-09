using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightTrack : MonoBehaviour
{
    public float Range = 0.5f;
    Vector3 wantedPos;

    private void Start()
    {
        wantedPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(CameraManager.CurrentCam.transform.position.y - transform.position.y) > Range)
        {
            wantedPos = new Vector3(wantedPos.x, CameraManager.CurrentCam.transform.position.y, wantedPos.z);
        }
        transform.position = Vector3.Lerp(transform.position, wantedPos, 3 * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}