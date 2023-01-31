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
    [SerializeField]
    RewardManager rewardManager;
    MainGameManager mainGameManager;

    private bool isFocusDone = false;
    private bool isNewStage = false;

    // Start is called before the first frame update
    void Start()
    {
        setSetting();
        setClear();
        applyJourney();
        setUser(curStage);
        activateNewEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (!LocalStorage.IS_SCENE_FADE_IN && !isFocusDone)
        {
            StartCoroutine(focusCamera(true, curStage));
        }
    }

    private IEnumerator focusCamera(bool isInit, StageController targetStage)
    {
        isFocusDone = true;
        LocalStorage.IS_CAMERA_FREE = false;
        while (Mathf.Abs(mainCamera.position.x - targetStage.transform.position.x) > 0.1f)
        {
            mainCamera.Translate(Vector3.left * (mainCamera.position.x - targetStage.transform.position.x) * Time.deltaTime * 2.5f);
            yield return new WaitForSeconds(0f);
        }
        if (isInit)
        {
            LocalStorage.IS_CAMERA_FREE = true;
        }
        else
        {
            mainGameManager.controllLoading(true, "BattleScene");
        }
    }

    private void applyJourney()
    {
        bool isEntryEmpty = true;
        foreach (int stagePos in LocalStorage.journeyInfo)
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
        if (LocalStorage.IS_CAMERA_FREE)
        {
            StartCoroutine(focusCamera(false, targetStage));
        }
    }

    public void enterStage(StageController targetStage)
    {
        curStage.closeAccessNext();
        moveCamera(targetStage);
        //curStage.clearStage();
        curStage = targetStage;
        setUser(curStage);
        curStage.openAccessNext();
    }

    public void openRewardModule()
    {
        StartCoroutine(toggleRewardModuleWithDelay(true, 1f));
    }

    public IEnumerator toggleRewardModuleWithDelay(bool isOpen, float delay)
    {
        yield return new WaitForSeconds(delay);
        rewardManager.toggle(true);
    }

    private void activateNewEvent()
    {
        if (isNewStage)
        {
            openRewardModule();
        }
    }
}
