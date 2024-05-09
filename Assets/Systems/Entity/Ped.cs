using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// The Pedestrian entity. Pedestrians is all NPCs or Players
/// </summary>
public class Ped : Entity
{

    //Multiplayer
    [System.Serializable]public class animations
    {
        [Header("Animations")]
        public float anim_CurrentMoveSpeed;
        public float anim_fallSpeed;
        public float anim_MoveX;
        public float anim_MoveZ;
        public bool anim_isGrounded;
        public float anim_talk;
    }
    [SerializeField]
    public animations Animations;


    //Ground Detection
    [SerializeField] [Tooltip("Manage the ground Detection")] public groundDetection GroundDetection;
    /// <summary>
    /// returns if this ped is Grounded
    /// </summary>
    public bool isGrounded { get { return GroundDetection.isGrounded; } }

    public List<Renderer> renderers = new List<Renderer>();

    /// <summary>
    /// Returns the ground hit info when this ped is grounded
    /// </summary>
    public RaycastHit groundHit { get { return GroundDetection.gHit; } }

    [Min(0)] [Tooltip("The speed multipiler of this ped movement")]public float MoveSpeed = 1;
    [Min(0)] [Tooltip("The speed of thurn to look at")]public float ThurnSpeed = 5;

    //Movement
    /// <summary>
    /// The current movement speed state
    /// </summary>
    public float _currentMoveSpeed { get; set; }
    /// <summary>
    /// The world direction to move
    /// </summary>
    public Vector3 _moveDirection { get; set; }
    /// <summary>
    /// The world direction to look
    /// </summary>
    public Vector3 _lookDirection { get; set; }
    /// <summary>
    /// The current animation move speed state
    /// </summary>
    public float animationCurrentMoveSpeed { get; set; }
    /// <summary>
    /// Returns the move speed percente (_currentMoveSpeed / MoveSpeed)
    /// </summary>
    /// <returns></returns>
    public float GetMoveSpeedPercent()
    {
        return _currentMoveSpeed / MoveSpeed;
    }
    /// <summary>
    /// Move this ped to some direction (direction is world coord, but point is local)
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="speedMultipiler"></param>
    public void Move(Vector3 direction, float speedMultipiler = 1)
    {
        _moveDirection = Vector3.ClampMagnitude(direction, 1);
        _currentMoveSpeed = MoveSpeed * speedMultipiler;
    }
    public void LookTo(Vector3 worldCoord)
    {
        _lookDirection = worldCoord;
    }
    Vector3 inverseDirection;
    public void OnAnimations()
    {
        animationCurrentMoveSpeed = Mathf.Lerp(animationCurrentMoveSpeed, _moveDirection.magnitude * _currentMoveSpeed, 8 * Time.deltaTime);
        inverseDirection = Vector3.Lerp(inverseDirection, transform.InverseTransformDirection(_moveDirection) * _currentMoveSpeed, 8 * Time.deltaTime);

        //Floats
        Animations.anim_CurrentMoveSpeed = animationCurrentMoveSpeed;
        Animations.anim_fallSpeed = rb.velocity.y;

        Animations.anim_MoveX = inverseDirection.z;
        Animations.anim_MoveZ = inverseDirection.x;

        //Bools
        Animations.anim_isGrounded = isGrounded;

        //Floats
        int emptyAnim = -1;
        if (anims.Count <= 0)
            return;
        foreach (var anim in anims)
        {
            if (anim != null)
            {
                anim.SetFloat("currentMoveSpeed", Animations.anim_CurrentMoveSpeed);
                anim.SetFloat("fallSpeed", Animations.anim_fallSpeed);

                anim.SetFloat("MoveX", Animations.anim_MoveX);
                anim.SetFloat("MoveZ", Animations.anim_MoveZ);

                anim.SetFloat("Talking", Animations.anim_talk);

                //Bools
                anim.SetBool("isGrounded", Animations.anim_isGrounded);
            } else
            {
                emptyAnim = anims.IndexOf(anim);
            }
        }

        if (emptyAnim > 0)
        {
            anims.RemoveAt(emptyAnim);
        }
    }

    /// <summary>
    /// Hide ped renderers on this frame
    /// </summary>
    /// <param name="shadowCaster"></param>
    public void HidePedOnThisFrame(bool shadowCaster)
    {
        CancelInvoke("ShowPed");
        foreach (var item in renderers)
        {
            item.enabled = false;
        }
        Invoke("ShowPed", .5f);
    }
    void ShowPed()
    {
        foreach (var item in renderers)
        {
            item.enabled = true;
        }
    }

    [Tooltip("The jump force of this ped")]
    public float JumpForce = 100;
    /// <summary>
    /// Make this ped Jump Immediatly
    /// </summary>
    public void Jump()
    {
        GroundDetection.isGrounded = false;
        //transform.Translate(Vector3.up * GroundDetection.detectionDistance * 1.1f);
        transform.Translate(Vector3.up * 0.1f);
        rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }


    //Mono Behaviour voids
    private void Awake()
    {
        LookTo(transform.forward);
    }
    public void Start()
    {
        InvokeRepeating("AntiVoid", 3, 3);
        LookTo(transform.forward);
    }
    public void FixedUpdate()
    {
        if (isFreezed)
            return;
        OnAnimations(); PutOnGroundCorrecly(GroundDetection, this);

        //Make this ped look at
        if (Vector3.Distance(transform.position, _lookDirection) > .4f)
        {
            Vector3 lookRotation = new Vector3(0, Quaternion.LookRotation(_lookDirection-transform.position).eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(lookRotation), ThurnSpeed * Time.deltaTime);
        }

        //Move this ped
        if (isGrounded)
        {
            rb.velocity = new Vector3(_moveDirection.x * _currentMoveSpeed, 0, _moveDirection.z * _currentMoveSpeed);
        }
        else
        {
            Vector3 vel = new Vector3(_moveDirection.x * (_currentMoveSpeed * 2), 0, _moveDirection.z * (_currentMoveSpeed * 2)) * 2;
            rb.velocity = Vector3.Lerp(rb.velocity, vel + rb.velocity, 1 * Time.deltaTime);
        }
    }

    Vector3 lastGroundedPos { get; set; }
    /// <summary>
    /// Reset ped when reach -1000 coord
    /// </summary>
    public void AntiVoid()
    {
        if (isGrounded)
        {
            lastGroundedPos = transform.position;
        } else
        {
            if (transform.position.y <= -1000)
            {
                transform.position = lastGroundedPos;
            }
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Draw Movement Gizmos
        UnityEditor.Handles.color = Color.cyan;
        UnityEditor.Handles.DrawLine(transform.position, transform.position + _moveDirection);

        //Draw Physics Gizmos
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, .5f);
        UnityEditor.Handles.DrawLine(transform.position, transform.position + (Vector3.down * GroundDetection.detectionDistance));
        UnityEditor.Handles.DrawWireDisc(transform.position + (Vector3.down * GroundDetection.detectionDistance), transform.up, .3f);
    }
#endif
}
