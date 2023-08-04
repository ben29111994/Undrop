using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PillarItem : MonoBehaviour
{
    public int PillarID;
    public Image[] icons;
    public Button selectButton;

    public GameObject lockStatus;
    public GameObject unlockStatus;
    public GameObject selectStatus;

    public Animator anim;

    private void Start()
    {
        IsUnlock();
    }

    public void SetIcon(Sprite spr,int id)
    {
        PillarID = id;

        for(int i = 0; i < icons.Length; i++)
        {
            icons[i].sprite = spr;
        }
    }

    public void SelectPillar(int ID)
    {
        if (IsUnlock() == false)
        {
            selectStatus.SetActive(false);
            return;
        }

        if(ID == PillarID)
        {
            anim.SetTrigger("Bubble");

            selectStatus.SetActive(true);
        }
        else
        {
            selectStatus.SetActive(false);
        }
    }

    private bool IsUnlock()
    {
        int p = PlayerPrefs.GetInt("Pillar" + PillarID);

        if(p == 0)
        {
            lockStatus.SetActive(true);
            unlockStatus.SetActive(false);
            selectStatus.SetActive(false);
            return false;
        }
        else
        {
            lockStatus.SetActive(false);
            unlockStatus.SetActive(true);
            return true;
        }
    }
}