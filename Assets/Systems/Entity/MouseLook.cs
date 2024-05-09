using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [System.Serializable]
    public enum style
    {
        FPS, StreetView
    }
    [SerializeField]
    public style Style;

    public static Vector2 mouseInputs;
    public static Vector3 rotation;
    public float Sensitivity = 1;
    public float Easing = 3;



    public void Update()
    {
        switch (Style)
        {
            case style.FPS:
                mouseInputs = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
                rotation += new Vector3(mouseInputs.y, mouseInputs.x, 0) * Sensitivity;
                rotation = new Vector3(Mathf.Clamp(rotation.x, -90, 90), rotation.y, 0);
                transform.localRotation = Quaternion.Euler(rotation);
                break;
            case style.StreetView:
                if (Input.GetButton("Fire1"))
                {
                    mouseInputs = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
                    rotation -= new Vector3(mouseInputs.y, mouseInputs.x, 0) * Sensitivity;
                    rotation = new Vector3(Mathf.Clamp(rotation.x, -90, 90), rotation.y, 0);
                }
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotation), Easing * Time.deltaTime);
                break;
            default:
                break;
        }
    }
}
