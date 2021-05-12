using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankManager : MonoBehaviour
{
    #region Data
    public int total_rank_points;
    public int current_index_points;
    public int current_rank;
    


    #endregion

    #region Visual
    public UIRankController ui_rank_controller;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Setup()
    {
        //Về sau nhớ load từ Data Manager ->save
        //if(DataManager.Instance.save_data!=null)
        //{
        //    current_rank = DataManager.Instance.save_data.current_rank;
        //    total_rank_points = DataManager.Instance.save_data.current_points;
        //    current_index_points = 0;
        //}
        //else
        {
            current_rank = 0;
            total_rank_points = 0;
            current_index_points = 0;
        }

        //Sau khi lấy data từ save thì update data lên UI
        ui_rank_controller.UpdateRankBadge(current_rank);
        
    }
    public void UpdateRankPoints(int points)
    {
        total_rank_points += points;
        StartCoroutine(OnUpdateRankPoints(points));
    }
    IEnumerator OnUpdateRankPoints(int points)
    {
        for(int i = 0;i<points;i++)
        {
            if (current_index_points > GameHelper.MAX_RANK_POINTS-1)
            {
                current_index_points = 0;
                ui_rank_controller.ResetPointsOnUI();
                UpRank();
                ui_rank_controller.UpdateRankBadge(current_rank);
            }
            ui_rank_controller.AddPointsOnUI(current_index_points);
            current_index_points++;            
            yield return null;
        }
    }
    void UpRank()
    {
        if (current_rank > GameHelper.MAX_RANK)
        {
            current_rank = GameHelper.MAX_RANK;
            return;
        }
        current_rank += 1;
        
    }
}
