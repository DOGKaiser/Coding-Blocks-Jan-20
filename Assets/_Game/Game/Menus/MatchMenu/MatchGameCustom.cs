using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchGameCustom : MatchGame  {
	
	public Fisher fisher;
	
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
