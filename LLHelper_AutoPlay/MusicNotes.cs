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

}

