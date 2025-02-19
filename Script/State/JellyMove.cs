using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyMove : IState<Jelly>
{
    private Jelly _Jelly;
    private Vector3 _Position;
    public void OperateEnter(Jelly sender)
    {
        _Jelly = sender;
        _Jelly.Speed = Random.Range(0.5f, _Jelly.MaxSpeed);
        _Position=_Jelly.gameObject.transform.position;
        if (_Jelly.Angle.x < 0) _Jelly.GetComponent<SpriteRenderer>().flipX = true;
        else _Jelly.GetComponent<SpriteRenderer>().flipX = false;
        Debug.Log("Angle : " + _Jelly.Angle);
        
        if(_Jelly.Angle.x!=0 || _Jelly.Angle.y!=0)
            _Jelly.GetComponent<Animator>().SetBool("isWalk", true);

    }
    public void OperateUpdate(Jelly sender)
    {
        if(sender == _Jelly && _Jelly!=null)
        {
            //Debug.Log("move");
            _Position = _Jelly.gameObject.transform.position;
            if (_Position.x >= 5f && _Jelly.Angle.x > 0f)
            {
                _Jelly.Angle = new Vector2(-_Jelly.Angle.x, _Jelly.Angle.y);
                _Jelly.GetComponent<SpriteRenderer>().flipX = !_Jelly.GetComponent<SpriteRenderer>().flipX;
            }
            else if (_Position.x < -5f && _Jelly.Angle.x < 0f)
            {
                _Jelly.Angle = new Vector2(-_Jelly.Angle.x, _Jelly.Angle.y);
                _Jelly.GetComponent<SpriteRenderer>().flipX = !_Jelly.GetComponent<SpriteRenderer>().flipX;
            }
            else if (_Position.y >= 0.75f && _Jelly.Angle.y > 0f)
            {
                _Jelly.Angle = new Vector2(_Jelly.Angle.x, -_Jelly.Angle.y);
            }
            else if (_Position.y <= -1.75f && _Jelly.Angle.y < 0f)
            {
                _Jelly.Angle = new Vector2(_Jelly.Angle.x, -_Jelly.Angle.y);

            }
            
            //_Jelly.GetComponent<Transform>().Translate(_Jelly.Angle * _Jelly.Speed * Time.deltaTime);
            _Jelly.gameObject.transform.Translate(_Jelly.Angle * _Jelly.Speed * Time.deltaTime);
        }
    }
    public void OperateExit(Jelly sender)
    {
        _Jelly.GetComponent<Animator>().SetBool("isWalk", false);
        Debug.Log("JellyMove Exit");
    }
}
