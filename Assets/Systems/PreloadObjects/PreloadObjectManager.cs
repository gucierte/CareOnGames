using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PreloadObjectManager : MonoBehaviour
{
    [HideInInspector]
    public List<PreloadObject> objects = new List<PreloadObject>();
    public List<GameObject> PrefabsToPreload = new List<GameObject>();
    [HideInInspector] public List<GameObject> instances = new List<GameObject>();
    public int StepTime = 100;

    public void PreLoadObject(PreloadObject obj)
    {
        switch (obj.PeloadType)
        {
            case PreloadObject.peloadType.InstantiateNew:
                //obj.gameObject.SetActive(true);
                GameObject go = Instantiate(obj.gameObject);
                go.gameObject.SetActive(true);
                go.transform.parent = this.transform;

                instances.Add(go);

                go.gameObject.SetActive(false);
                obj.gameObject.SetActive(obj.initialActivation);
                break;
            case PreloadObject.peloadType.JustEnable:
                obj.gameObject.SetActive(true);
                obj.gameObject.SetActive(obj.initialActivation);
                break;
            default:
                break;
        }
    }

    public void PreLoadObject(GameObject obj)
    {
        GameObject go = Instantiate(obj.gameObject);
        go.gameObject.SetActive(true);
        go.transform.parent = this.transform;
        instances.Add(go);
        go.gameObject.SetActive(false);
    }

    private void Awake()
    {
        objects = GameObject.FindObjectsByType<PreloadObject>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID).ToList();
        foreach (var o in objects)
        {
            o.GetInitialActivation();
        }
    }

    private void Start()
    {
        foreach (var o in objects)
        {
            PreLoadObject(o);
        }

        foreach (var o in PrefabsToPreload)
        {
            PreLoadObject(o);
        }
    }
}
