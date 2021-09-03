using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MusicController : MonoBehaviour
{
    List<AudioSource> audioSources;
    public Slider volumeSlider;
    void Start()
    {
        audioSources = GameController.Instance.audioSources.Values.ToList();
    }
    void OnEnable()
    {
        //Register Slider Events
        volumeSlider.onValueChanged.AddListener(delegate { changeVolume(volumeSlider.value); });
    }

    //Called when Slider is moved
    public void changeVolume(float sliderValue)
    {
        GameController.Instance.setAudioVolume(sliderValue);
        GameController.Instance.audioVolume = sliderValue;
    }

    void OnDisable()
    {
        //Un-Register Slider Events
        volumeSlider.onValueChanged.RemoveAllListeners();
    }
}
