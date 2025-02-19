using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyIdle : IState<Jelly>
{

    private Jelly _Jelly;

    public void OperateEnter(Jelly sender)
    {
        _Jelly = sender;
        if (_Jelly != null)
        {
            _Jelly.Speed = 0;
            int front_Or_Back = Random.Range(-1, 2);
            float x = Random.Range(0f, 1f) * (float)front_Or_Back;
            front_Or_Back= Random.Range(-1, 2);
            float y = Random.Range(0f, 1f) * (float)front_Or_Back;

            _Jelly.Angle=new Vector2(x, y);
            Debug.Log("IdleAngle : " + _Jelly.Angle);
        }
    }
    public void OperateUpdate(Jelly sender)
    {

    }
    public void OperateExit(Jelly sender)
    {
        Debug.Log("JellyIdle Exit");
    }



}
