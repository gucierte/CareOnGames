using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The master entity class, all dynamic object with status use this, like NPCs Players etc
/// </summary>
public class Entity : MonoBehaviour
{
    [Tooltip("The Rigidbody on this Entity")]public Rigidbody rb;
    [Tooltip("The animator on this Entity")]public List<Animator> anims = new List<Animator>();

    //Ground Detection
    [System.Serializable] public class groundDetection
    {
        public LayerMask walkableLayers;
        public float detectionDistance;
        /// <summary>
        /// returns if this ped is Grounded
        /// </summary>
        public RaycastHit gHit;
        /// <summary>
        /// Returns the ground hit info when this ped is grounded
        /// </summary>
        public bool isGrounded { get; set; }
    }
    /// <summary>
    /// Manage and put this ped on groundCorrecly
    /// </summary>
    public static void PutOnGroundCorrecly(groundDetection groundDetector, Entity target)
    {
        if (groundDetector.isGrounded)
        {
            if (Physics.Linecast(target.transform.position + Vector3.up, target.transform.position + (Vector3.down * groundDetector.detectionDistance), out groundDetector.gHit, groundDetector.walkableLayers))
            {
                //Set Position
                Vector3 pos = target.transform.position;
                pos.y = groundDetector.gHit.point.y;
                target.transform.position = Vector3.Lerp(target.transform.position, pos, 8 * Time.deltaTime);

                //Manage RigidBody
                target.rb.useGravity = false;
                target.rb.freezeRotation = true;
                //target.rb.velocity = Vector3.zero;

                groundDetector.isGrounded = true;
            }
            else
            {
                groundDetector.isGrounded = false;
                target.rb.useGravity = true;
            }
        } else
        {
            if (Physics.Linecast(target.transform.position + Vector3.up, target.transform.position, out groundDetector.gHit, groundDetector.walkableLayers))
            {
                //Set Position
                Vector3 pos = target.transform.position;
                pos.y = groundDetector.gHit.point.y;
                target.transform.position = Vector3.Lerp(target.transform.position, pos, 8 * Time.deltaTime);

                //Manage RigidBody
                target.rb.useGravity = false;
                target.rb.freezeRotation = true;
                //target.rb.velocity = Vector3.zero;

                groundDetector.isGrounded = true;
            }
            else
            {
                groundDetector.isGrounded = false;
                target.rb.useGravity = true;
            }
        }
    }

    //Freeze
    public int freezePriority { get; set; }
    public bool isFreezed { get; set; }

    /// <summary>
    /// Disable this ped actions (Freeze this) at single frame
    /// </summary>
    /// <param name="priority"></param>
    public void FreezeOnThisFrame(int priority)
    {
        CancelInvoke("UnFreeze");
        freezePriority = priority;
        rb.detectCollisions = false;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = priority > 0;
        isFreezed = true;
        Invoke("UnFreeze", Time.fixedDeltaTime * 1.1f);
    }
    /// <summary>
    /// Disable this ped actions (Freeze this) at single frame without priority
    /// </summary>
    /// <param name="priority"></param>
    public void FreezeOnThisFrame()
    {
        FreezeOnThisFrame(0);
    }
    /// <summary>
    /// Unfreeze this ped
    /// </summary>
    public void UnFreeze()
    {
        freezePriority = -1;
        rb.detectCollisions = true;
        rb.useGravity = true;
        rb.isKinematic = false;
        isFreezed = false;
    }
    public virtual void OnValidate()
    {
        if (rb == null) { rb = GetComponent<Rigidbody>(); }
        //if (anim == null) { anim = GetComponent<Animator>(); }
    }
}
