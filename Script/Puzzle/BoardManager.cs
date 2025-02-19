using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Playables;


public enum GamePlayState
{
    InputOK,
    AfterInputMoveCheck, // �Է��Ŀ� ���� ������ üũ�ϴ� ����
    MatchCheck, // �Է��Ŀ� ��Ī�Ǵ� ���� �ִ��� üũ�ϴ� ����
    AfterMatchCheck_MoveCheck,  // ��ġüũ���Ŀ� ���� �����̴� �������� üũ
    DropBlock,  // ���ο� ���� ��ġ
    AfterDropBlockBlock_MoveCheck,  // DropBlock���Ŀ� ���� �����̴� �������� üũ
    InputCancel,
    DropBlockMatchCheck,
    Undo

}

public class BoardManager : MonoBehaviour
{
    public enum MouseMoveDirection
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    private GamePlayState PlayState { get; set; } = GamePlayState.InputOK;

    [SerializeField] private Sprite[] _Sprites;
    [SerializeField] private GameObject _BlockPrefab;
    [SerializeField] private GameObject[,] _GameBoard;
    private Vector2 _screenPos;
    private float _screenWidth;
    private float _blockWidth;

    public float _Xmargin = 2f;
    public float _Ymargin = 0f;

    private float _Scale = 0f;

    [HideInInspector]
    public int _Column;
    [HideInInspector]
    public int _Row;

    private Vector3 _StartPos = Vector3.zero; // ���콺 Ŭ�� �� ��ǥ
    private Vector3 _EndPos = Vector3.zero;   // ���콺 Ŭ�� �� ������ �̵���ǥ

    private bool _isOver = false;

    private GameObject _ClickedObject = null;
    private bool _mouseClick = false;
    private float _mouseDistance = 0.1f;

    private bool _isInputOk = false;

    private int TYPECOUNT = 5; // ���� ���� ����

    private List<GameObject> _RemovingBlock = new List<GameObject>(); // ������ �� ����
    private List<GameObject> _RemovedBlock = new List<GameObject>(); // ������ �� ����


    [SerializeField] private int _YPOS = 3; // �� ���� �� ���� ��ġ
    private MouseMoveDirection _CurrentMoveDirection;

    private const int MATCHCOUNT = 3;

    [SerializeField] private GameUI _GameUI;
    [SerializeField] private GameObject _ResultUI;
    private int scoureValue;

    public int ScoureValue { get => scoureValue;
        set {
            scoureValue = value;
        }

    }

    private float playTime = 60f;

    public float PlayTime
    {
        get => playTime;
        set => playTime = value;
    }

    private int bonusTime;
    public int BonusTime
    {
        get
        {
            bonusTime = PlayerPrefs.GetInt("BonusTime", 0);
            return bonusTime;
        }
        set
        {
            PlayerPrefs.SetInt("BonusTime", value);
            bonusTime = PlayerPrefs.GetInt("BonusTime",0);
        }
    }

    private int bonusScoure;
    public int BonusScoure
    {
        get {
            bonusScoure = PlayerPrefs.GetInt("BonusScoure", 0);
            return bonusScoure;
        }
        set
        {
            PlayerPrefs.SetInt("BonusScoure", value);
            bonusScoure = PlayerPrefs.GetInt("BonusScoure",0);
        }
    }

    private int level=5;
    public int Level
    {
        get => level;
        set => level = value;
    } 

    private void Awake()
    {
        PlayState = GamePlayState.InputCancel;
        _screenPos = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        Debug.Log($"Screen(0,0,0) == World{_screenPos}");
        _screenPos.y = -_screenPos.y;
        _screenWidth = Mathf.Abs(_screenPos.x + _screenPos.x);

        _blockWidth = _BlockPrefab.GetComponent<Block>().BlockImage.sprite.rect.size.x / 100f; // pixel per unity = 100

    }
    private void Start()
    {
        //GameStart();
    }

    public void GameStart()
    {
        this.enabled = true;
        MakeBlock(level, level);

        StartCoroutine(StartPlay());
        PlayTime = 60+ BonusTime*10;
    }

    IEnumerator StartPlay()
    {
        yield return new WaitForSeconds(1f);
        PlayState = GamePlayState.AfterInputMoveCheck;
        yield return new WaitForSeconds(1.5f);
        PlayState = GamePlayState.AfterMatchCheck_MoveCheck;
    }

