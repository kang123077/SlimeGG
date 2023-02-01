using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    string keyToToggle;
    bool isActive = false;
    bool isAnimating = false;
    Transform slots;
    Vector2 screenSize;
    void Start()
    {
        getScreenSize();
        initSetting();
    }

    // Update is called once per frame
    void Update()
    {
        getScreenSize();
        adjustSize();
        trackCamera();
        checkKeyPress();
    }

    private void initSetting()
    {
        slots = transform.Find("Slots");
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.localPosition = new Vector3(0, screenSize.y, 1f);
        GetComponent<RectTransform>().sizeDelta = screenSize;
    }

    private void trackCamera()
    {
        transform.position = new Vector3(
            Camera.main.transform.position.x,
            transform.position.y,
            1f);
    }

    private void checkKeyPress()
    {
        if (Input.GetKeyDown(keyToToggle) && !isAnimating)
        {
            StartCoroutine(toggleCoroutine());
        }
    }

    private IEnumerator toggleCoroutine()
    {
        isActive = !isActive;
        LocalStorage.IS_CAMERA_FREE = false;
        isAnimating = true;
        while (isActive ? (transform.position.y > 0.01) : (transform.position.y < 9.99))
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate((isActive ? Vector3.down : Vector3.up) * (transform.position.y > 1 ? transform.position.y : 1f) * Time.deltaTime * 60);
        }
        transform.position = new Vector3(
            transform.position.x,
            isActive ? 0f : 10f,
            1f
            );
        isAnimating = false;
        LocalStorage.IS_CAMERA_FREE = !isActive;
    }

    void adjustSize()
    {
        GetComponent<RectTransform>().sizeDelta = screenSize;
        slots.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(screenSize.x * 4f / 16f, 0f);
        slots.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(screenSize.x * 12f / 16f, 0f);
        slots.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-screenSize.x * 12f / 16f, 0f);
        if (!isAnimating)
        {
            transform.localPosition = new Vector3(0, isActive ? 0f : screenSize.y, 1f);
        }
        Transform temp = slots.GetChild(0);

        Transform temp2 = temp.GetChild(0);
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
           temp2.GetComponent<RectTransform>().sizeDelta.x,
            screenSize.y * 0.05f);
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -screenSize.y * 0.05f);

        temp2 = temp.GetChild(1);
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
           temp2.GetComponent<RectTransform>().sizeDelta.x,
            screenSize.y * 0.3f);
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -screenSize.y * 0.1f);
        temp2.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * screenSize.y * 0.1f * 1.2f;
        temp2.GetComponent<GridLayoutGroup>().spacing = Vector2.one * screenSize.y * 0.1f * 0.2f;

        temp2 = temp.GetChild(2);
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
            temp2.GetComponent<RectTransform>().sizeDelta.x,
            screenSize.y * 0.05f);
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -screenSize.y * 0.4f);

        temp2 = temp.GetChild(3);
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
           temp2.GetComponent<RectTransform>().sizeDelta.x,
            screenSize.y * 0.15f);
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -screenSize.y * 0.45f);
        temp2.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * screenSize.y * 0.1f * 1.2f;
        temp2.GetComponent<GridLayoutGroup>().spacing = Vector2.one * screenSize.y * 0.1f * 0.2f;

        temp2 = temp.GetChild(4);
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
            temp2.GetComponent<RectTransform>().sizeDelta.x,
            screenSize.y * 0.05f);
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -screenSize.y * 0.6f);

        temp2 = temp.GetChild(5);
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
            temp2.GetComponent<RectTransform>().sizeDelta.x,
            screenSize.y * 0.3f);
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -screenSize.y * 0.65f);
        temp2.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * screenSize.y * 0.1f * 1.2f;
        temp2.GetComponent<GridLayoutGroup>().spacing = Vector2.one * screenSize.y * 0.1f * 0.2f;
    }

    void getScreenSize()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
    }
}
