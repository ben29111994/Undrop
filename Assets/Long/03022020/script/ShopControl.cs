using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopControl : MonoBehaviour
{
    public static ShopControl Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        shopPillar.Init();
        shopRope.Init();
    }

    public bool isShopping;

    public GameObject shop;
    public ShopPillar shopPillar;
    public ShopRope shopRope;

    public GameObject pillar;
    public GameObject pillarScroll;

    public GameObject rope;
    public GameObject ropeScroll;

    public UnlockPopup unlockPopup;

    public Text gemText;

    public Material[] pillarMaterial;
    public Material[] ropeMaterial;

    public Material currentPillarMaterial;
    public Material currentRopeMaterial;

    public void ChangePillarMaterial(int number)
    {
        currentPillarMaterial = pillarMaterial[number];

        List<Transform> listRope = GameController.instance.listRopes;

        for (int i = 0; i < listRope.Count; i++)
        {
            for(int j = 1; j < listRope[i].childCount; j++)
            {
                listRope[i].GetChild(j).GetComponent<Renderer>().material = currentPillarMaterial;
            }
        }
    }

    public void ChangeRopeMaterial(int number)
    {
        currentRopeMaterial = ropeMaterial[number];

        List<Transform> listRope = GameController.instance.listRopes;

        for(int i = 0; i < listRope.Count; i++)
        {
            listRope[i].GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material = currentRopeMaterial;
        }
    }

    public void Onclick_OpenShop()
    {
        Time.timeScale = 0;
        gemText.text = GameController.instance.CurrentGem.ToString();
        isShopping = true;

        shop.SetActive(true);

        pillar.SetActive(true);
        rope.SetActive(false);

        pillarScroll.SetActive(true);
        ropeScroll.SetActive(false);
    }

    public void OnClick_CloseShop()
    {
        Time.timeScale = 1;
        isShopping = false;

        shop.SetActive(false);
    }

    public void OnClick_Pillar()
    {
        pillar.SetActive(true);
        rope.SetActive(false);

        pillarScroll.SetActive(true);
        ropeScroll.SetActive(false);
    }

    public void OnClick_Rope()
    {
        pillar.SetActive(false);
        rope.SetActive(true);

        pillarScroll.SetActive(false);
        ropeScroll.SetActive(true);
    }
}
