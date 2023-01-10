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
    [SerializeField]
    public float ratioStick = 0.5f;
    [SerializeField]
    public float ratioOpen = 0.3f;

    private static Vector2 fullSize = new Vector2(1920f, 1080f);

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
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(
                isFull 
                ? 0 
                : fullSize.x * ratioStick,
                fullSize.y * ratioOpen
            );
            toggleButtonTf.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 20, Screen.width / 20);
            if (isOpen)
            {
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
            }
            else
            {
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, (isVertical ? -1f : -1f) * fullSize.y * ratioOpen);
            }
            return;
        }
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(
            fullSize.x * ratioOpen, 
            isFull 
            ? fullSize.y
            : fullSize.y * ratioStick
        );
        toggleButtonTf.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 20, Screen.width / 20);
        if (isOpen)
        {
            transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
        }
        else
        {
            transform.GetComponent<RectTransform>().anchoredPosition = new Vector3((isRight ? 1f : -1f) * fullSize.x * ratioOpen, 0f, 0f);
        }
    }

    public void toggleDrawer()
    {
        if (isOpen)
        {
            if (isVertical)
            {
                transform.localPosition = new Vector3(0f, (isVertical ? 1f : -1f) * fullSize.x * ratioOpen, 0f);
            }
            else
            {
                transform.localPosition = new Vector3((isRight ? 1f : -1f) * fullSize.x * ratioOpen, 0f, 0f);
            }
            isOpen = false;
        }
        else
        {
            if (isVertical)
            {
                transform.localPosition = new Vector3(0f, (isVertical ? 1f : -1f) * fullSize.x * ratioOpen);
            }
            else
            {
                transform.localPosition = new Vector3((isRight ? 1f : -1f) * fullSize.x * ratioOpen, 0f, 0f);
            }
            isOpen = true;
        }
    }
}
