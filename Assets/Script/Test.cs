using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Test:MonoBehaviour {

    // Use this for initialization
    List<Vector2> mousePos_List;
    public int outLine = 2;
    public SpriteRenderer sprite;
    public int Sample;
    Texture2D td;
    public RawImage im;
    Color[] colorBuff;
    EdgeCollider2D colliderLine;
    Rigidbody2D rig;
    Transform map;
    int sw = 0;
    void Start() {
        mousePos_List = new List<Vector2>();
        colorBuff = new Color[Screen.width * Screen.height];
        Debug.Log(Screen.width.ToString() + " " + Screen.height.ToString());
        sw = Screen.width;
        for (int i = 0; i < Screen.width * Screen.height; i++)
            colorBuff[i] = new Color(0, 1, 0, 1);
        td = new Texture2D(Screen.width, Screen.height);
        colliderLine = sprite.GetComponent<EdgeCollider2D>();
        rig = sprite.GetComponent<Rigidbody2D>();
        map = GameObject.Find("Map").transform;
        /*td.SetPixels(colorBuff);
        td.Apply(true);
        Sprite spriteA = Sprite.Create(td, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f));
        sprite.sprite = spriteA;*/
        setCureSize();
    }
    float spriteScale = 0;
    public void setCureSize() {
        spriteScale = (Camera.main.orthographicSize * 1000 / 5) / (float)td.height;

        sprite.transform.localScale = Vector3.one * spriteScale;
    }



    // Update is called once per frame
    bool isbutoon;
    Vector3? lastPos;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            mousePos_List.Clear();
            isbutoon = true;
            for (int i = 0; i < Screen.width * Screen.height; i++)
                colorBuff[i] = new Color(0, 0, 0, 0);
            lastPos = null;
            rig.simulated = false;
            sprite.transform.position = Vector3.zero;
            sprite.transform.rotation = Quaternion.Euler(Vector3.zero);
            rig.velocity = Vector2.zero;
            sprite.transform.parent = null;
            colliderLine.Reset();
        } else if (Input.GetMouseButtonUp(0)) {
            isbutoon = false;
            Vector2[] aa = new Vector2[mousePos_List.Count];
            float[] aaCount = new float[2] { 0, 0 };
            for (int i = 0; i < mousePos_List.Count; i++) {
                aa[i] = Camera.main.ScreenToWorldPoint(mousePos_List[i])/ spriteScale;
                aaCount[0] += aa[i].x;
                aaCount[1] += aa[i].y;
            }
            Vector2 tt2 = new Vector2(aaCount[0] / aa.Length, aaCount[1] / aa.Length);
            rig.centerOfMass = tt2;
            colliderLine.points = aa;
            rig.simulated = true;
            sprite.transform.parent = map;
        }
        if (isbutoon) {
            Vector3 newPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            if (newPos.x - outLine < 0 || newPos.x + outLine > Screen.width || newPos.y - outLine < 0 || newPos.y + outLine > Screen.height)
                return;

            if (lastPos != null) {
                Vector3 a = newPos - (Vector3)lastPos;
                float disD = Vector3.SqrMagnitude(a);
                Vector3 zhud = (Vector3)lastPos + a.normalized;
                while (Vector3.SqrMagnitude((zhud - (Vector3)lastPos)) < disD) {
                    for (int i = -outLine; i < outLine; i++) {
                        for (int j = -outLine; j < outLine; j++) {
                            try {
                                colorBuff[sw * ((int)zhud.y + i) + ((int)zhud.x + j)] = Color.black;
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
                for (int i = -2; i < 2; i++) {
                    for (int j = -2; j < 2; j++) {
                        colorBuff[sw * ((int)newPos.y + i) + ((int)newPos.x + j)] = Color.black;
                    }
                }
                mousePos_List.Add(newPos);
                lastPos = newPos;
            }
            td.SetPixels(colorBuff);
            td.Apply(true);
            Sprite spriteA = Sprite.Create(td, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f));
            sprite.sprite = spriteA;
        }
    }

}
