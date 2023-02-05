using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ContentController : MonoBehaviour
{
    public static InventoryManager inventoryManager;
    InventoryType type = InventoryType.None;
    MonsterLiveStat monsterLiveStat;
    ItemLiveStat itemLiveStat;
    Transform image;
    bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        image = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        adjustSize();
        onDrag();
    }

    public void initContent(MonsterLiveStat monsterLiveStat)
    {
        type = InventoryType.Monster;
        this.monsterLiveStat = monsterLiveStat;
        if (image == null)
        {
            image = transform.GetChild(0);
        }
        image.GetComponent<Image>().sprite = Resources.Load<Sprite>(
            $"{PathInfo.Monster.Sprite}{monsterLiveStat.saveStat.speicie}"
            );
    }

    public void initContent(ItemLiveStat itemLiveStat)
    {
        type = InventoryType.Item;
        this.itemLiveStat = itemLiveStat;
        if (image == null)
        {
            image = transform.GetChild(0);
        }
        image.GetComponent<Image>().sprite = Resources.Load<Sprite>(
            $"{PathInfo.Item.Sprite}{itemLiveStat.saveStat.itemName}"
            );
    }

    private void adjustSize()
    {
        if (image != null)
        {
            image.GetComponent<BoxCollider2D>().size = transform.parent.GetComponent<RectTransform>().sizeDelta;
        }
    }

    private void OnMouseDown()
    {
        if (!isMoving) isMoving = true;
        if (type == InventoryType.Monster)
        {
            inventoryManager.selectMonster(this);
        }
        LocalStorage.IS_CAMERA_FIX = true;
        if (type == InventoryType.Monster)
        {
            Debug.Log(monsterLiveStat.itemStatList.Count);
        }
    }

    private void OnMouseUp()
    {
        isMoving = false;
        LocalStorage.IS_CAMERA_FIX = false;
        transform.localPosition = Vector3.zero;
        if (type == InventoryType.Monster)
        {
            Debug.Log(monsterLiveStat.saveStat.id);
        }
    }

    private void onDrag()
    {
        if (isMoving)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.localPosition = new Vector3(
                transform.localPosition.x, transform.localPosition.y, -10f);
        }
    }

    public List<ItemLiveStat> getItemLiveStat()
    {
        return monsterLiveStat.itemStatList.Values.ToList();
    }

    public void destroySelf()
    {
        Destroy(gameObject);
    }
}
