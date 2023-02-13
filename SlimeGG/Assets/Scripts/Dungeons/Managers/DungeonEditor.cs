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
            new List<string>() {
                        "New: Normal",
                        "New: Hard",
                        "New: Event",
                        "New: Shop",
                        "New: Boss",
            },
            new List<System.Action<int>>() {
                        (i) => { createNewStage(StageType.Normal); },
                        (i) => { createNewStage(StageType.Hard); },
                        (i) => { createNewStage(StageType.Event); },
                        (i) => { createNewStage(StageType.Shop); },
                        (i) => { createNewStage(StageType.Boss); },
            }
            );
    }

    private void setModifyingOption(StageController stageController)
    {
        moreChoiceController.initInfo(
            new List<string>() {
                        "Remove",
                        "Change: Normal",
                        "Change: Hard",
                        "Change: Event",
                        "Change: Shop",
                        "Change: Boss",
            },
            new List<System.Action<int>>() {
                        (i) => { truncateTargetStage(stageController); },
                        (i) => { createNewStage(StageType.Normal); },
                        (i) => { createNewStage(StageType.Hard); },
                        (i) => { createNewStage(StageType.Event); },
                        (i) => { createNewStage(StageType.Shop); },
                        (i) => { createNewStage(StageType.Boss); },
            }
            );
    }

    private void createNewStage(StageType stageType)
    {
        Debug.Log($":: {targetPos} 에 신규 {stageType} 스테이지 생성 ::");
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
