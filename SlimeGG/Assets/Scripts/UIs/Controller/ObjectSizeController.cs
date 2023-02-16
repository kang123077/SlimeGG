using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSizeController : MonoBehaviour
{
    [SerializeField]
    public Vector2 sizeRatioToUnit, posRatioToUnit;
    [SerializeField]
    private bool isFixed = true, isRatioToUnit = true;
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
        float unitSize = isRatioToUnit ? MainGameManager.screenUnitSize : 1f;
        GetComponent<RectTransform>().sizeDelta = new Vector2(
            unitSize * sizeRatioToUnit.x,
            unitSize * sizeRatioToUnit.y
            );
        if (GetComponent<BoxCollider>() != null)
        {
            GetComponent<BoxCollider>().size = new Vector3(
            unitSize * sizeRatioToUnit.x,
            unitSize * sizeRatioToUnit.y,
            0.2f
            );
        }
        if (isFixed)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(
                unitSize * posRatioToUnit.x,
                unitSize * posRatioToUnit.y
                );
        }
    }
}
