using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FieldEditor : BasicEditor, IBasicEditor
{
    [SerializeField]
    private Transform fieldTf, entrySlotPrefab, EffectCreateTf;
    private int curStatus = -1;
    private Button buttonEffectCreateCancel, buttonEffectCreateConfirm;
    private TMP_InputField effectChoiceInput;
    private MoreChoiceController effectChoiceController;

    private BasicStat curDesignBasicStat;
    private FieldSaveStat fieldSaveStat;
    private PlaceableSlotController curClickedPlaceableSlotController;
    private EntrySlotController curClickedEntrySlotController;
    private List<PlaceableSlotController> mirroredPlaceableSlotControllers = new List<PlaceableSlotController>(), placeableSlotControllers = new List<PlaceableSlotController>();
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
                initSetting();
                break;
            case 0:
                base.setActions(
                    actionToEnterEditorMode: () => fieldTf.gameObject.SetActive(true),
                    actionToLeaveEditorMode: () =>
                    {
                        fieldTf.gameObject.SetActive(false);
                        clearAll();
                    },
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
        buttonEffectCreateCancel = EffectCreateTf.GetChild(0).GetComponent<Button>();
        buttonEffectCreateCancel.onClick.AddListener(() =>
        {
            EffectCreateTf.gameObject.SetActive(false);
        });

        buttonEffectCreateConfirm = EffectCreateTf.GetChild(1).GetComponent<Button>();
        effectChoiceController = EffectCreateTf.GetChild(2).GetComponent<MoreChoiceController>();
        effectChoiceInput = EffectCreateTf.GetChild(3).GetComponent<TMP_InputField>();
        fieldTf.gameObject.SetActive(false);
        Transform mirrorSlotsContainer = fieldTf.GetChild(4), originClotsContiner = fieldTf.GetChild(3);
        int cnt = mirrorSlotsContainer.childCount;
        int i = 0;
        while (i < cnt)
        {
            mirroredPlaceableSlotControllers.Add(mirrorSlotsContainer.GetChild(i).GetComponent<PlaceableSlotController>());
            mirroredPlaceableSlotControllers[i].setIsForDisplay(true);
            i++;
        }
        cnt = originClotsContiner.childCount;
        i = 0;
        while (i < cnt)
        {
            placeableSlotControllers.Add(originClotsContiner.GetChild(i).GetComponent<PlaceableSlotController>());
            placeableSlotControllers[i].setIsForDisplay(false);
            i++;
        }
        curStatus = 0;
    }

    private void generateNewTile(PlaceableSlotController targetSlotController, EntrySlotSaveStat saveStat = null)
    {
        // 정방향 생성 <- 오리지널
        Transform newEntrySlot = Instantiate(entrySlotPrefab);
        targetSlotController.installEntrySlot(newEntrySlot);
        EntrySlotController entrySlotController = newEntrySlot.GetComponent<EntrySlotController>();
        entrySlotControllers.Add(entrySlotController);

        // 역방향 생성 <- 미러
        newEntrySlot = Instantiate(entrySlotPrefab);
        mirroredPlaceableSlotControllers[findMirrorPosition(targetSlotController.getCoordinate())].installEntrySlot(newEntrySlot);

        if (saveStat != null)
        {
            entrySlotController.initEffects(saveStat.stats);
            newEntrySlot.GetComponent<EntrySlotController>().initEffects(saveStat.stats);
        }
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

    private void startDesignStat(BasicStatEnum basicStatEnum)
    {
        curDesignBasicStat = new BasicStat();
        curDesignBasicStat.name = basicStatEnum;
        effectChoiceInput.text = string.Empty;
    }

    private void tryApplyStatToTile(float amountInput)
    {
        curDesignBasicStat.amount = amountInput;
        curClickedEntrySlotController.addBasicStat(curDesignBasicStat);
        curDesignBasicStat = new BasicStat();
        closeEffetCreateWindow();
    }

    private void closeEffetCreateWindow()
    {
        effectChoiceInput.text = string.Empty;
        EffectCreateTf.gameObject.SetActive(false);
    }

    private void tryRemoveStatFromTile(BasicStat targetStat)
    {
        curClickedEntrySlotController.removeBasicStat(targetStat);
    }

    public void onClickStart(Transform clickedTf, Vector3 clickedPos)
    {
        if (base.isClickMoreChoiceWindow(clickedTf))
        {

        }
        else
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
                if (!curClickedEntrySlotController.transform.parent.GetComponent<PlaceableSlotController>().getIsForDisplay())
                {
                    Dictionary<string, UnityAction> temp = new Dictionary<string, UnityAction>()
                    {
                        {
                            "효과 부여", () =>
                            {
                                effectChoiceInput.text = string.Empty;
                                // 효과 부여용 창 오픈
                                effectChoiceController.initInfo(new Dictionary<string, UnityAction>()
                                    {
                                        {"체력: 합연산", () =>
                                            {
                                                startDesignStat(BasicStatEnum.hp);
                                            }
                                        },
                                        {"체력: 곱연산", () =>
                                            {
                                                startDesignStat(BasicStatEnum.hp_multiple);
                                            }
                                        },
                                        {"방어력: 합연산", () =>
                                            {
                                                startDesignStat(BasicStatEnum.def);
                                            }
                                        },
                                        {"방어력: 곱연산", () =>
                                            {
                                                startDesignStat(BasicStatEnum.def_multiple);
                                            }
                                        },
                                        {"공격력: 합연산", () =>
                                            {
                                                startDesignStat(BasicStatEnum.atk);
                                            }
                                        },
                                        {"공격력: 곱연산", () =>
                                            {
                                                startDesignStat(BasicStatEnum.atk_multiple);
                                            }
                                        },
                                        {"속도: 합연산 (기준: m/s)", () =>
                                            {
                                                startDesignStat(BasicStatEnum.spd);
                                            }
                                        },
                                        {"속도: 곱연산", () =>
                                            {
                                                startDesignStat(BasicStatEnum.spd_multiple);
                                            }
                                        },
                                        {"사정거리: 합연산", () =>
                                            {
                                                startDesignStat(BasicStatEnum.range);
                                            }
                                        },
                                        {"사정거리: 곱연산", () =>
                                            {
                                                startDesignStat(BasicStatEnum.range_multiple);
                                            }
                                        },
                                        {"쿨타임: 합연산 (기준: 초)", () =>
                                            {
                                                startDesignStat(BasicStatEnum.timeCoolCycle);
                                            }
                                        },
                                        {"쿨타임: 곱연산", () =>
                                            {
                                                startDesignStat(BasicStatEnum.timeCoolCycle_multiple);
                                            }
                                        },
                                        {"시전시간: 합연산 (기준: 초)", () =>
                                            {
                                                startDesignStat(BasicStatEnum.timeCastingCycle);
                                            }
                                        },
                                        {"시전시간: 곱연산", () =>
                                            {
                                                startDesignStat(BasicStatEnum.timeCastingCycle_multiple);
                                            }
                                        },
                                    }
                                );
                                buttonEffectCreateConfirm.onClick.RemoveAllListeners();
                                buttonEffectCreateConfirm.onClick.AddListener(() =>
                                {
                                    tryApplyStatToTile(float.Parse(effectChoiceInput.text));
                                });
                                EffectCreateTf.gameObject.SetActive(true);
                                base.closeChoiceWindowWithClear();
                            }
                        },
                    };
                    curClickedEntrySlotController.getBasicStats().ForEach((basicStat) =>
                    {
                        temp[$"효과 삭제: {translateEnumToStr(basicStat.name)} => {basicStat.amount}"] = () =>
                        {
                            tryRemoveStatFromTile(basicStat);
                            base.closeChoiceWindowWithClear();
                        };
                    });
                    temp["슬롯 삭제"] = () =>
                    {
                        removeEntrySlot(curClickedEntrySlotController);
                        base.closeChoiceWindowWithClear();
                    };
                    // 설치된 엔트리 슬롯 클릭
                    base.openChoiceWindowWithOptions(temp);
                    return;
                }
            }
        }
    }

    private string translateEnumToStr(BasicStatEnum basicStatEnum)
    {
        switch (basicStatEnum)
        {
            case BasicStatEnum.hp:
                return "체력: 합연산";
            case BasicStatEnum.hp_multiple:
                return "체력: 곱연산";
            case BasicStatEnum.def:
                return "방어력: 합연산";
            case BasicStatEnum.def_multiple:
                return "방어력: 곱연산";
            case BasicStatEnum.atk:
                return "공격력: 합연산";
            case BasicStatEnum.atk_multiple:
                return "공격력: 곱연산";
            case BasicStatEnum.spd:
                return "속도: 합연산";
            case BasicStatEnum.spd_multiple:
                return "속도: 곱연산";
            case BasicStatEnum.range:
                return "사거리: 합연산";
            case BasicStatEnum.range_multiple:
                return "사거리: 곱연산";
            case BasicStatEnum.timeCoolCycle:
                return "쿨타임: 합연산";
            case BasicStatEnum.timeCoolCycle_multiple:
                return "쿨타임: 곱연산";
            case BasicStatEnum.timeCastingCycle:
                return "시전시간: 합연산";
            case BasicStatEnum.timeCastingCycle_multiple:
                return "시전시간: 곱연산";
        }
        return string.Empty;
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
        FieldSaveStat saveStat = new FieldSaveStat();
        // 파일명 받기
        if (fileName == string.Empty)
        {
            saveStat.name = fieldSaveStat.name;
            saveStat.displayName = fieldSaveStat.displayName;
        }
        else
        {
            saveStat.name = fileName;
            saveStat.displayName = displayName;
        }
        foreach (EntrySlotController entrySlotController in entrySlotControllers)
        {
            EntrySlotSaveStat newStat = new EntrySlotSaveStat();
            newStat.x = entrySlotController.getCoordinate()[0];
            newStat.y = entrySlotController.getCoordinate()[1];
            newStat.stats = entrySlotController.getBasicStats();
            saveStat.entries.Add(newStat);
        }
        SaveFunction.saveField(saveStat.name, saveStat);
        clearAll();
    }

    public void loadFromFile(string fileName)
    {
        if (fileName == null || fileName.Length == 0) return;
        fieldSaveStat = CommonFunctions.loadObjectFromJson<FieldSaveStat>(
                    $"Assets/Resources/Jsons/Fields/{fileName}"
                    );
        foreach (EntrySlotSaveStat saveStat in fieldSaveStat.entries)
        {
            generateNewTile(placeableSlotControllers[(SettingVariables.Battle.entrySizeMax[0] * saveStat.y) + saveStat.x], saveStat);
        }
    }

    public override void clearAll()
    {
        fieldSaveStat = null;
        mirroredPlaceableSlotControllers.ForEach((controller) => controller.truncateEntrySlot());
        placeableSlotControllers.ForEach((controller) => controller.truncateEntrySlot());
        entrySlotControllers = new List<EntrySlotController>();
        curDesignBasicStat = null;
        curClickedEntrySlotController = null;
        curClickedPlaceableSlotController = null;
    }
}
