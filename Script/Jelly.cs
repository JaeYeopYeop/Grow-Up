using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Jelly : MonoBehaviour
{
    private const int MaxJellyExperienceOne = 50;
    private const int MaxJellyExperienceTwo = 200;
    private const int MaxJellyExperienceThree = 1000;
    [SerializeField] private Sprite[] _JellyImage;
    [SerializeField] private RuntimeAnimatorController[] _JellyAcs;
    [SerializeField] private GameObject _ExTextPrefab;
    private GamePlayManager _GamePlayManager;
    public enum JellyState
    {
        Idle,
        Move,
        Turn,
        Touch,
        Clicked
    }

    private int _JellyType=0;
    public int JellyType
    {
        get => _JellyType;
        set => _JellyType = value;
    }
    private int _JellyLevel;
    public int JellyLevel
    {
        get => _JellyLevel;
        set => _JellyLevel = value;
    }

    private long _JellyExperience;
    public long JellyExperience
    {
        get => _JellyExperience;
        set => _JellyExperience = value;
    }

    [HideInInspector]
    public int[] MaxJellyExperience = new int[3] {
    MaxJellyExperienceOne, MaxJellyExperienceTwo, MaxJellyExperienceThree,
    };

    private const float maxSpeed = 1f;
    public float MaxSpeed { get => maxSpeed; }
    private float _Speed;
    public float Speed { get => _Speed; set => _Speed = value; }

    private float stateTime = 3f;

    private Vector2 _angle = Vector2.zero;
    public Vector2 Angle{ get => _angle; set => _angle = value;}

    public StateMachine<Jelly> _StateMachine;
    public JellyState state = JellyState.Idle;
    public IState<Jelly> idle;
    public IState<Jelly> move;
    public IState<Jelly> turn;
    public IState<Jelly> touch;
    public IState<Jelly> clicked;
    private bool isCoroutineRunning = false;
    private bool isExCoroutineRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        idle = new JellyIdle();
        move = new JellyMove();
        turn = new JellyTurn();
        touch = new JellyTouch();
        clicked = new JellyClicked();
        _StateMachine = new StateMachine<Jelly>(this, idle);

        this.GetComponent<SpriteRenderer>().sprite = _JellyImage[JellyType];
        this.GetComponent<Animator>().runtimeAnimatorController = _JellyAcs[JellyLevel-1];


        // 필요 경험치 정해주기
        UpdateMaxJellyExperience();

        _GamePlayManager = GameObject.Find("GamePlay").GetComponent<GamePlayManager>();
        _GamePlayManager._Jellys.Add(this);

    }

    public void SetReference(GamePlayManager manager)
    {
        _GamePlayManager = manager;
    }

    public void UpdateJellyStats()
    {
        this.GetComponent<SpriteRenderer>().sprite = _JellyImage[JellyType];
        this.GetComponent<Animator>().runtimeAnimatorController = _JellyAcs[JellyLevel - 1];
        UpdateMaxJellyExperience();
    }

    private void UpdateMaxJellyExperience()
    {
        InitMaxJellyExperience();

        for (int i= 0; i <MaxJellyExperience.Count();i++)
        {
            MaxJellyExperience[i] *= JellyLevel*(JellyType+1);
        }
    }

    private void InitMaxJellyExperience()
    {
        MaxJellyExperience =new int[3]{ MaxJellyExperienceOne,MaxJellyExperienceTwo,MaxJellyExperienceThree};
    }

    private void OnMouseDown()
    {
        state = JellyState.Touch;
        _StateMachine.SetState(touch);
        //ChangeState();
    }
    private void ChangeState()
    {
        if (state == JellyState.Idle)
        {
            stateTime = Random.Range(1f,2.5f);
            bool moveOrTurn = Random.Range(1, 2) == 1 ? true : false;
            if (moveOrTurn)
            {
                _StateMachine.SetState(move);
                state= JellyState.Move;
            }
            else
            {
                _StateMachine.SetState(turn);
                state = JellyState.Turn;
            }
           
        }
        else if (state == JellyState.Move)
        {
            stateTime = Random.Range(2f, 3f);
            _StateMachine.SetState(idle);
            state= JellyState.Idle;
        }
        else if (state == JellyState.Turn)
        {
            stateTime = Random.Range(0.5f, 1f);
            _StateMachine.SetState(idle);
            state = JellyState.Idle;
        }
        else if(state == JellyState.Touch)
        {
            stateTime = Random.Range(2f, 3f);
            _StateMachine.SetState(idle);
            state = JellyState.Idle;
        }
    }

    IEnumerator StateCoroutine(float time)
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(time);
        ChangeState();
        isCoroutineRunning = false;
    }

    public void SetExperienceText(int plusValue,bool isClicked)
    {
        
        GameObject obj = Instantiate(_ExTextPrefab);
        
        Vector3 targetPosition= this.transform.position;
        obj.transform.position = new Vector3(targetPosition.x+0.3f,targetPosition.y+0.3f, -0.5f);
        obj.transform.SetParent(GameObject.Find("Canvas").gameObject.transform);    
        obj.GetComponent<ExperienceText>().Value = plusValue;
        obj.GetComponent<ExperienceText>().isClicked = isClicked;
        obj.name = $"Text({plusValue})";
        
    }


    public void SetGoodsText(int plusValue)
    {

        GameObject obj = Instantiate(_ExTextPrefab);

        Vector3 targetPosition = this.transform.position;
        obj.transform.position = new Vector3(targetPosition.x-1.5f, targetPosition.y, -0.5f);
        obj.transform.SetParent(GameObject.Find("Canvas").gameObject.transform);
        obj.GetComponent<ExperienceText>().Value = plusValue;
        obj.GetComponent<ExperienceText>()._textNum = 1;
        obj.GetComponent<ExperienceText>().isGoods = true;
        obj.name = $"Text({plusValue})";

    }

    private void UpdateExperience()
    {
        JellyExperience += 5;
        SetExperienceText(5,false);


        if (JellyExperience > MaxJellyExperience[JellyLevel-1])
            JellyLevelUp();
        Debug.Log($"JellyExperience Update... Ex : {JellyExperience}  Level : {JellyLevel}");
    }

    public void JellyLevelUp()
    {
        SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Grow);
        if (JellyLevel < 3)
        {
            this.GetComponent<Animator>().runtimeAnimatorController = _JellyAcs[(++JellyLevel)-1];
        }
        else
        {
            JellyLevel = 1;
            JellyType++;
            JellyExperience = JellyExperience - MaxJellyExperience[2];
            UpdateJellyStats();
        }
    }

    IEnumerator ExperienceCorutine(float time) {

        isExCoroutineRunning=true;
        yield return new WaitForSeconds(time);
        UpdateExperience();
        isExCoroutineRunning =false;
    }


    // Update is called once per frame
    void Update()
    {
        if (!_GamePlayManager._isOptionUIOnOff)
        {
            if (state != JellyState.Clicked)
            {
                if (!isCoroutineRunning)
                    StartCoroutine(StateCoroutine(stateTime));
                _StateMachine.DoOperateUpdate();

                if (!isExCoroutineRunning)
                    StartCoroutine(ExperienceCorutine(5f));
            }
            else
            {
                _StateMachine.DoOperateUpdate();
            }
        }
    }
}
