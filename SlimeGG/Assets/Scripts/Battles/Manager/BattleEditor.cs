using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BattleEditor : BasicEditor, IBasicEditor
{
    private int curStatus = 0;
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
            case 0:
                base.setActions(
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

    public void onClickStart(Transform clickedTf, Vector3 clickedPos)
    {
        base.closeChoiceWindowWithClear();
    }

    public void onClickLeftInPlace(Transform clickedTf)
    {

    }

    public void onClickRightInPlace(Transform clickedTf)
    {
        base.openChoiceWindowWithOptions(new Dictionary<string, UnityAction>()
        {

        });
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

    }

    public void loadFromFile(string fileName)
    {

    }

    public void clearAll()
    {

    }
}
