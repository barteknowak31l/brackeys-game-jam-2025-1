using System;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSO SO;
        private static AudioManager instance = null;
        private AudioSource audioSource;

        private void Awake()
        {
            if(!instance)
            {
                instance = this;
                audioSource = GetComponent<AudioSource>();
            }
        }

        public static void PlaySound(AudioClips sound, AudioSource source = null, float volume = 1)
        {
            SoundGroup soundGroup = instance.SO.sounds[(int)sound];
            AudioClip[] clips = soundGroup.sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

            if(source)
            {
                source.outputAudioMixerGroup = soundGroup.mixer;
                source.clip = randomClip;
                source.volume = volume * soundGroup.volume;
                source.Play();
            }
            else
            {
                instance.audioSource.outputAudioMixerGroup = soundGroup.mixer;
                instance.audioSource.PlayOneShot(randomClip, volume * soundGroup.volume);
            }
        }
    }

    [Serializable]
    public struct SoundGroup
    {
        [HideInInspector] public string name;
        [Range(0, 1)] public float volume;
        public AudioMixerGroup mixer;
        public AudioClip[] sounds;
    }
}