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

    // 현재 화면 해상도 가져오기
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


        // 마우스 포지션 가져오기
        Vector3 mousePos = Input.mousePosition;

        // 마우스 위치에 있는 오브젝트들 모두 찾기
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
                // 마우스 포지션 가져오기
                Vector3 mousePos = Input.mousePosition;
                /*
                                // 스크린 좌표를 Reference Resolution 비율에 맞게 변환
                                float adjustedX = (mousePos.x / screenWidth) * referenceWidth;
                                float adjustedY = (mousePos.y / screenHeight) * referenceHeight;
                                // 비율에 맞게 변환된 좌표로 새로운 마우스 포지션 생성
                                Vector3 adjustedMousePos = new Vector3(adjustedX, adjustedY, mousePos.z);
                */

                // 스크린에서의 타겟의 포지션(z값 얻기용)
                _screenPos = Camera.main.WorldToScreenPoint(target.transform.position);

                Vector3 targetPos = target.transform.position;
                // 마우스를 월드값에 맞추도록 변환, z값은 0이면 안되고 타겟의 스크린 z값을 사용
                Vector3 currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPos.z));
                // 타겟이 위치하게 될 실제 포지션(z값은 월드에서의 실제 값을 사용하도록 하고, xy는 마우스 값을 따라간다)
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
                // 마우스 포지션 가져오기
                Vector3 mousePos = Input.mousePosition;
                _screenPos = Camera.main.WorldToScreenPoint(target.transform.position);

                Vector3 targetPos= target.transform.position;
                Vector3 currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _screenPos.z));
                target.transform.position = new Vector3(currentPosition.x, currentPosition.y-0.2f, targetPos.z);
            }
        }

    }

}