    /// <summary>
    /// �� Ÿ���� �������� ����
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    public int GetRandomBlockType()
    {
        return UnityEngine.Random.Range(0, TYPECOUNT);
    }
    void MakeBlock(int column, int row)
    {
        _Column = column;
        _Row = row;
        float width = _screenWidth - _Xmargin * 2; // ��ũ�� ���� 
        float blockwidth = _blockWidth * row;    // �� ����
        _Scale = (width / blockwidth)/3;             // ���� ������
        if (_GameBoard != null)
        {
            foreach (var obj in _GameBoard)
            {
                if (obj != null)
                    Destroy(obj);
            }
            _GameBoard = null;
        }

        _GameBoard = new GameObject[column, row];



        for (int col = 0; col < column; col++)
        {
            for (int r = 0; r < row; r++)
            {
                _GameBoard[col, r] = Instantiate(_BlockPrefab) as GameObject;

                // ���� ���� ������ �� ����
                _GameBoard[col, r].transform.localScale = new Vector3(_Scale, _Scale, 0f);
                // ���� ���� ��ġ �� ����
                _GameBoard[col, r].transform.position = new Vector3(_screenPos.x + _Xmargin + (r * _blockWidth * _Scale) + (_blockWidth * _Scale) / 2,
                    _screenPos.y - _Ymargin - (col * _blockWidth * _Scale) - (_blockWidth * _Scale) / 2, 0f);

                // �� ���� Ÿ��(�̹���) �����ֱ�
                int type = GetRandomBlockType();
                _GameBoard[col, r].GetComponent<Block>().Type = type;
                _GameBoard[col, r].GetComponent<Block>().BlockImage.sprite = _Sprites[type];
                // �� ���� ���� ���� ����
                _GameBoard[col, r].GetComponent<Block>().Width = (_blockWidth * _Scale);
                _GameBoard[col, r].GetComponent<Block>().Column = col;
                _GameBoard[col, r].GetComponent<Block>().Row = r;
                _GameBoard[col, r].name = $"Block({col},{r})";
            }
        }


    }
    /// <summary>
    /// 3�� �̻� ��ġ�Ǵ� ��ã��
    /// </summary>
    private bool CheckMatchBlock()
    {
        List<GameObject> _matchList = new List<GameObject>(); // ��Ī�� �� ����
        List<GameObject> _tempMatchList = new List<GameObject>(); // �ӽ� ��Ī ������

        int checkType = 0;
        _RemovingBlock.Clear(); // �ʱ�ȭ

        // ���ι��� ��ġcheck
        for (int row = 0; row < _Row; row++)
        {
            if (_GameBoard[0, row] == null) continue;

            checkType = _GameBoard[0, row].GetComponent<Block>().Type;

            for (int col = 0; col < _Column; col++)
            {
                if (_GameBoard[col, row] == null) continue;
                if (checkType == _GameBoard[col, row].GetComponent<Block>().Type)
                {
                    _tempMatchList.Add(_GameBoard[col, row]);
                }
                else
                {
                    if (_tempMatchList.Count >= 3)
                    {
                        _matchList.AddRange(_tempMatchList);
                        _tempMatchList.Clear();
                        checkType = _GameBoard[col, row].GetComponent<Block>().Type;
                        _tempMatchList.Add(_GameBoard[col, row]);
                    }
                    else
                    {
                        _tempMatchList.Clear();
                        checkType = _GameBoard[col, row].GetComponent<Block>().Type;
                        _tempMatchList.Add(_GameBoard[col, row]);
                    }
                }
            }
            // ���������� check �� �� ��
            if (_tempMatchList.Count >= 3)
            {
                _matchList.AddRange(_tempMatchList);
                _tempMatchList.Clear();
            }
            else
            {
                _tempMatchList.Clear();
            }
        }

        // ���ι��� ��ġcheck
        for (int col = 0; col < _Column; col++) {
            if (_GameBoard[col, 0] == null) continue;
            checkType = _GameBoard[col, 0].GetComponent<Block>().Type;
            for (int row = 0; row < _Row; row++)
            {
                if (_GameBoard[col, row] == null) continue;

                if (checkType == _GameBoard[col, row].GetComponent<Block>().Type)
                {
                    _tempMatchList.Add(_GameBoard[col, row]);
                }
                else
                {
                    if (_tempMatchList.Count >= 3)
                    {
                        _matchList.AddRange(_tempMatchList);
                        _tempMatchList.Clear();
                        checkType = _GameBoard[col, row].GetComponent<Block>().Type;
                        _tempMatchList.Add(_GameBoard[col, row]);
                    }
                    else
                    {
                        _tempMatchList.Clear();
                        checkType = _GameBoard[col, row].GetComponent<Block>().Type;
                        _tempMatchList.Add(_GameBoard[col, row]);
                    }
                }
            }

            if (_tempMatchList.Count >= 3)
            {
                _matchList.AddRange(_tempMatchList);
                _tempMatchList.Clear();
            }
            else
            {
                _tempMatchList.Clear();
            }

        }

        // ��Ī �Ǵ� �� ó��

        // �ߺ��� �� ó��
        _matchList = _matchList.Distinct().ToList();

        if (_matchList.Count > 0)
        {
            SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Unlock);
            foreach (var match in _matchList)
            {
                _GameBoard[match.GetComponent<Block>().Column, match.GetComponent<Block>().Row] = null;
                match.gameObject.SetActive(false);
            }

            // ����
            //ScoureValue += _matchList.Count * 10;
            _GameUI.SetScoure(_matchList.Count * 10);


            _RemovingBlock.AddRange(_matchList);
            _RemovedBlock.AddRange(_RemovingBlock);
            _RemovedBlock.Distinct().ToList(); // �ߺ��� �� ó��

            MoveDownBlock();
            return true;

        }
        else
        {
            return false;
        }


    }

    /// <summary>
    /// ��Ī �� �� ���ڸ� ä���
    /// </summary>
    private void MoveDownBlock()
    {
        int movecount = 0;

        for (int row = 0; row < _Row; row++)
        {
            for (int col = _Column - 1; col >= 0; col--)
            {
                if (_GameBoard[col, row] == null)
                {
                    movecount++;
                }
                else
                {
                    if (movecount > 0)
                    {
                        var block = _GameBoard[col, row].GetComponent<Block>();
                        block.MovePos = block.transform.position;
                        block.MovePos = new Vector2(block.MovePos.x, block.MovePos.y - block.Width * movecount); // �̵��� ��ġ ���

                        _GameBoard[col, row] = null;

                        block.Column += movecount;
                        block.gameObject.name = string.Format($"Block[{block.Column}, {block.Row}]");
                        _GameBoard[block.Column, block.Row] = block.gameObject;

                        block.Move(DIRECTION.DOWN, movecount);
                    }
                }
            }
            movecount = 0;
        }


    }

    /// <summary>
    /// ���Ӱ� �� �����
    /// </summary>
    private GameObject GetNewBlock(int col, int row, int type)
    {
        GameObject block = _RemovedBlock[0];
        block.GetComponent<Block>().Init(col, row, type, _Sprites[type]);
        _RemovedBlock.Remove(block);
        return block;
    }

    /// <summary>
    /// ���� ������ ������ ���Ӱ� ä�� �� �����
    /// </summary>
    private void CreateMoveBlcok()
    {
        int movecount = 0;

        for (int row = 0; row < _Row; row++) {
            for (int col = _Column - 1; col >= 0; col--)
            {
                if (_GameBoard[col, row] == null)
                {
                    int type = GetRandomBlockType();
                    _GameBoard[col, row] = GetNewBlock(col, row, type);
                    _GameBoard[col, row].name = $"Block[{col}, {row}]";
                    _GameBoard[col, row].gameObject.SetActive(true);

                    var block = _GameBoard[col, row].GetComponent<Block>();

                    _GameBoard[col, row].transform.position = new Vector3(_screenPos.x + _Xmargin + (row * _blockWidth * _Scale) + (_blockWidth * _Scale) / 2,
                    _screenPos.y - _Ymargin - (col * _blockWidth * _Scale) - (_blockWidth * _Scale) / 2, 0f);


                    block.MovePos = block.transform.position;
                    float moveYPos = _GameBoard[col, row].GetComponent<Block>().MovePos.y + (_blockWidth * _Scale) * movecount++ + _YPOS;

                    _GameBoard[col, row].transform.position = new Vector2(
                        _GameBoard[col, row].GetComponent<Block>().MovePos.x, moveYPos);

                    block.Move(DIRECTION.DOWN, movecount);
                }

            }
            movecount = 0;
        }
    }


    /// <summary>
    /// ���� ��� ���� ��� ������� check
    /// </summary>
    public bool CheckBlockMove()
    {
        foreach (var obj in _GameBoard) {
            if (obj != null)
            {
                if (obj.GetComponent<Block>().State == BLCOKSTATE.MOVE)
                {
                    return true;
                }
            }
        }
        return false;
    }

    int[,] dir = new int[2, 4] { { 1, -1, 0, 0 }, { 0, 0, 1, -1 } };
    int[,] dir2 = new int[2, 4] { { 1, 1, -1, -1 }, { 1, -1, 1, -1 } };
    bool[,] visited = new bool[15, 15];// ������ ũ��� �������
    /// <summary>
    /// ������ ���� �������� ��ġ�� �Ǵ� ���� �ִ��� check
    /// �� �Լ��� ��� ���� �������� ���� �۵�
    /// </summary>
    private bool CheckAfterMoveBlockRec(int r, int c)
    {
        if (visited[c, r]) return false;
        if (r > _Row || r < 0 || c > _Column || c < 0) return false;

        visited[c, r] = true;


        int checkType = _GameBoard[c, r].GetComponent<Block>().Type;

        for (int i = 0; i < 4; i++)
        {
            int row = r + dir[1, i];
            int col = c + dir[0, i];
            if (row < _Row && row >= 0 &&
                col < _Column && col >= 0)
            {
                if (checkType == _GameBoard[col, row].GetComponent<Block>().Type)
                {
                    int rowtemp = row + dir[1, i];
                    int coltemp = col + dir[0, i];
                    for (int j = 0; j < 4; j++)
                    {
                        int row2 = rowtemp + dir[1, j];
                        int col2 = coltemp + dir[0, j];
                        if (row2 < _Row && row2 >= 0 &&
                            col2 < _Column && col2 >= 0 &&
                            !(dir[1, i] == -dir[1, j] && dir[0, i] == -dir[0, j])
                            )
                        {
                            if (checkType == _GameBoard[col2, row2].GetComponent<Block>().Type)
                            {
                                Debug.Log(c + "," + r + " type" + checkType + " st");
                                return true;
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            int row = r + dir2[1, i];
            int col = c + dir2[0, i];
            if (row < _Row && row >= 0 &&
                col < _Column && col >= 0)
            {
                if (checkType == _GameBoard[col, row].GetComponent<Block>().Type)
                {

                    for (int j = 0; j < 4; j++)
                    {
                        int row2 = r + dir2[1, j];
                        int col2 = c + dir2[0, j];
                        if (row2 < _Row && row2 >= 0 &&
                            col2 < _Column && col2 >= 0 &&
                            i != j &&
                            !(dir2[1, i] == -dir2[1, j] && dir2[0, i] == -dir2[0, j])
                            )
                        {
                            if (checkType == _GameBoard[col2, row2].GetComponent<Block>().Type)
                            {
                                Debug.Log(c + "," + r + " type" + checkType + " 11");
                                return true;
                            }
                        }
                    }
                }
            }
        }



        if (r + 1 < _Row)
        {
            if (CheckAfterMoveBlockRec(r + 1, c)) return true;
        }
        if (c + 1 < _Column)
        {
            if (CheckAfterMoveBlockRec(r, c + 1)) return true;
        }
        if (r - 1 >= 0)
        {
            if (CheckAfterMoveBlockRec(r - 1, c)) return true;
        }
        if (c - 1 >= 0)
        {
            if (CheckAfterMoveBlockRec(r, c - 1)) return true;
        }

        return false;
    }
    private void ResetVisited()
    {
        for (int i = 0; i < _Column; i++)
        {
            for (int j = 0; j < _Row; j++)
            {
                visited[i, j] = false;
            }
        }
    }


    public void OneTypeClearItem(int type)
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Button);
        if (!CheckBlockMove())
        {
            List<GameObject> _matchList = new List<GameObject>();
            _RemovingBlock.Clear();
            for (int i = 0; i < _Column; i++)
            {
                for (int j = 0; j < _Row; j++)
                {
                    if (_GameBoard[i, j].GetComponent<Block>().Type == type)
                    {
                        _matchList.Add(_GameBoard[i, j]);
                    }
                }
            }

            if (_matchList.Count > 0)
            {
                foreach (var match in _matchList)
                {
                    _GameBoard[match.GetComponent<Block>().Column, match.GetComponent<Block>().Row] = null;
                    match.gameObject.SetActive(false);
                }

                // ����
                //ScoureValue += _matchList.Count * 10;
                _GameUI.SetScoure(_matchList.Count * 10);


                _RemovingBlock.AddRange(_matchList);
                _RemovedBlock.AddRange(_RemovingBlock);
                _RemovedBlock.Distinct().ToList(); // �ߺ��� �� ó��(�Ƹ� ��������)

                MoveDownBlock();
                PlayState = GamePlayState.AfterDropBlockBlock_MoveCheck;
            }

        }
    }


    public void SuffleItem()
    {
        if (!CheckBlockMove())
        {
            SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Button);
            ShuffleGameBoard();
            PlayState = GamePlayState.DropBlockMatchCheck;
        }
    }



    /// <summary>
    /// ������ ���� �������� ��ġ�� �Ǵ� ���� �ִ��� check
    /// �� �Լ��� ��� ���� �������� ���� �۵�
    /// (old) ������ ������� ����
    /// </summary>
    private bool CheckAfterMoveBlock()
    {
        int checkType = -1;

        for (int row = 0; row < _Row; row++)
        {
            for (int col = _Column-1; col >= (MATCHCOUNT - 1); col--)
            {
                // �¿��� ��� �̵�
                if (row >= 0 && row < _Row - 1)
                {
                    // o x
                    // o x
                    // x o
                    checkType = _GameBoard[col, row + 1].GetComponent<Block>().Type;

                    if (checkType == _GameBoard[col - 1, row].GetComponent<Block>().Type &&
                        checkType == _GameBoard[col - 2, row].GetComponent<Block>().Type)
                    {
                        return true;
                    }

                    // o x
                    // x o
                    // o x
                    checkType = _GameBoard[col - 1, row + 1].GetComponent<Block>().Type;

                    if (checkType == _GameBoard[col - 2, row].GetComponent<Block>().Type &&
                        checkType == _GameBoard[col, row].GetComponent<Block>().Type)
                    {
                        return true;
                    }

                    // x o
                    // o x
                    // o x
                    checkType = _GameBoard[col - 2, row + 1].GetComponent<Block>().Type;

                    if (checkType == _GameBoard[col - 1, row].GetComponent<Block>().Type &&
                        checkType == _GameBoard[col, row].GetComponent<Block>().Type)
                    {
                        return true;
                    }
                }
                // �쿡�� �·� �̵�
                if (row > 0 && row <= _Row - 1)
                {
                    // x o
                    // x o
                    // o x
                    checkType = _GameBoard[col, row - 1].GetComponent<Block>().Type;

                    if (checkType == _GameBoard[col - 1, row].GetComponent<Block>().Type &&
                        checkType == _GameBoard[col - 2, row].GetComponent<Block>().Type)
                    {
                        return true;
                    }

                    // x o
                    // o x
                    // x o
                    checkType = _GameBoard[col-1, row -1].GetComponent<Block>().Type;

                    if (checkType == _GameBoard[col, row].GetComponent<Block>().Type &&
                        checkType == _GameBoard[col - 2, row].GetComponent<Block>().Type)
                    {
                        return true;
                    }

                    // o x
                    // x o
                    // x o
                    checkType = _GameBoard[col-2, row - 1].GetComponent<Block>().Type;

                    if (checkType == _GameBoard[col - 1, row].GetComponent<Block>().Type &&
                        checkType == _GameBoard[col, row].GetComponent<Block>().Type)
                    {
                        return true;
                    }
                }

                // �Ʒ����� ��, ������ �Ʒ�
                if (col >= MATCHCOUNT && col<_Column)
                {
                    // o 
                    // o 
                    // x 
                    // o 
                    checkType = _GameBoard[col, row].GetComponent<Block>().Type;

                    if ((checkType == _GameBoard[col - 2, row].GetComponent<Block>().Type) &&
                        (checkType == _GameBoard[col - 3, row].GetComponent<Block>().Type))
                    {
                        return true;
                    }

                    // o 
                    // x 
                    // o 
                    // o 
                    checkType = _GameBoard[col-3, row].GetComponent<Block>().Type;

                    if (checkType == _GameBoard[col -1, row].GetComponent<Block>().Type &&
                        checkType == _GameBoard[col, row].GetComponent<Block>().Type)
                    {
                        return true;
                    }
                }
            }

        }

        for (int col = 0; col < _Column; col++) {
            for (int row = 0; row < (_Row - MATCHCOUNT); row++)
            {
                // ������ �Ʒ�
                if (col < _Column - 1)
                {
                    
                    // x o o
                    // o x x
                    checkType = _GameBoard[col + 1, row].GetComponent<Block>().Type;

                    if (checkType == _GameBoard[col, row + 1].GetComponent<Block>().Type &&
                        checkType == _GameBoard[col, row + 2].GetComponent<Block>().Type)
                    {
                        return true;
                    }

                    // o x o
                    // x o x
                    checkType = _GameBoard[col + 1, row + 1].GetComponent<Block>().Type;

                    if (checkType == _GameBoard[col, row].GetComponent<Block>().Type &&
                        checkType == _GameBoard[col, row + 2].GetComponent<Block>().Type)
                    {
                        return true;
                    }

                    // o o x
                    // x x o
                    checkType = _GameBoard[col + 1, row + 2].GetComponent<Block>().Type;

                    if (checkType == _GameBoard[col, row].GetComponent<Block>().Type &&
                        checkType == _GameBoard[col, row + 1].GetComponent<Block>().Type)
                    {
                        return true;
                    }

                }
                // �Ʒ����� ����
                if (col > 0)
                {
                    // o x x
                    // x o o
                    checkType = _GameBoard[col -1, row].GetComponent<Block>().Type;

                    if (checkType == _GameBoard[col, row + 2].GetComponent<Block>().Type &&
                        checkType == _GameBoard[col, row + 1].GetComponent<Block>().Type)
                    {
                        return true;
                    }

                    // x o x
                    // o x o
                    checkType = _GameBoard[col - 1, row+1].GetComponent<Block>().Type;

                    if (checkType == _GameBoard[col, row + 2].GetComponent<Block>().Type &&
                        checkType == _GameBoard[col, row].GetComponent<Block>().Type)
                    {
                        return true;
                    }

                    // x x o
                    // o o x
                    checkType = _GameBoard[col - 1, row+2].GetComponent<Block>().Type;

                    if (checkType == _GameBoard[col, row].GetComponent<Block>().Type &&
                        checkType == _GameBoard[col, row + 1].GetComponent<Block>().Type)
                    {
                        return true;
                    }
                }
                // �¿��� ��, �쿡�� ��
                if (row < _Row - MATCHCOUNT)
                {
                    // o x o o
                    checkType = _GameBoard[col, row].GetComponent<Block>().Type;

                    if (checkType == _GameBoard[col, row + 3].GetComponent<Block>().Type &&
                        checkType == _GameBoard[col, row + 2].GetComponent<Block>().Type)
                    {
                        return true;
                    }

                    // o o x o
                    checkType = _GameBoard[col , row + 3].GetComponent<Block>().Type;

                    if (checkType == _GameBoard[col, row].GetComponent<Block>().Type &&
                        checkType == _GameBoard[col, row + 1].GetComponent<Block>().Type)
                    {
                        return true;
                    }
                }
            }
        }
        return false;

    }

    /// <summary>
    /// �� �����ֱ�
    /// </summary>
    private void ShuffleGameBoard()
    {
        List<Block> shuffleBlockList=new List<Block>();

        foreach (var block in _GameBoard)
        {
            shuffleBlockList.Add(block.GetComponent<Block>());
        }

        var rnd= new System.Random();

        var randomize = shuffleBlockList.OrderBy(item => rnd.Next()); // ����Ʈ�� ���� ���´�.

        List<Block> shuffledBlockList = new List<Block>();

        foreach(var obj in randomize)
        {
            shuffledBlockList.Add(obj);
        }

        for(int i = 0; i < _Column; i++)
        {
            for(int j = 0; j < _Row; j++)
            {
                _GameBoard[i, j] = shuffledBlockList[i * _Row + j].gameObject;
            }
        }

        // ���Ӻ��忡 ������ ���� ����ġ�� ��ġ
        for (int col=0; col < _Column; col++)
        {
            for (int row = 0; row < _Row; row++)
            {
                _GameBoard[col, row].transform.position = new Vector3(
                    _screenPos.x + _Xmargin + row * (_blockWidth * _Scale) + (_blockWidth * _Scale) / 2f,
                    _screenPos.y - _Ymargin - col * (_blockWidth * _Scale) - (_blockWidth * _Scale) / 2f, 0f);

                _GameBoard[col, row].name = $"Block[{col}, {row}]";
                _GameBoard[col,row].GetComponent<Block>().Column = col;
                _GameBoard[col,row].GetComponent<Block>().Row = row;
            }
        }
    
    }


    public void LeftMove()
    {
        foreach(var t in _GameBoard)
        {
            t.GetComponent<Block>().Move(DIRECTION.LEFT);
        }
    }
    public void RightMove()
    {
        foreach (var t in _GameBoard)
        {
            t.GetComponent<Block>().Move(DIRECTION.RIGHT);
        }
    }
    public void UpMove()
    {
        foreach (var t in _GameBoard)
        {
            t.GetComponent<Block>().Move(DIRECTION.UP);
        }
    }
    public void DownMove()
    {
        foreach (var t in _GameBoard)
        {
            t.GetComponent<Block>().Move(DIRECTION.DOWN);
        }
    }

    private float CalculateAngle(Vector3 from,Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up,to-from).eulerAngles.z;
    }

    private MouseMoveDirection CalculateDirection()
    {
        float angle= CalculateAngle(_StartPos, _EndPos);

        if(angle > 315f&& angle<=360f|| angle<=45f&& angle>=0f)
        {
            return MouseMoveDirection.UP;
        }
        else if(angle > 45f && angle <= 135f )
        {
            return MouseMoveDirection.LEFT;
        }
        else if(angle > 135f && angle <= 225f )
        {
            return MouseMoveDirection.DOWN;
        }
        else if (angle > 225 && angle <= 315f )
        {
            return MouseMoveDirection.RIGHT;
        }

        return MouseMoveDirection.LEFT;
    }
    

    /// <summary>
    /// ���콺 �̵� 
    /// </summary>
    private void MouseMove()
    {
        float diff=Vector2.Distance(_StartPos,_EndPos);
        
        if (_ClickedObject != null && diff > _mouseDistance && _isInputOk==true)
        {
            _isInputOk = false;
            MouseMoveDirection direction = CalculateDirection();
            _CurrentMoveDirection=direction;
            _ClickedObject.GetComponent<Block>().BlockImage.sortingOrder = 1; // �׻� Ŭ���� ���� ȭ�� ������ ���̵��� ����

            switch (direction)
            {
                case MouseMoveDirection.LEFT:
                    {
                        int column = _ClickedObject.GetComponent<Block>().Column;
                        int row= _ClickedObject.GetComponent<Block>().Row;

                        if (row > 0)
                        {
                            // row�� �ٲٱ�
                            _GameBoard[column, row].GetComponent<Block>().Row = row - 1;
                            _GameBoard[column, row-1].GetComponent<Block>().Row = row;

                            // ������ �ٲٱ�
                            _GameBoard[column, row]=_GameBoard[column, row-1];
                            _GameBoard[column, row - 1] = _ClickedObject;

                            // �� �����̱�
                            _GameBoard[column, row].GetComponent<Block>().Move(DIRECTION.RIGHT);
                            _GameBoard[column, row-1].GetComponent<Block>().Move(DIRECTION.LEFT);
                        }
                        else { _ClickedObject = null; }
                    }
                    break;

                case MouseMoveDirection.RIGHT:
                    {
                        int column = _ClickedObject.GetComponent<Block>().Column;
                        int row = _ClickedObject.GetComponent<Block>().Row;

                        if (row < _Row-1)
                        {
                            // row�� �ٲٱ�
                            _GameBoard[column, row].GetComponent<Block>().Row = row + 1;
                            _GameBoard[column, row + 1].GetComponent<Block>().Row = row;

                            // ������ �ٲٱ�
                            _GameBoard[column, row] = _GameBoard[column, row + 1];
                            _GameBoard[column, row + 1] = _ClickedObject;

                            // �� �����̱�
                            _GameBoard[column, row].GetComponent<Block>().Move(DIRECTION.LEFT);
                            _GameBoard[column, row + 1].GetComponent<Block>().Move(DIRECTION.RIGHT);
                        }
                        else { _ClickedObject = null; }
                    }
                    break;

                case MouseMoveDirection.UP:
                    {
                        int column = _ClickedObject.GetComponent<Block>().Column;
                        int row = _ClickedObject.GetComponent<Block>().Row;

                        if (column > 0)
                        {
                            // column�� �ٲٱ�
                            _GameBoard[column, row].GetComponent<Block>().Column = column - 1;
                            _GameBoard[column - 1, row].GetComponent<Block>().Column = column;

                            // ������ �ٲٱ�
                            _GameBoard[column, row] = _GameBoard[column-1, row];
                            _GameBoard[column - 1, row] = _ClickedObject;

                            // �� �����̱�
                            _GameBoard[column, row].GetComponent<Block>().Move(DIRECTION.DOWN);
                            _GameBoard[column-1, row].GetComponent<Block>().Move(DIRECTION.UP);
                        }
                        else { _ClickedObject = null; }
                    }
                    break;

                case MouseMoveDirection.DOWN:
                    {
                        int column = _ClickedObject.GetComponent<Block>().Column;
                        int row = _ClickedObject.GetComponent<Block>().Row;

                        if (column < _Column-1)
                        {
                            // column�� �ٲٱ�
                            _GameBoard[column, row].GetComponent<Block>().Column = column + 1;
                            _GameBoard[column + 1, row].GetComponent<Block>().Column = column;

                            // ������ �ٲٱ�
                            _GameBoard[column, row] = _GameBoard[column + 1, row];
                            _GameBoard[column + 1, row] = _ClickedObject;

                            // �� �����̱�
                            _GameBoard[column, row].GetComponent<Block>().Move(DIRECTION.UP);
                            _GameBoard[column + 1, row].GetComponent<Block>().Move(DIRECTION.DOWN);
                        }
                        else { _ClickedObject = null; }
                    }
                    break;
            }
            PlayState = GamePlayState.AfterInputMoveCheck;

            _mouseClick = false;
            _isInputOk = true;
        }



    }

    /// <summary>
    /// �������ٰ� ��ġ �Ǵ� ���� ���� ��
    /// </summary>
    private void UndoMove()
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_JellyTouch);
        _ClickedObject.GetComponent<Block>().BlockImage.sortingOrder = 1; // �׻� Ŭ���� ���� ȭ�� ������ ���̵��� ����
        switch (_CurrentMoveDirection)
        {
            case MouseMoveDirection.LEFT:
                {
                    int column = _ClickedObject.GetComponent<Block>().Column;
                    int row = _ClickedObject.GetComponent<Block>().Row;

                    // ���� row���� �ٲ۴�
                    _GameBoard[column, row].GetComponent<Block>().Row = row + 1;
                    _GameBoard[column, row + 1].GetComponent<Block>().Row = row;

                    // ������ �ٲٱ�
                    _GameBoard[column, row] = _GameBoard[column, row + 1];
                    _GameBoard[column, row + 1] = _ClickedObject;

                    // �� �����̱�
                    _GameBoard[column, row].GetComponent<Block>().Move(DIRECTION.LEFT);
                    _GameBoard[column, row + 1].GetComponent<Block>().Move(DIRECTION.RIGHT);

                    PlayState = GamePlayState.AfterInputMoveCheck;
                }
                break;
                
            case MouseMoveDirection.RIGHT:
                {
                    int column = _ClickedObject.GetComponent<Block>().Column;
                    int row = _ClickedObject.GetComponent<Block>().Row;

                    // row�� �ٲٱ�
                    _GameBoard[column, row].GetComponent<Block>().Row = row - 1;
                    _GameBoard[column, row - 1].GetComponent<Block>().Row = row;

                    // ������ �ٲٱ�
                    _GameBoard[column, row] = _GameBoard[column, row - 1];
                    _GameBoard[column, row - 1] = _ClickedObject;

                    // �� �����̱�
                    _GameBoard[column, row].GetComponent<Block>().Move(DIRECTION.RIGHT);
                    _GameBoard[column, row - 1].GetComponent<Block>().Move(DIRECTION.LEFT);

                    PlayState = GamePlayState.AfterInputMoveCheck;
                }
                break;

            case MouseMoveDirection.UP:
                {
                    int column = _ClickedObject.GetComponent<Block>().Column;
                    int row = _ClickedObject.GetComponent<Block>().Row;

                    // column�� �ٲٱ�
                    _GameBoard[column, row].GetComponent<Block>().Column = column + 1;
                    _GameBoard[column + 1, row].GetComponent<Block>().Column = column;

                    // ������ �ٲٱ�
                    _GameBoard[column, row] = _GameBoard[column + 1, row];
                    _GameBoard[column + 1, row] = _ClickedObject;

                    // �� �����̱�
                    _GameBoard[column, row].GetComponent<Block>().Move(DIRECTION.UP);
                    _GameBoard[column + 1, row].GetComponent<Block>().Move(DIRECTION.DOWN);

                    PlayState = GamePlayState.AfterInputMoveCheck;
                }
                break;

            case MouseMoveDirection.DOWN:
                {
                    int column = _ClickedObject.GetComponent<Block>().Column;
                    int row = _ClickedObject.GetComponent<Block>().Row;

                    // column�� �ٲٱ�
                    _GameBoard[column, row].GetComponent<Block>().Column = column - 1;
                    _GameBoard[column - 1, row].GetComponent<Block>().Column = column;

                    // ������ �ٲٱ�
                    _GameBoard[column, row] = _GameBoard[column - 1, row];
                    _GameBoard[column - 1, row] = _ClickedObject;

                    // �� �����̱�
                    _GameBoard[column, row].GetComponent<Block>().Move(DIRECTION.DOWN);
                    _GameBoard[column - 1, row].GetComponent<Block>().Move(DIRECTION.UP);

                    PlayState = GamePlayState.AfterInputMoveCheck;
                }
                break;
        }
    }


    /// <summary>
    /// ����� ��� ���� �ִ��� check
    /// </summary>
    private bool CheckAllBlockInBoard()
    {
        foreach (var block in _GameBoard)
        {
            if (block == null) {
                return false;
            }
        }
        return true;
    }

    void InputProcess()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mouseClick = true;

            _EndPos = _StartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            _StartPos.z = 0f;
            for (int i = 0; i < _Column; i++)
            {
                for (int j = 0; j < _Row; j++)
                {
                    if (_GameBoard[i, j] != null)
                    {
                        //�̹��� �ȿ� _StartPo�� �ִ��� Ȯ��
                        _isOver = _GameBoard[i, j].GetComponent<Block>().BlockImage.bounds.Contains(_StartPos);
                        if (_isOver)
                        {
                            _ClickedObject = _GameBoard[i, j];
                            goto LoopExit;
                        }
                    }
                }
            }
        LoopExit:;
        }


        if (Input.GetMouseButtonUp(0))
        {
            _ClickedObject = null;
            _mouseClick = false;
            _isInputOk = true;
        }

        if ((_ClickedObject != null) && (_mouseClick == true) &&
            ((Input.GetAxis("Mouse X") < 0) || (Input.GetAxis("Mouse X") > 0) ||
            (Input.GetAxis("Mouse Y") < 0) || (Input.GetAxis("Mouse Y") > 0)))
        {

            _EndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _EndPos.z = 0f;

            Debug.Log("MouseMove");
            MouseMove();
        }

    }

    private void Update()
    {
        switch (PlayState)
        {
            case GamePlayState.InputOK:
                InputProcess();
                break;

            case GamePlayState.AfterInputMoveCheck:
                if (!CheckBlockMove())
                    PlayState = GamePlayState.MatchCheck;
                
                break;

            case GamePlayState.MatchCheck:
                if (!CheckBlockMove())
                {

                    if (CheckMatchBlock())
                    {
                        PlayState = GamePlayState.AfterMatchCheck_MoveCheck;

                    }
                    else
                    {
                        if (_ClickedObject != null)
                        {
                            PlayState = GamePlayState.Undo;
                        }
                        else
                        {
                            PlayState = GamePlayState.InputOK;
                        }
                    }
                }
                
                break;

            case GamePlayState.AfterMatchCheck_MoveCheck:
                if (!CheckBlockMove())
                {
                    if (CheckAllBlockInBoard())
                    {
                        ResetVisited();
                        if (CheckAfterMoveBlockRec(0,0))
                            PlayState = GamePlayState.InputOK;
                        else
                        {
                            Debug.Log("�� �̻� ��ġ�Ǵ� ���� �������� ����.");
                            ShuffleGameBoard();
                            PlayState = GamePlayState.DropBlockMatchCheck;
                        }
                    }
                    else
                    {
                        PlayState = GamePlayState.DropBlock;
                    }

                }

                break;

            case GamePlayState.DropBlock:
                MoveDownBlock();
                PlayState = GamePlayState.AfterDropBlockBlock_MoveCheck;
                break;

            case GamePlayState.AfterDropBlockBlock_MoveCheck:
                if (!CheckBlockMove())
                {
                    CreateMoveBlcok();
                    PlayState = GamePlayState.DropBlockMatchCheck;
                }
                break;

            case GamePlayState.DropBlockMatchCheck:
                if (!CheckBlockMove())
                {
                    CheckMatchBlock();
                    PlayState = GamePlayState.AfterMatchCheck_MoveCheck;
                }
                break;

            case GamePlayState.Undo:
                UndoMove();
                PlayState = GamePlayState.AfterMatchCheck_MoveCheck;

                break;
        }

        if(PlayState != GamePlayState.InputCancel)
        {
            if(PlayTime<=0)
            {
                _ResultUI.SetActive(true);
                PlayState= GamePlayState.InputCancel;
            }
        }
    }

}