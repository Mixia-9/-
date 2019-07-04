using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 绘制阶段
 **/
public class Canvas_DrawState:Currency_FSMState {
    /**   组件 ↓**/
    SpriteRenderer sprite;
    Rigidbody2D r2d;
    EdgeCollider2D colliderLine;
    List<Vector2> mousePos_List;
    Color[] colorBuff;
    Texture2D td;
    Transform map;
    /**   数据 ↓**/
    float spriteScale;                  //画布缩放比例
    int outLine = 2;                    //画笔的粗细
    Vector3? lastPos;                   //记录上一帧画笔的位置
    int canvasWidth = 0;                //记录画布宽度
    bool initOver;                      //是否获得全部组件
    int sample;                         //用于优化画布绘制的数值

    public Canvas_DrawState(Currency_FSMSystem fsm, int id, SpriteRenderer sp, Rigidbody2D r2d, EdgeCollider2D ecol, List<Vector2> m_list, Color[] colorBuff, Texture2D td,Transform map,float spriteScale,int canvasWidth,int sample,int outLine) : base(fsm, id) {
        initOver = false;
        setBaseGetComponent(sp, r2d, ecol, m_list, colorBuff, td,map, spriteScale, canvasWidth, sample, outLine);
    }
    /// <summary>
    /// 得到需要用到的组件引用以及相关数据
    /// </summary>
    public void setBaseGetComponent(SpriteRenderer sp, Rigidbody2D r2d, EdgeCollider2D ecol, List<Vector2> m_list, Color[] colorBuff, Texture2D td,Transform map,float spriteScale,int canvasWidth,int sample,int outLine) {
        this.sprite = sp;
        this.r2d = r2d;
        this.colliderLine = ecol;
        this.mousePos_List = m_list;
        this.colorBuff = colorBuff;
        this.td = td;
        this.map = map;
        this.spriteScale = spriteScale;
        this.canvasWidth = canvasWidth;
        this.sample = sample;
        this.outLine = outLine;
        initOver = true;
    }

    /// <summary>
    /// 开始画东西前初始化准备
    /// </summary>
    public override void startAction() {
        if (!initOver)
            Debug.LogError("描绘需要的道具还没有初始化完成");
        Debug.Log("Canvas_startAction");
        mousePos_List.Clear();
        lastPos = null;
        for (int i = 0; i < colorBuff.Length; i++)
            colorBuff[i] = new Color(0, 0, 0, 0);
        r2d.simulated = false;
        sprite.transform.position = Vector3.zero;
        sprite.transform.rotation = Quaternion.Euler(Vector3.zero);
        r2d.velocity = Vector2.zero;
        sprite.transform.parent = null;
        colliderLine.Reset();
    }

    /// <summary>
    /// 开始画图
    /// </summary>
    public override void action() {
        Debug.Log("Canvas_action");
        Vector3 newPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        if (newPos.x - outLine < 0 || newPos.x + outLine > Screen.width || newPos.y - outLine < 0 || newPos.y + outLine > Screen.height)
            return;//画笔超出边界直接return
        newPos = newPos / (int)Mathf.Pow(2, sample);
        if (lastPos != null) {
            Vector3 a = newPos - (Vector3)lastPos;//上一个鼠标坐标和下一个鼠标坐标只见的向量，指向下一个向量
            float disD = Vector3.SqrMagnitude(a);
            Vector3 zhud = (Vector3)lastPos + a.normalized;//上一个鼠标坐标和下一个鼠标坐标只见过度
            while (Vector3.SqrMagnitude((zhud - (Vector3)lastPos)) < disD) {
                for (int i = -outLine; i < outLine; i++) {//设置某一点的粗细
                    for (int j = -outLine; j < outLine; j++) {
                        try {
                            colorBuff[canvasWidth * ((int)zhud.y + i) + ((int)zhud.x + j)] = Color.black;
                        } catch {
                            Debug.LogError(zhud.x + " " + zhud.y);
                        }
                    }
                }
                zhud = zhud + a.normalized;
            }
            lastPos = newPos;
            mousePos_List.Add(newPos);
        } else {
            for (int i = -outLine; i < outLine; i++) {
                for (int j = -outLine; j < outLine; j++) {
                    colorBuff[canvasWidth * ((int)newPos.y + i) + ((int)newPos.x + j)] = Color.black;
                }
            }
            mousePos_List.Add(newPos);
            lastPos = newPos;
        }
        td.SetPixels(colorBuff);
        td.Apply(true);
        Sprite spriteA = Sprite.Create(td, new Rect(0, 0, td.width, td.height), new Vector2(0.5f, 0.5f));
        sprite.sprite = spriteA;

    }

    /// <summary>
    /// 画图结束
    /// </summary>
    public override void exitAction() {
        Vector2[] aa = new Vector2[mousePos_List.Count];
        float[] aaCount = new float[2] { 0, 0 };
        for (int i = 0; i < mousePos_List.Count; i++) {
            aa[i] = Camera.main.ScreenToWorldPoint(mousePos_List[i]* (int)Mathf.Pow(2, sample)) / spriteScale;
            aaCount[0] += aa[i].x;
            aaCount[1] += aa[i].y;
        }
        Vector2 tt2 = new Vector2(aaCount[0] / aa.Length, aaCount[1] / aa.Length);
        r2d.centerOfMass = tt2;
        colliderLine.points = aa;
        r2d.simulated = true;
        sprite.transform.parent = map;
    }

}
