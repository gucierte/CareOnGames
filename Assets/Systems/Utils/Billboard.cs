using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera c;
    public bool KeepScale;
    [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(KeepScale))]
    public float ScaleFactor = 1;

    public void Perform()
    {
        transform.rotation = c.transform.rotation;
        if (KeepScale)
        {
            transform.localScale = Vector3.one * (Vector3.Distance(transform.position, c.transform.position) * ScaleFactor);
        }
    }

    private void Update()
    {
        c = CameraManager.CurrentCam;
        Perform();
    }

    public void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            c = UnityEditor.SceneView.currentDrawingSceneView.camera;
        }
#endif
        Perform();
    }
}
