using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitNinjaCanon : MonoBehaviour
{
    public Transform Aiming;

    private void OnDrawGizmos()
    {
        transform.forward = Aiming.position - transform.position;
    }
}
