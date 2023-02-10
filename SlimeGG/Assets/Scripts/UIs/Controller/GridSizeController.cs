using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSizeController : MonoBehaviour
{
    [SerializeField]
    public Vector2 cellSizeRatioToUnit, spacingRatioToUnit;
    [SerializeField]
    TextAnchor childAlignmentAnchor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<GridLayoutGroup>().childAlignment = childAlignmentAnchor;
        GetComponent<GridLayoutGroup>().cellSize = new Vector2(
            MainGameManager.screenUnitSize * cellSizeRatioToUnit.x,
            MainGameManager.screenUnitSize * cellSizeRatioToUnit.y
        );
        GetComponent<GridLayoutGroup>().spacing = new Vector2(
            MainGameManager.screenUnitSize * spacingRatioToUnit.x,
            MainGameManager.screenUnitSize * spacingRatioToUnit.y
        );
    }
}
