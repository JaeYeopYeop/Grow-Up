using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JellyTouch : IState<Jelly>
{
    private Jelly _Jelly;
    private Jelatine _Jelatine;

    private float _IsTouching = 0f;
    public void OperateEnter(Jelly sender)
    {
        _IsTouching = 0f;
        _Jelly = sender;
        _Jelly.GetComponent<Animator>().SetTrigger("doTouch");
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_JellyTouch);
        if (_Jelatine == null) { 
            _Jelatine= GameObject.Find("Jelatine").GetComponent<Jelatine>();
        }
        if (_Jelatine.JelatineValue < Jelatine.MaxJelatineValue)
        {
            _Jelly.JellyExperience+= PlayerPrefs.GetInt("ClickJelatine", 0);
            if (_Jelly.JellyExperience >= _Jelly.MaxJellyExperience[_Jelly.JellyLevel - 1])
                _Jelly.JellyLevelUp();
            int temp = (PlayerPrefs.GetInt("ClickJelatine", 0)/5) * _Jelly.JellyLevel;
            _Jelatine.SetJelatine(temp);
            _Jelly.SetExperienceText(PlayerPrefs.GetInt("ClickJelatine", 0), true);
            
        }
    }

    public void OperateExit(Jelly sender)
    {

    }

    public void OperateUpdate(Jelly sender)
    {
        Debug.Log("Touch");
        _IsTouching += Time.deltaTime;
        if (_IsTouching >= 0.5f)
        {
            _Jelly._StateMachine.SetState(_Jelly.clicked);
            _Jelly.state = Jelly.JellyState.Clicked;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _Jelly._StateMachine.SetState(_Jelly.idle);
            _Jelly.state = Jelly.JellyState.Idle;
        }
    }
}
