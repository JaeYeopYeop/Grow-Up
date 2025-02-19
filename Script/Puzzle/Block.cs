using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BLCOKSTATE
{
    STOP,
    MOVE
}
public enum DIRECTION
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public class Block : MonoBehaviour
{
    [SerializeField] private TextMesh _TextMesh;
    private int _type;
    public int Type 
    { 
        get {
            return _type;
        }
        set {
            _TextMesh.text = value.ToString();
            _type = value;
        } 
    }
    [SerializeField] private SpriteRenderer _BlockImage;
    public SpriteRenderer BlockImage { get => _BlockImage; }

    public float Speed { get; set; } = 5f; // 블럭의 이동 속도

    public BLCOKSTATE State { get; set; } = BLCOKSTATE.STOP; // 블럭 상태 저장
    public DIRECTION _BlockDirection;            // 블럭의 움직임 저장

    private Vector2 _movePos;
    public Vector2 MovePos { get => _movePos; set => _movePos = value; }

    public float Width { get; set; } // 블럭의 Width
    public int Column { get; set; } // 블럭의 Column
    public int Row { get; set; } // 블럭의 Row

    public void Init(int col,int row, int type, Sprite sprite)
    {
        Column = col;
        Row = row;
        Type = type;
        _BlockImage.sprite = sprite;
    }

    public void Move(DIRECTION Direction)
    {
        switch (Direction)
        {
            case DIRECTION.LEFT:
                _movePos= transform.position;
                _movePos.x -= Width;
                _BlockDirection = DIRECTION.LEFT;
                State = BLCOKSTATE.MOVE;
                break;

            case DIRECTION.RIGHT:
                _movePos = transform.position;
                _movePos.x += Width;
                _BlockDirection = DIRECTION.RIGHT;
                State = BLCOKSTATE.MOVE;
                break;

            case DIRECTION.UP:
                _movePos = transform.position;
                _movePos.y += Width;
                _BlockDirection = DIRECTION.UP;
                State = BLCOKSTATE.MOVE;
                break;

            case DIRECTION.DOWN:
                _movePos = transform.position;
                _movePos.y -= Width;
                _BlockDirection = DIRECTION.DOWN;
                State = BLCOKSTATE.MOVE;
                break;
        }
    }
    public void Move(DIRECTION direction, int movecount)
    {

        switch (direction) {
            case DIRECTION.LEFT:
                _BlockDirection = DIRECTION.LEFT;
                State= BLCOKSTATE.MOVE;
                break;

            case DIRECTION.RIGHT:
                _BlockDirection = DIRECTION.RIGHT;
                State = BLCOKSTATE.MOVE;
                break;
            case DIRECTION.UP:
                _BlockDirection = DIRECTION.UP;
                State = BLCOKSTATE.MOVE;
                break;
            case DIRECTION.DOWN:
                _BlockDirection = DIRECTION.DOWN;
                State = BLCOKSTATE.MOVE;
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (State == BLCOKSTATE.MOVE)
        {
            switch (_BlockDirection)
            {
                case DIRECTION.LEFT:
                    transform.Translate(Vector3.left* Time.deltaTime*Speed);
                    if (transform.position.x <= _movePos.x)
                    {
                        transform.position = _movePos;
                        State = BLCOKSTATE.STOP;
                        BlockImage.sortingOrder = 0;
                    }
                    break;

                case DIRECTION.RIGHT:
                    transform.Translate(Vector3.right * Time.deltaTime * Speed);
                    if (transform.position.x >= _movePos.x)
                    {
                        transform.position = _movePos;
                        State = BLCOKSTATE.STOP;
                        BlockImage.sortingOrder = 0;
                    }
                    break;

                case DIRECTION.UP:
                    transform.Translate(Vector3.up * Time.deltaTime * Speed);
                    if (transform.position.y >= _movePos.y)
                    {
                        transform.position = _movePos;
                        State = BLCOKSTATE.STOP;
                        BlockImage.sortingOrder = 0;
                    }
                    break;

                case DIRECTION.DOWN:
                    transform.Translate(Vector3.down * Time.deltaTime * Speed);
                    if (transform.position.y <= _movePos.y)
                    {
                        transform.position = _movePos;
                        State = BLCOKSTATE.STOP;
                        BlockImage.sortingOrder = 0;
                    }
                    break;
            }
        }
        
    }
}
