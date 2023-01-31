using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraMoveController : MonoBehaviour
{
    [SerializeField]
    Vector2 xLimitCoor;
    float correctionCoor = -1000f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LocalStorage.IS_CAMERA_FREE && !LocalStorage.IS_CAMERA_FIX && Input.GetMouseButton(0))
        {
            if (correctionCoor != -1000f)
            {
                float curMousePos = Camera.main.transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                float targetCoor = correctionCoor + curMousePos;
                transform.position = new Vector3(
                    targetCoor < xLimitCoor.x 
                    ? xLimitCoor.x
                    : targetCoor > xLimitCoor.y
                    ? xLimitCoor.y
                    : targetCoor
                    , 0f, -10f);
            }
            correctionCoor = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        }
        else if (correctionCoor != -1000f)
        {
            correctionCoor = -1000f;
        }
    }
}
