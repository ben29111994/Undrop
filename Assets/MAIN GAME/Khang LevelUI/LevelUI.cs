using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public Text txtLevelUI;
    public Button btnLevelUI;
    public int ID;

    private void OnEnable()
    {
        btnLevelUI.onClick.AddListener(OnBtnLevelUIClick);
    }

    private void OnDisable()
    {
        btnLevelUI.onClick.RemoveListener(OnBtnLevelUIClick);
    }

    public void Init(int world)
    {
        txtLevelUI.text = (world + 1).ToString();
        this.ID = world;
    }

    private void OnBtnLevelUIClick()
    {
        PlayerPrefs.SetInt("pointerLevel", ID);
        GameController.instance.CurrentTask = 0;
        GameController.instance.LoadScene();
    }

}
