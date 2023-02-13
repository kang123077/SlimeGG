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
    private int curIdx = 0;
    private bool isNewDungeon = true;
    private StageController curClickedStageController;
    private Vector3 startPos;
    private LineRenderer lineToVisualize;

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
                // 잇는 것을 보여주기 위한 선 오브젝트 생성
                GameObject newLine = new GameObject();
                newLine.AddComponent<LineRenderer>();
                newLine.transform.position = Vector3.zero;
                newLine.transform.localScale = Vector3.one;
                lineToVisualize = newLine.GetComponent<LineRenderer>();
                lineToVisualize.startWidth = 0.1f;
                lineToVisualize.endWidth = 0.1f;
                lineToVisualize.startColor = Color.black;
                lineToVisualize.endColor = Color.black;
                lineToVisualize.positionCount = 2;
                lineToVisualize.gameObject.SetActive(false);
                // 도구창 로딩
                setNewOptions();
                // 마우스 이벤트 활성화
                GetComponent<MouseEventManager>().initSetting(
                    (mousePos) =>
                    {
                        LocalStorage.isCameraPosessed = false;
                        lineToVisualize.gameObject.SetActive(false);
                        startPos = Vector3.one * -1f;
                        curClickedStageController = null;
                    },
                    // 클릭 시작
                    (clickedTransform, mousePos) =>
                    {
                        if (!checkIfSameTransform(clickedTransform, moreChoiceController.transform))
                        {
                            targetPos = Vector3.one * -1f;
                            moreChoiceController.GetComponent<TrackingWindowController>().closeWindow();
                            if (clickedTransform != null)
                            {
                                if (clickedTransform.GetComponent<StageController>() != null)
                                {
                                    // 스테이지 선택
                                    LocalStorage.isCameraPosessed = true;
                                    startPos = mousePos;
                                    curClickedStageController = clickedTransform.GetComponent<StageController>();
                                }
                            }
                        }
                    },
                    // 왼쪽 클릭 <- 클릭 땔때
                    (clickedTransform) =>
                    {
                    },
                    // 오른 클릭 <- 클릭 땔때
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
                            }
                            moreChoiceController.GetComponent<TrackingWindowController>().openWindow(true);
                        }
                        else
                        {
                            // 선택창 열려있음
                        }
                    },
                    // 왼 드래그
                    (mousePos) =>
                    {
                        // 스테이지 이동
                        if (curClickedStageController != null)
                        {
                            mousePos.z = 0f;
                            curClickedStageController.transform.position = mousePos;
                        }
                    },
                    // 오른 드래그
                    (mousePos) =>
                    {
                        if (curClickedStageController != null)
                        {
                            // 스테이지 <-> 스테이지 끌어서 연결
                            if (!lineToVisualize.gameObject.activeSelf)
                            {
                                lineToVisualize.SetPosition(0, new Vector3(startPos.x, startPos.y, -1f));
                                lineToVisualize.gameObject.SetActive(true);
                            }
                            visualizeCurrentBridge(mousePos);
                        }
                    },
                    actionforLeftClickEnd: (clickedTransform, mousePos) =>
                    {

                    },
                    actionforRightClickEnd: (clickedTransform, mousePos) =>
                    {
                        if (clickedTransform != null)
                        {
                            if (clickedTransform.GetComponent<StageController>())
                            {
                                createConnection(clickedTransform.GetComponent<StageController>());
                            }
                        }
                    });
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

    private void visualizeCurrentBridge(Vector3 curPos)
    {
        lineToVisualize.SetPosition(1, new Vector3(curPos.x, curPos.y, -1f));
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

    private void createConnection(StageController targetStageController)
    {
        curClickedStageController.addNextStage(targetStageController);
    }

    private void createNewStage(StageType stageType)
    {
        StageSaveStat newStat = new StageSaveStat();
        newStat.locationPos = new List<float>() { targetPos.x, targetPos.y };
        newStat.id = curIdx;
        newStat.type = stageType;
        generateStage(newStat);
        targetPos = Vector3.one * -1f;
        moreChoiceController.GetComponent<TrackingWindowController>().closeWindow();
        curIdx++;
    }

    private void modifyStage(StageController targetController, StageType stageType)
    {
        targetController.setStageType(stageType);
        moreChoiceController.GetComponent<TrackingWindowController>().closeWindow();
    }

    private void truncateTargetStage(StageController stageController)
    {
        List<StageController> stages = new List<StageController>();
        stages.AddRange(stageController.getPrevStageControllers());
        foreach (StageController prevStage in stages)
        {
            prevStage.removeNextStage(stageController);
        }
        Destroy(stageController.gameObject);
        moreChoiceController.GetComponent<TrackingWindowController>().closeWindow();
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
        curIdx = 1;
        Destroy(btnsTf.gameObject);
    }

    public void loadDungeon()
    {
        //curStatus = 1;
    }
}
