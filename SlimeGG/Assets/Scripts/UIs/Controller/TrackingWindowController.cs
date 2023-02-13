using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingWindowController : MonoBehaviour
{
    public bool isOpened = false;
    bool flipX = false, flipY = false, isAnchor = false;

    void Start()
    {
        initSetting();
    }

    // Update is called once per frame
    void Update()
    {
        adjustPosition();
        if (!isAnchor)
        {
            trackMouse();
        }
    }

    private void initSetting()
    {
    }

    public void openWindow(bool isAnchor = false)
    {
        if (isAnchor)
        {
            this.isAnchor = isAnchor;
            trackMouse();
        }
        isOpened = true;
        gameObject.SetActive(true);
    }

    public void closeWindow()
    {
        isAnchor = false;
        isOpened = false;
        gameObject.SetActive(false);
    }

    private void trackMouse()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x + ((flipX ? -1f : 1f) * ((GetComponent<RectTransform>().sizeDelta.x / 2f) + 20f)),
            Input.mousePosition.y + ((flipY ? -1f : 1f) * ((GetComponent<RectTransform>().sizeDelta.y / 2f) + 20f)),
            Input.mousePosition.z
            ));
        transform.position = new Vector3(
            point.x,
            point.y,
            0f
            );
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            transform.localPosition.y,
            0f);
    }

    private void adjustPosition()
    {
        flipX = !((Screen.width - Input.mousePosition.x) > (GetComponent<RectTransform>().sizeDelta.x + 100f));
        flipY = !((Screen.height - Input.mousePosition.y) > (GetComponent<RectTransform>().sizeDelta.y + 100f));
    }
}
