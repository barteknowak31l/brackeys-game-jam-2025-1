using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterVolumeController : MonoBehaviour
{
    public AudioMixer[] audioMixers; // Lista Audio Mixerów
    public string[] volumeParameters; // Nazwy parametrów głośności w mixerach
    public Slider volumeSlider;

    private float[] initialVolumes; // Początkowe wartości głośności mixerów

    void Start()
    {
        // Pobieramy ilość mixerów
        int count = audioMixers.Length;
        initialVolumes = new float[count];

        // Pobieramy początkowe wartości dla każdego mixera
        for (int i = 0; i < count; i++)
        {
            audioMixers[i].GetFloat(volumeParameters[i], out initialVolumes[i]);
        }

        // Opcjonalnie: Załaduj zapisany poziom głośności
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
    }

    public void SetVolume(float sliderValue)
    {
        for (int i = 0; i < audioMixers.Length; i++)
        {
            // Skalowanie wartości początkowej
            float newVolume = initialVolumes[i] + Mathf.Log10(sliderValue) * 20;
            newVolume = Mathf.Clamp(newVolume, -80f, 0f); // Ograniczenie do słyszalnych wartości

            audioMixers[i].SetFloat(volumeParameters[i], newVolume);
        }

        // Zapisujemy wartość suwaka
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }
}