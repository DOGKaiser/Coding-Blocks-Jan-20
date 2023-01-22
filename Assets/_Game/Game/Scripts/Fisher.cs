using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Fisher : MonoBehaviour {
    public static Fisher Instance;

    private string _fisherState;

    [SerializeField] Animator _animator;
    [SerializeField] Lure lure;
    [SerializeField] Transform endOfRod;
    [SerializeField] GameObject rod;
    [SerializeField] AudioSource audioSource;
    
    public float gripSlackIncrease = 12f;
    public float gripStrainDecrease = 6f;
    public float gripNormalDecrease = 1f;
    
    public float pullStrainStrength = 10f;
    public float pullNormalStrength = 4f;
    public float startGrip = 100;
    
    public bool debugReset = false;
    public bool debugTest = false;
    public Vector3 debugVector = Vector3.zero;
    public float debugTime = 0.5f;
    public Ease debugEase = Ease.InSine;

    bool _hooked;
    bool _lostRod;
    PlayerController _playerController;
    float _currentGrip;
    
    public List<AudioClip> clips;

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
            lure.transform.SetParent(MatchMenuCustom.Instance.MatchArea.transform, true); 
            Sequence sequence2 = DOTween.Sequence();
            sequence2.AppendInterval(0.25f);
            sequence2.AppendCallback(delegate { ChangeState(FisherStates.FISHER_LINE_OUT); });
            sequence2.Append(lure.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 2.75f));
        });
        sequence.Append(lure.transform.DOLocalMove(new Vector3(9f, 12.5f, 0), 0.75f).SetEase(Ease.Linear));
        sequence.Append(lure.transform.DOLocalMove(new Vector3(-5f, 2.1f, 0), 1.5f).SetEase(Ease.Linear));
        sequence.AppendCallback(delegate {
            PlaySound(clips[8]);
            lure.transform.DOLocalMove(new Vector3(-8f, 0f, 0), 1f).SetEase(Ease.Linear);
        });
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
            if (_timer >= 5) {
                SwapState();
                _timer = Random.Range(-1.5f, 2);
            }
        }

    }

    public void SetHooked(PlayerController playerController, bool hooked) {
        _playerController = playerController;
        _hooked = hooked;
        lure.transform.DOKill();
        if (_hooked) {
            _currentGrip = startGrip;
            ChangeState(FisherStates.FISHER_HOOK_CATCH);
            AudioManager.Instance.PlayMusic(((MatchMenuCustom)MatchMenu.Instance).MayhemMusic, 0.1f, 0.5f);
        } 
        else {
            // TODO: Reset
            ChangeState(FisherStates.FISHER_HOOK_LOSE_ROD);
            AudioManager.Instance.PlayMusic(((MatchMenuCustom)MatchMenu.Instance).MenuMusic, 0.1f, 0.5f);
        }
    }

    public void RodInWater() {
        GameObject newRod = ObjectPoolMgr.Instance.GetObject(rod, rod.transform.position, rod.transform.rotation, MatchMenu.Instance.MatchArea.transform);
        newRod.tag = "Blocker";
        Animator animator = newRod.GetComponent<Animator>();
        animator.enabled = true;
        animator.SetTrigger("Sink");
        newRod.GetComponent<PolygonCollider2D>().enabled = true;
    }
    
    void SwapState() {
        int coolStory = Random.Range(0, 2);
        switch (_fisherState) {
            case FisherStates.FISHER_NORMAL:
                PlaySound(clips[2]);
                PlaySoundLoop("Reeling", clips[5]);
                if (coolStory == 0) {
                    ChangeState(FisherStates.FISHER_STRAIN);
                } 
                else {
                    ChangeState(FisherStates.FISHER_SLACK);
                }
                break;
            case FisherStates.FISHER_STRAIN:
                PlaySound(clips[0]);
                PlaySoundLoop("Reeling", clips[3]);
                if (coolStory == 0) {
                    ChangeState(FisherStates.FISHER_NORMAL);
                } 
                else {
                    ChangeState(FisherStates.FISHER_SLACK);
                }
                break;
            case FisherStates.FISHER_SLACK:
                PlaySound(clips[1]);
                PlaySoundLoop("Reeling", clips[4]);
                if (coolStory == 0) {
                    ChangeState(FisherStates.FISHER_STRAIN);
                } 
                else {
                    ChangeState(FisherStates.FISHER_NORMAL);
                }
                break;
            default:
                AudioManager.Instance.StopLoopingClip("Reeling");
                break;
        }
    }

    public void ChangeState(string state) {
        _fisherState = state;
        _animator.SetTrigger(state);
    }
    
    public void PlaySound(AudioClip clip) {
        AudioManager.Instance.PlayClip(clip);
    }
    
    public void PlaySoundLoop(string audioId, AudioClip clip) {
        AudioManager.Instance.PlayLoopingClip(audioId, clip, 1);
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

        if (_currentGrip == 0) {
            LostGrip();
        }
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
    }
    
    public float GetGrip() {
        return _currentGrip;
    }
    
    public bool GetHooked() {
        return _hooked;
    }

    public void GotFish() {
        AudioManager.Instance.StopLoopingClip("Reeling");
        ChangeState(Fisher.FisherStates.FISHER_HOOK_GET_FISH);
        PlaySound(clips[13]);
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(5);
        sequence.AppendCallback(YouLost);
    }
    
    public void LostGrip() {
        AudioManager.Instance.StopLoopingClip("Reeling");
        SetHooked(null, false);
        PlaySound(clips[14]);
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(5);
        sequence.AppendCallback(YouWon);
    }

    private void YouLost() {
        LoseMenu.TransitionIn();
    }
    
    private void YouWon() {
        WinMenu.TransitionIn();
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
