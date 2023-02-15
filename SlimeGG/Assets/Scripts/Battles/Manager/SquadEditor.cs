using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquadEditor : BasicEditor, IBasicEditor
{
    [SerializeField]
    private Transform fieldTf, contentPrefab, monsterCreateTf;
    private int curStatus = -1;
    private Button monsterCreateCancel, monsterCreateConfirm;
    private MoreChoiceController monsterChoiceController;
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
        monsterCreateCancel = monsterCreateTf.GetChild(0).GetComponent<Button>();
        monsterCreateCancel.onClick.AddListener(() =>
        {
            monsterCreateTf.gameObject.SetActive(false);
        });

        monsterCreateConfirm = monsterCreateTf.GetChild(1).GetComponent<Button>();
        monsterChoiceController = monsterCreateTf.GetChild(2).GetComponent<MoreChoiceController>();
        fieldTf.gameObject.SetActive(false);
        curStatus = 0;
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
        
    }

    public void onClickStart(Transform clickedTf, Vector3 clickedPos)
    {
        
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

    public void clearAll()
    {

    }

}
