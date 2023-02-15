using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MoreChoiceController : MonoBehaviour
{
    [SerializeField]
    private Transform choicePrefab;
    private int curStatus = 0;
    private List<Transform> choices = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (curStatus)
        {
            case 0:
                initSetting();
                break;
            case 1:
                break;
        }
    }

    public void initSetting()
    {
        curStatus = 1;
    }

    public void initInfo(Dictionary<string, UnityAction> cellInfos)
    {
        foreach (Transform choice in choices)
        {
            Destroy(choice.gameObject);
        }
        choices = new List<Transform>();
        if (cellInfos == null) return;
        foreach (KeyValuePair<string, UnityAction> cellInfo in cellInfos)
        {
            Transform newChoice = Instantiate(choicePrefab);
            if (newChoice.childCount != 0)
            {
                newChoice.GetChild(0).GetComponent<TextMeshProUGUI>().text = cellInfo.Key;
            }
            else
            {
                newChoice.GetComponent<Image>().sprite = Resources.Load<Sprite>($"{PathInfo.Monster.Sprite}{cellInfo.Key}");
            }
            newChoice.GetComponent<Button>().onClick.AddListener(() =>
            {
                cellInfo.Value();
            });
            newChoice.SetParent(transform);
            newChoice.localScale = Vector3.one;
            newChoice.localPosition = new Vector3(
                0f, 0f, -1f);
            choices.Add(newChoice);
        }
    }
}
