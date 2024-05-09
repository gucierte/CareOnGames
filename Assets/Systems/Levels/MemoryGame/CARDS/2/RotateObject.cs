using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Transform target;
    public Vector2 StartPos { get; set; }
    public Vector2 CurrentPos { get; set; }
    public Vector2 Direction { get { return CurrentPos - StartPos; } }
    Vector3 StartEuler { get; set; }


    public void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartEuler = target.transform.eulerAngles;
                    StartPos = touch.position;
                    CurrentPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    CurrentPos = touch.position;
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    break;
            }
        }

        target.eulerAngles = new Vector3(StartEuler.x, StartEuler.y + Direction.x, StartEuler.z);
    }
}
