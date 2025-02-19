using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyTurn : IState<Jelly>
{
    Jelly _Jelly;
    public void OperateEnter(Jelly sender)
    {
        Debug.Log("JellyTurnEnter");
        _Jelly = sender;
        _Jelly.GetComponent<SpriteRenderer>().flipX = !_Jelly.GetComponent<SpriteRenderer>().flipX;
    }
    public void OperateUpdate(Jelly sender)
    {

    }
    public void OperateExit(Jelly sender)
    {
        Debug.Log("JellyTurnExit");
    }
}
