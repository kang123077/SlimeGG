using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEditor : MonoBehaviour
{
    [SerializeField]
    StageController stagePrefab;
    [SerializeField]
    Transform stageParentTf;

    private StageController[] stageControllers = new StageController[100];
    private int curStatus = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (curStatus)
        {
            case 0:
                // 최초 스테이지 배치
                placeInitialStage();
                break;
            case 1:
                //  
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    private void placeInitialStage()
    {
        StageSaveStat firstStat = new StageSaveStat();
        firstStat.id = 0;
        firstStat.locationPos = new List<float>() { -10f, 0f };
        firstStat.type = StageType.Event;
        generateStage(firstStat);
        curStatus = 1;
    }

    private void generateStage(StageSaveStat stat)
    {
        StageController newStage = Instantiate(stagePrefab);
        stageControllers[stat.id] = newStage;
        newStage.transform.SetParent(stageParentTf);
        newStage.initInfo(stat, null);
    }
}
