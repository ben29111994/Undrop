using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using Obi;
// using GameAnalyticsSDK;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    [Header("Variable")]
    public static GameController instance;
    public int maxLevel;
    public bool isStartGame = false;
    public bool isControl = false;
    int maxPlusEffect = 0;
    bool isVibrate = false;
    Rigidbody rigid;
    bool isDrag = false;
    public float shakeSpeed;
    public Vector3 m_EulerAngleVelocity;
    public Vector3 dir;
    public bool isShake = false;
    public int ropeID = 0;
    public LayerMask clickMask;
    GameObject clickObjectMask;
    public LayerMask dragMask;
    public int numOfRope;
    public List<int> numOfRopeList = new List<int>();
    public GameObject ropeLeft;
    public GameObject btnRope;
    public List<GameObject> platformsList = new List<GameObject>();
    public GameObject currentPlatform;
    public List<GameObject> simplePlatformList = new List<GameObject>();
    public float taskDuration;
    private float startTimer;
    private float endTimer;
    public ParticleSystem confetti;
    public ParticleSystem zoneGround;
    public GameObject markSlamEffect;
    bool isDioramaMode = false;
    public int chanceCount = 3;
    public float oldHeight;
    public float currentHeight;
    public float oldPlatformHeight;
    public GameObject mapBG;
    public List<GameObject> listMapBG = new List<GameObject>();

    public int CurrentTask
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentTask");
        }
        set
        {
            PlayerPrefs.SetInt("CurrentTask", value);
        }
    }

    public ObiRod rope;
    public static bool isCameraEffect;
    int platformIndex;


    [Header("UI")]
    public GameObject winPanel;
    public GameObject winText;
    public GameObject losePanel;
    public Text currentLevelText;
    public int currentLevel;
    public Canvas canvas;
    public GameObject startGameMenu;
    public GameObject inGameMenu;
    public GameObject levelMenu;
    public RectTransform topPanel;
    public GameObject buttonLevel;
    public InputField levelInput;
    public Text title;
    public Text timer;
    public Transform handWatch;
    public Image fill;
    public Image star1, star2, star3, star4, star5, star6;
    public List<Image> listRopeFill = new List<Image>();
    public List<Image> listTask = new List<Image>();
    static int currentBG = 0;
    public GameObject dropButton;
    public GameObject undoButton;
    public GameObject missionText;
    public Text status;
    public RectTransform oopsRect;
    public List<Button> listLevelButton = new List<Button>();
    public GameObject gemAnimation;
    public Text gemText;
    public GameObject gemScoring;
    public int CurrentGem
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentGem");
        }
        set
        {
            PlayerPrefs.SetInt("CurrentGem", value);
            gemText.text = value.ToString();
        }
    }

    int gemScore;
    public GameObject levelBonusTitle;
    public GameObject winPanelPopup;
    public GameObject winNext;
    public GameObject winRestart;
    public Text winRopeLeftText;
    public GameObject winLightingEffect;

    [Header("Objects")]
    public Text newRopeLeftText;
    public GameObject plusVarPrefab;
    GameObject conffetiSpawn;
    public GameObject ropePrefab;
    public GameObject ropeCustomPrefab;
    public GameObject markPrefab;
    public List<Transform> listRopes = new List<Transform>();
    public List<GameObject> listLevel = new List<GameObject>();
    public List<GameObject> listBonusLevel = new List<GameObject>();
    public List<Color> listRopeColor = new List<Color>();
    public List<Color> listBGColor = new List<Color>();
    public List<GameObject> targetObjectsList = new List<GameObject>();
    Transform startPoint;
    Transform endPoint;
    public GameObject BG;
    public GameObject explosionConfetti;
    //public GameObject tutorial;
    public GameObject hand, handClickDrop;
    public GameObject starAnim;
    public GameObject starExplode;
    public GameObject objectExplode;
    public GameObject gemExplode;
    GameObject platformObject;
    private bool isTimeLevel;
    public static bool isBonusLevel;
    public int levelNumber;
    private int worldNumber;
    private bool isDrop;

    public const string LevelNbGA = "LevelNbGA";
    private int levelNumberGA;
    private int worldNumberGA;
    public int ropeUsed;
    public List<GameObject> dioramaList = new List<GameObject>();
    public bool isShowDiorama = false;
    public GameObject highlightEffect;
    public GameObject shinyEffect;
    public GameObject dioramaConffeti;
    public GameObject skipDioramaButton;
    public GameObject dioramaMap;
    public GameObject nextGameObject;

    public int LevelNumberGA
    {
        get
        {
            int temp = PlayerPrefs.GetInt("LevelNbGA");
            return temp;
        }
        set
        {
            PlayerPrefs.SetInt("LevelNbGA", value);
        }
    }

    public int WorldNumberGA
    {
        get
        {
            int temp = (int)(LevelNumberGA / 4);
            // Debug.Log("Get WorlNbGA: " + temp);
            return temp;
        }
        set
        {
            // Debug.Log("Set WorldNbGA: " + value);
            worldNumberGA = value;
        }
    }


    public float TimeLevel
    {
        get
        {
            return PlayerPrefs.GetFloat("TimeLevel");
        }
        set
        {
            PlayerPrefs.SetFloat("TimeLevel", value);
        }
    }

    public int WorldNumber
    {
        get
        {
            int temp = (int)(levelNumber / 3) + 1;
            if (levelNumber > 0 && levelNumber % 3 == 0)
            {
                temp--;
            }
            return temp;
        }

        set
        {
            worldNumber = value;
        }
    }


    private void Awake()
    {
        tuIndex = -1;
        // MMVibrationManager.Vibrate();
        // MMVibrationManager.iOSInitializeHaptics();
        // GameAnalytics.Initialize();
    }

    private void OnApplicationQuit()
    {
        pointerLevel = currentLevel;
        PlayerPrefs.SetInt("pointerLevel", pointerLevel);
    }

    private void LateUpdate()
    {
        if (isTimeLevel)
        {
            TimeLevel += Time.unscaledDeltaTime;
        }
        if (Input.GetKeyDown(KeyCode.D)) PlayerPrefs.DeleteAll();
        if (Input.GetKeyDown(KeyCode.C)) CurrentGem += 999;

        UpdateButtonUndo();
    }

    private void UpdateButtonUndo()
    {
        if (isDrop)
        {
            undoButton.SetActive(false);
            return;
        }

        //if (isTutorial)
        //{
        //    if (currentLevelText.text == "LEVEL 1")
        //    {
        //        if (ropeID > 2)
        //        {
        //            undoButton.SetActive(true);
        //        }
        //        else
        //        {
        //            undoButton.SetActive(false);
        //        }
        //    }
        //    else if (currentLevelText.text == "LEVEL 2")
        //    {
        //        if (ropeID > 1)
        //        {
        //            undoButton.SetActive(true);
        //        }
        //        else
        //        {
        //            undoButton.SetActive(false);
        //        }
        //    }
        //}
        //else
        //{
            if (ropeID > 0)
            {
                undoButton.SetActive(true);
            }
            else
            {
                undoButton.SetActive(false);
            }
        //}
    }

    void SetFOV()
    {
        float ratio = Camera.main.aspect;
        topPanel.anchorMin = new Vector2(0.5f, 1f);
        topPanel.anchorMax = new Vector2(0.5f, 1f);
        topPanel.pivot = new Vector2(0.5f, 1f);

        if (ratio >= 0.74f) // 3:4
        {
            oopsRect.anchoredPosition = new Vector2(0, 270);
            topPanel.anchoredPosition = new Vector3(0f, 0f);
        }
        else if (ratio >= 0.56f) // 9:16
        {
            oopsRect.anchoredPosition = new Vector2(0, 270);
            topPanel.anchoredPosition = new Vector3(0f, 0f);

        }
        else if (ratio >= 0.45f) // 9:19
        {
            oopsRect.anchoredPosition = new Vector2(0, 445);
            topPanel.anchoredPosition = new Vector3(0f, -50f);
        }
    }

    public int pointerLevel;

    private void OnEnable()
    {
        SetFOV();
        Application.targetFrameRate = 60;
        instance = this;
        rigid = GetComponent<Rigidbody>();
        // AmbienceController.Instance.RandomAmbiences();
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        pointerLevel = PlayerPrefs.GetInt("pointerLevel");

        // Debug.Log("Curr: " + currentLevel + " " + pointerLevel);


        StartCoroutine(delayStart());
    }

    public int tuIndex;
    public Transform startPosTuObject;
    public Transform endPosTuObject;
    public Vector3[] startPosTuArray;
    public Vector3[] endPosTuArray;
    public StopDoPath stopDoPath;
    int tempLevel;

    IEnumerator delayStart()
    {
        // Debug.Log("Start LVNBGA: " + LevelNumberGA);
        // Debug.Log("Start WRLDNBGA: " + WorldNumberGA);
        isDioramaMode = false;
        if (isBonusLevel)
        {
            newRopeLeftText.text = "5/5";
        }
        else
        {
            newRopeLeftText.text = "5/5";
        }

        maxLevel = listLevel.Count - 1;
        startTimer = Time.unscaledTime;
        if (CurrentTask >= 3)
        {
            CurrentTask = 0;
        }
        isTimeLevel = true;

        listTask[CurrentTask].transform.parent.GetChild(1).gameObject.SetActive(true);

        for (int i = 0; i < CurrentTask; i++)
        {
            listTask[i].DOFillAmount(1, 0);
        }
        Camera.main.transform.DOMoveX(-50, 0);
        isCameraEffect = false;
        Camera.main.transform.DOMoveX(0, 1);
        var currentLevelCount = PlayerPrefs.GetInt("levelCount");
        // currentLevel = PlayerPrefs.GetInt("currentLevel");
        // currentLevelText.text = "LEVEL " + (currentLevelCount + 1).ToString();
        levelNumber = (pointerLevel + 1) * 3 - (3 - (CurrentTask + 1));
        //Debug.LogError(levelNumber);
        if(levelNumber > 45)
        {
            levelNumber = 0;
            PlayerPrefs.SetInt("currentLevel", 0);
            PlayerPrefs.SetInt("pointerLevel", 0);
        }
        if(levelNumber >= 31)
        {
            mapBG = listMapBG[1];
        }
        else
        {
            mapBG = listMapBG[0];
        }
        mapBG.SetActive(true);
        // Debug.Log(levelNumber + " " + currentLevel + " " + CurrentTask);
        currentLevelText.text = "LEVEL " + levelNumber.ToString();
        currentLevelText.transform.DOScale(Vector3.one * 1.2f, 0.2f).SetLoops(2, LoopType.Yoyo);
        CurrentGem += 0;
        var colorID = Random.Range(0, 5);
        yield return null;
        //ropePrefab.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material = ShopControl.Instance.currentRopeMaterial;

        //ropeCustomPrefab.transform.GetChild(0).GetComponent<Renderer>().material.color = listRopeColor[colorID];
        BG.GetComponent<Renderer>().material.color = listBGColor[currentBG];
        currentBG++;
        if (currentBG > listBGColor.Count - 1)
        {
            currentBG = 0;
        }

        SetPlatform();
        var dragMask = platformParent.transform.GetChild(0);
        dragMask.transform.parent = currentPlatform.transform;

        if (isBonusLevel)
        {
            numOfRope = 5;
        }
        for (int i = 0; i < numOfRope; i++)
        {
            GameObject ropeSpawn;
            // if (isBonusLevel)
            // {
            //     ropeSpawn = Instantiate(ropeCustomPrefab);
            // }
            // else
            // {
            ropeSpawn = Instantiate(ropePrefab);
            ropeSpawn.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material = ShopControl.Instance.currentRopeMaterial;
            for (int j = 1; j < ropeSpawn.transform.childCount; j++)
            {
                ropeSpawn.transform.GetChild(j).GetComponent<Renderer>().material = ShopControl.Instance.currentPillarMaterial;
            }
            // }
            listRopes.Add(ropeSpawn.transform);
        }

        pointerLevel = pointerLevel >= 37 ? (pointerLevel - 36) : pointerLevel;
        if (isBonusLevel)
        {
            gemText.transform.parent.SetParent(canvas.transform);
            gemText.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(10f, 30f);
            topPanel.gameObject.SetActive(true);

            for (int i = 0; i < topPanel.transform.childCount; i++)
            {
                if (i == 0)
                {
                    topPanel.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    topPanel.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            GameObject bonusLevelObject = Instantiate(listBonusLevel[pointerLevel]);
            bonusLevelObject.SetActive(true);
            bonusLevelObject.transform.SetParent(this.transform);
            bonusLevelObject.transform.localPosition = Vector3.zero;
            bonusLevelObject.transform.localRotation = Quaternion.identity;

            // listBonusLevel[pointerLevel].SetActive(true);

            ObiCollider[] gemList = bonusLevelObject.GetComponentsInChildren<ObiCollider>();
            foreach (var item in gemList)
            {
                targetObjectsList.Add(item.gameObject);
            }
            gemScore = targetObjectsList.Count;
            levelBonusTitle.SetActive(true);
            levelBonusTitle.transform.DOLocalMoveX(-1000, 0);
            levelBonusTitle.transform.DOLocalMoveX(0, 0.5f);
        }
        else
        {
            StartCoroutine(delayMissionText());

            GameObject currentLevelObject = Instantiate(listLevel[pointerLevel]);
            currentLevelObject.SetActive(true);
            currentLevelObject.transform.SetParent(this.transform);
            currentLevelObject.transform.localPosition = Vector3.zero;
            currentLevelObject.transform.localRotation = Quaternion.identity;

            targetObjectsList.Clear();

            if (currentLevelObject.transform.GetChild(CurrentTask).CompareTag("Multi"))
            {
                for (int i = 0; i < 2; i++)
                {
                    targetObjectsList.Add(currentLevelObject.transform.GetChild(CurrentTask).transform.GetChild(i).gameObject);
                    targetObjectsList[i].SetActive(true);
                }
            }
            else
            {
                targetObjectsList.Add(currentLevelObject.transform.GetChild(CurrentTask).gameObject);
                targetObjectsList[0].SetActive(true);
            }
        }

        yield return null;

        for (int i = 0; i < listRopes.Count; i++)
        {
            for (int j = 1; j < listRopes[i].childCount; j++)
            {
                listRopes[i].GetChild(j).GetComponent<Renderer>().material = ShopControl.Instance.currentPillarMaterial;
            }
        }

        // AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.StartEvent);

        startGameMenu.SetActive(true);
        title.DOColor(new Color32(255, 255, 255, 0), 3);
        ropeID = 0;
        yield return new WaitForSeconds(0.01f);
        if (CurrentTask == 2)
        {
            stopDoPath = targetObjectsList[0].GetComponent<StopDoPath>();
        }
        isControl = true;
        //if (pointerLevel == 0 && (CurrentTask == 0 || CurrentTask == 1))
        //{
        //    hand.GetComponent<Image>().enabled = false;
        //    tutorial.SetActive(true);
        //    Tutorial();
        //}
        //else
        //{
            hand.SetActive(true);
            startPosTuObject.gameObject.SetActive(false);
            endPosTuObject.gameObject.SetActive(false);
        //}
        //Debug.LogError(CurrentTask);
        oldHeight = targetObjectsList[0].transform.position.y;
        if(targetObjectsList[0].GetComponent<StopDoPath>().myPathTween != null)
        {
            oldHeight = 10;
        }
        else
        {
            oldHeight = 10;
        }
        oldPlatformHeight = currentPlatform.transform.position.y;
        currentHeight = oldHeight * chanceCount;
        if (targetObjectsList[0].GetComponent<StopDoPath>().myPathTween == null)
        {
            foreach (var item in targetObjectsList)
            {
                item.transform.DOMoveY(currentHeight, 0);
            }
        }
        Camera.main.transform.DOMoveY(15 + currentHeight - 6, 0.5f);
        clickObjectMask = currentPlatform.transform.GetChild(1).gameObject;
        currentPlatform.transform.DOMoveY(oldPlatformHeight + currentHeight - 4, 0).OnComplete(() =>
        {
            clickObjectMask.transform.parent = null;
        });
        targetObjectsList[0].GetComponent<StopDoPath>().myPathTween.Play();

        var currentObjectUnlocked = currentLevel * 3 + CurrentTask;
        //Debug.LogError(currentObjectUnlocked);
        var totalObject = mapBG.transform.childCount;
        for (int i = totalObject - 2; i >= currentObjectUnlocked; i--)
        {
            mapBG.transform.GetChild(i).gameObject.SetActive(false);
            if (currentObjectUnlocked == 0)
            {
                nextGameObject = mapBG.transform.GetChild(0).gameObject;
                nextGameObject.SetActive(false);
            }
            else if (i == currentObjectUnlocked)
            {

                nextGameObject = mapBG.transform.GetChild(i).gameObject;
                nextGameObject.SetActive(false);
            }
        }
        //if (currentObjectUnlocked >= 1)
        //{
        //    GameObject newObject = mapBG.transform.GetChild(currentObjectUnlocked - 1).gameObject;
        //    Vector3 originScale = newObject.transform.localScale;
        //    newObject.transform.localScale = originScale * 0.01f;
        //    newObject.transform.DOScale(originScale, 5);
            //Instantiate(shinyEffect, new Vector3(newObject.transform.position.x, newObject.transform.position.y + 1f, newObject.transform.position.z), Quaternion.identity);
        //}

        yield return new WaitForSeconds(1);
        levelBonusTitle.transform.DOLocalMoveX(1000, 0.5f);
        yield return new WaitForSeconds(0.5f);
        levelBonusTitle.SetActive(false);
    }

    public void NextChance()
    {
        endTimer = Time.unscaledTime;
        isTimeLevel = false;
        taskDuration = endTimer - startTimer;
        StopAllCoroutines();
        timer.transform.parent.gameObject.SetActive(false);
        isStartGame = true;

        foreach (var item in targetObjectsList)
        {
            item.GetComponent<Rigidbody>().isKinematic = true;
        }
        chanceCount--;
        if (chanceCount > 0)
        {
            currentHeight = oldHeight * chanceCount;
            foreach (var item in targetObjectsList)
            {
                item.transform.DOMoveY(currentHeight, 0);
            }
            Camera.main.transform.DOMoveY(15 + currentHeight - 6, 0.5f);
            //if (chanceCount != 1)
            clickObjectMask.transform.parent = currentPlatform.transform;
            currentPlatform.transform.DOMoveY(oldPlatformHeight + currentHeight - 4, 0).OnComplete(() =>
            {
                clickObjectMask.transform.parent = null;
            }); ;

            if (chanceCount == 1)
            {
                currentPlatform.tag = "Lose";
                //currentPlatform.GetComponent<MeshRenderer>().enabled = false;
            }
            //else
            //    Destroy(currentPlatform.gameObject);

            foreach (var item in listRopes)
            {
                item.gameObject.SetActive(false);
                //item.transform.GetChild(0).GetComponent<ObiSolver>().stretchShearConstraintParameters.iterations = 10;
                //// listRopes[i].transform.GetChild(0).GetComponent<ObiSolver>().worldLinearInertiaScale = 0.01f;
                //// listRopes[i].transform.GetChild(0).GetComponent<ObiFixedUpdater>().substeps = 3;
                //item.transform.GetChild(0).GetComponent<ObiSolver>().UpdateParameters();
            }
            listRopes.Clear();

            for (int i = 0; i < numOfRope; i++)
            {
                GameObject ropeSpawn;
                ropeSpawn = Instantiate(ropePrefab);
                ropeSpawn.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material = ShopControl.Instance.currentRopeMaterial;
                for (int j = 1; j < ropeSpawn.transform.childCount; j++)
                {
                    ropeSpawn.transform.GetChild(j).GetComponent<Renderer>().material = ShopControl.Instance.currentPillarMaterial;
                }
                listRopes.Add(ropeSpawn.transform);
            }
            isStartGame = true;
            isControl = true;
            isUpdatedRopes = false;
        }
        else
        {
            Lose();
        }
        ropeID = 0;
    }

    IEnumerator delayMissionText()
    {
        var randomStyle = Random.Range(0, 10);
        if (randomStyle < 5)
        {
            missionText.SetActive(true);
            missionText.transform.DOScale(Vector3.one * 1.2f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            missionText.transform.DOScale(Vector3.one, 0.2f);
            yield return new WaitForSeconds(2);
            missionText.transform.DOScale(Vector3.one * 1.2f, 0.2f);
            yield return new WaitForSeconds(0.2f);
            missionText.transform.DOScale(Vector3.one * 0.01f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            missionText.SetActive(false);
        }
        else
        {
            missionText.SetActive(true);
            missionText.transform.localScale = Vector3.one;
            missionText.GetComponent<RectTransform>().DOLocalMoveX(-1000, 0);
            missionText.GetComponent<RectTransform>().DOLocalMoveX(100, 0.5f);
            yield return new WaitForSeconds(0.5f);
            missionText.GetComponent<RectTransform>().DOLocalMoveX(0, 0.2f);
            yield return new WaitForSeconds(2);
            missionText.GetComponent<RectTransform>().DOLocalMoveX(-100, 0.2f);
            yield return new WaitForSeconds(0.2f);
            missionText.GetComponent<RectTransform>().DOLocalMoveX(1000, 0.5f);
            yield return new WaitForSeconds(0.5f);
            missionText.SetActive(false);
        }
    }

    private void Update()
    {
        if (isShake)
        {
            Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime * shakeSpeed);
            rigid.MoveRotation(rigid.rotation * deltaRotation);
        }

        if (isStartGame && isControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseDown();
            }

            if (Input.GetMouseButton(0))
            {
                OnMouseDrag();
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnMouseUp();
            }
        }
        else if (!isStartGame && isControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ButtonStartGame();
                OnMouseDown();
            }
        }

        //if (isDioramaMode)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        fh = Input.GetAxis("Mouse X") * speed;
        //        fv = Input.GetAxis("Mouse Y") * speed;
        //    }

        //    if (Input.GetMouseButton(0))
        //    {
        //        OnMouseDragDiorama();
        //    }

        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        isDrag = false;
        //    }
        //}
    }

    public int speed;
    float fh;
    float fv;
    float h;
    float v;

//    void OnMouseDragDiorama()
//    {
//#if UNITY_EDITOR
//        h = Input.GetAxis("Mouse X") * speed;
//        v = Input.GetAxis("Mouse Y") * speed;
//        if (Mathf.Abs(h - fh) > 0.25f)
//        {
//            isDrag = true;
//        }
//        if (Mathf.Abs(v - fv) > 0.25f)
//        {
//            isDrag = true;
//        }
//#endif
//#if UNITY_IOS
//        if (Input.touchCount > 0)
//        {
//            h = Input.touches[0].deltaPosition.x / 8;
//            v = Input.touches[0].deltaPosition.y / 8;
//            isDrag = true;
//        }
//#endif
//        if (isDrag)
//        {
//            var rot = new Vector3(v, -h, 0);
//            // rigid.AddTorque(rot, ForceMode.VelocityChange);
//            dioramaMap.transform.Rotate(0, -h, 0);
//        }
//    }

    // IEnumerator delayControl()
    // {
    //     isControl = false;
    //     yield return new WaitForSeconds(0.5f);
    //     isControl = true;
    // }

    public bool isAnimationMarkRope;
    Coroutine TimeAnimationMarkRope;
    float timeAnimation = 0;
    private IEnumerator C_TimeAnimationMarkRope()
    {
        isAnimationMarkRope = true;
        while (timeAnimation > 0)
        {
            timeAnimation -= 0.03f;
            yield return null;
        }
        // Debug.LogError("true");
        isAnimationMarkRope = false;
        TimeAnimationMarkRope = null;
    }

    void OnMouseDown()
    {
        if (ShopControl.Instance.isShopping) return;

        // if (isAnimationMarkRope) return;

        if (!isDrag && ropeID < listRopes.Count)
        {
            //Debug.LogError("Drag");
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);

            Vector3 startPos = Vector3.zero;
            //if (isTutorial)
            //{
            //    Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    RaycastHit hit1;

            //    if (Physics.Raycast(ray1, out hit1, Mathf.Infinity, layer))
            //    {
            //        if (hit1.collider.gameObject.CompareTag("Tu1"))
            //        {
            //            startPos = hit1.collider.gameObject.transform.position;
            //        }
            //        else
            //        {
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}

            // if (isBonusLevel && ropeID >= listRopes.Count)
            // {
            //     var rope = Instantiate(ropePrefab);
            //     listRopes.Add(rope.transform);
            // }
            startPoint = listRopes[ropeID].transform.GetChild(1);
            endPoint = listRopes[ropeID].transform.GetChild(2);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, clickMask))
            {
                isDrag = true;
                listRopes[ropeID].transform.DOKill();
                listRopes[ropeID].transform.position = hit.point;
                startPoint.transform.position = hit.point;
                endPoint.transform.position = hit.point;
                //if (isTutorial) startPoint.transform.position = new Vector3(startPos.x, startPoint.transform.position.y, startPos.z);
                listRopes[ropeID].gameObject.SetActive(true);
                var mark = Instantiate(markPrefab);
                mark.transform.GetComponent<Renderer>().material = ShopControl.Instance.currentPillarMaterial;
                mark.transform.position = new Vector3(startPoint.position.x, (startPoint.transform.position.y + currentPlatform.transform.position.y) / 2 - 0.5f, startPoint.position.z);
                var effect = Instantiate(markSlamEffect);
                effect.SetActive(true);
                effect.transform.position = new Vector3(mark.transform.localPosition.x, mark.transform.localPosition.y - 3, mark.transform.localPosition.z);
                var tempPoint = startPoint;
                mark.transform.DOScaleY(1.75f, 0.3f).OnComplete(() =>
                {
                    //mark.transform.DOScaleY(6.5f, 0.2f).SetLoops(2, LoopType.Yoyo);
                    //tempPoint.transform.DOMoveY(tempPoint.transform.position.y + 1, 0.2f).SetLoops(2, LoopType.Yoyo);
                    //mark.transform.DOScaleX(0.7f, 0.2f).SetLoops(2, LoopType.Yoyo);
                    //mark.transform.DOScaleZ(0.7f, 0.2f).SetLoops(2, LoopType.Yoyo);
                    //tempPoint.transform.DOScale(Vector3.one * 0.7f, 0.2f).SetLoops(2, LoopType.Yoyo);
                });
                mark.transform.parent = listRopes[ropeID].transform;
                rope = listRopes[ropeID].transform.GetChild(0).transform.GetChild(0).GetComponent<ObiRod>();
                StartCoroutine(C_TimeAnimationMarkRope());
                if (TimeAnimationMarkRope == null)
                {
                    timeAnimation = 1;
                    TimeAnimationMarkRope = StartCoroutine(C_TimeAnimationMarkRope());
                }
                else
                {
                    timeAnimation = 1;
                }
            }
        }
    }

    Vector3 currentMousePos;
    Vector3 lastMousePos;

    void OnMouseDrag()
    {
        if (ShopControl.Instance.isShopping) return;

        if (isDrag)
        {
            if (listRopes[ropeID] != null)
            {
                endPoint = listRopes[ropeID].transform.GetChild(2);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000, dragMask))
                {
                    endPoint.transform.position = hit.point;
                }

                currentMousePos = Input.mousePosition;
                Vector3 delta = lastMousePos - currentMousePos;
                lastMousePos = currentMousePos;
                if (delta != Vector3.zero) Vibration();

                float checkDrag = Vector3.Distance(startPoint.position, endPoint.position);
                if (checkDrag > 0.5f)
                {
                    listRopes[ropeID].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                }

                Vector3 dir = endPoint.position - startPoint.position;
                // if (isBonusLevel)
                // {
                //     var distance = Vector3.Distance(endPoint.position, startPoint.position);
                //     listRopes[ropeID].transform.GetChild(0).transform.localScale = new Vector3(1, 1, distance);
                //     listRopes[ropeID].transform.GetChild(0).transform.rotation = Quaternion.LookRotation(startPoint.position - endPoint.position);
                // }


                if (dir != Vector3.zero)
                    startPoint.transform.rotation = Quaternion.LookRotation(endPoint.position - startPoint.position);

                dir = startPoint.position - endPoint.position;

                if (dir != Vector3.zero)
                    endPoint.transform.rotation = Quaternion.LookRotation(startPoint.position - endPoint.position);
            }
        }
    }

    void OnMouseUp()
    {
        if (ShopControl.Instance.isShopping) return;

        if (isDrag)
        {
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);

            //if (isTutorial)
            //{
            //    Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    RaycastHit hit1;

            //    if (Physics.Raycast(ray1, out hit1, Mathf.Infinity, layer))
            //    {
            //        if (hit1.collider.gameObject.CompareTag("Tu2"))
            //        {
            //            //var mark = Instantiate(markPrefab);
            //            //mark.transform.GetComponent<Renderer>().material = ShopControl.Instance.currentPillarMaterial;

            //            Vector3 a = hit1.collider.gameObject.transform.position;
            //            endPoint.transform.position = new Vector3(a.x, endPoint.transform.position.y, a.z);

            //            endPoint.position = new Vector3(Mathf.Clamp(endPoint.position.x, -5f, 5f), endPoint.position.y, Mathf.Clamp(endPoint.position.z, -2.5f, 8.5f));

            //            if (dir != Vector3.zero)
            //                startPoint.transform.rotation = Quaternion.LookRotation(endPoint.position - startPoint.position);

            //            dir = startPoint.position - endPoint.position;

            //            if (dir != Vector3.zero)
            //                endPoint.transform.rotation = Quaternion.LookRotation(startPoint.position - endPoint.position);

            //            //mark.transform.position = new Vector3(endPoint.position.x, mark.transform.position.y, endPoint.position.z);
            //            //var effect = Instantiate(markSlamEffect);
            //            //effect.SetActive(true);
            //            //effect.transform.position = new Vector3(mark.transform.localPosition.x, -3, mark.transform.localPosition.z);
            //            //var tempPoint = endPoint;
            //            //mark.transform.DOScaleY(5.5f, 0.3f).OnComplete(() =>
            //            //{
            //            //    // mark.transform.DOScaleY(6.5f, 0.2f).SetLoops(2, LoopType.Yoyo);
            //            //    // tempPoint.transform.DOMoveY(tempPoint.transform.position.y + 1, 0.2f).SetLoops(2, LoopType.Yoyo);
            //            //    // mark.transform.DOScaleX(0.7f, 0.2f).SetLoops(2, LoopType.Yoyo);
            //            //    // mark.transform.DOScaleZ(0.7f, 0.2f).SetLoops(2, LoopType.Yoyo);
            //            //    // tempPoint.transform.DOScale(Vector3.one * 0.7f, 0.2f).SetLoops(2, LoopType.Yoyo);
            //            //});
            //            //mark.transform.parent = listRopes[ropeID].transform;
            //            if (TimeAnimationMarkRope == null)
            //            {
            //                TimeAnimationMarkRope = StartCoroutine(C_TimeAnimationMarkRope());
            //                timeAnimation = 1;
            //            }
            //            else
            //            {
            //                timeAnimation += 1;
            //            }

            //            if (isBonusLevel)
            //            {
            //                newRopeLeftText.text = (8 - (ropeID + 1)) + "/" + 8;
            //            }
            //            else
            //            {
            //                newRopeLeftText.text = (6 - (ropeID + 1)) + "/" + 5;
            //            }
            //            newropeAnim.SetTrigger("bubble");

            //            if (ropeID <= listRopeFill.Count - 1)
            //            {


            //                listRopeFill[ropeID].DOFillAmount(0, 1f);
            //                listRopeFill[ropeID].transform.parent.DOScale(Vector3.one * 1.25f, 0.5f).SetLoops(2, LoopType.Yoyo);
            //                dropButton.SetActive(true);

            //                if (isTutorial == false)
            //                    dropdownAnim.enabled = false;

            //                // undoButton.SetActive(true);
            //                if (ropeID == listRopeFill.Count - 1)
            //                {
            //                    // handClickDrop.transform.parent = inGameMenu.transform;
            //                    // handClickDrop.SetActive(true);
            //                    // handClickDrop.transform.position = new Vector3(dropButton.transform.position.x + 15, dropButton.transform.transform.position.y - 40, dropButton.transform.position.z);
            //                    // handClickDrop.transform.DOScale(Vector3.one * 1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);

            //                    dropButton.transform.eulerAngles = new Vector3(0, 0, -10);
            //                    dropButton.transform.DOPunchRotation(new Vector3(0, 0, 20), 0.5f, 5).SetLoops(-1, LoopType.Yoyo);
            //                }
            //            }
            //            if (ropeID >= 4)
            //            {
            //                ropeLeft.SetActive(true);
            //                string b = "LAST ONE";
            //                ropeLeft.transform.GetChild(0).gameObject.GetComponent<Text>().text = b;
            //                Invoke("HideRopeLeft", 1.5f);
            //            }
            //            else
            //            {
            //                CancelInvoke("HideRopeLeft");
            //                ropeLeft.SetActive(false);
            //                ropeLeft.SetActive(true);
            //                int r = 5 - ropeID;
            //                string c = r + " ROPES LEFT";
            //                ropeLeft.transform.GetChild(0).gameObject.GetComponent<Text>().text = c;
            //                Invoke("HideRopeLeft", 1.5f);
            //            }
            //            // listRopes[ropeID].transform.GetChild(0).GetComponent<ObiSolver>().stretchShearConstraintParameters.iterations = 4;
            //            ropeID++;
            //            SetPointTutorial();
            //        }
            //        else
            //        {
            //            var temp = listRopes[ropeID];
            //            GameObject ropeSpawn;
            //            // if (isBonusLevel)
            //            // {
            //            //     ropeSpawn = Instantiate(ropeCustomPrefab);
            //            // }
            //            // else
            //            // {
            //            ropeSpawn = Instantiate(ropePrefab);
            //            ropeSpawn.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material = ShopControl.Instance.currentRopeMaterial;
            //            for (int j = 1; j < ropeSpawn.transform.childCount; j++)
            //            {
            //                ropeSpawn.transform.GetChild(j).GetComponent<Renderer>().material = ShopControl.Instance.currentPillarMaterial;
            //            }
            //            // }
            //            listRopes[ropeID] = ropeSpawn.transform;
            //            Destroy(temp.gameObject);
            //        }
            //    }
            //    else
            //    {
            //        var temp = listRopes[ropeID];
            //        GameObject ropeSpawn;
            //        // if (isBonusLevel)
            //        // {
            //        //     ropeSpawn = Instantiate(ropeCustomPrefab);
            //        // }
            //        // else
            //        // {
            //        ropeSpawn = Instantiate(ropePrefab);
            //        for (int j = 1; j < ropeSpawn.transform.childCount; j++)
            //        {
            //            ropeSpawn.transform.GetChild(j).GetComponent<Renderer>().material = ShopControl.Instance.currentPillarMaterial;
            //        }
            //        ropeSpawn.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material = ShopControl.Instance.currentRopeMaterial;

            //        // }
            //        listRopes[ropeID] = ropeSpawn.transform;
            //        Destroy(temp.gameObject);
            //    }
            //}
            //else
            //{
                float checkDrag = Vector3.Distance(startPoint.position, endPoint.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000, clickMask))
                {
                    if (checkDrag > 2f)
                    {
                    var mark = Instantiate(markPrefab);
                    mark.transform.GetComponent<Renderer>().material = ShopControl.Instance.currentPillarMaterial;
                    mark.transform.position = new Vector3(endPoint.position.x, (endPoint.transform.position.y + currentPlatform.transform.position.y) / 2 - 0.5f, endPoint.position.z);
                    var effect = Instantiate(markSlamEffect);
                    effect.SetActive(true);
                    effect.transform.position = new Vector3(mark.transform.localPosition.x, mark.transform.localPosition.y - 3, mark.transform.localPosition.z);
                    var tempPoint = endPoint;
                    mark.transform.DOScaleY(1.75f, 0.3f).OnComplete(() =>
                    {
                            // mark.transform.DOScaleY(6.5f, 0.2f).SetLoops(2, LoopType.Yoyo);
                            // tempPoint.transform.DOMoveY(tempPoint.transform.position.y + 1, 0.2f).SetLoops(2, LoopType.Yoyo);
                            // mark.transform.DOScaleX(0.7f, 0.2f).SetLoops(2, LoopType.Yoyo);
                            // mark.transform.DOScaleZ(0.7f, 0.2f).SetLoops(2, LoopType.Yoyo);
                            // tempPoint.transform.DOScale(Vector3.one * 0.7f, 0.2f).SetLoops(2, LoopType.Yoyo);
                        });
                    mark.transform.parent = listRopes[ropeID].transform;
                    if (TimeAnimationMarkRope == null)
                        {
                            TimeAnimationMarkRope = StartCoroutine(C_TimeAnimationMarkRope());
                            timeAnimation = 1;
                        }
                        else
                        {
                            timeAnimation += 1;
                        }
                        if (isBonusLevel)
                        {
                            newRopeLeftText.text = (5 - (ropeID + 1)) + "/" + 5;
                        }
                        else
                        {
                            newRopeLeftText.text = (5 - (ropeID + 1)) + "/" + 5;
                        }
                        newropeAnim.SetTrigger("bubble");

                        if (ropeID <= listRopeFill.Count - 1)
                        {


                            listRopeFill[ropeID].DOFillAmount(0, 1f);
                            listRopeFill[ropeID].transform.parent.DOScale(Vector3.one * 1.25f, 0.5f).SetLoops(2, LoopType.Yoyo);
                            dropButton.SetActive(true);

                            //if (isTutorial == false)
                            //    dropdownAnim.enabled = false;

                            // undoButton.SetActive(true);
                            if (ropeID == listRopeFill.Count - 1)
                            {
                                handClickDrop.transform.SetParent(inGameMenu.transform);
                                handClickDrop.SetActive(true);
                                handClickDrop.transform.position = new Vector3(dropButton.transform.position.x + 15, dropButton.transform.transform.position.y - 50, dropButton.transform.position.z);
                                handClickDrop.transform.DOScale(Vector3.one * 1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                                dropButton.transform.eulerAngles = new Vector3(0, 0, -10);
                                dropButton.transform.DOPunchRotation(new Vector3(0, 0, 20), 0.5f, 5).SetLoops(-1, LoopType.Yoyo);
                            }
                        }
                        if (ropeID == 4)
                        {
                            ropeLeft.SetActive(true);
                            string b = "LAST ONE";
                            ropeLeft.transform.GetChild(0).gameObject.GetComponent<Text>().text = b;
                            Invoke("HideRopeLeft", 1.5f);
                        }
                        else
                        {
                            CancelInvoke("HideRopeLeft");
                            ropeLeft.SetActive(false);
                            ropeLeft.SetActive(true);
                            int r = 5 - ropeID;
                            string c = r + " ROPES LEFT";
                            ropeLeft.transform.GetChild(0).gameObject.GetComponent<Text>().text = c;
                            Invoke("HideRopeLeft", 1.5f);
                        }
                        // listRopes[ropeID].transform.GetChild(0).GetComponent<ObiSolver>().stretchShearConstraintParameters.iterations = 4;
                        ropeID++;
                    }
                    else
                    {
                        var temp = listRopes[ropeID];
                        GameObject ropeSpawn;
                        // if (isBonusLevel)
                        // {
                        //     ropeSpawn = Instantiate(ropeCustomPrefab);
                        // }
                        // else
                        // {
                        ropeSpawn = Instantiate(ropePrefab);
                        ropeSpawn.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material = ShopControl.Instance.currentRopeMaterial;
                        for (int j = 1; j < ropeSpawn.transform.childCount; j++)
                        {
                            ropeSpawn.transform.GetChild(j).GetComponent<Renderer>().material = ShopControl.Instance.currentPillarMaterial;
                        }
                        // }
                        listRopes[ropeID] = ropeSpawn.transform;
                        Destroy(temp.gameObject);
                    }
                }
                else
                {
                    var temp = listRopes[ropeID];
                    GameObject ropeSpawn;
                    // if (isBonusLevel)
                    // {
                    //     ropeSpawn = Instantiate(ropeCustomPrefab);
                    // }
                    // else
                    // {
                    ropeSpawn = Instantiate(ropePrefab);
                    ropeSpawn.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material = ShopControl.Instance.currentRopeMaterial;
                    for (int j = 1; j < ropeSpawn.transform.childCount; j++)
                    {
                        ropeSpawn.transform.GetChild(j).GetComponent<Renderer>().material = ShopControl.Instance.currentPillarMaterial;
                    }
                    // }
                    listRopes[ropeID] = ropeSpawn.transform;
                    Destroy(temp.gameObject);
                }
            //}
            // StartCoroutine(delayControl());
            isDrag = false;
        }
    }

    public Animator newropeAnim;

    IEnumerator AutoCreateRope(Vector3 startPos, Vector3 endPos)
    {
        // yield return new WaitForSeconds(0.2f);
        startPoint = listRopes[ropeID].transform.GetChild(1);
        endPoint = listRopes[ropeID].transform.GetChild(2);
        listRopes[ropeID].transform.position = startPos;
        startPoint.transform.position = new Vector3(startPos.x, -2, startPos.z);
        endPoint.transform.position = startPoint.transform.position;
        listRopes[ropeID].gameObject.SetActive(true);
        var mark = Instantiate(markPrefab);
        mark.transform.GetComponent<Renderer>().material = ShopControl.Instance.currentPillarMaterial;
        mark.transform.position = new Vector3(startPoint.position.x, mark.transform.position.y, startPoint.position.z);
        mark.transform.DOScaleY(5.5f, 0.1f);
        mark.transform.parent = listRopes[ropeID].transform;
        rope = listRopes[ropeID].transform.GetChild(0).transform.GetChild(0).GetComponent<ObiRod>();

        yield return new WaitForSeconds(0.1f);
        endPoint = listRopes[ropeID].transform.GetChild(2);
        endPoint.transform.DOMove(new Vector3(endPos.x, -2, endPos.z), 0.1f);
        float checkDrag = Vector3.Distance(startPoint.position, endPoint.position);
        endPoint.position = new Vector3(Mathf.Clamp(endPoint.position.x, -5f, 5f), endPoint.position.y, Mathf.Clamp(endPoint.position.z, -2.5f, 8.5f));
        if (checkDrag > 0.5f)
        {
            listRopes[ropeID].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.1f);
        startPoint.transform.rotation = Quaternion.LookRotation(endPoint.position - startPoint.position);
        endPoint.transform.rotation = Quaternion.LookRotation(startPoint.position - endPoint.position);
        var mark2 = Instantiate(markPrefab);
        mark2.transform.GetComponent<Renderer>().material = ShopControl.Instance.currentPillarMaterial;
        mark2.transform.position = new Vector3(endPoint.position.x, mark2.transform.position.y, endPoint.position.z);
        mark2.transform.DOScaleY(5.5f, 0.1f);
        mark2.transform.parent = listRopes[ropeID].transform;
        if (isBonusLevel)
        {
            newRopeLeftText.text = (5 - (ropeID + 1)) + "/" + 5;
        }
        else
        {
            newRopeLeftText.text = (5 - (ropeID + 1)) + "/" + 5;
        }
        newropeAnim.SetTrigger("bubble");
        if (ropeID <= listRopeFill.Count - 1)
        {


            listRopeFill[ropeID].DOFillAmount(0, 1f);
            if (ropeID == listRopeFill.Count - 1)
            {
                dropButton.transform.eulerAngles = new Vector3(0, 0, -10);
                dropButton.transform.DOPunchRotation(new Vector3(0, 0, 20), 0.5f, 5).SetLoops(-1, LoopType.Yoyo);
            }
        }
        ropeID++;
        //SetPointTutorial();
    }

    private void Vibration()
    {
        if (isVibration) return;
        StartCoroutine(C_Vibration());
    }
    bool isVibration;
    private IEnumerator C_Vibration()
    {
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        isVibration = true;
        yield return new WaitForSeconds(0.2f);
        isVibration = false;
    }

    public void PlusEffectMethod()
    {
        if (maxPlusEffect < 10)
        {
            Vector3 posSpawn = timer.transform.position;
            StartCoroutine(PlusEffect(posSpawn));
        }
    }

    IEnumerator PlusEffect(Vector3 pos)
    {
        maxPlusEffect++;
        if (!UnityEngine.iOS.Device.generation.ToString().Contains("5") && !isVibrate)
        {
            isVibrate = true;
            StartCoroutine(delayVibrate());
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        }
        var plusVar = Instantiate(plusVarPrefab);
        plusVar.transform.SetParent(canvas.transform);
        plusVar.transform.localScale = new Vector3(1, 1, 1);
        //plusVar.transform.position = worldToUISpace(canvas, pos);
        plusVar.transform.position = new Vector3(pos.x + Random.Range(-50, 50), pos.y + Random.Range(-100, -75), pos.z);
        plusVar.GetComponent<Text>().DOColor(new Color32(255, 255, 255, 0), 1f);
        plusVar.SetActive(true);
        plusVar.transform.DOMoveY(plusVar.transform.position.y + Random.Range(50, 90), 0.5f);
        plusVar.transform.DOMoveX(timer.transform.position.x, 0.5f);
        Destroy(plusVar, 0.5f);
        yield return new WaitForSeconds(0.01f);
        maxPlusEffect--;
    }

    IEnumerator delayVibrate()
    {
        yield return new WaitForSeconds(0.2f);
        isVibrate = false;
    }

    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        return parentCanvas.transform.TransformPoint(movePos);
    }

    public void ButtonStartGame()
    {
        startGameMenu.SetActive(false);
        // buttonLevel.SetActive(false);
        isStartGame = true;
        isControl = true;
    }

    public GameObject starParent;
    public GameObject burstConfetti;
    public string[] winString;

    IEnumerator Win()
    {
        ropeUsed = ropeID;
        endTimer = Time.unscaledTime;
        taskDuration = endTimer - startTimer;
        // GameAnalyticsManager.Instance.Log_TaskDuration(taskDuration);

        // Debug.Log("Win");

        //if (currentLevel == 0 && CurrentTask == 0)
        //{
        //    PlayerPrefs.SetInt("tutorial", 1);
        //}

        isTimeLevel = false;
        var timePanel = timer.transform.parent;
        timePanel.gameObject.SetActive(true);
        timePanel.transform.GetChild(1).gameObject.SetActive(true);

        handWatch.DORotate(new Vector3(0f, 0f, -360f), 3f, RotateMode.FastBeyond360).SetEase(Ease.Flash);

        fill.fillAmount = 1;
        fill.DOFillAmount(0, 3f).SetEase(Ease.Flash).OnComplete(() =>
        {
            if (isStartGame)
            {
                //confetti.gameObject.SetActive(true);
                //confetti.Stop();
                //confetti.Play();
            }
        });

        //Timer CountDown

        int countDown = 3;

        timer.text = countDown.ToString();
        timer.transform.DOScale(Vector3.one * 1.5f, 0.2f).OnComplete(() =>
        {
            timer.transform.DOScale(Vector3.one, 0.4f);
        });
        var cdSeq = DOTween.Sequence();
        cdSeq.AppendInterval(1).AppendCallback(() =>
        {
            countDown -= 1; timer.text = countDown.ToString();
        }).SetLoops(3).OnStepComplete(() =>
        {
            timer.transform.DOScale(Vector3.one * 1.5f, 0.2f).OnComplete(() =>
            {
                timer.transform.DOScale(Vector3.one, 0.4f);
            });
        }).Play();

        yield return new WaitForSeconds(countDown);


        timePanel.gameObject.SetActive(false);
        if (isStartGame)
        {
            var star = PlayerPrefs.GetInt("currentStar");
            // Debug.Log("Win");
            isStartGame = false;
            losePanel.SetActive(false);
            status.text = "DONE";
            TimeLevel = 0;
            // star1.transform.parent.GetComponent<Animator>().enabled = false;
            // star2.transform.parent.GetComponent<Animator>().enabled = false;
            // star3.transform.parent.GetComponent<Animator>().enabled = false;
            yield return new WaitForSeconds(0.1f);

            // if (CurrentTask <= 2)
            // {
            listTask[CurrentTask].DOFillAmount(1, 0.5f);

            ropeLeft.SetActive(true);
            int r = 5 - ropeID;
            string a = r + " ROPES LEFT";
            ropeLeft.transform.GetChild(0).gameObject.GetComponent<Text>().text = a;
            yield return new WaitForSeconds(0.5f);
            ropeLeft.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>().DOFade(0, 0.5f);
            Invoke("HideRopeLeft", 0);
            // }

            if (ropeID > 5)
            {
                ropeID = 5;
            }

            // yield return new WaitForSeconds(0.2f);

            //Hất object bay đi
            burstConfetti.SetActive(true);
            burstConfetti.transform.DOMoveY(currentPlatform.transform.position.y + 2, 0);
            if (!isBonusLevel)
            {
                foreach (var item in listRopes)
                {
                    item.transform.DOMoveY(item.transform.position.y - 1, 0.25f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutSine);
                }
            }
            yield return new WaitForSeconds(0.25f);
            foreach (var item in targetObjectsList)
            {
                if (!item.CompareTag("Pendulum"))
                {
                    item.GetComponent<Rigidbody>().isKinematic = true;
                }
                else
                {
                    item.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = true;
                }
                //item.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 250), 500, Random.Range(0, 250)));
                item.transform.DOLocalMove(new Vector3(Random.Range(-20, 20), 60, Random.Range(-20, 20)), 2);

                //item.SetActive(false);
                if (!isBonusLevel)
                {
                    var explosionSpawn = Instantiate(explosionConfetti);
                    explosionSpawn.transform.position = item.transform.position;
                    explosionSpawn.SetActive(true);
                }
                else
                {
                    var explosionSpawn = Instantiate(gemExplode);
                    explosionSpawn.transform.position = item.transform.position;
                    explosionSpawn.SetActive(true);
                }
                //MaskController.Instance.FillBlueColorGround(new Vector3(0, 0, 2.0f));

                Destroy(item, 4.1f);
            }

            //currentPlatform.transform.parent.DOScale(Vector3.one * 1.25f, 0.35f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
            //{
            //    currentPlatform.transform.parent.DOScale(Vector3.one * 1.1f, 0.35f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
            //    {
            //        currentPlatform.transform.parent.DOMove(currentPlatform.transform.position - Vector3.up * 5.0f, 0.25f);
            //        currentPlatform.transform.parent.DOScale(Vector3.zero, 0.35f);
            //    });
            //});

            //if (!isBonusLevel)
            //{
            //    yield return new WaitForSeconds(0.75f);
            //    for (int i = 0; i < listRopes.Count; i++)
            //    {
            //        listRopes[i].transform.GetChild(0).gameObject.SetActive(false);
            //        listRopes[i].transform.DOMoveY(-10, 0.5f);
            //    }
            //}

            //if (CurrentTask >= 2)
            //{
            //    isShowDiorama = true;
            //}
            yield return new WaitForSeconds(0.4f);
            burstConfetti.SetActive(false);
            currentPlatform.SetActive(false);
            Camera.main.transform.DOMoveY(39, 0.5f);
            for (int i = 0; i < listRopes.Count; i++)
            {
                listRopes[i].transform.gameObject.SetActive(false);
            }
            burstConfetti.SetActive(false);
            yield return new WaitForSeconds(0.01f);

            //Show new object
            nextGameObject.GetComponent<MeshCollider>().enabled = false;
            var originalPos = nextGameObject.transform.localPosition;
            yield return new WaitForSeconds(0.01f);
            nextGameObject.transform.DOLocalMoveY(15, 0);
            nextGameObject.SetActive(true);
            yield return new WaitForSeconds(0.01f);
            nextGameObject.transform.DOKill();
            nextGameObject.transform.DOLocalMoveY(originalPos.y, 2);
            var effect = Instantiate(highlightEffect, new Vector3(nextGameObject.transform.position.x, originalPos.y, nextGameObject.transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(1);
            Instantiate(shinyEffect, new Vector3(nextGameObject.transform.position.x, nextGameObject.transform.position.y + 1f, nextGameObject.transform.position.z), Quaternion.identity);

            yield return new WaitForSeconds(3);

            //if (!isBonusLevel)
            //{
                yesObject.transform.GetChild(0).GetComponent<Text>().text = winString[Random.Range(0, winString.Length)];
                yesObject.SetActive(true);
                winPanel.SetActive(true);
                winPanelPopup.transform.DOLocalMoveX(-1000, 0);
                yield return new WaitForSeconds(0.5f);
                winPanelPopup.SetActive(true);
                winPanelPopup.transform.DOLocalMoveX(0, 0.25f).SetEase(Ease.OutSine);
                // winText.text = "WORLD " + pointerLevel.ToString() + "\n" + "COMPLETED!";
                yield return new WaitForSeconds(0.35f);
                winText.SetActive(true);
                winText.transform.DOScale(Vector3.one * 1.2f, 0.3f).OnComplete(() =>
                {
                    winText.transform.DOScale(Vector3.one, 0.25f);
                });
                yield return new WaitForSeconds(0.15f);
                winRopeLeftText.text = a;
                winRopeLeftText.gameObject.SetActive(true);
                winRopeLeftText.transform.DOScale(Vector3.one * 1.2f, 0.3f).OnComplete(() =>
                {
                    winRopeLeftText.transform.DOScale(Vector3.one, 0.25f);
                });
                var screenToWorldPosition = Camera.main.ScreenToWorldPoint(winRopeLeftText.rectTransform.transform.position);
                starExplode.transform.position = new Vector3(screenToWorldPosition.x, starExplode.transform.position.y, starExplode.transform.position.z);
                starExplode.SetActive(true);
                yield return C_Rating();
                yield return new WaitForSeconds(0.02f);
                if (CurrentTask >= 2)
                {
                    LoadScene();
                    LevelUp();
                }
                else
                {
                    winNext.SetActive(true);
                    dropButton.SetActive(false);
                    winRestart.transform.DOScale(Vector3.one, 0.35f);
                    yield return new WaitForSeconds(0.05f);
                    winRestart.SetActive(true);
                    winNext.transform.DOScale(Vector3.one, 0.35f);
                }
                // winLightingEffect.transform.DOScale(Vector3.zero, 0);
                winLightingEffect.SetActive(true);
                winLightingEffect.transform.GetChild(0).DOScale(Vector3.one, .45f);
                winLightingEffect.transform.GetChild(1).DOScale(Vector3.one, .45f);
                winLightingEffect.transform.GetChild(2).DOScale(Vector3.one, .45f);

                //winPanelPopup.transform.GetChild(0).transform.parent = winPanel.transform;
                //winPanelPopup.SetActive(false);               
            //}
            //else
            //{
            //    // yesObject.transform.GetChild(0).GetComponent<Text>().text = winString[Random.Range(0, winString.Length)];
            //    // yesObject.SetActive(true);
            //    // burstConfetti.SetActive(true);
            //    // AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.CompleteEvent);
            //    // AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.EndEvent);

            //    gemScore *= 10;
            //    int sumscore = gemScore;
            //    gemScoring.GetComponent<Text>().text = sumscore.ToString();
            //    gemScoring.SetActive(true);
            //    gemScoring.transform.DOLocalMoveY(500, 0.2f);
            //    for (int i = 0; i < listRopes.Count; i++)
            //    {
            //        listRopes[i].transform.GetChild(0).gameObject.SetActive(false);
            //        listRopes[i].transform.DOMoveY(-10, 0.5f);
            //    }
            //    yield return new WaitForSeconds(2f);

            //    if (sumscore > 0)
            //    {
            //        gemExplode.SetActive(true);
            //        gemAnimation.SetActive(true);
            //        for (int i = sumscore; i >= 0; i--)
            //        {
            //            gemScoring.GetComponent<Text>().text = "+" + i.ToString();
            //            gemScoring.transform.DOScale(Vector3.one * 1.1f, 0.025f / sumscore).SetLoops(2, LoopType.Yoyo);
            //            yield return new WaitForSeconds(0.05f / sumscore);
            //        }

            //        // foreach (var item in targetObjectsList)
            //        // {
            //        //     item.SetActive(false);
            //        // }
            //        // yield return new WaitForSeconds(1f);

            //        for (int i = sumscore; i >= 1; i--)
            //        {
            //            CurrentGem += 1;
            //            gemText.transform.parent.DOScale(Vector3.one * 1.1f, 0.025f / sumscore).SetLoops(2, LoopType.Yoyo);
            //            yield return new WaitForSeconds(0.05f / sumscore);
            //        }
            //    }

            //    isBonusLevel = false;
            //    gemScoring.SetActive(false);
            //    yield return new WaitForSeconds(0.5f);
            //    CurrentTask = 0;
            //    LevelNumberGA++;
            //    yield return new WaitForSeconds(1);
            //    LoadScene();
            //}

        }
        // AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.CompleteEvent);
        // AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.EndEvent);
    }

    public void LevelUp()
    {
        LevelNumberGA++;

        CurrentTask++;

        if (CurrentTask >= 3)
        {
            currentLevel++;
            pointerLevel = currentLevel;
            PlayerPrefs.SetInt("pointerLevel", pointerLevel);

            if (currentLevel >= maxLevel)
            {
                var temp = currentLevel--;
                PlayerPrefs.SetInt("levelCount", temp);
                currentLevel = 1;
                pointerLevel = currentLevel;
                PlayerPrefs.SetInt("pointerLevel", pointerLevel);
            }
            var currentLevelCount = PlayerPrefs.GetInt("levelCount");
            currentLevelCount++;
            PlayerPrefs.SetInt("levelCount", currentLevelCount);
            PlayerPrefs.SetInt("currentLevel", currentLevel);
            //isBonusLevel = true;
        }
    }

    public GameObject yesObject;
    public GameObject oops;
    public Animator[] starAnimator;
    public Animator[] shadowRopeAnimator;

    public void Lose()
    {
        //currentPlatform.GetComponent<MeshRenderer>().enabled = false;
        if (isStartGame && !isBonusLevel)
        {
            endTimer = Time.unscaledTime;
            isTimeLevel = false;
            var star = PlayerPrefs.GetInt("currentStar");
            // AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.FailEvent);
            taskDuration = endTimer - startTimer;
            isStartGame = false;
            StopAllCoroutines();
            StartCoroutine(delayLose());
        }
        else if (isBonusLevel)
        {
            gemScore--;
            if (gemScore <= 0)
            {
                endTimer = Time.unscaledTime;
                isTimeLevel = false;
                taskDuration = endTimer - startTimer;
                isStartGame = false;
                StopAllCoroutines();
                isBonusLevel = false;
                StartCoroutine(delayLose());
            }
            // endTimer = Time.unscaledTime;
            // isTimeLevel = false;
            // AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.FailEvent);
            // taskDuration = endTimer - startTimer;
            // isStartGame = false;
            // StopAllCoroutines();
            // StartCoroutine(delayLose());
        }
        // AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.EndEvent);

    }

    public GameObject taptoplayobject;
    public static bool IsRefreshLife;

    IEnumerator delayLose()
    {
        timer.transform.parent.gameObject.SetActive(false);
        status.text = "OPPS!!";
        dropButton.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        losePanel.SetActive(true);
        LoadScene();
    }

    IEnumerator timeScanner()
    {
        var timePanel = timer.transform.parent;
        float angle = -1200;
        float a = Time.deltaTime * -360;
        while (angle < 0)
        {
            timePanel.transform.GetChild(0).RotateAround(timePanel.transform.position, Vector3.forward, a);
            angle += a;
            yield return null;
        }
        timePanel.transform.GetChild(0).RotateAround(timePanel.transform.position, Vector3.forward, angle);
    }

    public void LoadScene()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        var temp = conffetiSpawn;
        Destroy(temp);
        StartCoroutine(delayLoadScene());
    }

    IEnumerator delayLoadScene()
    {
        if (!isShowDiorama)
        {
            BG.GetComponent<Renderer>().material.DOBlendableColor(listBGColor[currentBG], 1);
            winPanelPopup.transform.DOLocalMoveX(-1000, 1);
            Camera.main.transform.DOMoveX(50, 1);
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene(0);
        }
        //else
        //{
        //    BG.GetComponent<Renderer>().material.DOBlendableColor(listBGColor[currentBG], 1);
        //    winPanelPopup.transform.DOLocalMoveX(-1000, 1);
        //    // Camera.main.backgroundColor = listBGColor[currentBG];
        //    Camera.main.transform.DOMove(new Vector3(30, 3.5f, -16), 1);
        //    Camera.main.transform.DORotateQuaternion(Quaternion.Euler(15, 0, 0), 1);
        //    dioramaMap = dioramaList[currentLevel];
        //    dioramaMap.SetActive(true);
        //    List<Vector3> originalPos = new List<Vector3>();
        //    List<GameObject> itemList = new List<GameObject>();
        //    for (int i = 0; i <= 2; i++)
        //    {
        //        var item = dioramaList[currentLevel].transform.GetChild(0).GetChild(i).gameObject;
        //        originalPos.Add(item.transform.position);
        //        itemList.Add(item);
        //        // item.transform.parent = null;
        //        item.transform.DOMoveY(1000, 0);
        //    }
        //    yield return new WaitForSeconds(1);
        //    dioramaMap.transform.DORotateQuaternion(Quaternion.Euler(0, dioramaMap.transform.eulerAngles.y + 270, 0), 0.5f).SetEase(Ease.Linear);
        //    yield return new WaitForSeconds(0.5f);
        //    dioramaMap.transform.DORotateQuaternion(Quaternion.Euler(0, dioramaMap.transform.eulerAngles.y + 270, 0), 0.5f).SetEase(Ease.Linear);
        //    yield return new WaitForSeconds(0.5f);
        //    dioramaMap.transform.DORotateQuaternion(Quaternion.Euler(0, dioramaMap.transform.eulerAngles.y + 270, 0), 0.5f).SetEase(Ease.Linear);
        //    yield return new WaitForSeconds(0.5f);
        //    dioramaMap.transform.DORotateQuaternion(Quaternion.Euler(0, dioramaMap.transform.eulerAngles.y + 270, 0), 0.5f).SetEase(Ease.Linear);
        //    yield return new WaitForSeconds(0.5f);
        //    dioramaMap.transform.DOKill();
        //    dioramaMap.transform.eulerAngles = new Vector3(0, -45, 0);
        //    dioramaMap.transform.DORotateQuaternion(Quaternion.Euler(0, dioramaMap.transform.eulerAngles.y + 270, 0), 0.5f).SetEase(Ease.Linear);
        //    yield return new WaitForSeconds(0.5f);
        //    dioramaMap.transform.DORotateQuaternion(Quaternion.Euler(0, dioramaMap.transform.eulerAngles.y + 270, 0), 0.5f).SetEase(Ease.Linear);
        //    yield return new WaitForSeconds(0.5f);
        //    dioramaMap.transform.DORotateQuaternion(Quaternion.Euler(0, dioramaMap.transform.eulerAngles.y + 270, 0), 0.5f).SetEase(Ease.Linear);
        //    yield return new WaitForSeconds(0.5f);
        //    dioramaMap.transform.DORotateQuaternion(Quaternion.Euler(0, dioramaMap.transform.eulerAngles.y + 270, 0), 0.5f).SetEase(Ease.Linear);
        //    yield return new WaitForSeconds(0.5f);

        //    for (int i = 0; i <= 2; i++)
        //    {
        //        itemList[i].transform.DOKill();
        //        itemList[i].transform.DOMoveY(originalPos[i].y, 1);
        //        var effect = Instantiate(highlightEffect, new Vector3(itemList[i].transform.position.x, originalPos[i].y, itemList[i].transform.position.z), Quaternion.identity);
        //        effect.transform.DOMoveY(-10, 2);
        //        // effect.transform.parent = itemList[i].transform;
        //    }
        //    yield return new WaitForSeconds(1);
        //    for (int i = 0; i <= 2; i++)
        //    {
        //        itemList[i].transform.DOKill();
        //        itemList[i].transform.DOShakeRotation(0.5f, 15);
        //    }
        //    Instantiate(shinyEffect, new Vector3(dioramaMap.transform.position.x, dioramaMap.transform.position.y + 1f, dioramaMap.transform.position.z), Quaternion.identity);
        //    dioramaConffeti.SetActive(true);
        //    isShowDiorama = false;
        //    skipDioramaButton.SetActive(true);
        //    isDioramaMode = true;
        //}
    }

    public void ButtonRestart()
    {
        LevelNumberGA--;
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        var temp = conffetiSpawn;
        Destroy(temp);
        StartCoroutine(delayLoadScene());
    }

    public void OnChangeMap()
    {
        if (levelInput != null)
        {
            int level = int.Parse(levelInput.text.ToString());
            Debug.Log(level);
            if (level < maxLevel)
            {
                PlayerPrefs.SetInt("pointerLevel", level);
                SceneManager.LoadScene(0);
            }
        }
    }

    public void ButtonNextLevel()
    {
        title.DOKill();
        isStartGame = true;
        CurrentTask++;
        if (CurrentTask >= 3)
        {
            currentLevel++;
            pointerLevel = currentLevel;
            PlayerPrefs.SetInt("pointerLevel", pointerLevel);
            PlayerPrefs.SetInt("currentLevel", currentLevel);
            CurrentTask = 0;
        }
        SceneManager.LoadScene(0);
    }

    public void TestDrop()
    {
        // if (isAnimationMarkRope) return;

        isDrop = true;
        handClickDrop.SetActive(false);
        if (!isBonusLevel)
        {
            // if (platformIndex == 10 || platformIndex == 13 || platformIndex == 14 || platformIndex == 39 || platformIndex == 40 || platformIndex == 43)
            // {
            //     MeshCollider meshCollider = platformsList[platformIndex].transform.GetChild(0).transform.GetChild(0).GetComponent<MeshCollider>();
            //     if (meshCollider)
            //     {
            //         meshCollider.convex = true;
            //         meshCollider.isTrigger = true;
            //     }
            // }
            var getAllItem = platformObject.GetComponentsInChildren<Transform>();
            foreach (var item in getAllItem)
            {
                if (item.gameObject.layer == 8)
                {
                    // Debug.LogError(item.name);
                    //item.gameObject.SetActive(false);
                }
            }
        }
        undoButton.SetActive(false);
        StartCoroutine(delayDrop());
    }

    int temp = 1;

    IEnumerator delayDrop()
    {
        if (targetObjectsList.Count > 1 && !isBonusLevel)    //Multi Objects
        {
            Drop(targetObjectsList[temp]);
            // targetObjectsList.RemoveAt(targetObjectsList.Count - 1);
            temp--;
            if (temp < 0)
            {
                dropButton.transform.DOKill();
                dropButton.SetActive(false);
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(Win());
            }
        }
        else if (targetObjectsList[0].CompareTag("Pendulum"))
        {
            UpdateRopes();
            var objectDrop = targetObjectsList[0].transform.GetChild(0).transform.GetChild(0).gameObject;
            var pos1 = objectDrop.transform.position;
            yield return new WaitForSeconds(0.05f);
            var pos2 = objectDrop.transform.position;
            var dir = pos2 - pos1;
            objectDrop.GetComponent<Rigidbody>().useGravity = true;
            var rad = targetObjectsList[0].transform.GetChild(0).eulerAngles.z;
            if (rad > 180)
            {
                rad = 360 - rad;
            }
            if (rad < 5)
            {
                rad = 5;
            }
            // Debug.LogError(rad);
            objectDrop.GetComponent<Rigidbody>().AddForce(dir * 140 * rad);
            objectDrop.transform.parent = targetObjectsList[0].transform;
            Destroy(targetObjectsList[0].transform.GetChild(0).gameObject);
            dropButton.transform.DOKill();
            dropButton.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Win());
        }
        else if (isBonusLevel)
        {
            foreach (var item in targetObjectsList)
            {
                item.transform.parent = null;
                Drop(item);
            }
            dropButton.transform.DOKill();
            dropButton.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Win());
        }
        else
        {
            Drop(targetObjectsList[0]);
            dropButton.transform.DOKill();
            dropButton.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Win());
        }

    }

    bool isUpdatedRopes = false;

    private void UpdateRopes()
    {
        if (!isUpdatedRopes)
        {
            for (int i = 0; i < listRopes.Count; i++)
            {
                if (i <= ropeID)
                {
                    listRopes[i].transform.DOMoveY(listRopes[i].transform.position.y + 3, 0.5f);
                    listRopes[i].transform.GetChild(0).GetComponent<ObiSolver>().stretchShearConstraintParameters.iterations = 4;
                    // listRopes[i].transform.GetChild(0).GetComponent<ObiSolver>().worldLinearInertiaScale = 0.01f;
                    // listRopes[i].transform.GetChild(0).GetComponent<ObiFixedUpdater>().substeps = 3;
                    listRopes[i].transform.GetChild(0).GetComponent<ObiSolver>().UpdateParameters();
                }
                else
                {
                    listRopes.RemoveAt(i);
                    i--;
                }
            }
            isUpdatedRopes = true;
        }
    }

    private void Drop(GameObject target)
    {
        StartCoroutine(C_Drop(target));
    }

    IEnumerator C_Drop(GameObject target)
    {
        isControl = false;

        UpdateRopes();

        if (target.GetComponent<StopDoPath>())
        {
            target.GetComponent<StopDoPath>().StopToDrop();
        }

        if (target.GetComponent<TrignometricMovement>())
        {
            target.GetComponent<TrignometricMovement>().isRun = false;
        }

        if (target.GetComponent<TrignometricRotation>())
        {
            target.GetComponent<TrignometricRotation>().enabled = false;
        }

        yield return new WaitForSeconds(0.5f);

        if (target.GetComponent<Rigidbody>())
        {
            target.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public IEnumerator C_StarAnimMethod(Transform target, int ropeNum, Image starFill)
    {
        shadowRopeAnimator[ropeNum].SetTrigger("awake");

        int count = listRopes.Count + 20;
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = winRopeLeftText.transform.position;
            var spawnStar = Instantiate(starAnim);
            spawnStar.transform.SetParent(canvas.transform);
            spawnStar.transform.position = pos;
            spawnStar.GetComponent<Gem>().ActiveAnimation(target.position);
        }

        yield return new WaitForSeconds(0.5f);
        starFill.transform.parent.GetComponent<Animator>().SetTrigger("hit");
        starFill.DOFillAmount(1.0f, 0.25f);
        yield return new WaitForSeconds(0.15f);
        starFill.transform.parent.GetComponent<Animator>().SetTrigger("highlight");
    }

    private IEnumerator C_Rating()
    {
        if (!isBonusLevel)
        {
            starParent.SetActive(true);

            star1.DOKill();
            star2.DOKill();
            star3.DOKill();

            starAnimator[0].SetTrigger("awake");
            yield return new WaitForSeconds(0.1f);
            starAnimator[1].SetTrigger("awake");
            yield return new WaitForSeconds(0.1f);
            starAnimator[2].SetTrigger("awake");
            yield return new WaitForSeconds(0.1f);

            if (ropeID < 4)
            {
                yield return C_StarAnimMethod(star1.transform, 5, star1);
                yield return C_StarAnimMethod(star2.transform, 4, star2);
                yield return C_StarAnimMethod(star3.transform, 3, star3);

                var star = PlayerPrefs.GetInt("currentStar");
                star += 3;
                PlayerPrefs.SetInt("currentStar", star);
            }
            else if (ropeID >= 4 && ropeID < 5)
            {
                yield return C_StarAnimMethod(star1.transform, 5, star1);
                yield return C_StarAnimMethod(star2.transform, 4, star2);
                winLightingEffect.transform.GetChild(2).gameObject.SetActive(false);

                var star = PlayerPrefs.GetInt("currentStar");
                star += 2;
                PlayerPrefs.SetInt("currentStar", star);
            }
            else if (ropeID >= 5)
            {
                yield return C_StarAnimMethod(star1.transform, 5, star1);
                winLightingEffect.transform.GetChild(2).gameObject.SetActive(false);
                winLightingEffect.transform.GetChild(1).gameObject.SetActive(false);

                var star = PlayerPrefs.GetInt("currentStar");
                star += 1;
                PlayerPrefs.SetInt("currentStar", star);
            }

            yield return new WaitForSeconds(0.2f);
            starParent.GetComponent<Animator>().SetTrigger("hide");
        }
    }

    //public bool isTutorial;
    public LayerMask layer;

    //public void Tutorial()
    //{
    //    isTutorial = true;
    //    SetPointTutorial();
    //}

    //private IEnumerator C_Tutorial()
    //{
    //    isTutorial = true;
    //    bool isTap = false;
    //    while (isTap == false)
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;
    //        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
    //        {
    //            if (hit.collider.CompareTag("Tutorial_1"))
    //            {

    //            }
    //        }
    //        yield return null;
    //    }
    //    isTutorial = false;
    //}

    public GameObject[] tutorialAnimation;
    public Animator dropdownAnim;

    //private void SetPointTutorialUndo()
    //{
    //    tuIndex--;

    //    btnRope.gameObject.SetActive(false);
    //    if (tuIndex >= 3)
    //    {

    //        if (isTutorial)
    //        {
    //            dropButton.transform.eulerAngles = new Vector3(0, 0, 0);

    //            dropdownAnim.enabled = true;
    //            //    dropButton.transform.DOPunchRotation(new Vector3(0, 0, 5), 0.75f, 5).SetLoops(-1, LoopType.Yoyo);
    //        }
    //        else
    //        {
    //            dropButton.transform.eulerAngles = new Vector3(0, 0, -10);

    //            dropdownAnim.enabled = false;
    //            dropButton.transform.DOPunchRotation(new Vector3(0, 0, 20), 0.5f, 5).SetLoops(-1, LoopType.Yoyo);

    //        }

    //        btnRope.gameObject.SetActive(true);
    //        startPosTuObject.gameObject.SetActive(false);
    //        endPosTuObject.gameObject.SetActive(false);
    //        tutorial.SetActive(false);

    //        handClickDrop.transform.SetParent(dropButton.transform, false);
    //        handClickDrop.SetActive(true);
    //        handClickDrop.transform.localPosition = new Vector3(30f, -120f, 0f);
    //        handClickDrop.transform.DOScale(Vector3.one * 1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    //        handClickDrop.transform.SetParent(inGameMenu.transform);
    //        return;
    //    }

    //    tutorial.SetActive(true);

    //    startPosTuObject.gameObject.SetActive(true);
    //    endPosTuObject.gameObject.SetActive(true);
    //    for (int i = 0; i < tutorialAnimation.Length; i++)
    //    {
    //        if (i == tuIndex)
    //        {
    //            tutorialAnimation[i].SetActive(true);
    //        }
    //        else
    //        {
    //            tutorialAnimation[i].SetActive(false);
    //        }
    //    }
    //    startPosTuObject.transform.position = startPosTuArray[tuIndex];
    //    endPosTuObject.transform.position = endPosTuArray[tuIndex];
    //    if ((CurrentTask == 0 && tuIndex < (startPosTuArray.Length - 2)) || (CurrentTask == 1 && tuIndex < (startPosTuArray.Length - 1)))
    //    {
    //        tutorialAnimation[tuIndex].SetActive(false);
    //        undoButton.SetActive(false);
    //        StartCoroutine(AutoCreateRope(startPosTuObject.transform.position, endPosTuObject.transform.position));
    //    }
    //}

    //private void SetPointTutorial()
    //{
    //    tuIndex++;

    //    btnRope.gameObject.SetActive(false);
    //    if (tuIndex >= 3)
    //    {

    //        if (isTutorial)
    //        {
    //            dropButton.transform.eulerAngles = new Vector3(0, 0, 0);

    //            dropdownAnim.enabled = true;
    //            //    dropButton.transform.DOPunchRotation(new Vector3(0, 0, 5), 0.75f, 5).SetLoops(-1, LoopType.Yoyo);
    //        }
    //        else
    //        {
    //            dropButton.transform.eulerAngles = new Vector3(0, 0, -10);

    //            dropdownAnim.enabled = false;
    //            dropButton.transform.DOPunchRotation(new Vector3(0, 0, 20), 0.5f, 5).SetLoops(-1, LoopType.Yoyo);

    //        }

    //        btnRope.gameObject.SetActive(true);
    //        startPosTuObject.gameObject.SetActive(false);
    //        endPosTuObject.gameObject.SetActive(false);
    //        tutorial.SetActive(false);

    //        handClickDrop.transform.SetParent(dropButton.transform, false);
    //        handClickDrop.SetActive(true);
    //        handClickDrop.transform.localPosition = new Vector3(30f, -120f, 0f);
    //        handClickDrop.transform.DOScale(Vector3.one * 1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    //        handClickDrop.transform.SetParent(inGameMenu.transform);

    //        return;
    //    }
    //    startPosTuObject.gameObject.SetActive(true);
    //    endPosTuObject.gameObject.SetActive(true);
    //    for (int i = 0; i < tutorialAnimation.Length; i++)
    //    {
    //        if (i == tuIndex)
    //        {
    //            tutorialAnimation[i].SetActive(true);
    //        }
    //        else
    //        {
    //            tutorialAnimation[i].SetActive(false);
    //        }
    //    }
    //    startPosTuObject.transform.position = startPosTuArray[tuIndex];
    //    endPosTuObject.transform.position = endPosTuArray[tuIndex];
    //    if ((CurrentTask == 0 && tuIndex < (startPosTuArray.Length - 2)) || (CurrentTask == 1 && tuIndex < (startPosTuArray.Length - 1)))
    //    {
    //        tutorialAnimation[tuIndex].SetActive(false);
    //        undoButton.SetActive(false);
    //        StartCoroutine(AutoCreateRope(startPosTuObject.transform.position, endPosTuObject.transform.position));
    //    }

    //}

    public Transform platformParent;

    void SetPlatform()
    {
        if (!isBonusLevel)
        {
            tempLevel = pointerLevel >= 37 ? (int)Random.Range(1f, 37f) : pointerLevel;  //Random platform After level 37.
            platformIndex = tempLevel * 3 + (CurrentTask + 1);
        }
        else
        {
            platformIndex = 0;
        }
        platformObject = Instantiate(platformsList[platformIndex]);
        platformObject.transform.SetParent(platformParent);
        platformObject.SetActive(true);
        currentPlatform = platformObject;
    }

    private void HideRopeLeft()
    {
        ropeLeft.SetActive(false);
    }

    public void PlayZoneGround(Vector3 position, GameObject objectTarget)
    {
        // if (isStartGame)
        // {
        if (zoneGround)
        {
            MaskController.Instance.FillRedColorGround(position);
            //zoneGround.gameObject.SetActive(true);
            //zoneGround.Stop();
            //zoneGround.transform.position = position;
            //zoneGround.Play();
            // StartCoroutine(delayExplode(objectTarget));
        }
        // }
    }

    IEnumerator delayExplode(GameObject objectTarget)
    {
        yield return new WaitForSeconds(0.2f);
        objectExplode.transform.position = objectTarget.transform.position;
        objectExplode.SetActive(true);
        objectTarget.GetComponent<BoxCollider>().enabled = false;
        objectTarget.SetActive(false);
    }

    public void UndoButton()
    {
        if (ropeID >= 1)
        {
            ropeID--;
            listRopes[ropeID].transform.DOMoveY(-10, 1);
            Destroy(listRopes[ropeID].transform.GetChild(3).gameObject, 0.5f);
            Destroy(listRopes[ropeID].transform.GetChild(4).gameObject, 0.5f);

            //if (isTutorial)
            //    SetPointTutorialUndo();
        }
    }

    public void LevelButton()
    {
        isControl = false;
        isStartGame = false;
        //tutorial.SetActive(false);
        startGameMenu.SetActive(false);
        inGameMenu.SetActive(false);
        topPanel.gameObject.SetActive(false);
        levelMenu.SetActive(true);
        Camera.main.transform.DOMoveX(50, 1);
    }

    public void BackButton()
    {
        //if (pointerLevel == 0 && (CurrentTask == 0 || CurrentTask == 1))
        //{
        //    StartCoroutine(delayTutorial());
        //}
        startGameMenu.SetActive(true);
        inGameMenu.SetActive(true);
        topPanel.gameObject.SetActive(true);
        levelMenu.SetActive(false);
        Camera.main.transform.DOMoveX(0, 1);
        isControl = true;
    }

    //IEnumerator delayTutorial()
    //{
    //    yield return new WaitForSeconds(1);
    //    tutorial.SetActive(true);
    //}
}
