using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;

public class ShopPillar : MonoBehaviour
{
    public static ShopPillar Instance;

    public Animator buyAnimator;
    public GameObject pagePrefab;

    [Header("Pillars")]
    public SimpleScrollSnap pillar_SSS;
    public int price;
    public Text priceText;
    public Sprite[] pillarSprite;
    public PillarItem pillarItemPrefab;
    public Transform pageParentTransform;
    private List<PillarItem> listPillarItem = new List<PillarItem>();

    private int CurrentPillar
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentPillar");
        }
        set
        {
            PlayerPrefs.SetInt("CurrentPillar", value);
        }
    }

    public void Init()
    {
        Instance = this;

        PlayerPrefs.SetInt("Pillar0", 1);

        int n = 0;
        Transform currentBoard = Instantiate(pagePrefab, pageParentTransform).transform.GetChild(0);

        for (int i = 0; i < pillarSprite.Length; i++)
        {
            PillarItem pillarItem = Instantiate(pillarItemPrefab, currentBoard);
            listPillarItem.Add(pillarItem);
            pillarItem.SetIcon(pillarSprite[i], i);

            int j = i;
            pillarItem.selectButton.onClick.AddListener(() => OnClick_SelectPillar(j));

            n++;
            if (n >= 9)
            {
                n = 0;
                currentBoard = Instantiate(pagePrefab, pageParentTransform).transform.GetChild(0);
            }
        }

        OnClick_SelectPillar(CurrentPillar);
        UpdatePrice();
    }

    public void OnClick_SelectPillar(int ID)
    {
        CurrentPillar = ID;

        for (int i = 0; i < listPillarItem.Count; i++)
        {
            listPillarItem[i].SelectPillar(ID);
        }

        ShopControl.Instance.ChangePillarMaterial(ID);
    }

    public void OnClick_UnlockRandomPillar()
    {
        buyAnimator.SetTrigger("Bubble");

        if (IsPillarOver()) return;

        int myGem = GameController.instance.CurrentGem;
        if (myGem < price) return;

        GameController.instance.CurrentGem -= price;
        ShopControl.Instance.gemText.text = GameController.instance.CurrentGem.ToString();

        int r = Random.Range(0, listPillarItem.Count);

        for (int i = 0; i < 1; i++)
        {
            if (PlayerPrefs.GetInt("Pillar" + r) == 1)
            {
                r = Random.Range(0, listPillarItem.Count);
                i--;
            }
        }

        int currentPage = pillar_SSS.CurrentPanel;

        if (r < 9)
        {
            if (currentPage == 1)
            {
                pillar_SSS.GoToPreviousPanel();
            }
        }
        else
        {
            if (currentPage == 0)
            {
                pillar_SSS.GoToNextPanel();
            }
        }

        PlayerPrefs.SetInt("Pillar" + r, 1);
        OnClick_SelectPillar(r);
        UpdatePrice();
        ShopControl.Instance.unlockPopup.ActivePopup(pillarSprite[r]);
    }

    private bool IsPillarOver()
    {
        int unlockIndex = 0;

        for (int i = 0; i < listPillarItem.Count; i++)
        {
            if (PlayerPrefs.GetInt("Pillar" + i) == 1)
            {
                unlockIndex++;
            }
        }

        if (unlockIndex >= listPillarItem.Count)
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

        for (int i = 0; i < listPillarItem.Count; i++)
        {
            if (PlayerPrefs.GetInt("Pillar" + i) == 1)
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