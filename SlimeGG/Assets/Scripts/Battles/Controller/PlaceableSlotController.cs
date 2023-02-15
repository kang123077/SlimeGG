using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableSlotController : MonoBehaviour
{
    private SpriteRenderer background;
    private bool isPosessed;
    private int curStatus = 0;
    // Start is called before the first frame update
    void Start()
    {
        initSetting();
    }

    // Update is called once per frame
    void Update()
    {
        switch(curStatus)
        {
            case 0:
                // 초기화 안됨
                initSetting();
                break;
            case 1:
                // 초기화 완료
                break;
        }
    }

    private void initSetting()
    {
        isPosessed = false;
        background = GetComponent<SpriteRenderer>();
        background.color = pickColor(false);
        curStatus = 1;
    }

    private static Color pickColor(bool isPosessed)
    {
        if (isPosessed) return new Color(0.8f, 0.8f, 0.8f, 1f);
        return new Color(.2f, .2f, .2f, 1f);
    }
}
