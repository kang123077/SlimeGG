using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BasicEditor : MonoBehaviour
{
    [SerializeField]
    MoreChoiceController moreChoiceController;
    [SerializeField]
    Transform saveInputTf, loadInputTf, initialBtnsTf, saveButtonsTf;

    TMP_InputField inputLoadFile, inputSaveFileName, inputSaveDisplayName;
    Button buttonInitialNew, buttonInitialLoad,
        buttonLoadConfirm, buttonLoadCancel,
        buttonSaveConfirm, buttonSaveCancel,
        buttonEditSaveInOrigin, buttonEditSaveInNew, buttonEditClear, buttonEditQuit;

    protected MouseEventManager mouseEventManager;
    protected int baseStatus = 0;
    protected bool isNew = false;

    private bool isMouseEventApplied, isActionSet;
    private UnityAction actionToClearAll, actionToEnterEditorMode, actionToLeaveEditorMode;
    private System.Action<string> actionToLoadByFileName;
    private System.Action<string, string> actionToSave;
    // Start is called before the first frame update
    protected virtual void Start()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        switch (baseStatus)
        {
            case 0:
                // 기본 컴포넌트 찾아서 연결
                setComponents();
                break;
            case -1:
                // 기본 액션 선언 + 마우스 이벤트 선언까지 대기
                if (isActionSet && isMouseEventApplied)
                {
                    applyFunctionsToButtons();
                    openMainMenu();
                }
                break;
            case 1:
                // 새 에디터 | 불러오기 선택 대기중
                break;
            case 2:
                // 에디팅중
                break;
        }
    }

    protected void setActions(
        UnityAction actionToClearAll,
        UnityAction actionToEnterEditorMode,
        UnityAction actionToLeaveEditorMode,
        System.Action<string> actionToLoadByFileName,
        System.Action<string, string> actionToSave
        )
    {
        this.actionToClearAll = actionToClearAll;
        this.actionToEnterEditorMode = actionToEnterEditorMode;
        this.actionToLeaveEditorMode = actionToLeaveEditorMode;
        this.actionToLoadByFileName = actionToLoadByFileName;
        this.actionToSave = actionToSave;
        isActionSet = true;
    }
    protected void initMouseEvents(
        System.Action<Vector3> actionforStop,
        System.Action<Transform?, Vector3> actionForClickStart,
        System.Action<Transform?> actionForLeftClickInPosition,
        System.Action<Transform?> actionForRightClickInPosition,
        System.Action<Vector3> actionForLeftDrag,
        System.Action<Vector3> actionForRightDrag,
        System.Action<Transform?, Vector3> actionforRightClickEnd,
        System.Action<Transform?, Vector3> actionforLeftClickEnd
        )
    {
        mouseEventManager = GetComponent<MouseEventManager>();
        mouseEventManager.initSetting(
            actionforStop: actionforStop,
            actionForClickStart: actionForClickStart,
            actionForLeftClick: actionForLeftClickInPosition,
            actionForRightClick: actionForRightClickInPosition,
            actionForLeftDrag: actionForLeftDrag,
            actionForRightDrag: actionForRightDrag,
            actionforLeftClickEnd: actionforLeftClickEnd,
            actionforRightClickEnd: actionforRightClickEnd);
        mouseEventManager.setActivation(true);
        isMouseEventApplied = true;
    }
    private void setComponents()
    {
        // 새로 만들기, 불러오기
        buttonInitialLoad = initialBtnsTf.GetChild(0).GetComponent<Button>();
        buttonInitialNew = initialBtnsTf.GetChild(1).GetComponent<Button>();

        // 저장하기, 다른 이름으로 저장하기, 전부 지우기(초기화), 나가기
        buttonEditClear = saveButtonsTf.GetChild(0).GetComponent<Button>();
        buttonEditSaveInNew = saveButtonsTf.GetChild(1).GetComponent<Button>();
        buttonEditSaveInOrigin = saveButtonsTf.GetChild(2).GetComponent<Button>();
        buttonEditQuit = saveButtonsTf.GetChild(3).GetComponent<Button>();

        // 불러오기 확정, 불러오기 취소
        inputLoadFile = loadInputTf.GetChild(0).GetComponent<TMP_InputField>();
        buttonLoadCancel = loadInputTf.GetChild(1).GetComponent<Button>();
        buttonLoadConfirm = loadInputTf.GetChild(2).GetComponent<Button>();

        // 다른 이름으로 저장하기 확정, 취소 
        inputSaveFileName = saveInputTf.GetChild(0).GetComponent<TMP_InputField>();
        inputSaveDisplayName = saveInputTf.GetChild(1).GetComponent<TMP_InputField>();
        buttonSaveCancel = saveInputTf.GetChild(2).GetComponent<Button>();
        buttonSaveConfirm = saveInputTf.GetChild(3).GetComponent<Button>();
        baseStatus = -1;
    }

    private void applyFunctionsToButtons()
    {
        // 새로 만들기
        buttonInitialNew.onClick.AddListener(() =>
        {
            startNewFile();
            actionToEnterEditorMode();
        });
        // 불러오기
        buttonInitialLoad.onClick.AddListener(() =>
        {
            openLoadInput();
        });

        // 불러오기 취소
        buttonLoadCancel.onClick.AddListener(() =>
        {
            loadInputTf.gameObject.SetActive(false);
        });
        // 불러오기 확인
        buttonLoadConfirm.onClick.AddListener(() =>
        {
            loadFile();
        });

        // 다른 이름으로 저장하기 취소
        buttonSaveCancel.onClick.AddListener(() =>
        {
            cancelSaving();
        });
        // 다른 이름으로 저장하기 확인
        buttonSaveConfirm.onClick.AddListener(() =>
        {
            confirmSaving();
            returntoMainMenu();
        });

        // 전부 지우기 = 초기화
        buttonEditClear.onClick.AddListener(() =>
        {
            clearAll();
        });
        // 다른 이름으로 저장하기 버튼 클릭
        buttonEditSaveInNew.onClick.AddListener(() =>
        {
            openSavingInput();
        });
        // 기존 파일에 덮어쓰기 버튼 클릭
        buttonEditSaveInOrigin.onClick.AddListener(() =>
        {
            confirmSaving();
            returntoMainMenu();
        });
        // 나가기 버튼 클릭
        buttonEditQuit.onClick.AddListener(() =>
        {
            MainGameManager.exitGame();
        });
        baseStatus = 1;
    }
    protected void openChoiceWindowWithOptions(Dictionary<string, UnityAction> choiceOptions)
    {
        moreChoiceController.initInfo(choiceOptions);
        moreChoiceController.GetComponent<TrackingWindowController>().openWindow(true);
    }

    protected void closeChoiceWindowWithClear()
    {
        moreChoiceController.GetComponent<TrackingWindowController>().closeWindow();
        moreChoiceController.initInfo(null);
    }

    public void startNewFile()
    {
        isNew = true;
        clearAll();
        initialBtnsTf.gameObject.SetActive(false);
    }

    public void returntoMainMenu()
    {
        actionToLeaveEditorMode();
        clearAll();
        openMainMenu();
        baseStatus = 1;
    }
    private void openMainMenu()
    {
        saveInputTf.gameObject.SetActive(false);
        loadInputTf.gameObject.SetActive(false);
        saveButtonsTf.gameObject.SetActive(false);
        initialBtnsTf.gameObject.SetActive(true);
    }
    public void clearAll()
    {
        // 전체 비우기
        saveButtonsTf.gameObject.SetActive(true);
        actionToClearAll();
        baseStatus = 2;
    }

    public void loadFile()
    {
        isNew = false;
        string searchFileName = inputLoadFile.text;
        if (searchFileName == null || searchFileName.Length == 0) return;
        // 파일 정보 불러오기
        actionToLoadByFileName(searchFileName);
        loadInputTf.gameObject.SetActive(false);
        initialBtnsTf.gameObject.SetActive(false);
        actionToEnterEditorMode();
        baseStatus = 2;
    }

    public void openLoadInput()
    {
        // 클릭: 파일 불러오기 클릭
        loadInputTf.gameObject.SetActive(true);
    }

    public void openSavingInput()
    {
        // 클릭: 다른 이름으로 저장하기
        saveInputTf.gameObject.SetActive(true);
    }

    public void confirmSaving()
    {
        // 클릭: 저장하기 클릭
        string inputName;
        string displayName;
        if (isNew)
        {
            inputName = inputSaveFileName.text;
            displayName = inputSaveDisplayName.text;
        }
        else
        {
            // 기존 파일 이름 복사
            inputName = string.Empty;
            displayName = string.Empty;
        }
        if (inputName == null || displayName == null || inputName.Length < 2 || displayName.Length < 2) return;
        actionToSave(inputName, displayName);
        returntoMainMenu();
    }

    public void cancelSaving()
    {
        saveInputTf.gameObject.SetActive(false);
        inputSaveFileName.text = string.Empty;
        inputSaveDisplayName.text = string.Empty;
    }
}
