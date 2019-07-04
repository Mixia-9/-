using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Currency_FSMSystem {

    // Use this for initialization
    private Dictionary<int, Currency_FSMState> states = new Dictionary<int, Currency_FSMState>();
    private int currentStateID;
    private Currency_FSMState currentState;

    public void Update() {
        if (currentState != null)
            currentState.action();
    }
    public void addState(Currency_FSMState playerState) {
        if (playerState == null) {
            Debug.LogError("FSMState不能为空"); return;
        }
        if (states.ContainsKey(playerState.ID)) {
            Debug.LogError("状态" + playerState.ID + "已经存在，无法重复添加"); return;
        }
        states.Add(playerState.ID, playerState);
    }
    public void deleteState(int id) {
        if (id == 0) {
            Debug.LogError("无法删除空状态"); return;
        }
        if (states.ContainsKey(id) == false) {
            Debug.LogError("无法删除不存在的状态：" + id); return;
        }
        states.Remove(id);
    }

    public void performTransition(int id) {

        if (states.ContainsKey(id) == false) {
            Debug.LogError("在状态机里面不存在状态" + id + "，无法进行状态转换！"); return;
        }
        if (id == currentStateID) {
            Debug.Log("当前就是" + id.ToString() + "状态"); return;
        }

        Currency_FSMState state = states[id];
        if (currentState != null)
            currentState.exitAction();
        currentState = state;
        currentStateID = id;
        currentState.startAction();
    }
}
