using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField]
    StageController[] nextStageList;
    public bool isClear { get; set; }
    SpriteRenderer bgSprite;

    // Start is called before the first frame update
    void Start()
    {
        bgSprite = GetComponent<SpriteRenderer>();
        bgSprite.color = new Color(0.4f, 0.4f, 0.4f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
