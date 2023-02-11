using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoveController : MonoBehaviour
{
    [SerializeField]
    private DirectionEnum directionToMove = DirectionEnum.None;
    [SerializeField]
    private float distanceToMoveRatioToUnit = 0f;
    [SerializeField]
    private string keyToToggle;

    private bool isActive = false;
    private bool isAnimating = false;
    private ObjectSizeController objectSizeController;
    // Start is called before the first frame update
    void Start()
    {
        objectSizeController = GetComponent<ObjectSizeController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToToggle) && !isAnimating)
        {
            if (distanceToMoveRatioToUnit == 0f || directionToMove == DirectionEnum.None)
            {
                return;
            }
            StartCoroutine(toggleObject());
        }
    }

    private IEnumerator toggleObject()
    {
        LocalStorage.UIOpenStatus.reward = !isActive;
        isActive = !isActive;
        isAnimating = true;
        float distanceUnitLeft = distanceToMoveRatioToUnit; ;
        while (
            distanceUnitLeft > 0f
            )
        {
            yield return new WaitForSeconds(0.01f);
            float ratioMove = Mathf.Max((0.01f * 30 * SettingVariables.slideToggleSpd), 0.1f);
            Vector2 newPos = objectSizeController.posRatioToUnit;
            if (distanceUnitLeft <= 0.1f)
            {
                switch (directionToMove)
                {
                    case DirectionEnum.Left:
                        if (isActive) newPos.x -= distanceUnitLeft;
                        else newPos.x += distanceUnitLeft;
                        break;
                    case DirectionEnum.Right:
                        if (isActive) newPos.x += distanceUnitLeft;
                        else newPos.x -= distanceUnitLeft;
                        break;
                    case DirectionEnum.Up:
                        if (isActive) newPos.y += distanceUnitLeft;
                        else newPos.y -= distanceUnitLeft;
                        break;
                    case DirectionEnum.Down:
                        if (isActive) newPos.y -= distanceUnitLeft;
                        else newPos.y += distanceUnitLeft;
                        break;
                }
                objectSizeController.posRatioToUnit = newPos;
                distanceUnitLeft = 0f;
            }
            else
            {
                switch (directionToMove)
                {
                    case DirectionEnum.Left:
                        if (isActive) newPos.x -= (distanceUnitLeft * ratioMove);
                        else newPos.x += (distanceUnitLeft * ratioMove);
                        break;
                    case DirectionEnum.Right:
                        if (isActive) newPos.x += (distanceUnitLeft * ratioMove);
                        else newPos.x -= (distanceUnitLeft * ratioMove);
                        break;
                    case DirectionEnum.Up:
                        if (isActive) newPos.y += (distanceUnitLeft * ratioMove);
                        else newPos.y -= (distanceUnitLeft * ratioMove);
                        break;
                    case DirectionEnum.Down:
                        if (isActive) newPos.y -= (distanceUnitLeft * ratioMove);
                        else newPos.y += (distanceUnitLeft * ratioMove);
                        break;
                }
                objectSizeController.posRatioToUnit = newPos;
                distanceUnitLeft *= 1f - ratioMove;
            }
        }
        isAnimating = false;
    }
}
