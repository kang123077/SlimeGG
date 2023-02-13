using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    [SerializeField]
    StageController stagePrefab;
    [SerializeField]
    Transform stageParentTf;
    StageController curStage;
    [SerializeField]
    Transform mainCamera;
    [SerializeField]
    Transform userTf;
    MainGameManager mainGameManager;

    private bool isFocusDone = false;
    private bool isNewStage = false;

    private StageController[] stageControllers;
    private int curStatus = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (curStatus)
        {
            case 0:
                //  최초 로드
                //  스테이지 생성
                generateStages();
                break;
            case 1:
                //  curStage 지정 -> stageControllers의 첫번째 칸
                setCurrentStageIntoZero();
                break;
            case 2:
                // 저장된 여정 적용
                applyJourney();
                break;
            case 3:
                //  유저 캐릭터 배치
                setUserCharacter(curStage);
                break;
            case 4:
                // 카메라 자동 이동
                StartCoroutine(focusCamera(true, curStage));
                break;
            case 5:
                // 로딩 종료
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
        }
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
        curStatus = 5;
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
        curStatus = 3;
    }

    private void setCurrentStageIntoZero()
    {
        mainGameManager = transform.GetComponent<MainGameManager>();
        curStage = stageControllers[0];
        curStage.setDungeonManager(this);
        curStage.clearStage();
        curStage.openAccessNext();
        curStatus = 2;
    }

    private void setUserCharacter(StageController targetStage)
    {
        userTf.position = targetStage.transform.position;
        curStatus = 4;
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
        setUserCharacter(curStage);
        curStage.openAccessNext();
    }

    private void generateStages()
    {
        int stageCnt = LocalDictionary.dungeons[LocalStorage.CurrentLocation.dungeonName].Count;
        stageControllers = new StageController[stageCnt];
        StageSaveStat[] tempAligner = new StageSaveStat[stageCnt];
        foreach (StageSaveStat stageSaveStat in LocalDictionary.dungeons[LocalStorage.CurrentLocation.dungeonName])
        {
            tempAligner[stageSaveStat.id] = stageSaveStat;
        }
        for (int i = stageCnt - 1; i >= 0; --i)
        {
            StageController newStage = Instantiate(stagePrefab);
            List<StageController> nextStages = new List<StageController>();
            if (tempAligner[i].next != null)
            {
                foreach (int nextNum in tempAligner[i].next)
                {
                    nextStages.Add(stageControllers[nextNum]);
                }
            }
            newStage.transform.SetParent(stageParentTf);
            newStage.initInfo(tempAligner[i], nextStages);
            stageControllers[i] = newStage;
        }
        curStatus = 1;
    }
}
