using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterVolumeController : MonoBehaviour
{
    public AudioMixer[] audioMixers; 
    public string[] volumeParameters;
    public Slider volumeSlider;

    private float[] initialVolumes;

    void Start()
    {
        int count = audioMixers.Length;
        initialVolumes = new float[count];

        for (int i = 0; i < count; i++)
        {
            audioMixers[i].GetFloat(volumeParameters[i], out initialVolumes[i]);
        }

        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
    }

    public void SetVolume(float sliderValue)
    {
        for (int i = 0; i < audioMixers.Length; i++)
        {
            float newVolume = initialVolumes[i] + Mathf.Log10(sliderValue) * 20;
            newVolume = Mathf.Clamp(newVolume, -80f, 0f);

            audioMixers[i].SetFloat(volumeParameters[i], newVolume);
        }

        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }
}