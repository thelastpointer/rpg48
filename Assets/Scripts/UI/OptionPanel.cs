using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace RPG.UI
{
    public class OptionPanel : MonoBehaviour
    {
        public Slider GraphicsSlider;
        public Slider SoundSlider;
        public Slider MusicSlider;

        public AudioMixer Audio;

        bool setup = false;

        public void Show()
        {
            setup = true;
            
            GraphicsSlider.value = QualitySettings.GetQualityLevel();
            SoundSlider.value = 0;
            MusicSlider.value = 0;

            setup = false;

            gameObject.SetActive(true);
        }

        public void OnGraphicValue(float value)
        {
            if (setup)
                return;

            int level = (int)value;

            if ((level >= 0) && (level < QualitySettings.names.Length))
                QualitySettings.SetQualityLevel(level, true);
        }

        // Note that we convert between logarithmic and linear scales here
        public void OnSoundVolume(float value)
        {
            if (setup)
                return;

            float vol = 20f * Mathf.Log10(value);
            if (value == 0)
                vol = -80f;
            Audio.SetFloat("SoundVolume", vol);
        }

        // Note that we convert between logarithmic and linear scales here
        public void OnMusicVolume(float value)
        {
            if (setup)
                return;

            float vol = 20f * Mathf.Log10(value);
            if (value == 0)
                vol = -80f;
            Audio.SetFloat("MusicVolume", vol);
        }
    }
}