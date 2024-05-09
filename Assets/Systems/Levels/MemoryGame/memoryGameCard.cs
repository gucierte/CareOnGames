using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class memoryGameCard : Interactive
{
    public Animator anim;
    public static memoryGameCard lastFlippedCard;

    public List<Renderer> front;
    public Renderer back;
    [Space]
    public List<Texture> FrontTextures;
    public Color basicColour = new Color(1, 1, 1, 1);
    public bool flipped;
    public memoryGameCard ParentCard;
    [Header("SFXs")]
    public AudioSource SFX_Fail;
    public bool Done;



    private void OnValidate()
    {
        if (ParentCard)
        {
            ParentCard.basicColour = basicColour;
            for (int i = 0; i < front.Count; i++)
            {
                ParentCard.FrontTextures[i] = FrontTextures[i];
                ParentCard.front[i].sharedMaterial = front[i].sharedMaterial;
            }

            ParentCard.ParentCard = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < front.Count; i++)
        {
            front[i].material.SetTexture("_MainTex", FrontTextures[i]);
            front[i].material.color = basicColour;

            if (ParentCard)
            {
                ParentCard.basicColour = basicColour;
                ParentCard.FrontTextures[i] = FrontTextures[i];
                ParentCard.front[i].material = front[i].material;
            }

            anim = GetComponent<Animator>();
        }
    }

    private void FixedUpdate()
    {
        Quaternion rot = Quaternion.Euler(0, 0, 0);

        if (memoryGame.Ready && memoryGame.gameTime >= 5)
        {
            if (!flipped) { rot = Quaternion.Euler(0, 0, 180); }
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 3 * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rot, 5 * Time.deltaTime);

        interactible = !Done;
    }

    void Hover()
    {
        CancelInvoke(nameof(HoverOff));
        anim.SetBool("Hover", true);
        Invoke(nameof(HoverOff), Time.deltaTime + Time.fixedDeltaTime);
    }
    void HoverOff()
    {
        anim.SetBool("Hover", false);
    }

    public override void OnPlayerAim(XRController controller)
    {
        if (!interactible)
            return;
        if (Vector3.Distance(transform.position, controller.transform.position) > interactionRange)
            return;
        Hover();

        if (controller.Controller.GetButtonDown(WebXR.WebXRController.ButtonTypes.Trigger))
        {
            if (!flipped)
            {
                Flip();
            } else
            {
                UnFlip();
            }
        }

        base.OnPlayerAim(controller);
    }

    public override void OnPlayerAim(MobileControll controller)
    {
        base.OnPlayerAim(controller);
        if (!interactible)
            return;
        if (Vector3.Distance(transform.position, controller.transform.position) > interactionRange)
            return;
        Hover();

        controller.Load();
        if (controller.loaded >= 1)
        {
            if (!MobileControll.interact)
            {
                if (!flipped)
                {
                    Flip();
                }
                else
                {
                    UnFlip();
                }
                MobileControll.interact = true;
            }
        }
    }

    public override void OnMouseOver()
    {
        base.OnMouseOver();
        if (!interactible)
            return;
        if (Vector3.Distance(transform.position, CameraManager.CurrentCam.transform.position) > interactionRange)
            return;
        Hover();

        if (Input.GetButtonDown("Fire1"))
        {
            if (!flipped)
            {
                Flip();
            }
            else
            {
                UnFlip();
            }
        }
    }


    public void Flip()
    {
        MusicFlow.main.Flow += .1f;
        if (memoryGame.gameTime < 5)
        {
            memoryGame.gameTime = 5;
        }
        if (lastFlippedCard == ParentCard)
        {
            MusicFlow.main.Flow += .3f;
            interactible = false;
            ParentCard.interactible = false;
            lastFlippedCard = null;
            anim.SetBool("Done", true);
            ParentCard.anim.SetBool("Done", true);
            ParentCard.Done = true;
            Done = true;
        } else
        {
            if (lastFlippedCard)
            {
                lastFlippedCard.UnFlip();
            }
            lastFlippedCard = this;
            SFX_Fail.Play();
        }

        memoryGame.main.CheckFinish();

        flipped = true;
    }

    public void UnFlip()
    {
        flipped = false;
        Done = false;
        if (lastFlippedCard == this)
        {
            anim.SetBool("Done", false);
            lastFlippedCard = null;
        }
    }
}
