using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class LD_Parallax : MonoBehaviour {

    private Vector2 レン;
    private Vector3 スタート, ビLウ;
    private Camera シ;
    private SpriteRenderer 写真;

    //スタティックのデータを設定します
    void Start() {
        スタート = transform.position;
        シ = GameObject.Find("Main Camera").GetComponent<Camera>();
        GameObject 左 = GameObject.Find("hidari");
        GameObject 右 = GameObject.Find("migi");
        GameObject ue = GameObject.Find("ue");
        GameObject shita = GameObject.Find("shita");
        写真 = GetComponent<SpriteRenderer>();
        レン = new Vector2(右.transform.position.x - 左.transform.position.x,
            ue.transform.position.y - shita.transform.position.y);
        ビLウ = レン - 写真.sprite.rect.size;
    }

    //写真はプレーヤーに移動します
    void Update() {
        Vector3 カム = シ.gameObject.transform.position;
        Vector2 シLウ = レン - シ.pixelRect.size, レ = new Vector2 (シLウ.x/ ビLウ.x, シLウ.y / ビLウ.y);
        Debug.Log("L: "+レン+ "\tR: "+レ);
        transform.position = new Vector3(レ.x * カム.x, レ.y * カム.y, 0)+ スタート;
    }
}