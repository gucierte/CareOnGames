using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MobileControllHUD : MonoBehaviour
{
    public GameObject LoadingAim;
    public Image LoadingOrb;

    private void Start()
    {
        LoadingAim.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        LoadingAim.gameObject.SetActive(MobileControll.use);
        if (!MobileControll.use)
            return;
        LoadingOrb.fillAmount = MobileControll.main().loaded;
    }
}
