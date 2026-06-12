using UnityEngine;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour
{
    [Header("Room Dimensions (cm)")]
    public float width = 600f;   
    public float length = 1000f; 
    public float height = 200f;  

    [Header("Grid Settings (cm)")]
    public float cubeSize = 100f; // 1m(100cm)로 설정. 10cm 로 설정 시 큐브가 너무 작아져서 조작이 어려움.

    private List<GameObject> roomParts = new List<GameObject>();

    void Start() { GenerateRoom(); }

    public void GenerateRoom()
    {
        foreach (var part in roomParts) { if (part != null) Destroy(part); }
        roomParts.Clear();

        float w = width / 100f;
        float l = length / 100f;
        float h = height / 100f;
        float thickness = 0.1f; 

        // 벽 생성
        roomParts.Add(CreateWall("Floor", new Vector3(w/2, -thickness/2, l/2), new Vector3(w, thickness, l)));
        roomParts.Add(CreateWall("Wall_Left", new Vector3(-thickness/2, h/2, l/2), new Vector3(thickness, h, l)));
        roomParts.Add(CreateWall("Wall_Right", new Vector3(w + thickness/2, h/2, l/2), new Vector3(thickness, h, l)));
        roomParts.Add(CreateWall("Wall_Back", new Vector3(w/2, h/2, -thickness/2), new Vector3(w, h, thickness)));
    }

    GameObject CreateWall(string name, Vector3 pos, Vector3 scale)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = name;
        cube.transform.parent = this.transform;
        cube.transform.position = pos;
        cube.transform.localScale = scale;
        
        Material mat = Resources.Load<Material>("WallMat");
        if(mat != null) cube.GetComponent<Renderer>().material = mat;

        return cube;
    }

    public void SetWidth(string val) { if(float.TryParse(val, out width)) GenerateRoom(); }
    public void SetLength(string val) { if(float.TryParse(val, out length)) GenerateRoom(); }
    public void SetHeight(string val) { if(float.TryParse(val, out height)) GenerateRoom(); }
}