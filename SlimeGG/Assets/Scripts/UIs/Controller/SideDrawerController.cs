using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SideDrawerController : MonoBehaviour
{
    private Transform toggleButtonTf;
    private bool isOpen = false;
    [SerializeField]
    public bool isRight = false;
    [SerializeField]
    public bool isFull;
    [SerializeField]
    public bool isVertical = false;

    // Start is called before the first frame update
    void Start()
    {
        toggleButtonTf = transform.Find("Toggle Button");
    }

    // Update is called once per frame
    void Update()
    {
        if (isVertical)
        {
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(isFull ? 0 : 100f, Screen.height / 2);
            toggleButtonTf.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 10, Screen.height / 2 / 5);
            if (isOpen)
            {
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
            }
            else
            {
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, (isVertical ? -1f : -1f) * Screen.height / 2, 0f);
            }
            return;
        }
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 3, isFull ? transform.GetComponent<RectTransform>().sizeDelta.y : Screen.height / 5);
        toggleButtonTf.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 3 / 10, Screen.width / 3 / 10 * 2);
        if (isOpen)
        {
            transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
        }
        else
        {
            transform.GetComponent<RectTransform>().anchoredPosition = new Vector3((isRight ? 1f : -1f) * Screen.width / 3, 0f, 0f);
        }
    }

    public void toggleDrawer()
    {
        if (isOpen)
        {
            if (isVertical)
            {
                transform.localPosition = new Vector3(0f, (isVertical ? 1f : -1f) * Screen.width * 5 / 12, 0f);
            }
            else
            {
                transform.localPosition = new Vector3((isRight ? 1f : -1f) * Screen.width * 5 / 12, 0f, 0f);
            }
            isOpen = false;
        }
        else
        {
            if (isVertical)
            {
                transform.localPosition = new Vector3(0f, (isVertical ? 1f : -1f) * Screen.width / 4, 0f);
            }
            else
            {
                transform.localPosition = new Vector3((isRight ? 1f : -1f) * Screen.width / 2, 0f, 0f);
            }
            isOpen = true;
        }
    }
}
