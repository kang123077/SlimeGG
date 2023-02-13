using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEventManager : MonoBehaviour
{
    private bool isActive = false;
    private int curClickedMouseButton = -1;
    private Vector3 initMousePos = Vector3.one * -1f;

    private System.Action<Vector3> actionForClickStart, actionForLeftClick, actionForRightClick, actionForLeftDrag, actionForRightDrag;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
            checkMouseEvent();
    }

    public void initSetting(
        System.Action<Vector3> actionForClickStart,
        System.Action<Vector3> actionForLeftClick,
        System.Action<Vector3> actionForRightClick,
        System.Action<Vector3> actionForLeftDrag,
        System.Action<Vector3> actionForRightDrag)
    {
        this.actionForClickStart = actionForClickStart;
        this.actionForLeftClick = actionForLeftClick;
        this.actionForRightClick = actionForRightClick;
        this.actionForLeftDrag = actionForLeftDrag;
        this.actionForRightDrag = actionForRightDrag;
    }

    private void checkMouseEvent()
    {
        switch (curClickedMouseButton)
        {
            case -1:
                if (Input.GetMouseButton(0))
                {
                    initMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    curClickedMouseButton = 0;
                    actionForClickStart(initMousePos);
                    return;
                }
                if (Input.GetMouseButton(1))
                {
                    initMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    curClickedMouseButton = 1;
                    actionForClickStart(initMousePos);
                    return;
                }
                break;
            case 0:
                if (Input.GetMouseButton(0))
                {
                    return;
                }
                if (Vector3.Distance(initMousePos, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 0.1f)
                {
                    actionForLeftClick(initMousePos);
                }
                else
                {
                    actionForLeftDrag(initMousePos);
                }
                curClickedMouseButton = -1;
                break;
            case 1:
                if (Input.GetMouseButton(1))
                {
                    return;
                }
                if (Vector3.Distance(initMousePos, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 0.1f)
                {
                    actionForRightClick(initMousePos);
                }
                else
                {
                    actionForRightDrag(initMousePos);
                }
                curClickedMouseButton = -1;
                break;
        }
    }

    public void setActivation(bool activation)
    {
        isActive = activation;
    }

    public bool getActivation()
    {
        return isActive;
    }
}
