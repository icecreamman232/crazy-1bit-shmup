using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIRankController : MonoBehaviour
{
    public Image rank_badge_img;
    public List<Sprite> rank_badge_sprites;
    public List<Image> rank_points;

    public int last_index_points;

    private Color disable_state_color;
    private Color enable_state_color;

    // Start is called before the first frame update
    void Start()
    {
        last_index_points = 0;
        disable_state_color = new Color(0.5f, 0.5f, 0.5f, 1);
        enable_state_color = new Color(1, 1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddPointsOnUI(int index)
    {
        rank_points[index].color = enable_state_color;
    }
    public void ResetPointsOnUI()
    {
        for(int i = 0; i< GameHelper.MAX_RANK_POINTS;i++)
        {
            rank_points[i].color = disable_state_color;
        }
        
    }
    public void UpdateRankBadge(int rank_id)
    {
        rank_badge_img.sprite = rank_badge_sprites[rank_id];
    }
}
