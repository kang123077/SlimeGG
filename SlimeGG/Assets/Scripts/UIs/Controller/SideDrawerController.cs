using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SideDrawerController : MonoBehaviour
{
    private Transform toggleButtonTf;
    private bool isOpen = true;
    private bool isArrived = true;
    [SerializeField]
    public bool isRight;
    [SerializeField]
    public bool isHeightFull;

    // Start is called before the first frame update
    void Start()
    {
        toggleButtonTf = transform.Find("Toggle Button");
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 3, isHeightFull ? transform.GetComponent<RectTransform>().sizeDelta.y : Screen.height / 5);
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
            transform.localPosition = new Vector3((isRight ? 1f : -1f) * Screen.width * 5 / 6, 0f, 0f);
            isOpen = false;
            isArrived = false;
        }
        else
        {
            transform.localPosition = new Vector3((isRight ? 1f : -1f) * Screen.width / 2, 0f, 0f);
            isOpen = true;
            isArrived = false;
        }
    }
}
