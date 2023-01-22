using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PlasticGui.WorkspaceWindow.CodeReview.Summary;
using UnityEngine;

public class Fisher : MonoBehaviour {
    public static Fisher Instance;

    private string _fisherState;

    [SerializeField] Animator _animator;
    [SerializeField] Lure lure;
    [SerializeField] Transform endOfRod;

    
    public float gripSlackIncrease = 12f;
    public float gripStrainDecrease = 6f;
    public float gripNormalDecrease = 1f;
    
    public float pullStrainStrength = 10f;
    public float pullNormalStrength = 4f;

    public bool debugReset = false;
    public bool debugTest = false;
    public Vector3 debugVector = Vector3.zero;
    public float debugTime = 0.5f;
    public Ease debugEase = Ease.InSine;

    bool _hooked;
    PlayerController _playerController;
    float _currentGrip;

    // Start is called before the first frame update
    void Start() {
        Instance = this;
        _fisherState = FisherStates.FISHER_IDLE;
        
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(2.5f);
        sequence.AppendCallback(CastLine);
    }

    float _timer;

    void DebugCall() {
        lure.transform.DOLocalMove(debugVector, debugTime).SetEase(debugEase);
    }
    
    void CastLine() {
        ChangeState(FisherStates.FISHER_CAST);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(lure.transform.DOLocalMove(new Vector3(3, 1, 0), 0.3f).SetEase(Ease.Linear));
        sequence.AppendInterval(0.25f);
        sequence.AppendCallback(delegate {
            lure.transform.SetParent(null, true); 
            Sequence sequence2 = DOTween.Sequence();
            sequence2.AppendInterval(0.25f);
            sequence2.AppendCallback(delegate { ChangeState(FisherStates.FISHER_LINE_OUT); });
            sequence.Append(lure.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 2.75f));
        });
        sequence.Append(lure.transform.DOLocalMove(new Vector3(9f, 12.5f, 0), 0.75f).SetEase(Ease.Linear));
        sequence.Append(lure.transform.DOLocalMove(new Vector3(-8f, 0f, 0), 2.5f));
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (debugTest) {
            debugTest = false;
            CastLine();
        }

        if (debugReset) {
            lure.transform.SetParent(endOfRod, false);
            debugReset = false;
            
            lure.transform.localPosition = new Vector3(0, -0.9f, 0);
            ChangeState(FisherStates.FISHER_IDLE);
        }
        
        if (_hooked) {
            CheckGrip(_playerController.PullingAway());
            
            _timer += Time.deltaTime;
            if (_timer >= 10) {
                SwapState();
                _timer = 0;
            }
        }

    }

    public void SetHooked(PlayerController playerController, bool hooked) {
        _playerController = playerController;
        _hooked = hooked;
        _currentGrip = 100;
        if (_hooked) {
            ChangeState(FisherStates.FISHER_HOOK_CATCH);
        } 
        else {
            // TODO: Reset
            ChangeState(FisherStates.FISHER_NORMAL);
        }
    }
    
    void SwapState() {
        switch (_fisherState) {
            case FisherStates.FISHER_NORMAL:
                ChangeState(FisherStates.FISHER_STRAIN);
                break;
            case FisherStates.FISHER_STRAIN:
                ChangeState(FisherStates.FISHER_SLACK);
                break;
            case FisherStates.FISHER_SLACK:
                ChangeState(FisherStates.FISHER_NORMAL);
                break;
        }
    }

    public void ChangeState(string state) {
        _fisherState = state;
        _animator.SetTrigger(state);
    }

    void CheckGrip(bool pullingAway) {
        if (pullingAway) {
            switch (_fisherState) {
                case FisherStates.FISHER_IDLE:
                    break;
                case FisherStates.FISHER_NORMAL:
                    _currentGrip -= Time.deltaTime * gripNormalDecrease;
                    break;
                case FisherStates.FISHER_STRAIN:
                    _currentGrip -= Time.deltaTime * gripStrainDecrease;
                    break;
                case FisherStates.FISHER_SLACK:
                    _currentGrip += Time.deltaTime * gripSlackIncrease;
                    break;
            }
        }
        
        _currentGrip = Mathf.Min(_currentGrip, 100);
        _currentGrip = Mathf.Max(_currentGrip, 0);
        ((MatchMenuCustom)MatchMenuCustom.Instance).SetGrip(_currentGrip);
    }
    
    public Vector2 GetPullStrength() {
        Vector2 pullStrength = Vector2.zero;
        switch (_fisherState) {
            case FisherStates.FISHER_NORMAL:
                pullStrength = ((Vector2)(transform.position - _playerController.transform.position).normalized) * pullNormalStrength;
                break;
            case FisherStates.FISHER_STRAIN:
                pullStrength = ((Vector2)(transform.position - _playerController.transform.position).normalized) * pullStrainStrength;
                break;
        }

        return pullStrength;
        
        _currentGrip = Mathf.Min(_currentGrip, 100);
        _currentGrip = Mathf.Max(_currentGrip, 0);
        ((MatchMenuCustom)MatchMenuCustom.Instance).SetGrip(_currentGrip);
    }
    
    public float GetGrip() {
        return _currentGrip;
    }
    
    public bool GetHooked() {
        return _hooked;
    }
    
        
    // Fisher States
    public class FisherStates {
        public const string FISHER_IDLE = "Fisher_Idle";
        public const string FISHER_NORMAL = "Fisher_Normal";
        public const string FISHER_STRAIN = "Fisher_Strain";
        public const string FISHER_SLACK = "Fisher_Slack";
        public const string FISHER_CAST = "Fisher_Cast";
        public const string FISHER_LINE_OUT = "Fisher_Line_Out";
        public const string FISHER_HOOK_CATCH = "Fisher_Hook_Catch";
        public const string FISHER_HOOK_GET_FISH = "Fisher_Hook_Get_Fish";
        public const string FISHER_HOOK_LOSE_ROD = "Fisher_Hook_Lose_Rod";
        public const string FISHER_HOOK_GET_FISH_IDLE = "Fisher_Hook_Get_Fish_Idle";
        public const string FISHER_HOOK_LOSE_ROD_IDLE = "Fisher_Hook_Lose_Rod_Idle";
        
    }
}
