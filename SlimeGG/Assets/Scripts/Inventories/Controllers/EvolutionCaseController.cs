using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionCaseController : MonoBehaviour
{
    private Image thumbImg;
    [SerializeField]
    private ExpModuleController expModuleController;

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
        else
        {
            initSetting();
        }
    }

    private void initSetting()
    {
        if (isInit) return;
        thumbImg = transform.GetChild(0).GetComponent<Image>();
        expModuleController = Instantiate(expModuleController);
        expModuleController.transform.SetParent(transform);
        expModuleController.transform.localScale = Vector3.one;

        isInit = true;
        adjustSize();
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
    }
}
