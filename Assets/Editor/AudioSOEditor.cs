#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using AudioManager;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

[CustomEditor(typeof(AudioSO))]
public class AudioSOEditor : UnityEditor.Editor
{
    private void OnEnable()
    {
        ref SoundGroup[] soundGroups = ref ((AudioSO)target).sounds;

        if (soundGroups == null)
            return;

        string[] names = Enum.GetNames(typeof(AudioClips));
        bool differentSize = names.Length != soundGroups.Length;

        Dictionary<string, SoundGroup> sounds = new();

        if (differentSize)
        {
            for (int i = 0; i < soundGroups.Length; ++i)
            {
                
                if (!sounds.ContainsKey(soundGroups[i].name))
                    sounds.Add(soundGroups[i].name, soundGroups[i]);
            }
        }

        Array.Resize(ref soundGroups, names.Length);
        for (int i = 0; i < soundGroups.Length; i++)
        {
            string currentName = names[i];
            soundGroups[i].name = currentName;
            if (soundGroups[i].volume == 0) soundGroups[i].volume = 1;

            if (differentSize)
            {
                if (sounds.ContainsKey(currentName))
                {
                    SoundGroup current = sounds[currentName];
                    UpdateElement(ref soundGroups[i], current.volume, current.sounds, current.mixer);
                }
                else
                    UpdateElement(ref soundGroups[i], 1, new AudioClip[0], null);
            
            
            }
        }
    }
    static void UpdateElement(ref SoundGroup element, float volume, AudioClip[] sounds, AudioMixerGroup mixer)
    {
        element.volume = volume;
        element.sounds = sounds;
        element.mixer = mixer;
    }
}
#endif