using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public void initInfo(Dictionary<string, System.Action<int>> cellInfos)
    {
        foreach (Transform choice in choices)
        {
            Destroy(choice.gameObject);
        }
        choices = new List<Transform>();
        foreach (KeyValuePair<string, System.Action<int>> cellInfo in cellInfos)
        {
            Transform newChoice = Instantiate(choicePrefab);
            newChoice.GetChild(0).GetComponent<TextMeshProUGUI>().text = cellInfo.Key;
            newChoice.GetComponent<Button>().onClick.AddListener(() =>
            {
                cellInfo.Value(0);
            });
            newChoice.SetParent(transform);
            newChoice.localScale = Vector3.one;
            newChoice.localPosition = new Vector3(
                0f, 0f, -1f);
            choices.Add(newChoice);
        }
    }

    private void OnMouseDown()
    {
        //Debug.Log("클릭 시작");
    }

    private void OnMouseUp()
    {
        //Debug.Log("클릭 종료");
    }
}
