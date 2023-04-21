using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform _startPoint; //Transfrom to move boss into scene

    [SerializeField]
    private float _moveSpeed; //How Fast the boss Moves

    [SerializeField]
    private Transform[] _shotPoints; // Array of Transforms to fire lasers from

    [SerializeField] // transforms to where the boss can move to
    private Transform _middlePoint, _rightPoint, _leftPoint, _movePoint;

    [SerializeField] //flags for when the boss can be hit and when to wait till its next movement
    private bool _invincible = true, _wait;


    [Header("AI")]

    [SerializeField] // time between next decision
    private float _decisionDuration;

    [SerializeField] // states the boss can be in
    public enum BossState { Intro, Idle, Normal, MoveRight, MoveLeft, MoveMiddle }

    [System.Serializable]
    public class DecisionWeight // class for making decisions
    {
        public int weight; // how much weight this decision has
        public BossState state; // the state of the decision
        public DecisionWeight(int weight, BossState state)
        {
            this.weight = weight;
            this.state = state;
        }
    }
    [field: SerializeField]
    List<DecisionWeight> weights;



    [SerializeField] //setting the enum variable for us
    private BossState _actionState = BossState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        weights = new List<DecisionWeight>();
        if (_startPoint)
        {
            _startPoint.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region AI State Machine
        switch (_actionState)
        {
            case BossState.Idle:
                if (_decisionDuration > 0)
                {
                    _decisionDuration -= Time.deltaTime;
                }
                else
                {
                    DecideWithWeight(0, 100, 0, 0);
                }
                break;

            case BossState.MoveRight:
                MoveRight();
                if (_wait == false)
                {
                    if (_decisionDuration > 0)
                    {
                        _decisionDuration -= Time.deltaTime;
                    }
                    else
                    {
                        DecideWithWeight(0, 0, 70, 40);
                    }
                }
                break;

            case BossState.MoveLeft:
                Debug.Log("Moving Left");
                MoveLeft();
                if (_wait == false)
                {
                    if (_decisionDuration > 0)
                    {
                        _decisionDuration -= Time.deltaTime;
                    }
                    else
                    {
                        DecideWithWeight(0, 50, 10, 10);
                    }
                }
                break;
            case BossState.MoveMiddle:
                Debug.Log("Moving middle");
                MoveMiddle();
                if (_wait == false)
                {
                    if (_decisionDuration > 0)
                    {
                        _decisionDuration -= Time.deltaTime;
                    }
                    else
                    {
                        DecideWithWeight(0, 45, 45, 10);
                    }
                }
                break;

            case BossState.Intro:
                IntroState();
                break;
            default:
                break;
        }
        #endregion



    }

    void IntroState()
    {
        if (_startPoint)
        {
            if (Mathf.Abs(transform.position.y - _startPoint.transform.position.y) > Mathf.Epsilon)
            {
                if (transform.position.y > _startPoint.transform.position.y)
                {
                    transform.Translate(new Vector2(transform.position.x, -_moveSpeed * Time.deltaTime));
                }
                else
                {
                    _decisionDuration = Random.Range(.5f, .6f);
                    _rightPoint.SetParent(null);
                    _leftPoint.SetParent(null);
                    _middlePoint.SetParent(null);
                    _movePoint.SetParent(null);
                    _actionState = BossState.Idle;
                }
            }
        }
    }

    void NormalState()
    {


    }

    void MoveRight()
    {
        if (_wait == false) return;
        if (_rightPoint)
        {
            if (Mathf.Abs(transform.position.x - _rightPoint.transform.position.x) > Mathf.Epsilon)
            {
                if (transform.position.x < _rightPoint.transform.position.x)
                {
                    transform.Translate(new Vector2(_moveSpeed * Time.deltaTime, 0));
                }
                else
                {
                    _wait = false;
                    _decisionDuration = Random.Range(.3f, .5f);
                }
            }


        }
    }

    void MoveLeft()
    {
        if (_wait == false) return;
        if (_leftPoint)
        {
            if (Mathf.Abs(transform.position.x - _leftPoint.transform.position.x) > Mathf.Epsilon)
            {
                if (transform.position.x > _leftPoint.transform.position.x)
                {
                    transform.Translate(new Vector2(-_moveSpeed * Time.deltaTime, 0));
                }
                else
                {
                    _wait = false;
                    _decisionDuration = Random.Range(.2f, .4f);
                }
            }
        }
    }

    void MoveMiddle()
    {
        if (_wait == false) return;
        if (_middlePoint)
        {
            if (Mathf.Abs(transform.position.x - _middlePoint.transform.position.x) > Mathf.Epsilon)
            {
                transform.position = Vector2.MoveTowards(transform.position, _middlePoint.transform.position, _moveSpeed * Time.deltaTime);
               
            }
            else
            {
                _wait = false;
                _decisionDuration = Random.Range(.2f, .3f);
            }
        }
    }

    private void DecideWithWeight(int wait, int moveRight, int moveLeft, int moveMiddle)
    {
        weights.Clear();
        if (wait > 0)
        {
            weights.Add(new DecisionWeight(wait, BossState.Idle));
        }
        if (moveRight > 0)
        {
            weights.Add(new DecisionWeight(moveRight, BossState.MoveRight));
        }
        if (moveLeft > 0)
        {
            weights.Add(new DecisionWeight(moveLeft, BossState.MoveLeft));
        }
        if (moveMiddle > 0)
        {
            weights.Add(new DecisionWeight(moveMiddle, BossState.MoveMiddle));
        }

        int total = wait + moveRight + moveLeft + moveMiddle;
        int intDecision = Random.Range(0, total - 1);
        Debug.Log("Total: " + total);
        Debug.Log("Decision: " + intDecision);

        foreach (DecisionWeight weight in weights)
        {
            intDecision -= weight.weight;
            Debug.Log("Decision-For loop: " + intDecision);
            Debug.Log("weight-for loop: " + weight);


            if (intDecision <= 0)
            {
                Debug.Log("weight-if condition: " + weight);
                SetDecision(weight.state);
                break;
            }
        }
    }

    private void SetDecision(BossState action)
    {
        _actionState = action;
        _wait = true;
        if (action == BossState.Idle)
        {
            NormalState();
        }
        else if (action == BossState.MoveLeft)
        {
            MoveLeft();
        }
        else if (action == BossState.MoveRight)
        {
            MoveRight();
        }
        else if (action == BossState.MoveMiddle)
        {
            MoveMiddle();
        }
    }
}
