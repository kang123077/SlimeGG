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
    private bool isNewStage = false;

    public static Dictionary<string, StageController> stageControllers;
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
                if (!LocalStorage.isCameraPosessed)
                {
                    StartCoroutine(focusCamera(true, curStage));
                }
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
        LocalStorage.isCameraPosessed = true;
        while (Mathf.Abs(mainCamera.position.x - targetStage.transform.position.x) > 0.1f)
        {
            mainCamera.Translate(Vector3.left * (mainCamera.position.x - targetStage.transform.position.x) * 0.01f * 3.0f);
            yield return new WaitForSeconds(0.01f);
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
        Debug.Log(LocalStorage.Live.journeyInfo.Count);
        foreach (int stageIdx in LocalStorage.Live.journeyInfo)
        {
            isEntryEmpty = false;
            curStage.clearStage();
            enterStage(stageControllers[stageIdx.ToString()]);
        }
        isNewStage = isEntryEmpty;
        curStatus = 3;
    }

    private void setCurrentStageIntoZero()
    {
        LocalStorage.Live.journeyInfo.Add(0);
        mainGameManager = transform.GetComponent<MainGameManager>();
        curStage = stageControllers[0.ToString()];
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
        LocalStorage.Live.journeyInfo.Add(targetStage.idx);
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
        stageControllers = new Dictionary<string, StageController>();
        Dictionary<string, StageSaveStat> tempAligner = new Dictionary<string, StageSaveStat>();
        foreach (StageSaveStat stageSaveStat in LocalDictionary.dungeons[LocalStorage.CurrentLocation.dungeonName].stages)
        {
            tempAligner[stageSaveStat.id.ToString()] = stageSaveStat;
        }
        foreach (KeyValuePair<string, StageSaveStat> keyValuePair in tempAligner)
        {
            StageController newStage = Instantiate(stagePrefab);
            newStage.initInfo(keyValuePair.Value, keyValuePair.Value.next, idx: int.Parse(keyValuePair.Key));
            newStage.transform.SetParent(stageParentTf);
            stageControllers[keyValuePair.Key] = newStage;
        }
        foreach (KeyValuePair<string, StageController> keyValuePair in stageControllers)
        {
            keyValuePair.Value.setDungeonManager(this);
            keyValuePair.Value.callNextStages(stageControllers);
        }
        curStatus = 1;
    }
}
