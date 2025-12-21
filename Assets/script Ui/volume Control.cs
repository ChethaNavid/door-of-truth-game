using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;

    void Start()
    {
        // Set default volume when scene starts
        SetMusicVolume(musicSlider.value);
    }

    public void SetMusicVolume(float value)
    {
        // Prevent Log10(0)
        value = Mathf.Clamp(value, 0.0001f, 1f);

        myMixer.SetFloat("music", Mathf.Log10(value) * 20);
    }
}
