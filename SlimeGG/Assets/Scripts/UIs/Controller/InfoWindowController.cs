using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoWindowController : MonoBehaviour
{
    bool isOpened = false;
    Vector2 size = new Vector2(300f, 200f);
    bool flipX = false, flipY = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<RectTransform>().sizeDelta = size;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpened)
        {
            adjustPosition();
            trackMouse();
        }
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
            Input.mousePosition.x + ((flipX ? -1f : 1f) * ((size.x / 2f) + 20f)),
            Input.mousePosition.y + ((flipY ? -1f : 1f) * ((size.y / 2f) + 20f)),
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
        flipX = !((Screen.width - Input.mousePosition.x) > (size.x + 100f));
        flipY = !((Screen.height - Input.mousePosition.y) > (size.y + 100f));
    }
}
