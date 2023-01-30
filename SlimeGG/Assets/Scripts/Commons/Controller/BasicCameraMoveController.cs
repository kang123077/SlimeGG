using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraMoveController : MonoBehaviour
{
    Vector2 clickPoint = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (LocalStorage.IS_CAMERA_FREE && Input.GetMouseButton(0))
        //{
        //    if (clickPoint != Vector2.zero)
        //    {
        //        Vector3 position
        //            = Camera.main.ScreenToViewportPoint((Vector2)Input.mousePosition - clickPoint);
        //        Debug.Log(position == Vector3.zero);

        //        Vector3 objPosition = Camera.main.ScreenToWorldPoint(position);
        //        transform.Translate(Vector3.left * objPosition.x);
        //        //transform.position = new Vector3(-objPosition.x, 0f, -10f);
        //    }
        //    clickPoint = Input.mousePosition;
        //}
        //else
        //{
        //    clickPoint = Vector2.zero;
        //}
    }

    private void OnMouseDrag()
    {
        Debug.Log("???00");
    }
}
