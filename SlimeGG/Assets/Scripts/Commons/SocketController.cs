using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketController : MonoBehaviour
{
    SpriteRenderer socketBackSprite = null;
    Color colorTriggered = new Color(0.1603774f, 0.1603774f, 0.1603774f, 1f);
    Color colorNormal = new Color(0.2830189f, 0.2763439f, 0.2763439f, 1f);

    void Start()
    {
        socketBackSprite = transform.Find("Socket Back").GetComponent<SpriteRenderer>();
    }

    void OnMouseEnter()
    {
        socketBackSprite.color = colorTriggered;
    }

    void OnMouseExit()
    {
        socketBackSprite.color = colorNormal;
    }
}
