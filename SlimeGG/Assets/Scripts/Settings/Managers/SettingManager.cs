using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingManager : MonoBehaviour
{
    private bool isInit = false;
    private bool isActive = false;
    private bool isAnimating = false;

    // Start is called before the first frame update
    void Start()
    {
        initSetting();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInit)
        {
            adjustSize();
        }
        else
        {
            initSetting();
        }
    }

    private void initSetting()
    {
        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.up * MainGameManager.screenUnitSize * 9f;
        isInit = true;
        adjustSize();
    }

    private void adjustSize()
    {
        if (!isAnimating)
        {
            transform.GetComponent<RectTransform>().anchoredPosition =
                isActive
                ? Vector2.zero
                : Vector2.up * MainGameManager.screenUnitSize * 9f;
        }
        transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(
            MainGameManager.screenUnitSize * 4f,
            MainGameManager.screenUnitSize * 1f
            );
        transform.GetComponent<GridLayoutGroup>().spacing = new Vector2(
            MainGameManager.screenUnitSize * 1f,
            MainGameManager.screenUnitSize * 1f
            );
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = MainGameManager.adjustFontSize;
        }
    }

    private IEnumerator toggleCoroutine()
    {
        LocalStorage.UIOpenStatus.setting = !isActive;
        isActive = !isActive;
        isAnimating = true;
        while (isActive
            ? (transform.GetComponent<RectTransform>().anchoredPosition.y > 1)
            : (MainGameManager.screenUnitSize * 9f - transform.GetComponent<RectTransform>().anchoredPosition.y > 1))
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(
                    (
                        isActive
                        ? (Vector3.down)
                        : (Vector3.up)
                    )
                    * (
                        isActive
                        ? (transform.GetComponent<RectTransform>().anchoredPosition.y)
                        : (MainGameManager.screenUnitSize * 9f - transform.GetComponent<RectTransform>().anchoredPosition.y)
                    )
                    * 0.01f
                    * SettingVariables.slideToggleSpd
                );
        }
        isAnimating = false;
    }

    public void toggle()
    {
        StartCoroutine(toggleCoroutine());
    }
}
