using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class JellyClicked : IState<Jelly>
{
    private Jelly _Jelly;

    GameObject target = null;
    private bool isMouseDrag = false;

    // ���� ȭ�� �ػ� ��������
    float screenWidth = Screen.width;
    float screenHeight = Screen.height;

    // Reference Resolution (200x95)
    float referenceWidth = 200f;
    float referenceHeight = 95f;

    private Vector3 _screenPos;

    private JellySell _JellySell=null;

    public void OperateEnter(Jelly sender)
    {
        _Jelly = sender;
        if(_JellySell == null) 
            _JellySell=GameObject.Find("SellButton").GetComponent<JellySell>();

    }

    public void OperateExit(Jelly sender)
    {
    }

    public void OperateUpdate(Jelly sender)
    {
        MouseProcess();
    }


    GameObject GetTarget()
    {
        GameObject _target = null;


        // ���콺 ������ ��������
        Vector3 mousePos = Input.mousePosition;

        // ���콺 ��ġ�� �ִ� ������Ʈ�� ��� ã��
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Debug.DrawRay(ray.origin,ray.direction*10,Color.red,10f);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction * 10);

        foreach (RaycastHit2D rch in hits)
        {
            if (rch.collider != null && rch.transform.tag.Contains("Jelly"))
            {
                _target = rch.collider.gameObject;
                break;
            }
        }

        return _target;
    }

    private void MouseProcess()
    {
        if (Input.GetMouseButton(0))
        {
            if(target == null)
                target = GetTarget();
            if (target != null)
            {
                isMouseDrag = true;
                // ���콺 ������ ��������
                Vector3 mousePos = Input.mousePosition;
                /*
                                // ��ũ�� ��ǥ�� Reference Resolution ������ �°� ��ȯ
                                float adjustedX = (mousePos.x / screenWidth) * referenceWidth;
                                float adjustedY = (mousePos.y / screenHeight) * referenceHeight;
                                // ������ �°� ��ȯ�� ��ǥ�� ���ο� ���콺 ������ ����
                                Vector3 adjustedMousePos = new Vector3(adjustedX, adjustedY, mousePos.z);
                */

                // ��ũ�������� Ÿ���� ������(z�� ����)
                _screenPos = Camera.main.WorldToScreenPoint(target.transform.position);

                Vector3 targetPos = target.transform.position;
                // ���콺�� ���尪�� ���ߵ��� ��ȯ, z���� 0�̸� �ȵǰ� Ÿ���� ��ũ�� z���� ���
                Vector3 currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPos.z));
                // Ÿ���� ��ġ�ϰ� �� ���� ������(z���� ���忡���� ���� ���� ����ϵ��� �ϰ�, xy�� ���콺 ���� ���󰣴�)
                target.transform.position = new Vector3(currentPosition.x, currentPosition.y-0.2f, targetPos.z);


            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDrag = false;
            if (target != null)
            {
                if (_JellySell._Jelly != null)
                {
                    
                    int value = _Jelly.JellyLevel * (_Jelly.JellyType + 1) * 10;
                    _Jelly.SetGoodsText(value);
                    
                    _JellySell.JellySellNow();
                }


                _screenPos = Camera.main.WorldToScreenPoint(target.transform.position);

                Vector3 targetPos = target.transform.position;
                Vector3 currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPos.z));
                target.transform.position = new Vector3(currentPosition.x, currentPosition.y - 0.2f, targetPos.z);
                Vector3 nowPosition = target.transform.position;
                if (nowPosition.x < -5f || nowPosition.x > 5f || nowPosition.y < -1.75f || nowPosition.y > 0.75f)
                {
                    target.transform.position = Vector3.zero;
                }

                _Jelly._StateMachine.SetState(_Jelly.idle);
                _Jelly.state = Jelly.JellyState.Idle;
            }
            else
            {
                _Jelly._StateMachine.SetState(_Jelly.idle);
                _Jelly.state = Jelly.JellyState.Idle;

            }
            target=null;
        }

        if (isMouseDrag)
        {
            if (target != null)
            {
                // ���콺 ������ ��������
                Vector3 mousePos = Input.mousePosition;
                _screenPos = Camera.main.WorldToScreenPoint(target.transform.position);

                Vector3 targetPos= target.transform.position;
                Vector3 currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _screenPos.z));
                target.transform.position = new Vector3(currentPosition.x, currentPosition.y-0.2f, targetPos.z);
            }
        }

    }

}
