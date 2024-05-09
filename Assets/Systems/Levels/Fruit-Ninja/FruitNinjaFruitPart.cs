using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitNinjaFruitPart : Interactive
{
    public float Health = 100;
    public override void OnInteraction()
    {
        base.OnInteraction();
        Health = 0;
    }
}
