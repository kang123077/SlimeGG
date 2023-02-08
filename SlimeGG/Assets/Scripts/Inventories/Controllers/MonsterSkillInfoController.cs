using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillInfoController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        adjustSize();
    }

    void adjustSize()
    {
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(
            transform.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenUnitSize * 3f);
        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenUnitSize * 6f);

        Transform temp2 = transform.GetChild(0);
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 5f,
            temp2.GetComponent<RectTransform>().sizeDelta.y
            );
        temp2.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        temp2 = transform.GetChild(1);
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 5f,
            temp2.GetComponent<RectTransform>().sizeDelta.y
            );
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(-MainGameManager.screenUnitSize * 5f, 0);
    }
}
