using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceController : MonoBehaviour
{

    #region CONST
    #endregion

    #region EDITOR PARAMS

    public List<GameObject> ambiencesList;

    #endregion

    #region PARAMS
    #endregion

    #region PROPERTIES

    public static AmbienceController Instance { get; private set; }

    #endregion

    #region EVENTS
    #endregion

    #region METHODS

    private void Awake()
    {
        Instance = this;
    }

    public void RandomAmbiences()
    {
        ambiencesList[(int)Random.Range(0f, ambiencesList.Count)].SetActive(true);
    }


    #endregion

    #region DEBUG
    #endregion

}
