using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fisher : MonoBehaviour {
    public static Fisher Instance;

    private string _fisherState;

    [SerializeField] Animator _animator;
    
    public float gripSlackIncrease = 12f;
    public float gripStrainDecrease = 6f;
    public float gripNormalDecrease = 1f;
    
    public float pullStrainStrength = 10f;
    public float pullNormalStrength = 4f;

    bool _hooked;
    PlayerController _playerController;
    float _currentGrip;

    // Start is called before the first frame update
    void Start() {
        Instance = this;
        _fisherState = FisherStates.FISHER_IDLE;
    }

    float _timer;

    // Update is called once per frame
    void FixedUpdate() {
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
            _fisherState = FisherStates.FISHER_NORMAL;
        }
    }
    
    void SwapState() {
        switch (_fisherState) {
            case FisherStates.FISHER_NORMAL:
                _fisherState = FisherStates.FISHER_STRAIN;
                break;
            case FisherStates.FISHER_STRAIN:
                _fisherState = FisherStates.FISHER_SLACK;
                break;
            case FisherStates.FISHER_SLACK:
                _fisherState = FisherStates.FISHER_NORMAL;
                break;
        }
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
    }
}
