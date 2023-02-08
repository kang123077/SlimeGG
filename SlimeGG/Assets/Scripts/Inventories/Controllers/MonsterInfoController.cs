using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInfoController : MonoBehaviour
{
    private MonsterSkillInfoController skillInfoController;

    private Transform starSlot;
    private Image thumbImg;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI specieText;

    private TextMeshProUGUI descText;
    private Transform basicStatSlot;

    private bool isInit = false;
    private MonsterLiveStat monsterLiveStat;
    private Dictionary<BasicStatEnum, StatSlotController> basicStatControllers = new Dictionary<BasicStatEnum, StatSlotController>();

    [SerializeField]
    private Transform StatSlotPrefab;

    // Start is called before the first frame update
    void Start()
    {
        skillInfoController = transform.GetChild(2).GetComponent<MonsterSkillInfoController>();
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

    public void initInfo(MonsterLiveStat monsterLiveStat)
    {
        this.monsterLiveStat = monsterLiveStat;
        initUpInfo();
    }

    private void initUpInfo()
    {
        nameText.text = $"{monsterLiveStat.dictionaryStat.displayName}";
        specieText.text = $"{monsterLiveStat.dictionaryStat.displayName}";
        descText.text = $"{monsterLiveStat.dictionaryStat.desc}";
        thumbImg.sprite = Resources.Load<Sprite>(
            $"{PathInfo.Monster.Sprite}{monsterLiveStat.saveStat.speicie}"
            );
        foreach (BasicStat basicStat in monsterLiveStat.dictionaryStat.basic)
        {
            basicStatControllers[basicStat.name].initInfo(basicStat);
        }
    }

    private void initSetting()
    {
        Transform temp = transform.GetChild(0).GetChild(0);
        starSlot = temp.GetChild(0);
        thumbImg = temp.GetChild(1).GetComponent<Image>();
        nameText = temp.GetChild(2).GetComponent<TextMeshProUGUI>();
        specieText = temp.GetChild(3).GetComponent<TextMeshProUGUI>();

        temp = transform.GetChild(0).GetChild(1);
        descText = temp.GetChild(0).GetComponent<TextMeshProUGUI>();
        basicStatSlot = temp.GetChild(1);

        Transform basicStatTf = Instantiate(StatSlotPrefab);
        basicStatTf.SetParent(basicStatSlot);
        basicStatControllers[BasicStatEnum.hp] = basicStatTf.GetComponent<StatSlotController>();

        basicStatTf = Instantiate(StatSlotPrefab);
        basicStatTf.SetParent(basicStatSlot);
        basicStatControllers[BasicStatEnum.atk] = basicStatTf.GetComponent<StatSlotController>();

        basicStatTf = Instantiate(StatSlotPrefab);
        basicStatTf.SetParent(basicStatSlot);
        basicStatControllers[BasicStatEnum.spd] = basicStatTf.GetComponent<StatSlotController>();

        basicStatTf = Instantiate(StatSlotPrefab);
        basicStatTf.SetParent(basicStatSlot);
        basicStatControllers[BasicStatEnum.def] = basicStatTf.GetComponent<StatSlotController>();

        basicStatTf = Instantiate(StatSlotPrefab);
        basicStatTf.SetParent(basicStatSlot);
        basicStatControllers[BasicStatEnum.timeCoolCycle] = basicStatTf.GetComponent<StatSlotController>();

        basicStatTf = Instantiate(StatSlotPrefab);
        basicStatTf.SetParent(basicStatSlot);
        basicStatControllers[BasicStatEnum.timeCastingCycle] = basicStatTf.GetComponent<StatSlotController>();

        isInit = true;
    }

    void adjustSize()
    {
        nameText.fontSize = MainGameManager.adjustFontSize;
        specieText.fontSize = MainGameManager.adjustFontSize;
        descText.fontSize = MainGameManager.adjustFontSize * 1.2f;
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
        adjustUpside();
        adjustMidSide();
    }

    private void adjustUpside()
    {
        starSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize / 3f * 5f,
            MainGameManager.screenUnitSize / 3f
            );
        starSlot.GetComponent<RectTransform>().anchoredPosition = Vector2.down * MainGameManager.screenUnitSize * 0.2f;
        starSlot.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * MainGameManager.screenUnitSize / 3f;
        starSlot.GetComponent<GridLayoutGroup>().spacing = Vector2.right * MainGameManager.screenUnitSize / 3f * 0.5f;

        thumbImg.GetComponent<RectTransform>().sizeDelta = Vector2.one * MainGameManager.screenUnitSize * 1.08f;
        thumbImg.GetComponent<RectTransform>().anchoredPosition = Vector2.down * MainGameManager.screenUnitSize * 0.7f;

        nameText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize / 3f * 5f,
            MainGameManager.screenUnitSize / 3f
            );
        nameText.GetComponent<RectTransform>().anchoredPosition = Vector2.down * MainGameManager.screenUnitSize * 2.2f;

        specieText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize / 3f * 5f,
            MainGameManager.screenUnitSize / 3f
            );
        specieText.GetComponent<RectTransform>().anchoredPosition = Vector2.down * MainGameManager.screenUnitSize * 2.5f;
    }

    private void adjustMidSide()
    {
        descText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 7f,
            MainGameManager.screenUnitSize * 1f
            );
        descText.GetComponent<RectTransform>().anchoredPosition = Vector2.down * MainGameManager.screenUnitSize * 0.2f;

        basicStatSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 7f,
            MainGameManager.screenUnitSize * 1.4f
            );
        basicStatSlot.GetComponent<RectTransform>().anchoredPosition = Vector2.down * MainGameManager.screenUnitSize * 1.4f;
        basicStatSlot.GetComponent<GridLayoutGroup>().spacing = Vector2.right * MainGameManager.screenUnitSize * 0.2f;
        basicStatSlot.GetComponent<GridLayoutGroup>().cellSize = new Vector2(
            MainGameManager.screenUnitSize * 2f,
            MainGameManager.screenUnitSize * 0.6f
            );
    }
}
