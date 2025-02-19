using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameObject _Explanation;

    public void ShowMiniGamePage()
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Button);
        SceneManager.LoadScene("PuzzleScene");
    }

    public void OnExplanation()
    {
        _Explanation.SetActive(true);
    }

    public void OffExplanation()
    {
        _Explanation.SetActive(false); 
    }
}
