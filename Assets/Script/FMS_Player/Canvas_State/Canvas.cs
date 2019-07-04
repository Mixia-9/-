using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas:MonoBehaviour {

    // Use this for initialization
    public enum StateID {
        NullStateID = 0,
        Draw,
        instantiation,
    }
    Currency_FSMSystem fsm;
    /**   组件 ↓**/
    SpriteRenderer sprite;                  //用于显示画布
    Rigidbody2D r2d;                        //重力相关
    EdgeCollider2D colliderLine;            //画完之后需要赋值的碰撞边界
    Texture2D td;                           //相当于画板
    Transform map;                          //地图
    /**   数据 ↓**/
    List<Vector2> mousePos_List;            //用于存放画线点坐标
    Color[] colorBuff;                      //存放画布颜色
    float spriteScale;                      //设置画布大小
    int canvasWidth;                        //画布宽度
    [Range(0,2)]
    public int sample = 0;                  //优化画布，降低分辨率
    [Range(1, 4)]
    public int outLine = 2;                 //画笔粗细
    void Start() {
        init();
    }

    /// <summary>
    /// 各项初始化
    /// </summary>
    private void init() {

        /**   组件初始化内容 ↓**/
        map = GameObject.Find("Map").transform;
        sprite = GameObject.Find("myCanvas").GetComponent<SpriteRenderer>();
        colliderLine = sprite.GetComponent<EdgeCollider2D>();
        r2d = sprite.GetComponent<Rigidbody2D>();
        /**   数据初始化内容 ↓**/
        int height = Screen.height>> sample;
        int width = Screen.width>> sample;
        Debug.Log(height + " " + width);
        mousePos_List = new List<Vector2>();
        colorBuff = new Color[width * height];
        td = new Texture2D(width, height);
        spriteScale = (Camera.main.orthographicSize * 1000 / 5) / (float)td.height;
        sprite.transform.localScale = Vector3.one * spriteScale;
        canvasWidth = width;



        /*for (int i = 0; i<height* width; i++)
            colorBuff[i] = new Color(0, 1, 0, 1);
        td.SetPixels(colorBuff);
        td.Apply(true);
        Sprite spriteA = Sprite.Create(td, new Rect(0, 0,td.width , td.height), new Vector2(0.5f, 0.5f));
        sprite.sprite = spriteA;*/

        /** 状态机初始化内容 ↓**/
        fsm = new Currency_FSMSystem();
        Currency_FSMState canvasState = new Canvas_DrawState(fsm, (int)StateID.Draw, sprite, r2d, colliderLine, mousePos_List, colorBuff, td, map, spriteScale, canvasWidth, sample,outLine);
        Currency_FSMState instantiationState = new Canvas_InstantiationState(fsm, (int)StateID.instantiation);
        fsm.addState(canvasState);
        fsm.addState(instantiationState);
        /**       启用       ↓**/
        UpdateManager.add_playerEventList_(updateFsmSystem);
    }

    private void updateFsmSystem() {
        if (Input.GetMouseButtonDown(0)) {
            fsm.performTransition((int)StateID.Draw);
        } else if (Input.GetMouseButtonUp(0)) {
            fsm.performTransition((int)StateID.instantiation);
        }
        fsm.Update();
    }
}
