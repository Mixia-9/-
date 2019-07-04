using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DeathState:Currency_FSMState {

    public Player_DeathState(Currency_FSMSystem fsm, int id) : base(fsm, id) {

    }
    public override void startAction() {
        Debug.Log("DeathStartAction");
    }
    public override void action() {
        Debug.Log("deathAction");
    }
    public override void exitAction() {
        Debug.Log("DeathExitAction");
    }
}
