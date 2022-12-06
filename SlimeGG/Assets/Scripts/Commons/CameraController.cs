using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float speed_move = 5.0f;
    private float speed_wheel = 3000.0f;
    private new Transform transform;
    private new Camera camera;

    void Start()
    {
        transform = GetComponent<Transform>();
        camera = GetComponent<Camera>();
    }

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
            (transform.position.z <= 3 && transform.position.z >= -3)
            || (transform.position.z <= 3 && keyD >= 0)
            || (transform.position.z >= -3 && keyD <= 0)
        )
            transform.Translate(Vector3.forward * keyD);
    }
}
