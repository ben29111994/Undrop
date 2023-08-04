using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;

public class ShopRope : MonoBehaviour
{
    public static ShopRope Instance;

    public Animator buyAnimator;
    public GameObject pagePrefab;

    [Header("Pillars")]
    public SimpleScrollSnap rope_SSS;
    public int price;
    public Text priceText;
    public Sprite[] ropeSprite;
    public RopeItem ropeItemPrefab;
    public Transform pageParentTransform;
    private List<RopeItem> listRopeItem = new List<RopeItem>();

    private int CurrentRope
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentRope");
        }
        set
        {
            PlayerPrefs.SetInt("CurrentRope", value);
        }
    }

    public void Init()
    {
        Instance = this;

        PlayerPrefs.SetInt("Rope0", 1);

        int n = 0;
        Transform currentBoard = Instantiate(pagePrefab, pageParentTransform).transform.GetChild(0);

        for (int i = 0; i < ropeSprite.Length; i++)
        {
            RopeItem ropeItem = Instantiate(ropeItemPrefab, currentBoard);
            listRopeItem.Add(ropeItem);
            ropeItem.SetIcon(ropeSprite[i], i);

            int j = i;
            ropeItem.selectButton.onClick.AddListener(() => OnClickSelect_RopeItem(j));

            n++;
            if (n >= 9)
            {
                n = 0;
                currentBoard = Instantiate(pagePrefab, pageParentTransform).transform.GetChild(0);
            }
        }

        OnClickSelect_RopeItem(CurrentRope);
        UpdatePrice();
    }

    public void OnClickSelect_RopeItem(int ID)
    {
        CurrentRope = ID;

        for (int i = 0; i < listRopeItem.Count; i++)
        {
            listRopeItem[i].SelectPillar(ID);
        }

        ShopControl.Instance.ChangeRopeMaterial(ID);
    }

    public void OnClick_UnlockRandomROpe()
    {
        buyAnimator.SetTrigger("Bubble");

        if (IsRopeOVer()) return;

        int myGem = GameController.instance.CurrentGem;
        if (myGem < price) return;

        GameController.instance.CurrentGem -= price;
        ShopControl.Instance.gemText.text = GameController.instance.CurrentGem.ToString();

        int r = Random.Range(0, listRopeItem.Count);

        for (int i = 0; i < 1; i++)
        {
            if (PlayerPrefs.GetInt("Rope" + r) == 1)
            {
                r = Random.Range(0, listRopeItem.Count);
                i--;
            }
        }

        int currentPage = rope_SSS.CurrentPanel;

        if (r < 9)
        {
            if (currentPage == 1)
            {
                rope_SSS.GoToPreviousPanel();
            }
        }
        else
        {
            if (currentPage == 0)
            {
                rope_SSS.GoToNextPanel();
            }
        }

        PlayerPrefs.SetInt("Rope" + r, 1);
        OnClickSelect_RopeItem(r);
        UpdatePrice();
        ShopControl.Instance.unlockPopup.ActivePopup(ropeSprite[r]);
    }

    private bool IsRopeOVer()
    {
        int unlockIndex = 0;

        for (int i = 0; i < listRopeItem.Count; i++)
        {
            if (PlayerPrefs.GetInt("Rope" + i) == 1)
            {
                unlockIndex++;
            }
        }

        if (unlockIndex >= listRopeItem.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdatePrice()
    {
        int unlockIndex = 0;

        for (int i = 0; i < listRopeItem.Count; i++)
        {
            if (PlayerPrefs.GetInt("Rope" + i) == 1)
            {
                unlockIndex++;
            }
        }

        if (unlockIndex <= 1)
        {
            price = 350;
        }
        else
        {
            price = 500;
        }

        priceText.text = price.ToString();
    }
}