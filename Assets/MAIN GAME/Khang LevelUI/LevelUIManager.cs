using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    public LevelUI levelUIPrefab;
    public Transform levelUIParent;
    public List<LevelUI> levelUIsList = new List<LevelUI>();

    private void OnEnable()
    {
        SpawnLevelUIs(GameController.instance.WorldNumber);
    }

    public void SpawnLevelUIs(int totalWorlds)
    {
        if (totalWorlds >= GameController.instance.maxLevel)
        {
            totalWorlds = GameController.instance.maxLevel;
        }

        if (levelUIsList.Count > 0)
        {
            foreach (LevelUI item in levelUIsList)
            {
                Destroy(item.gameObject);
            }
            levelUIsList.Clear();
        }

        for (int i = 0; i < totalWorlds; i++)
        {
            LevelUI levelUI = Instantiate(levelUIPrefab);
            levelUI.transform.SetParent(levelUIParent, false);
            levelUI.Init(i);
            levelUIsList.Add(levelUI);
        }
    }

}
