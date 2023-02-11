using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ExpModuleController : MonoBehaviour
{
    [SerializeField]
    Transform expSinglePrefab;
    private TextMeshProUGUI expText;
    private GridLayoutGroup expContainer;
    private Dictionary<ElementEnum, ExpSingleController> curExpController = new Dictionary<ElementEnum, ExpSingleController>();

    private bool isInit = false;
    private float sizeRatio = 1f;
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
        expText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        expText.text = "";
        expContainer = transform.GetChild(1).GetComponent<GridLayoutGroup>();
        isInit = true;
        adjustSize();
    }

    private void adjustSize()
    {
        expText.fontSize = MainGameManager.adjustFontSize * sizeRatio;
        expText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            expText.GetComponent<RectTransform>().sizeDelta.x * sizeRatio,
            MainGameManager.screenUnitSize * 0.2f * sizeRatio
            );
        expContainer.GetComponent<RectTransform>().sizeDelta = Vector2.right *
            MainGameManager.screenUnitSize * sizeRatio;
        expContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            0f,
            -MainGameManager.screenUnitSize * 0.3f * sizeRatio
            );
        expContainer.cellSize = new Vector2(
            MainGameManager.screenUnitSize * sizeRatio,
            MainGameManager.screenUnitSize * 0.3f * sizeRatio
            );
        expContainer.spacing = Vector2.up * MainGameManager.screenUnitSize * 0.1f * sizeRatio;
    }

    public void initInfo(List<ElementEnum> elements, List<ElementStat> expStats)
    {
        if (!isInit) initSetting();
        if (elements == null)
        {
            expText.text = "";
            foreach (ExpSingleController expSingle in curExpController.Values)
            {
                expSingle.destoySelf();
            }
            curExpController = new Dictionary<ElementEnum, ExpSingleController>();
            return;
        }
        expText.text = "EXP";
        foreach (ExpSingleController expSingle in curExpController.Values)
        {
            expSingle.destoySelf();
        }
        curExpController = new Dictionary<ElementEnum, ExpSingleController>();
        Transform temp;
        foreach (ElementStat expStat in expStats)
        {
            temp = Instantiate(expSinglePrefab);
            curExpController[expStat.name] = temp.GetComponent<ExpSingleController>();
            temp.SetParent(expContainer.transform);
            temp.localScale = Vector3.one;
            temp.GetComponent<ExpSingleController>().initInfo(expStat);
            temp.GetComponent<ExpSingleController>().setSizeRatio(sizeRatio);
        }
    }

    public void viewExpectation(List<ElementStat> elementStats)
    {
        foreach (ElementStat elementStat in elementStats)
        {
            if (curExpController.ContainsKey(elementStat.name))
                curExpController[elementStat.name].initExpectAmount(elementStat.amount);
        }
    }

    public void setSizeRatio(float ratio)
    {
        this.sizeRatio = ratio;
    }
}
