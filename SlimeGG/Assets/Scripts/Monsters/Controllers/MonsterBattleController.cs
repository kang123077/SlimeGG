using Unity.VisualScripting;
using UnityEngine;

public class MonsterBattleController : MonoBehaviour
{
    private float zCoor = 16f;
    private Vector2 direction = new Vector2(0f, 0f);
    private MonsterInfo monsterInfo;
    private Transform bg;
    private Animator anim;

    public void initInfo(MonsterInfo monsterInfo)
    {
        this.monsterInfo = monsterInfo;
        bg = transform.Find("Image");
        bg.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(
            PathInfo.PATH_SPRITE +
            PathInfo.Monster.paths[monsterInfo.curGrowthState][monsterInfo.monsterName]
            );
        Destroy(bg.GetComponent<PolygonCollider2D>());
        bg.AddComponent<PolygonCollider2D>();
        anim = bg.GetComponent<Animator>();
        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(
            PathInfo.PATH_ANIMATION +
            PathInfo.Monster.paths[monsterInfo.curGrowthState][monsterInfo.monsterName] +
            "/Controller"
            );
    }

    private void OnMouseDown()
    {
        //print("클릭 클릭");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
