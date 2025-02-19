using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleSceneOption : MonoBehaviour
{
    [SerializeField] private GameObject _OptionUI;
    public void OnOffOptionUI()
    {
        if(_OptionUI.activeSelf)
            _OptionUI.SetActive(false);
        else
            _OptionUI.SetActive(true);
    }

    public void GrowUpSceneOn()
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Button);
        SceneManager.LoadScene("GrowScene");
    }

}
