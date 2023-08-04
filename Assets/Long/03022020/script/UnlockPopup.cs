using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockPopup : MonoBehaviour
{
    public Image icon;

    public void ActivePopup(Sprite sprite)
    {
        icon.sprite = sprite;
        gameObject.SetActive(true);
        StartCoroutine(C_Hide());
    }

    private IEnumerator C_Hide()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        gameObject.SetActive(false);
    }
}
