using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfoController : MonoBehaviour
{

    private MonsterSkillInfoController skillInfoController;
    // Start is called before the first frame update
    void Start()
    {
        skillInfoController = transform.GetChild(2).GetComponent<MonsterSkillInfoController>();
    }

    // Update is called once per frame
    void Update()
    {
        adjustSize();
    }

    void adjustSize()
    {
        // 맨 위
        Transform temp2 = transform.GetChild(0);
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
            temp2.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenUnitSize * 3f);
        temp2.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Transform temp3 = temp2.GetChild(0);
        temp3.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 2f,
            temp3.GetComponent<RectTransform>().sizeDelta.y);
        temp3 = temp2.GetChild(1);
        temp3.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 8f,
            temp3.GetComponent<RectTransform>().sizeDelta.y);
        temp3.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            -MainGameManager.screenUnitSize * 8f,
            0f
            );

        // 중간
        temp2 = transform.GetChild(1);
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
            temp2.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenUnitSize * 3f);
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenUnitSize * 3f);
        temp3 = temp2.GetChild(0);
        temp3.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 1.5f,
            temp3.GetComponent<RectTransform>().sizeDelta.y);
        temp3 = temp2.GetChild(1);
        temp3.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 8.5f,
            temp3.GetComponent<RectTransform>().sizeDelta.y);
        temp3.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            -MainGameManager.screenUnitSize * 8.5f,
            0f
            );
    }
}
