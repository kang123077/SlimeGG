using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEventManager : MonoBehaviour
{
    public static Vector3 targetMousePos = Vector3.one * -1f;

    private bool isActive = false;
    private int curClickedMouseButton = -1;

    private System.Action<Vector3> actionForLeftDrag, actionForRightDrag;
    private System.Action<Transform> actionForClickStart, actionForLeftClick, actionForRightClick;
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
        System.Action<Transform> actionForClickStart,
        System.Action<Transform> actionForLeftClick,
        System.Action<Transform> actionForRightClick,
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
                    targetMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    curClickedMouseButton = 0;
                    actionForClickStart(getTransformBelowMouse());
                    return;
                }
                if (Input.GetMouseButton(1))
                {
                    targetMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    curClickedMouseButton = 1;
                    actionForClickStart(getTransformBelowMouse());
                    return;
                }
                break;
            case 0:
                if (Input.GetMouseButton(0))
                {
                    return;
                }
                if (Vector3.Distance(targetMousePos, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 0.1f)
                {
                    actionForLeftClick(getTransformBelowMouse());
                }
                else
                {
                    actionForLeftDrag(targetMousePos);
                }
                curClickedMouseButton = -1;
                break;
            case 1:
                if (Input.GetMouseButton(1))
                {
                    return;
                }
                if (Vector3.Distance(targetMousePos, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 0.1f)
                {
                    actionForRightClick(getTransformBelowMouse());
                }
                else
                {
                    actionForRightDrag(targetMousePos);
                }
                curClickedMouseButton = -1;
                break;
        }
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

    public void setActivation(bool activation)
    {
        isActive = activation;
    }

    public bool getActivation()
    {
        return isActive;
    }
}
