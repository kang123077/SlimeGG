using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoreChoiceController : MonoBehaviour
{
    private int curStatus = 0;
    private List<Transform> childs = new List<Transform>();
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
        gameObject.AddComponent<GridSizeController>();
        gameObject.GetComponent<GridSizeController>().initInfo(
            new Vector2(
            GetComponent<ObjectSizeController>().sizeRatioToUnit.x - 0.2f,
            0.3f
            ),
            Vector2.one * 0.1f,
            TextAnchor.UpperLeft,
            1
            );
        curStatus = 1;
    }

    public void initInfo(List<string> cellNames, List<System.Action<int>> cellActions)
    {
        foreach (Transform child in childs)
        {
            Destroy(child.gameObject);
        }
        childs = new List<Transform>();
        for (int i = 0; i < cellNames.Count; i++)
        {
            GameObject newCell = new GameObject();
            GameObject newText = new GameObject();
            newCell.AddComponent<Image>();
            newText.AddComponent<RectTransform>();
            newText.AddComponent<ObjectSizeController>();
            newText.AddComponent<TextMeshProUGUI>();

            newText.gameObject.AddComponent<FontResizeController>();
            newText.GetComponent<FontResizeController>().fonsSizeRatioToUnit = 1;
            newText.transform.SetParent(newCell.transform);
            newText.transform.localScale = Vector3.one;
            newText.transform.localPosition = Vector3.zero;
            newText.GetComponent<TextMeshProUGUI>().text = cellNames[i];
            newText.GetComponent<TextMeshProUGUI>().color = Color.black;

            newCell.AddComponent<Button>();
            newCell.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("클릭");
                cellActions[i](0);
            });
            newCell.transform.SetParent(transform);
            newCell.transform.localScale = Vector3.one;
            newText.transform.GetComponent<ObjectSizeController>().sizeRatioToUnit = new Vector2(4.3f, 0.3f);
            childs.Add(newCell.transform);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("클릭 시작");
    }

    private void OnMouseUp()
    {
        Debug.Log("클릭 종료");
    }
}
