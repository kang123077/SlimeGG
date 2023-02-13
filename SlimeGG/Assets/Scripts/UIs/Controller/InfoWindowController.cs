using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoWindowController : MonoBehaviour
{
    bool isOpened = false;
    bool flipX = false, flipY = false;
    private bool isInit = false;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI descText;

    // Start is called before the first frame update
    void Start()
    {
        initSetting();
    }

    // Update is called once per frame
    void Update()
    {
        adjustPosition();
        if (isOpened)
        {
            trackMouse();
        }
    }

    private void initSetting()
    {
        titleText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        descText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        isInit = true;
    }

    public void openWindow()
    {
        trackMouse();
        isOpened = true;
        gameObject.SetActive(true);
    }

    public void closeWindow()
    {
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

    public void initInfo(string title, string desc)
    {
        if (!isInit) initSetting();
        titleText.text = title != null ? title : "";
        descText.text = desc != null ? desc : "";
    }
}
