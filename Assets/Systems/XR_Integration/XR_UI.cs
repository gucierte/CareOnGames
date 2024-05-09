using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Selectable))]
public class XR_UI : Interactive, iPlayerInteraction
{
    public bool isReady { get; set; }
    public static XR_UI lastSelection { get; set; }

    //Basic vars
    public RectTransform rectTransform { get { return (RectTransform)transform; } }
    public Selectable selectable;
    public Slider slider;
    BoxCollider col;

    public void SetCollider(BoxCollider collider, RectTransform rect)
    {
        Bounds b = RectTransformUtility.CalculateRelativeRectTransformBounds(rect);
        Vector3 s = b.size;
        s.z = Mathf.Abs((selectable.targetGraphic.depth) * 0.01f);

        collider.center = b.center;
        collider.size = s;
    }
    public void Setup()
    {
        selectable = GetComponent<Selectable>();
        col = GetComponent<BoxCollider>();
        col.isTrigger = true;

        SetCollider(col, rectTransform);
    }

    //Callback
    public override void OnPlayerAim(XRController controller)
    {
        if (!this.enabled)
            return;
        if (!isReady)
            return;
        base.OnPlayerAim(controller);
        isPlayerAiming();

        //Player press interaction btn
        if (controller.Controller.GetButton(WebXR.WebXRController.ButtonTypes.Trigger))
        {
            OnSlider(controller.ray.hit);
        }

        //Player start press interaction btn
        if (controller.Controller.GetButtonDown(WebXR.WebXRController.ButtonTypes.Trigger))
        {
            selectable.OnPointerDown(new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left });
            selectable.OnSelect(new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left });
            lastSelection = this;
        }

        //Player release interaction btn
        if (controller.Controller.GetButtonUp(WebXR.WebXRController.ButtonTypes.Trigger))
        {
            selectable.OnPointerUp(new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left });
        }
    }
    public override void OnPlayerAim(MobileControll controller)
    {
        //base.OnPlayerAim(controller);
        if (!this.enabled)
            return;
        if (!isReady)
            return;
        isPlayerAiming();
        if (Vector3.Distance(transform.position, controller.cam.transform.position) > interactionRange)
            return;
        controller.Load();
        if (controller.loaded >= 1)
        {
            OnSlider(controller.hit);
            if (!MobileControll.interact)
            {
                OnTriggerEvent.Invoke();
                selectable.OnSelect(new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left });
                lastSelection = this;
                MobileControll.interact = true;
            }
        }
    }
    public override void OnMouseOver()
    {
        if (!this.enabled)
            return;
        if (!isReady)
            return;
        base.OnMouseOver();
    }
    public override void OnPlayerTriggerDown(XRController controller)
    {
        if (!isReady)
            return;
        base.OnPlayerTriggerDown(controller);
        if (controller.ray.hit.collider != null)
        {
            if (controller.ray.hit.collider.gameObject != this.gameObject)
            {
                selectable.OnPointerUp(new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left });
                selectable.OnDeselect(new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left });
            }
        } else
        {
            selectable.OnPointerUp(new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left });
            selectable.OnDeselect(new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left });
        }
    }

    //Behaviours
    public void isPlayerAiming()
    {
        CancelInvoke(nameof(PlayerIsNotAiming));
        selectable.OnPointerEnter(new PointerEventData(EventSystem.current));
        Invoke(nameof(PlayerIsNotAiming), Time.deltaTime + Time.fixedDeltaTime);
    }
    void PlayerIsNotAiming()
    {
        selectable.OnPointerExit(new PointerEventData(EventSystem.current));
    }
    public void OnSlider(RaycastHit hit)
    {
        slider = GetComponent<Slider>();
        col = GetComponent<BoxCollider>();
        //Slider
        if (slider)
        {
            Vector3 p = transform.InverseTransformPoint(hit.point);
            switch (slider.direction)
            {
                case Slider.Direction.LeftToRight:
                    p.x = ((p.x / col.size.x) + 0.5f); //Range between 0 and 1
                    p.x *= slider.maxValue; //Range between 0 and MaxSliderValue
                    slider.value = p.x;
                    break;
                case Slider.Direction.RightToLeft:
                    p.x = ((-p.x / col.size.x) + 0.5f); //Range between 0 and 1
                    p.x *= slider.maxValue; //Range between 0 and MaxSliderValue
                    slider.value = p.x;
                    break;
                case Slider.Direction.BottomToTop:
                    p.y = ((p.y / col.size.y) + 0.5f); //Range between 0 and 1
                    p.y *= slider.maxValue; //Range between 0 and MaxSliderValue
                    slider.value = p.y;
                    break;
                case Slider.Direction.TopToBottom:
                    p.y = ((-p.y / col.size.y) + 0.5f); //Range between 0 and 1
                    p.y *= slider.maxValue; //Range between 0 and MaxSliderValue
                    slider.value = p.y;
                    break;
                default:
                    break;
            }
        }
    }

    //Mono
    void Awake()
    {
        OnMouseOver();
        //Debug.Log("");
    }
    private void OnEnable()
    {
        Setup();
        Invoke(nameof(EnableThis), (Time.deltaTime + Time.fixedDeltaTime) * 2);
    }
    private void OnDisable()
    {
        isReady = false;
    }
    public void EnableThis()
    {
        isReady = true;
    }
    public void OnValidate()
    {
        Setup();
    }
    public override void OnDrawGizmosSelected()
    {
        Setup();
        base.OnDrawGizmosSelected();
    }
}
