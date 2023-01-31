using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoWindowController : MonoBehaviour
{
    bool isOpened = false;
    Vector2 size = new Vector2(300f, 200f);
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
            Input.mousePosition.x + (size.x / 2f) + 20f,
            Input.mousePosition.y - (size.y / 2f) + 20f,
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
}
