using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{

    [SerializeField] private GameUI _GameUI;
    [SerializeField] private BoardManager _BoardManager;
    [SerializeField] private Animator[] _MenuAC;

    [SerializeField] private GameObject[] _MenuList;

    [SerializeField] private Coin _Coin;
    [SerializeField] private Jelatine _Jelatine;

    [SerializeField] private Button _TimeBtn;
    [SerializeField] private Button _ScoureBtn;

    [SerializeField] private Text TimeUpgradeText;
    [SerializeField] private Text ScoureUpgradeText;

    [SerializeField] private Text RequireTimeUpgradeText;
    [SerializeField] private Text RequireScoureUpgradeText;
    private int _MenuCount=0;
    private const int _MaxLevel = 11;


    private void Start()
    {
        if (_BoardManager.BonusTime == 12)
        {
            _TimeBtn.interactable = false;
        }
        else
        {
            JellyGold jellyGold = new JellyGold();
            TimeUpgradeText.text = _BoardManager.BonusTime.ToString();
            RequireTimeUpgradeText.text= (jellyGold[_BoardManager.BonusTime] * 10).ToString();
        }

        if (_BoardManager.BonusScoure == 12)
        {
            _ScoureBtn.interactable = false;
        }
        else
        {
            JellyJelatine jellyJelatine = new JellyJelatine();
            ScoureUpgradeText.text = (_BoardManager.BonusScoure).ToString();
            RequireScoureUpgradeText.text= (jellyJelatine[_BoardManager.BonusScoure] * 10).ToString();
        }
    }

    public void LevelBtnClick(Text ThisLevel)
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Button);
        _BoardManager.Level = int.Parse(ThisLevel.text)+4;
        _BoardManager.GameStart();
        this.gameObject.SetActive(false);
        _GameUI.gameObject.SetActive(true);
    }
    
    public void TimeUpgradeBtnClick()
    {
        if (_BoardManager.BonusTime < 12)
        {
            SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Buy);
            JellyGold jellyGold = new JellyGold();
            if (_Coin.CoinValue - jellyGold[_BoardManager.BonusTime]*10 >= 0)
            {
                _Coin.SetCoin(-jellyGold[_BoardManager.BonusTime] * 10);
                TimeUpgradeText.text = (++_BoardManager.BonusTime).ToString();
                RequireTimeUpgradeText.text = (jellyGold[_BoardManager.BonusTime] * 10).ToString();
                if (_BoardManager.BonusTime == 12)
                {
                    _TimeBtn.interactable = false;
                }
            }
        }
        else
        {
            SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Fail);
        }
    }

    public void ScoureUpgradeBtnClick()
    {
        if(_BoardManager.BonusScoure < 12)
        {
            SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Buy);
            JellyJelatine jellyJelatine = new JellyJelatine();
            if(_Jelatine.JelatineValue - jellyJelatine[_BoardManager.BonusScoure] * 10 >= 0)
            {
                _Jelatine.SetJelatine(-jellyJelatine[_BoardManager.BonusScoure] * 10);
                ScoureUpgradeText.text=(++_BoardManager.BonusScoure).ToString();
                RequireScoureUpgradeText.text = (jellyJelatine[_BoardManager.BonusScoure] * 10).ToString();
                if (_BoardManager.BonusScoure == 12)
                {
                    _ScoureBtn.interactable = false;
                }
            }
        }
        else
        {
            SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Fail);
        }

    }

    IEnumerator Hide(GameObject target)
    {
        yield return new WaitForSeconds(1f);
        target.SetActive(false);
    }

    public void RightBtn()
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Button);
        if (_MenuCount == 0)
        {
            _MenuAC[_MenuCount].SetTrigger("DoLeft");
            StartCoroutine(Hide(_MenuList[_MenuCount++]));
            BackLeft();
        }
        else
        {
            _MenuAC[_MenuCount].SetTrigger("DoLeft");
            StartCoroutine(Hide(_MenuList[_MenuCount--]));
            BackLeft();
        }
    }

    public void LeftBtn() {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Button);
        if (_MenuCount == 0)
        {
            _MenuAC[_MenuCount].SetTrigger("DoRight");
            StartCoroutine(Hide(_MenuList[_MenuCount++]));
            BackRight();
        }
        else
        {
            _MenuAC[_MenuCount].SetTrigger("DoRight");
            StartCoroutine(Hide(_MenuList[_MenuCount--]));
            BackRight();
        }
    }

    private void BackRight()
    {
        _MenuList[_MenuCount].SetActive(true);
        _MenuAC[_MenuCount].SetTrigger("DoBackRight");
    }

    private void BackLeft()
    {
        _MenuList[_MenuCount].SetActive(true);
        _MenuAC[_MenuCount].SetTrigger("DoBackLeft");
    }
}
