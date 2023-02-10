using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSizeController : MonoBehaviour
{
    [SerializeField]
    public Vector2 sizeRatioToUnit, posRatioToUnit;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        adjustSize();
    }

    private void adjustSize()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * sizeRatioToUnit.x,
            MainGameManager.screenUnitSize * sizeRatioToUnit.y
            );
        GetComponent<RectTransform>().anchoredPosition = new Vector2(
            MainGameManager.screenUnitSize * posRatioToUnit.x,
            MainGameManager.screenUnitSize * posRatioToUnit.y
            );

        if (GetComponent<BoxCollider>() != null)
        {
            GetComponent<BoxCollider>().size = new Vector2(
            MainGameManager.screenUnitSize * sizeRatioToUnit.x,
            MainGameManager.screenUnitSize * sizeRatioToUnit.y
            );
        }
    }
}
