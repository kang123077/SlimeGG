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
                // 마우스 이벤트 활성화
                GetComponent<MouseEventManager>().initSetting(
                    (mousePos) =>
                    {
                        if (!checkIfMouseInTragetTf(moreChoiceController.transform))
                        {
                            moreChoiceController.GetComponent<TrackingWindowController>().closeWindow();
                        }
                    },
                    (mousePos) => { },
                    (mousePos) =>
                    {
                        // 선택창 뜨기
                        if (!moreChoiceController.GetComponent<TrackingWindowController>().isOpened)
                        {
                            moreChoiceController.GetComponent<TrackingWindowController>().openWindow(true);
                        }
                    },
                    (mousePos) => { },
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

    private bool checkIfMouseInTragetTf(Transform targetTf)
    {
        return getTransformBelowMouse() && getTransformBelowMouse().name == targetTf.name;
    }

    private Transform getTransformBelowMouse()
    {
        Vector3 curMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        curMousePos = new Vector3(
            curMousePos.x,
            curMousePos.y,
            -12f
            );
        Debug.DrawRay(curMousePos, Vector3.forward * 14f, Color.green, 1000f);
        RaycastHit hit;
        if (Physics.Raycast(curMousePos, Vector3.forward, out hit, 14f))
        {
            return hit.transform;
        }
        return null;
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
