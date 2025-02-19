using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    [SerializeField] private BoardManager _BoardManager;
    [SerializeField] private Text _ScoureText;
    [SerializeField] private GameObject _FirstItemPanel;
    [SerializeField] private Text _TimeText;
    [SerializeField] private GameObject _FirstClickPanel;
    private int scoureUpSpeed = 50;
    public bool PlayTImeWalk=false;

    private void OnEnable()
    {
        _FirstClickPanel.SetActive(true);
    }

    private void Update()
    {
        if (PlayTImeWalk)
        {
            if (_BoardManager.PlayTime >= 0)
            {
                _BoardManager.PlayTime -= Time.deltaTime;
                _TimeText.text = ((int)_BoardManager.PlayTime).ToString();
            }
            else
            {
                _BoardManager.PlayTime = 0;
                _TimeText.text = "0";
                PlayTImeWalk = false;
            }
        }
    }

    public void FirstItem()
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Button);
        if (!_FirstItemPanel.activeSelf)
        {
            _FirstItemPanel.SetActive(true);
        }
        else
        {
            _FirstItemPanel.SetActive(false);
        }

    }


    public void SetScoure(int value)
    {
        StartCoroutine(NumScoure(value));
    }

    IEnumerator NumScoure(int targetNum)
    {
        float count = _BoardManager.ScoureValue;
        _BoardManager.ScoureValue += targetNum +_BoardManager.BonusScoure ;

        if (targetNum > 0)
        {
            while (_BoardManager.ScoureValue >= count)
            {
                count += Time.deltaTime * scoureUpSpeed;
                _ScoureText.text=count.ToString("n0");
                yield return null;
            }
        }
        else
        {
            while (_BoardManager.ScoureValue <= count)
            {
                count -= Time.deltaTime * scoureUpSpeed;
                _ScoureText.text = count.ToString("n0");
                yield return null;
            }

        }
        _ScoureText.text = _BoardManager.ScoureValue.ToString("n0");


    }
}
