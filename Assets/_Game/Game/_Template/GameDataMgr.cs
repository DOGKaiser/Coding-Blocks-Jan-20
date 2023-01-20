using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDataMgr : BaseGameDataMgr {
	private static readonly GameDataMgr instance = new GameDataMgr();
	static GameDataMgr() { }
	private GameDataMgr() { }
	public static GameDataMgr Instance {
		get { return instance; }
	}
	
	Dictionary<string, SkillAttributeType> _attributeTypes;

	// ------------------------------------------------------

	public override void Init() {
		base.Init();
		MatchSettingGameData.Instance.Init();

		// Attribute Type Configs
		_attributeTypes = new Dictionary<string, SkillAttributeType>();
		SkillAttributeType[] attributeTypes = Resources.LoadAll<SkillAttributeType>("");
		UnityTools.DataLogs<SkillAttributeType>(attributeTypes, "AttributeTypes");
		foreach (SkillAttributeType t in attributeTypes) { _attributeTypes.Add(t.name, t); }
	}

	public List<SkillAttributeType> GetAttributeTypes() {
		return _attributeTypes.Values.ToList<SkillAttributeType>();
	}

	public SkillAttributeType GetAttributeType(string attributeName) {
		return _attributeTypes[attributeName];
	}

	public string GetRandomAttributeTypeKey() {
		return _attributeTypes.Keys.ElementAt(Random.Range(0, _attributeTypes.Keys.Count));
	}
}