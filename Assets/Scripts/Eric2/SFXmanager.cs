using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXmanager : MonoBehaviour
{
    /// <summary>
    /// sound effect manager
    /// syntax: SFXmanager.playsound("pingpong");
    /// syntax: SFXmanager_volume.playsound("camera", 0.3f);
    /// </summary>


    // Variables
    [System.Serializable]
    public struct sound_item
    {
        public string sound_name;
        public AudioClip clip;
    }
    public sound_item[] sound_item_list;
    public static SFXmanager SFX;
    
    static AudioSource source;
    static Dictionary<string, AudioClip> clip_dict = new Dictionary<string, AudioClip>();


    // Awake
    void Awake()
    {
        // make SFX a singleton
        if (SFX == null)
        {
            SFX = this;
        }
    }


    // Start
    void Start()
    {
        // convert sound_item_list to clip_dict
        foreach(sound_item s in sound_item_list)
        {
            clip_dict[s.sound_name] = s.clip;
        }

        // add AudioSource component
        source = gameObject.AddComponent<AudioSource>();
    }


    // play sound
    public static void playsound(string sound_name)
    {
        // play sound
        source.clip = clip_dict[sound_name];
        source.Play();
    }


    // stop sound
    public static void stopsound(string sound_name)
    {
        // stop sound
        source.clip = clip_dict[sound_name];
        source.Stop();
    }


    // play sound volume
    public static void playsound_volume(string sound_name, float volume)
    {
        // play sound
        source.clip = clip_dict[sound_name];
        source.volume = volume;
        source.Play();
    }


    // play sound loop
    public static void playsound_loop(string sound_name)
    {
        // play sound
        source.clip = clip_dict[sound_name];
        source.loop = true;
        source.Play();
    }

}


///////////////////////////////////// trash ///////////////////////////////////////

//public List<AudioClip> clip_list = new List<AudioClip>();

//clip_dict["pingpong"] = clip_list[0];
//clip_dict["player1"] = clip_list[1];
//clip_dict["player2"] = clip_list[2];
//clip_dict["win"] = clip_list[3];
//clip_dict["lose"] = clip_list[4];