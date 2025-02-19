using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private GameObject _OptionPanel;
    [SerializeField]private Animator _Ac;
    private bool _isMuteBgm=false;
    private bool _isMuteSfx=false;

    [SerializeField] private Button _BgmMuteBtn;
    [SerializeField] private Button _SfxMuteBtn;

    [SerializeField] private Slider _BgmSlider;
    [SerializeField] private Slider _SfxSlider;

    [SerializeField] private Sprite _OptionImage;
    [SerializeField] private GameObject _OptionBtn;

    // Start is called before the first frame update
    void Start()
    {
        _Ac = this.GetComponent<Animator>();
        _BgmSlider.onValueChanged.AddListener(UpdateBgmSlider);
        _SfxSlider.onValueChanged.AddListener(UpdateSfxSlider);

    }

    private void OnEnable()
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_PauseIn);
        _Ac.SetTrigger("DoUp");
    }

    public void ShowDouwnUI()
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_PauseOut);
        _OptionBtn.GetComponent<Image>().sprite = _OptionImage;
        _Ac.SetTrigger("DoDown");
        Invoke("ThisGameObjectOff", 1);
    }
    private void ThisGameObjectOff()
    {
        _OptionPanel.SetActive(false);
    }

    public void OnClickMuteBgm()
    {
        if (!_isMuteBgm)
        {
            SoundManager.instance.StopBgm();
            ColorBlock color = _BgmMuteBtn.colors;
            color.normalColor = Color.gray;
            color.selectedColor = Color.gray;
            color.highlightedColor = Color.gray;
            _BgmMuteBtn.colors=color;

        }
        else
        {
            SoundManager.instance.PlayBgm(SoundManager.EBGM.BGM_Original);
            ColorBlock color = _BgmMuteBtn.colors;
            color.normalColor = Color.white;
            color.selectedColor = Color.white;
            color.highlightedColor = Color.white;
            _BgmMuteBtn.colors = color;
        }
        _isMuteBgm=!_isMuteBgm;
        
    }

    public void OnClickMuteSfx()
    {
        if (!_isMuteSfx)
        {
            SoundManager.instance.MuteOnSfx();
            ColorBlock color = _SfxMuteBtn.colors;
            color.normalColor = Color.gray;
            color.selectedColor = Color.gray;
            color.highlightedColor = Color.gray;
            _SfxMuteBtn.colors = color;
        }
        else
        {
            SoundManager.instance.MuteOffSfx();
            ColorBlock color = _SfxMuteBtn.colors;
            color.normalColor = Color.white;
            color.selectedColor = Color.white;
            color.highlightedColor = Color.white;
            _SfxMuteBtn.colors = color;
        }
        _isMuteSfx=!_isMuteSfx;
    }

    public void UpdateBgmSlider(float value) {
        SoundManager.instance.SetBgmVolume(value);
    }

    public void UpdateSfxSlider(float value)
    {
        SoundManager.instance.SetSfxVolume(value);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
