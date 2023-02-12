using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    [SerializeField]
    StageController curStage;
    [SerializeField]
    Transform mainCamera;
    [SerializeField]
    Transform userTf;
    MainGameManager mainGameManager;

    private bool isInit = false;

    private bool isFocusDone = false;
    private bool isNewStage = false;

    // Start is called before the first frame update
    void Start()
    {
        initSetting();
        //activateNewEvent(RewardType.Choice_Monster);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInit && !isFocusDone)
        {
            StartCoroutine(focusCamera(true, curStage));
        }
    }

    private void initSetting()
    {
        setSetting();
        setClear();
        applyJourney();
        setUser(curStage);
        isInit = true;
    }

    private IEnumerator focusCamera(bool isInit, StageController targetStage)
    {
        isFocusDone = true;
        LocalStorage.isCameraPosessed = true;
        while (Mathf.Abs(mainCamera.position.x - targetStage.transform.position.x) > 0.1f)
        {
            mainCamera.Translate(Vector3.left * (mainCamera.position.x - targetStage.transform.position.x) * Time.deltaTime * 2.5f);
            yield return new WaitForSeconds(0f);
        }
        LocalStorage.isCameraPosessed = false;
        if (!isInit)
        {
            mainGameManager.controllLoading(true, "BattleScene");
        }
    }

    private void applyJourney()
    {
        bool isEntryEmpty = true;
        foreach (int stagePos in LocalStorage.Live.journeyInfo)
        {
            isEntryEmpty = false;
            curStage.clearStage();
            enterStage(curStage.getNextStage(stagePos));
        }
        isNewStage = isEntryEmpty;
    }

    private void setSetting()
    {
        mainGameManager = transform.GetComponent<MainGameManager>();
        curStage.setDungeonManager(this);
    }

    private void setClear()
    {
        curStage.clearStage();
        curStage.openAccessNext();
    }

    private void setUser(StageController targetStage)
    {
        userTf.position = targetStage.transform.position;
    }

    public void moveCamera(StageController targetStage)
    {
        if (LocalStorage.isCameraMoveable())
        {
            StartCoroutine(focusCamera(false, targetStage));
        }
    }

    public void enterStage(StageController targetStage)
    {
        LocalStorage.CurrentLocation.curLocation = targetStage;
        curStage.closeAccessNext();
        moveCamera(targetStage);
        //curStage.clearStage();
        curStage = targetStage;
        setUser(curStage);
        curStage.openAccessNext();
    }
}
