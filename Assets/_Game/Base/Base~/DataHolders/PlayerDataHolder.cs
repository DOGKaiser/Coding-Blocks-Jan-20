using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDataHolder {

    public bool isPublic;
    public bool isReadyOnly;


    public virtual void InitAIData(List<DataHolderClass> currentPlayers, DataHolderClass pdata) {
        InitData(pdata);
    }

    public abstract void InitData(DataHolderClass pdata);
    public abstract void LoadFromBuffer(DataHolderClass pdata, BufferReadWrite buffer, string type);
    public abstract void SaveToBuffer(DataHolderClass pdata, BufferReadWrite buffer, string type);
    public abstract string GetHolderString();
}
