using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    ITEM0   = 0,
    ITEM1   = 1,
    ITEM2   = 2,
    ITEM3   = 3
}
public enum ItemRank
{
    COMMON      = 0,
    UNCOMMON    = 1,
    RARE        = 2,
    LEGENDARY   = 3,
}



public class BaseItem : MonoBehaviour
{
    public Sprite[] item_sprites;
    private SpriteRenderer m_sprite_renderer;

    protected int m_item_id;
    public int m_rank_point;
    protected ItemRank m_item_rank;
    protected ItemType m_item_type;
    private void Awake()
    {
        m_sprite_renderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void SetupItem(ItemType item_type)
    {
        m_item_type = item_type;
        GetItemValue();
        GetItemSprite();
       
    }
    /// <summary>
    /// Mỗi item sẽ có weight riêng, hàm này dùng để random và chọn ra item sẽ rớt tại thời điểm gọi đến
    /// </summary>
    ItemType GetItemType()
    {
        return (ItemType)Random.Range(0, 4);
    }
    void GetItemSprite()
    {
        switch (m_item_type)
        {
            case ItemType.ITEM0:
                m_sprite_renderer.sprite = item_sprites[0];
                break;
            case ItemType.ITEM1:
                m_sprite_renderer.sprite = item_sprites[1];
                break;
            case ItemType.ITEM2:
                m_sprite_renderer.sprite = item_sprites[2];
                break;
            case ItemType.ITEM3:
                m_sprite_renderer.sprite = item_sprites[3];
                break;
        }
    }
    void GetItemValue()
    {
        switch (m_item_type)
        {
            case ItemType.ITEM0:
                m_rank_point = 1;
                break;
            case ItemType.ITEM1:
                m_rank_point = 2;
                break;
            case ItemType.ITEM2:
                m_rank_point = 2;
                break;
            case ItemType.ITEM3:
                m_rank_point = 3;
                break;
        }
    }
}