using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanelUI : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void OnEnable()
    {
        var opt = OptionManager.Instance;
        if (opt == null) return;

        bgmSlider.SetValueWithoutNotify(
            opt.isBgmMuted ? 0f : opt.bgmVolume);
        sfxSlider.SetValueWithoutNotify(
            opt.isSfxMuted ? 0f : opt.sfxVolume);

        bgmSlider.onValueChanged.AddListener(opt.SetBgmVolume);
        sfxSlider.onValueChanged.AddListener(opt.SetSfxVolume);
    }

    private void OnDisable()
    {
        bgmSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
    }
}