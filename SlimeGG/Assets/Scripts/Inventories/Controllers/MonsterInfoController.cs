using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class MonsterInfoController : MonoBehaviour
{
    private Transform starSlot;
    private Image thumbImg;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI specieText;

    private TextMeshProUGUI descText;
    private Transform basicStatSlot;

    private Transform evolSlotTf;
    private Transform skillSlotTf;

    private bool isInit = false;
    private MonsterLiveStat monsterLiveStat;
    private Dictionary<BasicStatEnum, StatSlotController> basicStatControllers = new Dictionary<BasicStatEnum, StatSlotController>();

    private ExpModuleController expModuleController;
    private List<EvolutionCaseController> evolutionCaseControllers = new List<EvolutionCaseController>();
    private List<SkillInfoController> skillInfoControllers = new List<SkillInfoController>();

    [SerializeField]
    private Transform StatSlotPrefab;
    [SerializeField]
    private Transform ExpModulePrefab;
    [SerializeField]
    private EvolutionCaseController evolutionCaseControllerPrefab;
    [SerializeField]
    private SkillInfoController skillInfoControllerPrefab;
    [SerializeField]
    private Transform starPrefab;

    public Vector2 size = new Vector2(10f, 7f);

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
        } else
        {
            initSetting();
        }
    }

    public void initInfo(MonsterLiveStat monsterLiveStat)
    {
        this.monsterLiveStat = monsterLiveStat;
        initUpInfo();
        initMidInfo();
        initBotInfo();
    }

    private void initUpInfo()
    {
        nameText.text = $"{monsterLiveStat.dictionaryStat.displayName}";
        specieText.text = $"{monsterLiveStat.dictionaryStat.displayName}";
        descText.text = $"{monsterLiveStat.dictionaryStat.desc}";
        thumbImg.gameObject.SetActive(true);
        thumbImg.sprite = Resources.Load<Sprite>(
            $"{PathInfo.Monster.Sprite}{monsterLiveStat.saveStat.speicie}"
            );
        foreach (BasicStat basicStat in monsterLiveStat.dictionaryStat.basic)
        {
            basicStatControllers[basicStat.name].initBaseInfo(basicStat);
        }
        Dictionary<BasicStatEnum, BasicStat> temp = new Dictionary<BasicStatEnum, BasicStat>();
        foreach (ItemLiveStat itemStat in monsterLiveStat.itemStatList.Values)
        {
            foreach (BasicStat basicStat in itemStat.dictionaryStat.effect)
            {
                if (temp.ContainsKey(basicStat.name))
                {
                    temp[basicStat.name].amount += basicStat.amount;
                }
                else
                {
                    temp[basicStat.name] = new BasicStat(basicStat);
                }
            }
        }
        foreach (BasicStat basicStat in temp.Values)
        {
            basicStatControllers[basicStat.name].initCorrectionInfo(basicStat);
        }
        for (int i = 0; i < starSlot.childCount; i++)
        {
            Destroy(starSlot.GetChild(i).gameObject);
        }
        for (int i = 0; i < monsterLiveStat.dictionaryStat.tier; i++)
        {
            Transform newStar = Instantiate(starPrefab);
            newStar.transform.SetParent(starSlot);
            newStar.GetComponent<RectTransform>().localScale = Vector3.one;
            newStar.GetComponent<RectTransform>().sizeDelta = Vector2.one * MainGameManager.screenUnitSize * 0.4f;
        }
    }

    private void initMidInfo()
    {
        expModuleController.initInfo(monsterLiveStat.dictionaryStat.element, monsterLiveStat.saveStat.exp);
        foreach (EvolutionCaseController evolutionCaseController in evolutionCaseControllers)
        {
            evolutionCaseController.destorySelf();
        }
        evolutionCaseControllers = new List<EvolutionCaseController>();
        foreach (string nextMonster in monsterLiveStat.dictionaryStat.nextMonster)
        {
            EvolutionCaseController temp = Instantiate(evolutionCaseControllerPrefab);
            temp.transform.SetParent(evolSlotTf);
            temp.transform.localScale = Vector3.one;
            evolutionCaseControllers.Add(temp);
            temp.initInfo(nextMonster);
        }
    }

    private void initBotInfo()
    {
        foreach (SkillInfoController skillInfoController in skillInfoControllers)
        {
            skillInfoController.destorySelf();
        }
        skillInfoControllers = new List<SkillInfoController>();
        foreach (string skillName in monsterLiveStat.dictionaryStat.skills)
        {
            SkillInfoController temp = Instantiate(skillInfoControllerPrefab);
            temp.transform.SetParent(skillSlotTf);
            temp.transform.localScale = Vector3.one;
            skillInfoControllers.Add(temp);
            temp.initInfo(skillName);
        }
    }

    public void viewExpectation(MonsterLiveStat candidateMonsterLiveStat)
    {
        expModuleController.viewExpectation(candidateMonsterLiveStat.saveStat.exp);
    }

    private void initSetting()
    {
        Transform temp = transform.GetChild(0).GetChild(0);
        starSlot = temp.GetChild(0);
        thumbImg = temp.GetChild(1).GetComponent<Image>();
        thumbImg.gameObject.SetActive(false);
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

        temp = transform.GetChild(1);

        evolSlotTf = temp.GetChild(0);
        Transform expContainer = Instantiate(ExpModulePrefab);
        expModuleController = expContainer.GetComponent<ExpModuleController>();
        expContainer.SetParent(temp);
        expContainer.localScale = Vector3.one;
        expContainer.localPosition = Vector3.zero;

        skillSlotTf = transform.GetChild(2);

        isInit = true;
        adjustSize();
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
            MainGameManager.screenUnitSize * size.y / 3f);
        temp2.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Transform temp3 = temp2.GetChild(0);
        temp3.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * size.x / 5f,
            temp3.GetComponent<RectTransform>().sizeDelta.y);
        temp3 = temp2.GetChild(1);
        temp3.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * size.x / 5f * 4f,
            temp3.GetComponent<RectTransform>().sizeDelta.y);
        temp3.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            -MainGameManager.screenUnitSize * size.x / 5f * 4f,
            0f
            );

        // 중간
        temp2 = transform.GetChild(1);
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
            temp2.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenUnitSize * size.y / 3f);
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenUnitSize * size.y / 3f);

        // 중간
        temp2 = transform.GetChild(2);
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
            temp2.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenUnitSize * size.y / 3f);
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenUnitSize * size.y / 3f * 2f);

        adjustUpside();
        adjustMidSide();
        adjustBotSide();
    }

    private void adjustUpside()
    {
        starSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize / 4f * 5f,
            MainGameManager.screenUnitSize / 4f
            );
        starSlot.GetComponent<RectTransform>().anchoredPosition = Vector2.down * MainGameManager.screenUnitSize * 0.25f;
        starSlot.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * MainGameManager.screenUnitSize / 4f;
        starSlot.GetComponent<GridLayoutGroup>().spacing = Vector2.right * MainGameManager.screenUnitSize / 4f * 0.5f;

        thumbImg.GetComponent<RectTransform>().sizeDelta = Vector2.one * MainGameManager.screenUnitSize * 0.9f;
        thumbImg.GetComponent<RectTransform>().anchoredPosition = Vector2.down * MainGameManager.screenUnitSize * 0.5f;

        nameText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize / 4f * 5f,
            MainGameManager.screenUnitSize / 4f
            );
        nameText.GetComponent<RectTransform>().anchoredPosition = Vector2.down * MainGameManager.screenUnitSize * 1.6f;

        specieText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize / 4f * 5f,
            MainGameManager.screenUnitSize / 4f
            );
        specieText.GetComponent<RectTransform>().anchoredPosition = Vector2.down * MainGameManager.screenUnitSize * 1.9f;

        descText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 7f,
            MainGameManager.screenUnitSize * 0.8f
            );
        descText.GetComponent<RectTransform>().anchoredPosition = Vector2.down * MainGameManager.screenUnitSize * 0.1f;

        basicStatSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 7f,
            MainGameManager.screenUnitSize * 1.2f
            );
        basicStatSlot.GetComponent<RectTransform>().anchoredPosition = Vector2.down * MainGameManager.screenUnitSize * 1f;
        basicStatSlot.GetComponent<GridLayoutGroup>().spacing = Vector2.right * MainGameManager.screenUnitSize * 0.2f;
        basicStatSlot.GetComponent<GridLayoutGroup>().cellSize = new Vector2(
            MainGameManager.screenUnitSize * 2f,
            MainGameManager.screenUnitSize * 0.5f
            );
    }

    private void adjustMidSide()
    {
        expModuleController.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize,
            MainGameManager.screenUnitSize * 2f
            );
        expModuleController.GetComponent<RectTransform>().anchoredPosition = Vector2.right * MainGameManager.screenUnitSize * 0.5f;
        evolSlotTf.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 7f,
            MainGameManager.screenUnitSize * 2f
            );
        evolSlotTf.GetComponent<RectTransform>().anchoredPosition = Vector2.right * MainGameManager.screenUnitSize * 2.5f;
        evolSlotTf.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * MainGameManager.screenUnitSize * 2f;
        evolSlotTf.GetComponent<GridLayoutGroup>().spacing = Vector2.one * MainGameManager.screenUnitSize * 0.2f;
    }

    private void adjustBotSide()
    {
        skillSlotTf.GetComponent<GridLayoutGroup>().cellSize = new Vector2(
            MainGameManager.screenUnitSize * 4f,
            MainGameManager.screenUnitSize * 2f
            );
        skillSlotTf.GetComponent<GridLayoutGroup>().spacing = Vector2.right * MainGameManager.screenUnitSize * 2f / 3f;
        skillSlotTf.GetComponent<GridLayoutGroup>().padding = new RectOffset((int)((int)MainGameManager.screenUnitSize * 2f / 3f), 0, 0, 0);
    }

    public void truncateData()
    {
        nameText.text = $"";
        specieText.text = $"";
        descText.text = $"";
        thumbImg.gameObject.SetActive(false);
        thumbImg.sprite = null;
        for (int i = 0; i < starSlot.childCount; i++)
        {
            Destroy(starSlot.GetChild(i).gameObject);
        }
        foreach (StatSlotController statSlotController in basicStatControllers.Values)
        {
            statSlotController.initBaseInfo(null);
        }
        expModuleController.initInfo(null, null);
        foreach (EvolutionCaseController evolutionCaseController in evolutionCaseControllers)
        {
            evolutionCaseController.destorySelf();
        }
        evolutionCaseControllers = new List<EvolutionCaseController>();
        foreach (SkillInfoController skillInfoController in skillInfoControllers)
        {
            skillInfoController.destorySelf();
        }
        skillInfoControllers = new List<SkillInfoController>();
    }
}
