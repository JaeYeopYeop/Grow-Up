using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JellyUI : MonoBehaviour
{
    [SerializeField] private GamePlayManager _GamePlayManager;
    [SerializeField] private Transform _Space;
    [SerializeField] private GameObject _JellyPrefab;
    [SerializeField] private Jelatine _Jelatine;
    [SerializeField] private Coin _Coin;

    [SerializeField] private Animator _Ac;
    [SerializeField] private GameObject _Unlock;
    [SerializeField] private GameObject _Lock;

    [SerializeField] private Image _JellyImage;
    [SerializeField] private Text _JellyName;
    [SerializeField] private Text _JellyNum;
    [SerializeField] private Text _JellyPrice;

    [SerializeField] private Image _JellyImageLock;
    [SerializeField] private Text _JellyNameLock;
    [SerializeField] private Text _JellyNumLock;
    [SerializeField] private Text _JellyPriceLock;

    private int page = 0;
    private JellyJelatine _JellyJelatine = new JellyJelatine();
    private JellyGold _JellyGold = new JellyGold();
    //[SerializeField] private Button _PrevBtn;
    //[SerializeField] private Button _NextBtn;
    // Start is called before the first frame update
    void Start()
    {
        _Ac = this.GetComponent<Animator>();

        _JellyImage.sprite = _GamePlayManager._JellyImageList[page];
        _JellyName.text = _GamePlayManager._JellyNameList[page];
        _JellyNum.text = string.Format("#{0}", page);
        _JellyPrice.text = string.Format("{0:n0}", _JellyJelatine[page]);
    }
    private void OnEnable()
    {
        _Ac.SetTrigger("DoUp");
        SetUIData();
    }
    public void ShowDouwnUI()
    {
        _Ac.SetTrigger("DoDown");
    }

    private void SetUIData()
    {
        if (PlayerPrefs.GetInt($"UnlockJelly{page}", 0) >= 1)
        {
            _Unlock.SetActive(true);
            _Lock.SetActive(false);

            _JellyImage.sprite = _GamePlayManager._JellyImageList[page];
            _JellyName.text = _GamePlayManager._JellyNameList[page];
            _JellyNum.text = string.Format("#{0}", page);
            _JellyPrice.text = string.Format("{0:n0}", _JellyJelatine[page]);
        }
        else
        {
            _Unlock.SetActive(false);
            _Lock.SetActive(true);
            
            _JellyImageLock.sprite = _GamePlayManager._JellyImageList[page];
            _JellyNameLock.text = _GamePlayManager._JellyNameList[page];
            _JellyNumLock.text = string.Format("#{0}", page);
            _JellyPriceLock.text = string.Format("{0:n0}", _JellyGold[page]);
        }
    }

    public void OnClickPrevBtn()
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Button);
        page--;
        if (page < 0) page = 11;

        SetUIData();
    }

    public void OnClickNextBtn() 
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Button);
        page++;
        if(page> 11) page = 0;

        SetUIData();
    }

    public void OnClickUnlockBtn()
    {
        
        if (_Coin.CoinValue >= _JellyGold[page])
        {
            SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Unlock);
            _Coin.SetCoin(-(_JellyGold[page]));
            PlayerPrefs.SetInt($"UnlockJelly{page}", 1);
            SetUIData();
        }
        else
        {
            SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Fail);
        }
    }
    public void OnClickBuyJellyBtn()
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Buy);
        if (_JellyJelatine[page] <= _Jelatine.JelatineValue && _GamePlayManager.JellyMax>=_GamePlayManager._Jellys.Count)
        {
            _Jelatine.SetJelatine(-(_JellyJelatine[page]));
            GameObject obj = Instantiate(_JellyPrefab);
            obj.transform.SetParent(_Space);
            obj.transform.position = Vector3.zero;
            obj.GetComponent<Jelly>().JellyType = page;
            obj.GetComponent<Jelly>().JellyExperience = 1L;
            obj.GetComponent<Jelly>().JellyLevel = 1;
        }
    }

    public void SetReference(GamePlayManager manager)
    {
        _GamePlayManager = manager;
    }
}
