using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoController : MonoBehaviour
{
    private Image thumbImg;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descText;
    private bool isInit = false;
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
    }

    private void initSetting()
    {
        if (isInit) return;
        thumbImg = transform.GetChild(0).GetComponent<Image>();
        nameText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        descText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        isInit = true;
    }

    private void adjustSize()
    {
        nameText.fontSize = MainGameManager.adjustFontSize;
        descText.fontSize = MainGameManager.adjustFontSize;
        thumbImg.GetComponent<RectTransform>().sizeDelta = Vector2.one * MainGameManager.screenUnitSize * 0.6f;
        thumbImg.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        nameText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 1.8f,
            MainGameManager.screenUnitSize * 0.3f
            );
        nameText.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            MainGameManager.screenUnitSize * 0.9f,
            -MainGameManager.screenUnitSize * 0.3f
            );

        descText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 3.5f,
            MainGameManager.screenUnitSize * 1.1f
            );
        descText.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            MainGameManager.screenUnitSize * 0.25f,
            -MainGameManager.screenUnitSize * 0.8f
            );
    }

    public void destorySelf()
    {
        Destroy(transform.gameObject);
    }

    public void initInfo(string skillName)
    {
        if (!isInit) initSetting();
        thumbImg.sprite = Resources.Load<Sprite>(
            $"{PathInfo.Skill.Sprite}{skillName}."
            );
        SkillStat skillStat = LocalDictionary.skills[skillName];
        nameText.text = skillStat.name;
        descText.text = skillStat.desc;
    }
}
