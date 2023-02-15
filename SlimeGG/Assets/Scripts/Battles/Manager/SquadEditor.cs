using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SquadEditor : BasicEditor, IBasicEditor
{
    [SerializeField]
    private Transform fieldTf, contentPrefab, monsterCreateTf;
    private int curStatus = -1;
    private Button monsterCreateCancel;
    private MoreChoiceController monsterChoiceController;

    private EntrySlotController curClickedEntrySlotController;
    private ContentController curClickedContentController;
    private List<Dictionary<string, UnityAction>> cellInfosByTier = new List<Dictionary<string, UnityAction>>();
    private List<EntrySlotController> entrySlotControllers = new List<EntrySlotController>(); 
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        switch (curStatus)
        {
            case -1:
                if (LocalStorage.DataCall.DICTIONARY)
                    initSetting();
                break;
            case 0:
                base.setActions(
                    actionToEnterEditorMode: () => fieldTf.gameObject.SetActive(true),
                    actionToLeaveEditorMode: () =>
                    {
                        fieldTf.gameObject.SetActive(false);
                        clearEditor();
                    },
                    actionToSave: (fileName, displayName) => saveIntoFile(fileName, displayName),
                    actionToClearAll: () => clearEditor(),
                    actionToLoadByFileName: (fileName) => loadFromFile(fileName)
                    );
                curStatus++;
                break;
            case 1:
                base.initMouseEvents(
                    actionForClickStart: (clickedTf, clickedPos) => onClickStart(clickedTf, clickedPos),
                    actionForLeftClickInPosition: (clickedTf) => onClickLeftInPlace(clickedTf),
                    actionForRightClickInPosition: (clickedTf) => onClickRightInPlace(clickedTf),
                    actionForLeftDrag: (clickedPos) => onDragLeft(clickedPos),
                    actionForRightDrag: (clickedPos) => onDragRight(clickedPos),
                    actionforLeftClickEnd: (clickedTf, clickedPos) => onClickLeftEnd(clickedTf, clickedPos),
                    actionforRightClickEnd: (clickedTf, clickedPos) => onClickRightEnd(clickedTf, clickedPos),
                    actionforStop: (lastMousePos) => onClickStop(lastMousePos)
                    );
                curStatus++;
                break;
        }
    }

    private void initSetting()
    {
        monsterCreateCancel = monsterCreateTf.GetChild(0).GetComponent<Button>();
        monsterCreateCancel.onClick.AddListener(() =>
        {
            monsterCreateTf.gameObject.SetActive(false);
        });
        // 티어 별 몬스터들을 위한 cellinfo 초기화
        foreach (List<MonsterDictionaryStat> monstersByTier in LocalDictionary.speicesByTier)
        {
            Dictionary<string, UnityAction> tempCells = new Dictionary<string, UnityAction>();
            if (monstersByTier != null)
            {
                foreach (MonsterDictionaryStat monster in monstersByTier)
                {
                    tempCells[$"{monster.name}"] = () =>
                    {
                        generateMonster(monster);
                        monsterCreateTf.gameObject.SetActive(false);
                    };
                }
            }
            cellInfosByTier.Add(tempCells);
        }
        monsterChoiceController = monsterCreateTf.GetChild(1).GetComponent<MoreChoiceController>();
        fieldTf.gameObject.SetActive(false);
        int i = 0;
        Transform slotcontainer = fieldTf.GetChild(3);
        int maxCnt = slotcontainer.childCount;
        while (i < maxCnt)
        {
            entrySlotControllers.Add(slotcontainer.GetChild(i++).GetComponent<EntrySlotController>());
        }
        curStatus = 0;
    }

    private void generateMonster(MonsterDictionaryStat monster)
    {
        Transform newMonster = Instantiate(contentPrefab);
        ContentController contentController = newMonster.GetComponent<ContentController>();
        contentController.initContent(GeneratorFunction.generateMonsterLiveStatFromDictionaryStat(monster), false);
        curClickedEntrySlotController.installMonster(contentController);
    }

    public void loadFromFile(string fileName)
    {

    }

    public void onClickLeftEnd(Transform clickedTf, Vector3 clickedPos)
    {

    }

    public void onClickLeftInPlace(Transform clickedTf)
    {

    }

    public void onClickRightEnd(Transform clickedTf, Vector3 clickedPos)
    {

    }

    public void onClickRightInPlace(Transform clickedTf)
    {
        if (clickedTf == null) return;
        if ((curClickedContentController = clickedTf.GetComponent<ContentController>()) != null)
        {
            // 몬스터 클릭
            base.openChoiceWindowWithOptions(new Dictionary<string, UnityAction>()
            {
                {
                    "삭제", () =>
                    {
                        base.closeChoiceWindowWithClear();
                        curClickedContentController.transform.parent.GetComponent<EntrySlotController>().truncateMonster(curClickedContentController);
                        curClickedContentController.destroySelf();
                        curClickedContentController = null;
                    }
                }
            });
            return;
        }
        if ((curClickedEntrySlotController = clickedTf.parent.GetComponent<EntrySlotController>()) != null)
        {
            // 엔트리 슬롯 클릭
            base.openChoiceWindowWithOptions(new Dictionary<string, UnityAction>()
            {
                {
                    "몬스터 생성: 1티어", () =>
                    {
                        base.closeChoiceWindowWithClear();
                        monsterChoiceController.initInfo(cellInfosByTier[1]);
                        monsterCreateTf.gameObject.SetActive(true);
                    }
                },
                {
                    "몬스터 생성: 2티어", () =>
                    {
                        base.closeChoiceWindowWithClear();
                        monsterChoiceController.initInfo(cellInfosByTier[2]);
                        monsterCreateTf.gameObject.SetActive(true);
                    }
                },
                {
                    "몬스터 생성: 3티어", () =>
                    {
                        base.closeChoiceWindowWithClear();
                        monsterChoiceController.initInfo(cellInfosByTier[3]);
                        monsterCreateTf.gameObject.SetActive(true);
                    }
                },
                {
                    "몬스터 생성: 4티어", () =>
                    {
                        base.closeChoiceWindowWithClear();
                        monsterChoiceController.initInfo(cellInfosByTier[4]);
                        monsterCreateTf.gameObject.SetActive(true);
                    }
                },
            });
            return;
        }
    }

    public void onClickStart(Transform clickedTf, Vector3 clickedPos)
    {
        if (!base.isClickMoreChoiceWindow(clickedTf))
        {
            base.closeChoiceWindowWithClear();
        }
    }

    public void onClickStop(Vector3 lastMousePos)
    {

    }

    public void onDragLeft(Vector3 clickedPos)
    {

    }

    public void onDragRight(Vector3 clickedPos)
    {

    }

    public void saveIntoFile(string fileName, string displayName)
    {

    }

    public void clearEditor()
    {
    }

}