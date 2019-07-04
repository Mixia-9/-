using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Currency_FSMState {
    protected int stateID;
    public int ID { get { return stateID; } }

    protected Currency_FSMSystem fsm;

    public Currency_FSMState(Currency_FSMSystem fsm, int id) {
        this.fsm = fsm;
        stateID = id;
    }


    public abstract void startAction();
    public abstract void action();
    public abstract void exitAction();
}
