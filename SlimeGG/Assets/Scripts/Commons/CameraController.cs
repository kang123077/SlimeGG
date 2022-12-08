using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float speed_move = 5.0f;
    private float speed_wheel = 0.2f;
    private float zoom_max = 8.0f;
    private float zoom_min = 2.5f;

    void Start() { }

    void Update()
    {
        moveCameraByKeyBoard();
    }

    void moveCameraByKeyBoard()
    {
        float keyH = Input.GetAxis("Horizontal");
        float keyV = Input.GetAxis("Vertical");
        float keyD = Input.GetAxis("Mouse ScrollWheel");
        keyH = keyH * speed_move * Time.deltaTime;
        keyV = keyV * speed_move * Time.deltaTime;
        keyD = keyD * speed_wheel * Time.deltaTime;

        if ((transform.position.x >= 3 && keyH <= 0) || (transform.position.x <= 8 && keyH >= 0))
            transform.Translate(Vector3.right * keyH);
        if ((transform.position.y >= -5 && keyV <= 0) || (transform.position.y <= -1 && keyV >= 0))
            transform.Translate(Vector3.up * keyV);

        if (
            (Camera.main.orthographicSize <= zoom_max && Camera.main.orthographicSize >= zoom_min)
            || (Camera.main.orthographicSize <= zoom_max && keyD <= 0)
            || (Camera.main.orthographicSize >= zoom_min && keyD >= 0)
        )
            Camera.main.orthographicSize += speed_wheel * (keyD != 0 ? (keyD > 0 ? -1 : 1) : 0);
    }
}
