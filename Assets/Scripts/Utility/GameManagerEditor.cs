using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    bool gamestate_foldout;
    bool score_foldout;
    bool coin_foldout;
    bool level_design_foldout;
    bool reference_holders_folderout;
    bool UI_foldout;
    bool audio_foldout;

    SerializedProperty current_game_state_prop;

    SerializedProperty current_score_prop;
    SerializedProperty score_text_prop;

    SerializedProperty current_coin_prop;

    SerializedProperty list_monster_lanes_prop;
    SerializedProperty endless_mode_data_prop;
    //SerializedProperty speed_factor_prop;
    SerializedProperty wave_index_prop;
    SerializedProperty current_spd_prop;

    SerializedProperty space_ship_prop;
    SerializedProperty star_front_layer_prop;
    SerializedProperty star_back_layer_prop;
    SerializedProperty die_explosion_fx_prop;
    SerializedProperty camera_shake_fx_prop;
    SerializedProperty left_barrel_prop;
    SerializedProperty right_barrel_prop;

    SerializedProperty ui_endgame_controller_prop;
    SerializedProperty ui_ship_health_bar_prop;

    SerializedProperty BGM_prop;
    SerializedProperty sfx_prop;
    SerializedProperty monster_die_sfx_prop;


    readonly GUIContent current_game_state_content = new GUIContent("Current GameState");

    readonly GUIContent current_score_content = new GUIContent("Current Score");
    readonly GUIContent score_text_prop_content = new GUIContent("Score Text");

    readonly GUIContent current_coin_content = new GUIContent("Current Coin");

    readonly GUIContent list_monster_lanes_content = new GUIContent("Monster Lanes");
    readonly GUIContent endless_mode_data_content = new GUIContent("Endless Mode DO");

    //Constant không thể hiển thị trên inspector
    //readonly GUIContent speed_factor_content = new GUIContent("Speed Factor");

    readonly GUIContent wave_index_content = new GUIContent("Wave Index");
    readonly GUIContent current_spd_content = new GUIContent("Current Speed");

    readonly GUIContent space_ship_content = new GUIContent("Ship");
    readonly GUIContent star_front_layer_content = new GUIContent("Front Star Particle");
    readonly GUIContent star_back_layer_content = new GUIContent("Back Star Particle");
    readonly GUIContent die_explosion_fx_content = new GUIContent("Die Explosion FX");
    readonly GUIContent camera_shake_fx_content = new GUIContent("Camera Shaking FX");
    readonly GUIContent left_barrel_content = new GUIContent("Left Screen Collider");
    readonly GUIContent right_barrel_content = new GUIContent("Right Screen Collider");

    readonly GUIContent ui_endgame_controller_content = new GUIContent("UI Endgame");
    readonly GUIContent ui_ship_health_bar_content = new GUIContent("UI Ship HP");

    readonly GUIContent BGM_content = new GUIContent("BGM");
    readonly GUIContent sfx_content = new GUIContent("SFX");
    readonly GUIContent monster_die_sfx_content = new GUIContent("Monster Die SFX");

    readonly GUIContent score_content = new GUIContent("Score Management");
    readonly GUIContent level_design = new GUIContent("Level Design");
    readonly GUIContent reference_holders = new GUIContent("Reference Holders");
    readonly GUIContent ui_content = new GUIContent("UI");
    readonly GUIContent audio_content = new GUIContent("Audio");

    readonly GUIContent coin_content = new GUIContent("Coin");
    readonly GUIContent gamestate_content = new GUIContent("GameState");

    void OnEnable()
    {
        current_game_state_prop = serializedObject.FindProperty("current_game_state");

        current_score_prop  = serializedObject.FindProperty("current_score");
        score_text_prop     = serializedObject.FindProperty("score_number_text");

        current_coin_prop = serializedObject.FindProperty("current_coin");

        list_monster_lanes_prop = serializedObject.FindProperty("list_monster_lanes");
        endless_mode_data_prop  = serializedObject.FindProperty("endless_mode_data");
        //speed_factor_prop       = serializedObject.FindProperty("speed_factor");
        wave_index_prop         = serializedObject.FindProperty("wave_index");
        current_spd_prop        = serializedObject.FindProperty("current_spd");

        space_ship_prop         = serializedObject.FindProperty("space_ship");
        star_front_layer_prop   = serializedObject.FindProperty("star_front_layer");
        star_back_layer_prop    = serializedObject.FindProperty("star_back_layer");
        die_explosion_fx_prop   = serializedObject.FindProperty("die_explosion_fx");
        camera_shake_fx_prop    = serializedObject.FindProperty("camera_shake_fx");
        left_barrel_prop        = serializedObject.FindProperty("left_barrel");
        right_barrel_prop       = serializedObject.FindProperty("right_barrel");

        ui_endgame_controller_prop = serializedObject.FindProperty("ui_endgame_controller");
        ui_ship_health_bar_prop = serializedObject.FindProperty("ui_ship_health_bar");

        BGM_prop = serializedObject.FindProperty("BGM");
        sfx_prop = serializedObject.FindProperty("sfx");
        monster_die_sfx_prop = serializedObject.FindProperty("monster_die_sfx");
    }
    public override void OnInspectorGUI()
    {
        //Dòng này sẽ vẽ lại toàn bộ GUI cũ
        //base.OnInspectorGUI()

        serializedObject.Update();
        EditorGUILayout.PropertyField(current_game_state_prop, current_game_state_content);

        #region Score
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
        score_foldout = EditorGUILayout.Foldout(score_foldout, score_content);
        if(score_foldout)
        {          
            EditorGUILayout.PropertyField(current_score_prop, current_score_content);
            EditorGUILayout.PropertyField(score_text_prop, score_text_prop_content);         
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion

        #region Coin
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
        coin_foldout = EditorGUILayout.Foldout(coin_foldout, coin_content);
        if (coin_foldout)
        {
            EditorGUILayout.PropertyField(current_coin_prop, current_coin_content);
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion

        #region Level
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
        level_design_foldout = EditorGUILayout.Foldout(level_design_foldout, level_design);
        if (level_design_foldout)
        {
            EditorGUILayout.PropertyField(endless_mode_data_prop, endless_mode_data_content);
            //EditorGUILayout.PropertyField(speed_factor_prop, speed_factor_content);
            EditorGUILayout.PropertyField(wave_index_prop, wave_index_content);
            EditorGUILayout.PropertyField(current_spd_prop, current_spd_content);
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(list_monster_lanes_prop, list_monster_lanes_content);
            
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion

        #region Refs
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
        reference_holders_folderout = EditorGUILayout.Foldout(reference_holders_folderout, reference_holders);
        if (reference_holders_folderout)
        {
            EditorGUILayout.PropertyField(space_ship_prop, space_ship_content);
            EditorGUILayout.PropertyField(star_front_layer_prop, star_front_layer_content);
            EditorGUILayout.PropertyField(star_back_layer_prop, star_back_layer_content);
            EditorGUILayout.PropertyField(die_explosion_fx_prop, die_explosion_fx_content);
            EditorGUILayout.PropertyField(camera_shake_fx_prop, camera_shake_fx_content);
            EditorGUILayout.PropertyField(left_barrel_prop, left_barrel_content);
            EditorGUILayout.PropertyField(right_barrel_prop, right_barrel_content);
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion

        #region UI
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
        UI_foldout = EditorGUILayout.Foldout(UI_foldout, ui_content);
        if (UI_foldout)
        {
            EditorGUILayout.PropertyField(ui_endgame_controller_prop, ui_endgame_controller_content);
            EditorGUILayout.PropertyField(ui_ship_health_bar_prop, ui_ship_health_bar_content);
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion


        #region Audio
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
        audio_foldout = EditorGUILayout.Foldout(audio_foldout, audio_content);
        if (audio_foldout)
        {
            EditorGUILayout.PropertyField(BGM_prop, BGM_content);
            EditorGUILayout.PropertyField(sfx_prop, sfx_content);
            EditorGUILayout.PropertyField(monster_die_sfx_prop, monster_die_sfx_content);
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion
        serializedObject.ApplyModifiedProperties();
    }
}
