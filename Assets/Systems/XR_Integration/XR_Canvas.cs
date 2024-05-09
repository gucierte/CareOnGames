using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XR_Canvas : MonoBehaviour
{
    [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(editable))]
    public Canvas canvas;

    [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(editable))]
    public List<Selectable> selectables = new List<Selectable>();
    [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(editable))]
    public List<XR_UI> selectablesXRs = new List<XR_UI>();
    bool editable = false;

    private void OnEnable()
    {
        canvas = GetComponent<Canvas>();
        SetupSelectables();
    }

    private void Start()
    {
        InvokeRepeating(nameof(SetupSelectables), 1, 1);
    }

    public void SetupSelectables()
    {
        canvas.worldCamera = CameraManager.CurrentCam;
        selectablesXRs = new List<XR_UI>();
        selectables = GetComponentsInChildren<Selectable>(true).ToList();
        foreach (var s in selectables)
        {
            if (s.GetComponent<XR_UI>())
                return;
            XR_UI xr = s.gameObject.AddComponent<XR_UI>();
            xr.Setup();
            selectablesXRs.Add(xr);
        }
    }
    public void OnDrawGizmos()
    {
        canvas = GetComponent<Canvas>();
        SetupSelectables();
    }
}
