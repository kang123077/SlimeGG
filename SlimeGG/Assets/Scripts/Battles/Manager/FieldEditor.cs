using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FieldEditor : BasicEditor, IBasicEditor
{
    [SerializeField]
    private Transform fieldTf, entrySlotPrefab;
    private List<PlaceableSlotController> mirroredPlaceableSlotControllers = new List<PlaceableSlotController>();
    private PlaceableSlotController curClickedPlaceableSlotController;
    private EntrySlotController curClickedEntrySlotController;
    private int curStatus = -1;
    private List<EntrySlotController> entrySlotControllers = new List<EntrySlotController>();
    protected virtual void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        base.Update();
        switch (curStatus)
        {
            case -1:
                initSetting();
                break;
            case 0:
                base.setActions(
                    actionToEnterEditorMode: () => fieldTf.gameObject.SetActive(true),
                    actionToLeaveEditorMode: () => fieldTf.gameObject.SetActive(false),
                    actionToSave: (fileName, displayName) => saveIntoFile(fileName, displayName),
                    actionToClearAll: () => clearAll(),
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
        fieldTf.gameObject.SetActive(false);
        Transform mirrorSlotsContainer = fieldTf.GetChild(4);
        int cnt = mirrorSlotsContainer.childCount;
        int i = 0;
        while (i < cnt)
        {
            mirroredPlaceableSlotControllers.Add(mirrorSlotsContainer.GetChild(i).GetComponent<PlaceableSlotController>());
            mirroredPlaceableSlotControllers[i].setIsForDisplay(true);
            i++;
        }
        curStatus = 0;
    }

    private void generateNewTile(PlaceableSlotController targetSlotController)
    {
        // 정방향 생성 <- 오리지널
        Transform newEntrySlot = Instantiate(entrySlotPrefab);
        targetSlotController.installEntrySlot(newEntrySlot);
        EntrySlotController entrySlotController = newEntrySlot.GetComponent<EntrySlotController>();
        entrySlotControllers.Add(entrySlotController);

        // 역방향 생성 <- 미러
        newEntrySlot = Instantiate(entrySlotPrefab);
        mirroredPlaceableSlotControllers[findMirrorPosition(targetSlotController.getCoordinate())].installEntrySlot(newEntrySlot);
    }

    private int findMirrorPosition(int[] coordinate)
    {
        int[] c = SettingVariables.Battle.entrySizeMax;
        int maxX = c[0], maxY = c[1];
        return (coordinate[1] * maxX) + (maxX - 1 - coordinate[0]);
    }

    private void removeEntrySlot(EntrySlotController targetEntrySlotController)
    {
        entrySlotControllers.Remove(targetEntrySlotController);
        PlaceableSlotController mirrorPlaceableController = mirroredPlaceableSlotControllers[findMirrorPosition(targetEntrySlotController.getCoordinate())];
        EntrySlotController mirrorToRemove = mirrorPlaceableController.getInstalledEntrySlotController();
        mirrorPlaceableController.uninstallEntrySlot();
        mirrorToRemove.destroySelf();
        targetEntrySlotController.destroySelf();
    }

    public void onClickStart(Transform clickedTf, Vector3 clickedPos)
    {
        if (base.isClickMoreChoiceWindow(clickedTf))
        {

        } else
        {
            base.closeChoiceWindowWithClear();
        }
    }

    public void onClickLeftInPlace(Transform clickedTf)
    {

    }

    public void onClickRightInPlace(Transform clickedTf)
    {
        if (clickedTf == null)
        {
            // 빈공간 -> 패스
        }
        else
        {
            // 뭔가 클릭함
            if ((curClickedPlaceableSlotController = clickedTf.GetComponent<PlaceableSlotController>()) != null)
            {
                // 배치 가능 타일 클릭
                if (curClickedPlaceableSlotController.getIsAvailable())
                {
                    // 비어있음 -> 새로운 타일 생성 옵션 떠야 함
                    base.openChoiceWindowWithOptions(new Dictionary<string, UnityAction>()
                    {
                        { "신규 타일 배치", () =>
                            {
                                generateNewTile(curClickedPlaceableSlotController);
                                base.closeChoiceWindowWithClear();
                            }
                        }
                    });
                }
                return;
            }
            if ((curClickedEntrySlotController = clickedTf.parent.GetComponent<EntrySlotController>()) != null)
            {
                // 설치된 엔트리 슬롯 클릭
                base.openChoiceWindowWithOptions(new Dictionary<string, UnityAction>()
                    {
                        { "삭제", () =>
                            {
                                removeEntrySlot(curClickedEntrySlotController);
                                base.closeChoiceWindowWithClear();
                            }
                        }
                    });
                return;
            }
        }
    }

    public void onDragLeft(Vector3 clickedPos)
    {

    }

    public void onDragRight(Vector3 clickedPos)
    {

    }

    public void onClickLeftEnd(Transform clickedTf, Vector3 clickedPos)
    {

    }

    public void onClickRightEnd(Transform clickedTf, Vector3 clickedPos)
    {

    }

    public void onClickStop(Vector3 lastMousePos)
    {

    }

    public void saveIntoFile(string fileName, string displayName)
    {
        FieldSaveStat fieldSaveStat = new FieldSaveStat();
        // 파일명 받기
        fieldSaveStat.name = fileName;
        fieldSaveStat.displayName = displayName;
        foreach (EntrySlotController entrySlotController in entrySlotControllers)
        {
            EntrySlotSaveStat newStat = new EntrySlotSaveStat();
            newStat.x = entrySlotController.x;
            newStat.y = entrySlotController.y;
            fieldSaveStat.entries.Add(newStat);
        }
        SaveFunction.saveField(fieldSaveStat.name, fieldSaveStat);
    }

    public void loadFromFile(string fileName)
    {

    }

    public void clearAll()
    {

    }
}
