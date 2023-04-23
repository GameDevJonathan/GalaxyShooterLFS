using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [Header("Starting Variables")]
    [SerializeField]
    private Transform _startPoint; //Transfrom to move boss into scene

    [SerializeField]
    private float _moveSpeed; //How Fast the boss Moves


    [SerializeField] // transforms to where the boss can move to
    private Transform _middlePoint, _rightPoint, _leftPoint, _movePoint;

    [SerializeField] //flags for when the boss can be hit and when to wait till its next movement
    private bool _invincible = true, _wait;

    [Header("Boss Laser")]
    [SerializeField]
    private bool _startFiring;
    [SerializeField]
    private Transform[] _shotPoints; // Array of Transforms to fire lasers from
    
    [SerializeField]
    private float _fireRate;
    
    [SerializeField]
    private Coroutine _laserRoutine;

    [SerializeField]
    private LaserBehaviour _laserPrefab;



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

        if (_startFiring && _laserRoutine == null)
        {
            Debug.Log("BossBehavior::Update Method:Coroutine");
            _laserRoutine = StartCoroutine(LaserHell(_fireRate));
        }
    }

    void IntroState() // intro state to move boss into scene
    {
        if (_startPoint) //check to see if we have a transform 
        {
            //check to see if the absolute value of the boss's y minus the starting point y is greater then a very small number
            if (Mathf.Abs(transform.position.y - _startPoint.transform.position.y) > Mathf.Epsilon)
            {
                //if our y is greater then the starting points y move downward
                if (transform.position.y > _startPoint.transform.position.y)
                {
                    transform.Translate(new Vector2(transform.position.x, -_moveSpeed * Time.deltaTime));
                }
                else
                {
                    _decisionDuration = Random.Range(.5f, .6f); //random decision time
                    _rightPoint.SetParent(null); //set the parent of this transform to null
                    _leftPoint.SetParent(null); // see above
                    _middlePoint.SetParent(null); //see above
                    _movePoint.SetParent(null); // see above
                    _actionState = BossState.Idle; //put state into idle
                    _startFiring = true; //start firing routine
                }
            }
        }
    }   

    void MoveRight() // move the boss right
    {
        if (_wait == false) return; //return if we are not waiting
        if (_rightPoint) // check to see if we have a transform
        {
            // check to see if the absolute value is greater then a small number
            if (Mathf.Abs(transform.position.x - _rightPoint.transform.position.x) > Mathf.Epsilon) 
            {
                //check to see if our x is less then right point x
                if (transform.position.x < _rightPoint.transform.position.x)
                {
                    transform.Translate(new Vector2(_moveSpeed * Time.deltaTime, 0));
                }
                else
                {
                    _wait = false; //set wait to false
                    _decisionDuration = Random.Range(.3f, .5f); //set random wait time
                }
            }
        }
    }

    void MoveLeft() //look at move right
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
        if (_wait == false) return; //return if we are not waiting
        if (_middlePoint) // check to see if we have transform
        {
            //check to see if absolute value is greater then a small number and if so move to this point
            if (Mathf.Abs(transform.position.x - _middlePoint.transform.position.x) > Mathf.Epsilon)
            {
                transform.position = Vector2.MoveTowards(transform.position, _middlePoint.transform.position, _moveSpeed * Time.deltaTime);
               
            }
            else
            {
                _wait = false; // set wait to false since we are not waiting to make a decision
                _decisionDuration = Random.Range(.2f, .3f); //set wait time to random
            }
        }
    }

    //When we call this function pass in integer "weights" for each decision. 
    private void DecideWithWeight(int wait, int moveRight, int moveLeft, int moveMiddle)
    {
        //clear current list
        weights.Clear();
        if (wait > 0) // if the wait weight is greater then zero, add this weight to the list with weighted value
        {
            weights.Add(new DecisionWeight(wait, BossState.Idle));
        }
        if (moveRight > 0) //see above
        {
            weights.Add(new DecisionWeight(moveRight, BossState.MoveRight));
        }
        if (moveLeft > 0)// " "
        {
            weights.Add(new DecisionWeight(moveLeft, BossState.MoveLeft));
        }
        if (moveMiddle > 0)// " "
        {
            weights.Add(new DecisionWeight(moveMiddle, BossState.MoveMiddle));
        }

        int total = wait + moveRight + moveLeft + moveMiddle; // taly the total weights
        int intDecision = Random.Range(0, total - 1); // set a random number between 0 and the total weight -1        

        foreach (DecisionWeight weight in weights) //loop through possible decisions in list
        {
            intDecision -= weight.weight; // decrease value based on currents weights weight.

            if (intDecision <= 0) //if value is less then zero set the decison with the current weight
            {
                SetDecision(weight.state);
                break; // exit loop.. if we don't break out of loop we will continue to run until end and have bugs
            }
        }
    }

    private void SetDecision(BossState action)
    {
        _actionState = action;
        _wait = true;
        #region unneccasry code
        //if (action == BossState.Idle)
        //{
        //    NormalState();
        //}
        //else if (action == BossState.MoveLeft)
        //{
        //    MoveLeft();
        //}
        //else if (action == BossState.MoveRight)
        //{
        //    MoveRight();
        //}
        //else if (action == BossState.MoveMiddle)
        //{
        //    MoveMiddle();
        //}
        #endregion
    }

    IEnumerator LaserHell(float fireRate)
    {
        if (_shotPoints != null)
        {
            foreach(Transform shotpoint in _shotPoints)
            {
                LaserBehaviour laser;
                laser = Instantiate(_laserPrefab, shotpoint.position, Quaternion.identity);
                laser._human = false;
                laser._up = false;
            }
        }
        yield return new WaitForSeconds(fireRate);
        _laserRoutine = null;
    }
}
