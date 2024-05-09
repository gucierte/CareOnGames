using EzySlice;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FruitNinjaFruit : Interactive
{
    public static List<FruitNinjaFruit> fruits = new List<FruitNinjaFruit>();
    public static List<GameObject> allParts = new List<GameObject>();

    public Material CrossSectionMat;
    public SphereCollider col;
    public Rigidbody rb;
    public float DefaultSpeed;
    public float DefaultSpeedDown;
    public float Health = 100;
    public int Score = 1;
    public bool isBomb;
    [Space]
    public GameObject SliceParticle;
    public Transform SliceReference;
    public GameObject ExplodeParticle;
    [System.Serializable]
    public enum special
    {
        None, SlowMo, StopTime, CutAll
    }
    [SerializeField]
    public special Special;


    public override void OnInteraction()
    {
        base.OnInteraction();
    }

    public GameObject[] Slice(Vector3 planeWorldPosition, Vector3 controllerSpeed)
    {
        GameObject particleInstance = Instantiate(SliceParticle);

        particleInstance.transform.position = planeWorldPosition;
        particleInstance.transform.up = controllerSpeed;
        particleInstance.gameObject.SetActive(true);
        particleInstance.transform.parent = null;

        switch (Special)
        {
            case special.None:
                if (Time.timeScale == 0.0001f)
                {
                    Time.timeScale = 1;
                    FruitNinja.Main().StopTime(15);
                }
                break;
            case special.SlowMo:
                Time.timeScale = 0.1f;
                break;
            case special.StopTime:
                FruitNinja.Main().StopTime(15);
                break;
            case special.CutAll:
                break;
            default:
                break;
        }

        GameObject[] go = null;

        if (SliceReference)
        {
            go = gameObject.SliceInstantiate(planeWorldPosition, SliceReference.forward, CrossSectionMat);
        } else
        {
            go = gameObject.SliceInstantiate(planeWorldPosition, new Vector3(Random.Range(-360,360), Random.Range(-360, 360), Random.Range(-360, 360)), CrossSectionMat);
        }

        foreach (var item in go)
        {
            item.layer = gameObject.layer;
        }

        return go;
    }

    public void Explode(Vector3 planeWorldPosition, Vector3 controllerSpeed)
    {
        GameObject particleInstance = Instantiate(ExplodeParticle);
        particleInstance.transform.position = planeWorldPosition;
        particleInstance.transform.up = controllerSpeed;
        particleInstance.gameObject.SetActive(true);
        particleInstance.transform.parent = null;

        Destroy(this.gameObject);
    }

    private void Start()
    {
        fruits.Add(this);
    }

    private void OnDestroy()
    {
        fruits.Remove(this);
    }

    private void OnValidate()
    {
        if (!col)
        {
            col = GetComponent<SphereCollider>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == FruitNinja.Main().FruitKiller.gameObject)
        {
            if (!isBomb)
            {
                FruitNinja.Main().MissFruit(this);
            } else
            {
                Destroy(this.gameObject);
            }
        }
    }
}