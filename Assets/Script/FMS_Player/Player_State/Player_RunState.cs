using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_RunState:Currency_FSMState {


    public Player_RunState(Currency_FSMSystem fsm, int id) : base(fsm,id) {

    }
    public override void startAction() {
        Debug.Log("RunStartAction");
    }

    public override void action() {
        Debug.Log("Runaction");
    }
    public override void exitAction() {
        Debug.Log("RunExitAction");
    }
}
