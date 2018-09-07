using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LLHelper_AutoPlay
{

    [Serializable]
    public class Rank_info
    {
        public int rank;
        public int rank_min;
        public int rank_max;
    }
    [Serializable]
    public class Notes_list
    {
        public float timing_sec;
        public int notes_attribute;
        public int notes_level;
        public int effect;
        public float effect_value;
        public byte position;
    }
    [Serializable]
    public class Live_info
    {
        public int live_difficulty_id;
        public bool is_random;
        public float notes_speed;
        public Notes_list[] notes_list;
    }
    [Serializable]
    public class Deck_info
    {
        public int total_smile;
        public int total_cute;
        public int total_cool;
        public int total_hp;
        public int prepared_hp_damage;
    }
    [Serializable]
    public class Live_list
    {
        public Live_info live_info;
        public Deck_info deck_info;
    }
    [Serializable]
    public class Response_data
    {
        public Rank_info[] rank_info;
        public Live_list[] live_list;
        public string energy_full_time;
        public int over_max_energy;
        public int live_se_id;
        public bool available_live_resume;
    }
    [Serializable]
    public class Release_info
    {
        public int id;
        public string key;
    }
    [Serializable]
    public class MusicNotes
    {
        public Response_data response_data;
        public Release_info[] release_info;
        public int status_code;
    }

    [Serializable]
    public class RD3Map
    {
        public int live_track_id;
        public string name;
        public string name_kana;
        public string title_asset;
        public string sound_asset;
        public int member_category;
        public int member_tag_id;
        public int live_setting_id;
        public int difficulty;
        public int stage_level;
        public int attribute_icon_id;
        public string live_icon_asset;
        public int asset_background_id;
        public string notes_setting_asset;
        public int c_rank_score;
        public int b_rank_score;
        public int a_rank_score;
        public int s_rank_score;
        public int c_rank_combo;
        public int b_rank_combo;
        public int a_rank_combo;
        public int s_rank_combo;
        public string difficulty_text;
    }


}

