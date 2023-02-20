using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionCaseController : MonoBehaviour
{
    [SerializeField]
    private ExpModuleController expModuleController;
    private Image thumbImg, backgroundImg;
    private Button evolButton;
    private List<ElementStat> elementStats;

    private bool isInit = false, isEvolable = false;
    private InventoryManager inventoryManager;
    private ContentController contentController;
    Color evolColor = Color.yellow;
    private string specieName;
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
        if (isInit) return;
        thumbImg = transform.GetChild(0).GetComponent<Image>();
        backgroundImg = transform.GetChild(1).GetComponent<Image>();
        evolButton = transform.GetChild(1).GetComponent<Button>();
        backgroundImg.gameObject.SetActive(false);
        expModuleController = Instantiate(expModuleController);
        expModuleController.transform.SetParent(transform);
        expModuleController.transform.localScale = Vector3.one;
        expModuleController.transform.localPosition = Vector3.zero;

        isInit = true;
        adjustSize();
        StartCoroutine(glitterImage());
    }

    private void adjustSize()
    {
        thumbImg.GetComponent<RectTransform>().sizeDelta = Vector2.one * MainGameManager.screenUnitSize * 0.8f;
        thumbImg.GetComponent<RectTransform>().anchoredPosition = Vector2.right * MainGameManager.screenUnitSize * 0.1f;

        expModuleController.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 0.9f,
            MainGameManager.screenUnitSize * 0.9f * 2f
            );
        expModuleController.GetComponent<RectTransform>().anchoredPosition = Vector2.right * MainGameManager.screenUnitSize * 1.2f;
    }

    public void destorySelf()
    {
        Destroy(transform.gameObject);
    }

    public void initInfo(string specieName)
    {
        if (!isInit) initSetting();
        MonsterDictionaryStat nextSpecie = LocalDictionary.speicies[specieName];
        thumbImg.sprite = Resources.Load<Sprite>(
            $"{PathInfo.Monster.Sprite}{specieName}"
            );
        expModuleController.initInfo(nextSpecie.element, nextSpecie.elementEvol);
        expModuleController.setSizeRatio(0.8f);
        elementStats = nextSpecie.elementEvol;
        this.specieName = specieName;
    }

    private IEnumerator glitterImage()
    {
        float minOpacity = 0.0f, maxOpacity = 0.3f, curOpacity = 0.0f;
        bool isIncrement = true;
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            if (isEvolable ^ backgroundImg.gameObject.activeSelf)
            {
                backgroundImg.gameObject.SetActive(isEvolable);
            }

            if (isIncrement)
            {
                curOpacity += 0.01f / SettingVariables.fadeToggleSpd;
            }
            else
            {
                curOpacity -= 0.01f / SettingVariables.fadeToggleSpd;
            }
            backgroundImg.color = new Color(evolColor.r, evolColor.g, evolColor.b, curOpacity);
            if (curOpacity >= maxOpacity || curOpacity <= minOpacity)
            {
                isIncrement = !isIncrement;
            }
        }
    }

    public void checkIfEvolable(List<ElementStat> compareStats)
    {
        evolButton.onClick.RemoveAllListeners();
        foreach (ElementStat elementStat in elementStats)
        {
            ElementStat temp;
            // 조건 원소가 없음
            if ((temp = Array.Find<ElementStat>(compareStats.ToArray(), (stat) => { return stat.name == elementStat.name; })) == null)
            {
                isEvolable = false;
                return;
            }
            // 조건 수치 미달
            if (temp.amount < elementStat.amount)
            {
                isEvolable = false;
                return;
            }
        }
        isEvolable = true;
        // 버튼 기능 부여 -> 진화
        evolButton.onClick.AddListener(() =>
        {
            inventoryManager.evolveMonster(contentController, specieName);
        }
        );
        return;
    }

    public void setInventoryManager(InventoryManager inventoryManager)
    {
        this.inventoryManager = inventoryManager;
    }

    public void setContentController(ContentController contentController)
    {
        this.contentController = contentController;
    }
}
