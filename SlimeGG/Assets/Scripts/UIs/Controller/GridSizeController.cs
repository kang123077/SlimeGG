using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSizeController : MonoBehaviour
{
    [SerializeField]
    public Vector2 cellSizeRatioToUnit, spacingRatioToUnit;
    [SerializeField]
    public TextAnchor childAlignmentAnchor;
    [SerializeField]
    private bool isRatioToUnit = true;
    [SerializeField]
    private int constraintCount = 0;

    public void initInfo(Vector2 cellSizeRatioToUnit, Vector2 spacingRatioToUnit, TextAnchor childAlignmentAnchor, int constraintCount, bool isRatioToUnit = true)
    {
        this.cellSizeRatioToUnit = cellSizeRatioToUnit;
        this.spacingRatioToUnit = spacingRatioToUnit;
        this.childAlignmentAnchor = childAlignmentAnchor;
        this.constraintCount = constraintCount;
        this.isRatioToUnit = isRatioToUnit;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (constraintCount != 0)
        {
            GetComponent<GridLayoutGroup>().constraintCount = constraintCount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float unitSize = isRatioToUnit ? MainGameManager.screenUnitSize : 1f;
        GetComponent<GridLayoutGroup>().childAlignment = childAlignmentAnchor;
        GetComponent<GridLayoutGroup>().cellSize = new Vector2(
            unitSize * cellSizeRatioToUnit.x,
            unitSize * cellSizeRatioToUnit.y
        );
        GetComponent<GridLayoutGroup>().spacing = new Vector2(
            unitSize * spacingRatioToUnit.x,
            unitSize * spacingRatioToUnit.y
        );
    }
}
