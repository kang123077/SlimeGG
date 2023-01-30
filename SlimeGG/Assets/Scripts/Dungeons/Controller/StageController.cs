using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField]
    StageController[] nextStageList;
    public bool isClear { get; set; }
    SpriteRenderer bgSprite;
    Transform lineListTf;
    [SerializeField]
    Transform linePrefab;
    Transform[] lineList;

    // Start is called before the first frame update
    void Start()
    {
        bgSprite = GetComponent<SpriteRenderer>();
        bgSprite.color = new Color(0.4f, 0.4f, 0.4f, 1);
        lineListTf = transform.Find("Lines");

        lineList = new Transform[nextStageList.Length];
        for (int i = 0; i < lineList.Length; i++)
        {
            Transform newLineTf = Instantiate(linePrefab);
            newLineTf.SetParent(lineListTf);
            newLineTf.localPosition = Vector3.zero;
            newLineTf.GetComponent<LineRenderer>().SetPositions(new Vector3[]
            {
                (nextStageList[i].transform.localPosition - transform.localPosition) * 0.1f,
                (nextStageList[i].transform.localPosition - transform.localPosition) * 0.9f
            });
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
