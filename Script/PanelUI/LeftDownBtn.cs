using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftDownBtn : MonoBehaviour
{
    [SerializeField] private GamePlayManager _GamePlay;
    [SerializeField] private GameObject _JellyPanel;
    [SerializeField] private GameObject _PlantPanel;
    [SerializeField] private GameObject _OptionPanel;
    [SerializeField] private GameObject _JellyBtn;
    [SerializeField] private GameObject _PlantBtn;
    [SerializeField] private GameObject _OptionBtn;


    [SerializeField] private Sprite _JellyImage;
    [SerializeField] private Sprite _JellyOverImage;
    [SerializeField] private Sprite _PlantImage;
    [SerializeField] private Sprite _PlantOverImage;
    [SerializeField] private Sprite _OptionImage;
    [SerializeField] private Sprite _OptionOverImage;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetReference(GamePlayManager manager)
    {
        _GamePlay = manager;
    }

    private void ShowDownJellyPanel()
    {
        _JellyPanel.SetActive(false);
    }

    public void OnClickJellyBtn()
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Button);
        if (!_PlantPanel.activeSelf)
        {

            if (!_JellyPanel.activeSelf)
            {
                _JellyPanel.SetActive(true);
                _JellyBtn.GetComponent<Image>().sprite = _JellyOverImage;
            }
            else
            {
                _JellyBtn.GetComponent<Image>().sprite = _JellyImage;
                _JellyPanel.GetComponentInChildren<Animator>().SetTrigger("DoDown");
                Invoke("ShowDownJellyPanel", 1f);
            }
        }

    }

    private void ShowDownPlantPanel()
    {
        _PlantPanel.SetActive(false);
    }

    public void OnClickPlantBtn()
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Button);
        if (!_JellyPanel.activeSelf)
        {
            if (!_PlantPanel.activeSelf)
            {
                _PlantPanel.SetActive(true);
                _PlantBtn.GetComponent<Image>().sprite = _PlantOverImage;
            }
            else
            {
                _PlantBtn.GetComponent<Image>().sprite = _PlantImage;
                _PlantPanel.GetComponentInChildren<Animator>().SetTrigger("DoDown");
                Invoke("ShowDownPlantPanel", 1f);
            }
        }
    }

    public void OnClickOptionBtn()
    {
        if (!_OptionPanel.activeSelf)
        {
            _OptionPanel.SetActive(true);
            _OptionBtn.GetComponent<Image>().sprite = _OptionOverImage;
        }
        else
        {
            _OptionBtn.GetComponent<Image>().sprite= _OptionImage;
            _OptionPanel.GetComponentInChildren<Animator>().SetTrigger("DoDown");
            Invoke("ShowDownOptionPanel", 1f);

        }
    }
    void ShowDownOptionPanel()
    {
        _OptionPanel.SetActive(false);
    }
}
