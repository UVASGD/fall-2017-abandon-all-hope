using UnityEngine;
using System.Collections;

public class LD_Parallax : MonoBehaviour {
    public float スピード = 1;

    private Vector3 スタート;
    private float レン;

    void Start() {
        スタート = transform.position;
        GameObject 左 = GameObject.Find("hidari");
        GameObject 右 = GameObject.Find("migi");
        レン = 右.transform.position.x - 左.transform.position.x;
    }

    void Update() {
        float ポス = Mathf.Repeat(Time.time * スピード, レン);
        transform.position = スタート + Vector3.forward * ポス;
    }
}