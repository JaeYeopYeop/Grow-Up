using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    private T m_sender;

    public IState<T> CurrentState { get; set; }

    //기본 상태를 생성시에 설정하게 생성자 선언
    public StateMachine(T sender, IState<T> state)
    {
        m_sender = sender;
        SetState(state);
    }

    public void SetState(IState<T> state)
    {
        Debug.Log("State : "+ state.ToString());

        // error
        if(state== null)
        {
            Debug.LogError("There's no State");return;
        }
        // 같은 state
        else if (state==CurrentState)
        {
            Debug.Log("same State"); return;
        }
        // state 변경
        if(CurrentState!=null)
            CurrentState.OperateExit(m_sender);

        CurrentState = state;
        CurrentState.OperateEnter(m_sender);
        
    }

    //  State Update
    public void DoOperateUpdate()
    {
        if (CurrentState == null) {
            Debug.LogError("no State");
            return;
        }
        CurrentState.OperateUpdate(m_sender);
    }
}
