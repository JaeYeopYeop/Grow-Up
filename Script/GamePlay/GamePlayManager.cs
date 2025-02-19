using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager Instance;
    
    public List<Jelly> _Jellys;

    public bool _isJellyUIOnOff=false;
    public bool _isOptionUIOnOff=false;

    [SerializeField] private GameObject _OptionPanel;

    public Sprite[] _JellyImageList;
    public String[] _JellyNameList;

    [SerializeField] private JellySaver _JellySaver;
    private int _JellyMax;
    public int JellyMax
    {
        get
        {
            _JellyMax = PlayerPrefs.GetInt("JellyMax", 0);
            return _JellyMax;

        }
        set
        {
            PlayerPrefs.SetInt("JellyMax", value);
            _JellyMax = value;
        }
    }
    private int _ClickJelatine;
    public int ClickJelatine
    {
        get
        {
            _ClickJelatine = PlayerPrefs.GetInt("ClickJelatine", 0);
            return _ClickJelatine;
        }
        set
        {
            PlayerPrefs.SetInt("ClickJelatine", value);
            _ClickJelatine = value;
        }
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(JellyMax==0) JellyMax = 1;
        if(ClickJelatine==0)ClickJelatine = 1;
        SoundManager.instance.PlayBgm(SoundManager.EBGM.BGM_Original);

    }

    public void FindRefrence()
    {
        if (_OptionPanel == null) _OptionPanel = GameObject.Find("Canvas").transform.Find("OptionPanel").gameObject;
        if (_JellySaver == null) _JellySaver = GameObject.Find("JellySaver").GetComponent<JellySaver>();
    }

    public void OptionOnOff()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_OptionPanel == null)
            {
                _OptionPanel = GameObject.Find("Canvas").transform.Find("OptionPanel").gameObject;
            }
            if (!_OptionPanel.activeSelf)
            {
                _OptionPanel.SetActive(true);
            }
            else
            {
                SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_PauseOut);
                _OptionPanel.GetComponentInChildren<Animator>().SetTrigger("DoDown");
                Invoke("ShowDownOptionPanel", 1f);
            }
        }
    }

    void ShowDownOptionPanel()
    {
        _OptionPanel.SetActive(false);
    }


    private void Update()
    {
        OptionOnOff();
    }


}
public class JellyGold
{
    private int[] _JellySellGold = new int[12] {
        1,4,15,50,160,500,1600,5000,16000,50000,160000,500000
        };

    public int this[int i]
    {
        get
        {
            return _JellySellGold[i];
        }
        set
        {
            _JellySellGold[i] = value;
        }
    }
}

public class JellyJelatine
{
    private int[] _JellyJelatine=new int[12] {
    1,4,15,50,160,500,1600,5000,16000,50000,160000,500000
    };

    public int this[int i]
    {
        get
        {
            return _JellyJelatine[i];
        }
        set
        {
            _JellyJelatine[i] = value;
        }
    }
}