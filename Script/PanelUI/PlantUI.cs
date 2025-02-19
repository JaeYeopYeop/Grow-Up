using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantUI : MonoBehaviour
{
    [SerializeField] private GamePlayManager _GamePlayManager;
    [SerializeField] private Jelatine _Jelatine;
    [SerializeField] private Coin _Coin;

    [SerializeField] private Animator _Ac;

    [SerializeField] private Text JellyMaxText;
    [SerializeField] private Text ClickJelatineText;

    [SerializeField] private Text APTLevel;
    [SerializeField] private Text CreateLevel;

    private int RequireValue(int value)
    {
        return (int)(value*value)*15;
    }

    private void SetData()
    {
        int value = _GamePlayManager.JellyMax;
        int requirValue = RequireValue(value);
        JellyMaxText.text = requirValue.ToString("n0");

        int value2 = _GamePlayManager.ClickJelatine;
        int requirValue2 = RequireValue(value2);
        ClickJelatineText.text = requirValue2.ToString("n0");

        APTLevel.text = value.ToString();
        CreateLevel.text = value2.ToString();
    }

    private void UseCoin(int value)
    {
        _Coin.SetCoin(-value);
    }

    private void UseJelatine(int value)
    {
        _Jelatine.SetJelatine(-value);
    }

    private void OnEnable()
    {
        _Ac.SetTrigger("DoUp");
        SetData();
    }
    public void ShowDouwnUI()
    {
        _Ac.SetTrigger("DoDown");
    }

    public void OnClickBuyJellyMax()
    {
        
        int value = _GamePlayManager.JellyMax;

        
        int requirValue = RequireValue(value);
        if (_Coin.CoinValue >= requirValue)
        {
            SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Buy);
            UseCoin(value);
            _GamePlayManager.JellyMax = ++value;
        }
        else
        {
            SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Fail);
        }
        SetData();


    }

    public void OnClickBuyClickJelatine()
    {
        
        int value = _GamePlayManager.ClickJelatine;

        
        int requirValue = RequireValue(value);
        if (_Jelatine.JelatineValue >= requirValue)
        {
            SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Buy);
            UseJelatine(value);
            _GamePlayManager.ClickJelatine = ++value;
        }
        else
        {
            SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Fail);
        }
        SetData();
    }

    public void SetReference(GamePlayManager manager)
    {
        _GamePlayManager = manager;
    }
}
