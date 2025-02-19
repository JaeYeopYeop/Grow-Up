using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private BoardManager _BoardManager;
    [SerializeField] private MainUI _MainUI;
    [SerializeField] private GameUI _GameUI;
    [SerializeField] private Text ResultScoureText;
    [SerializeField] private Text PlusJelatine;
    [SerializeField] private Text PlusGold;
    [SerializeField] private Coin _Coin;
    [SerializeField] private Jelatine _Jelatine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Clear);
        ResultScoureText.text = _BoardManager.ScoureValue.ToString();
        PlusJelatine.text="+ "+(_BoardManager.ScoureValue/50).ToString();
        PlusGold.text="+ "+(_BoardManager.ScoureValue/50).ToString();
        _Coin.SetCoin(_BoardManager.ScoureValue / 50);
        _Jelatine.SetJelatine(_BoardManager.ScoureValue / 50);
    }

    private void OnDisable()
    {
        _BoardManager.ScoureValue = 0;
    }
    public void ShowMainBtn()
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Button);
        _MainUI.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        _GameUI.gameObject.SetActive(false);
    }
}
