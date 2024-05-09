using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;

public class pedXRController : MonoBehaviour
{
    public static pedXRController main;

    [Tooltip("The ped to recive controlls")]
    public Ped ped;

    [Space]
    [Tooltip("The left hand controller")] public XRController leftHand;
    [Tooltip("The right hand controller")] public XRController rightHand;
    /// <summary>
    /// The direction of inputs
    /// </summary>
    public Vector2 inputDir { get; set; }
    /// <summary>
    /// The direction of movement
    /// </summary>
    public Vector3 moveDir { get; set; }
    /// <summary>
    /// The angle to thurn when press thumbstick
    /// </summary>
    public float thurnAngle { get; set; }
    [Header("Options")]
    public bool canMove = true;
    public bool canThurn = true;
    public bool canJump = true;

    public void OnControllerXR()
    {
        inputDir = (leftHand.Controller.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick) + rightHand.Controller.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick));
        moveDir = CameraManager.CurrentCam.transform.TransformDirection(new Vector3(inputDir.x, 0, inputDir.y));

        if (canMove)
        {
            ped.Move(moveDir, moveDir.magnitude);
        }

        if (canThurn)
        {
            if (leftHand.Controller.GetButtonDown(WebXRController.ButtonTypes.Thumbstick))
            {
                float v = leftHand.Controller.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick).x;
                if (Mathf.Abs(v) >= 0.4)
                {
                    if (v > 0)
                    {
                        thurnAngle += 90;
                    }
                    else
                    {
                        thurnAngle -= 90;
                    }
                }
                else
                {
                    thurnAngle -= 90;
                }
            }
            if (rightHand.Controller.GetButtonDown(WebXRController.ButtonTypes.Thumbstick))
            {
                float v = rightHand.Controller.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick).x;
                if (Mathf.Abs(v) >= 0.4)
                {
                    if (v > 0)
                    {
                        thurnAngle += 90;
                    }
                    else
                    {
                        thurnAngle -= 90;
                    }
                }
                else
                {
                    thurnAngle += 90;
                }
            }

        }

        if (canJump)
        {
            if (leftHand.Controller.GetButtonDown(WebXRController.ButtonTypes.ButtonA) ||
                rightHand.Controller.GetButtonDown(WebXRController.ButtonTypes.ButtonA))
            {
                if (ped.isGrounded)
                {
                    ped.Jump();
                }
            }
        }


        thurnAngle = Mathf.LerpAngle(thurnAngle, thurnAngle, 1);
    }
    public void OnControllerPC()
    {
        inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * (Input.GetAxis("Fire3") + 1);
        moveDir = CameraManager.CurrentCam.transform.TransformDirection(new Vector3(inputDir.x, 0, inputDir.y));

        if (canMove)
        {
            ped.Move(moveDir, moveDir.magnitude);
        }


        if (canJump)
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (ped.isGrounded)
                {
                    ped.Jump();
                }
            }
        }


        thurnAngle = Mathf.LerpAngle(thurnAngle, thurnAngle, 1);
    }

    //Mono
    private void OnEnable()
    {
        thurnAngle = transform.eulerAngles.y;
        main = this;
    }
    private void FixedUpdate()
    {
        Quaternion r = Quaternion.Euler(0f, thurnAngle, 0f);
        transform.rotation = Quaternion.Lerp(CameraManager.Main().XRCamera.transform.rotation, r, 8 * Time.deltaTime);
    }
    private void Update()
    {
        if (WebXR_Manager.manager.XRState == WebXRState.NORMAL)
        {
            OnControllerPC();
        } else
        {
            OnControllerXR();
        }

    }
    private void OnValidate()
    {
        if (!ped)
        {
            ped = GetComponent<Ped>();
        }
    }
}
