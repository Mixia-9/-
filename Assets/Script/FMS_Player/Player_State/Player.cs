using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateID {
    NullStateID = 0,
    Run,
    Desth,
}

public class Player : MonoBehaviour {
    private Currency_FSMSystem fsm;
    public void init() {
        
    }

    private void Start() {
        initFsm();
    }
    private void initFsm() {
        fsm = new Currency_FSMSystem();
        Currency_FSMState runState = new Player_RunState(fsm, (int)StateID.Run);

        Currency_FSMState desthState = new Player_DeathState(fsm, (int)StateID.Desth);

        fsm.addState(runState);
        fsm.addState(desthState);
        fsm.performTransition((int)StateID.Run);
        UpdateManager.add_playerEventList_(updateFsmSystem);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            fsm.performTransition((int)StateID.Run);
        } else if (Input.GetKeyDown(KeyCode.S)) {
            fsm.performTransition((int)StateID.Desth);
        }
    }

    private void updateFsmSystem() {
        fsm.Update();
    }

}
