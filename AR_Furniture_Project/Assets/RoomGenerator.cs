using UnityEngine;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour
{
    [Header("Room Dimensions (cm)")]
    public float width = 400f;  // 가로 (X)
    public float length = 500f; // 세로 (Z)
    public float height = 250f; // 높이 (Y)

    private List<GameObject> roomParts = new List<GameObject>();

    void Start()
    {
        // 첫 시작 시 기본 방 생성
        GenerateRoom();
    }

    public void GenerateRoom()
    {
        // 1. 기존에 생성된 벽면이 있다면 삭제 (초기화)
        foreach (var part in roomParts) { if (part != null) Destroy(part); }
        roomParts.Clear();

        // 2. cm 단위를 유니티 미터(m) 단위로 변환 (100cm = 1m)
        float w = width / 100f;
        float l = length / 100f;
        float h = height / 100f;

        // 3. 6개 면 생성 (이름, 위치, 크기, 회전)
        // 바닥 (Floor)
        roomParts.Add(CreatePlane("Floor", new Vector3(w/2, 0, l/2), new Vector3(w, 1, l), Quaternion.identity));
        // 천장 (Ceiling)
        roomParts.Add(CreatePlane("Ceiling", new Vector3(w/2, h, l/2), new Vector3(w, 1, l), Quaternion.Euler(180, 0, 0)));
        // 벽면들
        roomParts.Add(CreatePlane("Wall_Left", new Vector3(0, h/2, l/2), new Vector3(l, 1, h), Quaternion.Euler(0, 0, -90)));
        roomParts.Add(CreatePlane("Wall_Right", new Vector3(w, h/2, l/2), new Vector3(l, 1, h), Quaternion.Euler(0, 0, 90)));
        roomParts.Add(CreatePlane("Wall_Front", new Vector3(w/2, h/2, l), new Vector3(w, 1, h), Quaternion.Euler(-90, 0, 0)));
        roomParts.Add(CreatePlane("Wall_Back", new Vector3(w/2, h/2, 0), new Vector3(w, 1, h), Quaternion.Euler(90, 0, 0)));
    }

    GameObject CreatePlane(string name, Vector3 pos, Vector3 scale, Quaternion rot)
    {
        GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
        p.name = name;
        p.transform.parent = this.transform;
        p.transform.position = pos;
        p.transform.rotation = rot;
        // Plane의 기본 크기가 10m x 10m이므로 0.1을 곱해 수치를 맞춥니다.
        p.transform.localScale = new Vector3(scale.x / 10f, 1, scale.z / 10f);
        return p;
    }

    // UI 연결용 함수들
    public void SetWidth(string val) { if(float.TryParse(val, out width)) GenerateRoom(); }
    public void SetLength(string val) { if(float.TryParse(val, out length)) GenerateRoom(); }
    public void SetHeight(string val) { if(float.TryParse(val, out height)) GenerateRoom(); }
}