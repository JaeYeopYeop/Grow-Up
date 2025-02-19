using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class JellySell : MonoBehaviour
{

    [SerializeField] private GamePlayManager _GamePlayManager;
    [SerializeField] private Jelatine _Jelatine;
    [SerializeField] private Coin _Coin;

    public Jelly _Jelly=null;

    public void SetReference(GamePlayManager manager)
    {
        _GamePlayManager = manager;
    }
    public void JellySellEnter()
    {
        foreach(Jelly jelly in _GamePlayManager._Jellys)
        {
            if (jelly.state == Jelly.JellyState.Clicked)
            {
                _Jelly = jelly;
                Debug.Log($"Enter {_Jelly.name}");
                
            }
            
        }
    }

    public void JellySellNow()
    {
        if( _Jelly != null)
        {
            SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_JellySell);
            JellyGold _JellyGold= new JellyGold();
            int total = (_Jelly.JellyLevel * _JellyGold[_Jelly.JellyType]);
            _Coin.SetCoin(total);
            Debug.Log($"SellJelly {_Jelly.name}, Get Coin {total}");
            _GamePlayManager._Jellys.Remove(_Jelly);
            Destroy(_Jelly.gameObject);
            _Jelly = null;
        }
    }

    public void JellySellExit()
    {
        if( _Jelly != null )
            Debug.Log($"Exit {_Jelly.name}");
        _Jelly = null;
    }
    
}
