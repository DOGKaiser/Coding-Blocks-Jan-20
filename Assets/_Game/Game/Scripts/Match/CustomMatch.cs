using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMatch : MatchGame {

    // 1. Map - make a static array atm of next MapEventConfig (battle/event/etc)
    // 2. Config can change map state
    // 3. Complete and go back to Map. Repeat 2 until finished

    public delegate void OnCharacterDied(TargetData td, Character character);
    public event OnCharacterDied CharacterDied;
    
    public override void Init(MatchSetting matchSetting) {
        base.Init(matchSetting);
  
    }

    public override void Start(int startSeed) {
        base.Start(startSeed);

        AddState(new MatchStartState(this));
    }

    public void CharacterHasDied(TargetData td, Character character) {
        CharacterDied?.Invoke(td, character);
    }
    
}
