using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    private T m_sender;

    public IState<T> CurrentState { get; set; }

    //�⺻ ���¸� �����ÿ� �����ϰ� ������ ����
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
        // ���� state
        else if (state==CurrentState)
        {
            Debug.Log("same State"); return;
        }
        // state ����
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
