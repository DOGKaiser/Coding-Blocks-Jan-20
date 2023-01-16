using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedDelegate {

    public delegate void BlankFunction();
    event BlankFunction mEventToCall;
    List<BlankFunction> mFunctionsToRemove = new List<BlankFunction>();

    public void Invoke() {
        List<BlankFunction> tempRemoves = new List<BlankFunction>(mFunctionsToRemove.ToArray());
        mFunctionsToRemove.Clear();

        mEventToCall?.Invoke();
        for (int i = tempRemoves.Count - 1; i >= 0; i--) {
            mEventToCall -= tempRemoves[i];
        }
    }

    public void AddFunction(BlankFunction function, bool removeAfterCall = true) {
        mEventToCall += function;
        if (removeAfterCall) {
            mFunctionsToRemove.Add(function);
        }
    }
}
