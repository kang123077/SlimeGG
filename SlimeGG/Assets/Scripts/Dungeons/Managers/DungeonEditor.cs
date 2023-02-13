using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonEditor : MonoBehaviour
{
    [SerializeField]
    StageController stagePrefab;
    [SerializeField]
    Transform stageParentTf;
    [SerializeField]
    MoreChoiceController moreChoiceController;

    private StageController[] stageControllers = new StageController[100];
    private int curStatus = 0;
    private bool isNewDungeon = true;

    private Vector3 targetPos;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (curStatus)
        {
            case 0:
                // 새로 만들기 | 불러오기 선택
                break;
            case 1:
                // 도구창 로딩
                setNewOptions();
                // 마우스 이벤트 활성화
                GetComponent<MouseEventManager>().initSetting(
                    // 클릭 시작
                    (clickedTransform) =>
                    {
                        if (!checkIfSameTransform(clickedTransform, moreChoiceController.transform))
                        {
                            targetPos = Vector3.one * -1f;
                            moreChoiceController.GetComponent<TrackingWindowController>().closeWindow();
                        }
                    },
                    // 왼쪽 클릭
                    (clickedTransform) => { },
                    // 오른 클릭
                    (clickedTransform) =>
                    {
                        targetPos = MouseEventManager.targetMousePos;
                        targetPos.z = 0f;
                        if (!moreChoiceController.GetComponent<TrackingWindowController>().isOpened)
                        {
                            //  선택창 안열려있음
                            if (clickedTransform == null)
                            {
                                // 빈공간 클릭
                                setNewOptions();
                            }
                            else if (clickedTransform.GetComponent<StageController>())
                            {
                                //  존재하는 스테이지 클릭
                                setModifyingOption(clickedTransform.GetComponent<StageController>());
                            }
                            else
                            {
                                // 
                            }
                            moreChoiceController.GetComponent<TrackingWindowController>().openWindow(true);
                        }
                        else
                        {
                            // 선택창 열려있음
                        }
                    },
                    // 왼 드래그
                    (mousePos) => { },
                    // 오른 드래그
                    (mousePos) => { });
                GetComponent<MouseEventManager>().setActivation(true);
                // 최초 스테이지 배치
                placeInitialStage();
                break;
            case 2:
                // 배치
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    private void setNewOptions()
    {
        moreChoiceController.initInfo(
            new Dictionary<string, System.Action<int>>()
            {
                {
                        "생성 -> 일반",
                        (i) => { createNewStage(StageType.Normal); }
                },
                {
                        "생성 -> 어려움",
                        (i) => { createNewStage(StageType.Hard); }
                },
                {
                        "생성 -> 이벤트",
                        (i) => { createNewStage(StageType.Event); }
                },
                {
                        "생성 -> 상점",
                        (i) => { createNewStage(StageType.Shop); }
                },
                {
                        "생성 -> 보스",
                        (i) => { createNewStage(StageType.Boss); }
                },
            }
        );
    }

    private void setModifyingOption(StageController stageController)
    {
        moreChoiceController.initInfo(
            new Dictionary<string, System.Action<int>>()
            {
                {
                        "삭제",
                        (i) => { truncateTargetStage(stageController); }
                },
                {
                        "변경 -> 일반",
                        (i) => { modifyStage(stageController, StageType.Normal); }
                },
                {
                        "변경 -> 어려움",
                        (i) => { modifyStage(stageController, StageType.Hard); }
                },
                {
                        "변경 -> 이벤트",
                        (i) => { modifyStage(stageController, StageType.Event); }
                },
                {
                        "변경 -> 상점",
                        (i) => { modifyStage(stageController, StageType.Shop); }
                },
                {
                        "변경 -> 보스",
                        (i) => { modifyStage(stageController, StageType.Boss); }
                },
            }
        );
    }

    private void createNewStage(StageType stageType)
    {
        Debug.Log($":: {targetPos} 에 신규 {stageType} 스테이지 생성 ::");
    }

    private void modifyStage(StageController targetController, StageType stageType)
    {
        Debug.Log($":: {targetController} -> {stageType} 스테이지 변경 ::");
    }

    private void truncateTargetStage(StageController stageController)
    {
        Debug.Log("삭제");
    }

    private bool checkIfSameTransform(Transform tf1, Transform tf2)
    {
        return tf1 && tf2 && tf1 == tf2;
    }

    private void placeInitialStage()
    {
        StageSaveStat firstStat = new StageSaveStat();
        firstStat.id = 0;
        firstStat.locationPos = new List<float>() { -10f, 0f };
        firstStat.type = StageType.Event;
        generateStage(firstStat);
        curStatus = 2;
    }

    private void generateStage(StageSaveStat stat)
    {
        StageController newStage = Instantiate(stagePrefab);
        stageControllers[stat.id] = newStage;
        newStage.transform.SetParent(stageParentTf);
        newStage.initInfo(stat, null);
    }

    public void startNewDungeon(Transform btnsTf)
    {
        curStatus = 1;
        Destroy(btnsTf.gameObject);
    }

    public void loadDungeon()
    {
        //curStatus = 1;
    }
}
