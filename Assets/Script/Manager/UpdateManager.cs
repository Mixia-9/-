using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpdateManager:MonoBehaviour {
    public delegate void UpdateManage();
    event UpdateManage playerEventList_LateUpdate;//人物事件需要放入lateUpdate处理的。
    event UpdateManage playerEventList_FixUpdate;//人物事件需要放入FixUpdate处理的。
    event UpdateManage playerEventList_Update;//人物事件需要放入Update处理的。

    static UpdateManager updateManges;
    static UpdateManager getUpdateManges() {
        if (updateManges == null) {
            updateManges = Object.FindObjectOfType<UpdateManager>();
            if (updateManges == null) {
                GameObject managerObject = new GameObject();
                managerObject.name = "UpdateManges";
                UpdateManager.updateManges = managerObject.AddComponent<UpdateManager>();
            }
        }
        return updateManges;
    }


    #region 事件的删除与添加
    public static void add_playerEventList_Late(UpdateManage updatemanage) {
        UpdateManager managers = getUpdateManges();
        if (managers.playerEventList_LateUpdate == null || !managers.playerEventList_LateUpdate.GetInvocationList().Contains(updatemanage))
            managers.playerEventList_LateUpdate += updatemanage;
    }
    public static void sub_playerEventList_Late(UpdateManage updatemanage) {
        UpdateManager managers = getUpdateManges();
        if (managers.playerEventList_LateUpdate != null)
            managers.playerEventList_LateUpdate -= updatemanage;
    }

    public static void add_playerEventList_Fix(UpdateManage updatemanage) {
        UpdateManager managers = getUpdateManges();
        if (managers.playerEventList_FixUpdate == null || !managers.playerEventList_FixUpdate.GetInvocationList().Contains(updatemanage))
            managers.playerEventList_FixUpdate += updatemanage;
    }
    public static void sub_playerEventList_Fix(UpdateManage updatemanage) {
        UpdateManager managers = getUpdateManges();
        if (managers.playerEventList_FixUpdate != null)
            managers.playerEventList_FixUpdate -= updatemanage;
    }

    public static void add_playerEventList_(UpdateManage updatemanage) {
        UpdateManager managers = getUpdateManges();
        if (managers.playerEventList_Update == null || !managers.playerEventList_Update.GetInvocationList().Contains(updatemanage))
            managers.playerEventList_Update += updatemanage;
    }
    public static void sub_playerEventList_(UpdateManage updatemanage) {
        UpdateManager managers = getUpdateManges();
        if (managers.playerEventList_Update != null)
            managers.playerEventList_Update -= updatemanage;
    }

    public static void clearAllEvent() {
        UpdateManager managers = getUpdateManges();
        managers.playerEventList_Update -= managers.playerEventList_Update;
        managers.playerEventList_FixUpdate -= managers.playerEventList_FixUpdate;
        managers.playerEventList_LateUpdate -= managers.playerEventList_LateUpdate;
    }

    #endregion
    private void Update() {
        if (playerEventList_Update != null) {
            playerEventList_Update();
        }
    }


    private void LateUpdate() {
        if (playerEventList_LateUpdate != null) {
            playerEventList_LateUpdate();
        }
    }

    private void FixedUpdate() {
        if (playerEventList_FixUpdate != null) {
            playerEventList_FixUpdate();
        }
    }

}