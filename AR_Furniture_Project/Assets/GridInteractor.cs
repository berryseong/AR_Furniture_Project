using UnityEngine;
using UnityEngine.EventSystems; // UI 터치 감지용

public class GridInteractor : MonoBehaviour
{
    [Header("불러올 이미지 이름")]
    public string imageName = "desk1"; 
    
    private GameObject previewCube; 
    private RoomGenerator roomGen; 

    void Start()
    {
        roomGen = FindObjectOfType<RoomGenerator>();

        // 연두색 미리보기 큐브 생성
        previewCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        previewCube.name = "PreviewCube";
        Destroy(previewCube.GetComponent<Collider>()); 

        Material mat = Resources.Load<Material>("BlockMat");
        if (mat != null) 
        {
            previewCube.GetComponent<Renderer>().material = new Material(mat);
            previewCube.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f, 0.5f); 
        }
    }

    void Update()
    {
        // 1m 크기 가져오기
        float currentCubeSize = roomGen.cubeSize / 100f;
        previewCube.transform.localScale = Vector3.one * currentCubeSize;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // 부딪힌 지점에서 살짝 띄워서 계산
            Vector3 placePoint = hit.point + (hit.normal * 0.01f); 

            // 수학적 그리드 스냅 (1m 단위로 위치 강제 고정)
            float snapX = Mathf.Floor(placePoint.x / currentCubeSize) * currentCubeSize + (currentCubeSize / 2f);
            float snapY = Mathf.Floor(placePoint.y / currentCubeSize) * currentCubeSize + (currentCubeSize / 2f);
            float snapZ = Mathf.Floor(placePoint.z / currentCubeSize) * currentCubeSize + (currentCubeSize / 2f);

            Vector3 snapPos = new Vector3(snapX, snapY, snapZ);

            float roomW = roomGen.width / 100f;
            float roomH = roomGen.height / 100f;
            float roomL = roomGen.length / 100f;

            // 방 영역 내부인지 검사
            bool isInsideX = snapPos.x >= 0 && snapPos.x <= roomW;
            bool isInsideY = snapPos.y >= 0 && snapPos.y <= roomH;
            bool isInsideZ = snapPos.z >= 0 && snapPos.z <= roomL;

            if (isInsideX && isInsideY && isInsideZ)
            {
                previewCube.SetActive(true);
                previewCube.transform.position = snapPos;

                if (Input.GetMouseButtonDown(0))
                {
                    // UI 관통 방지 로직
                    if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return; 
                    if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;

                    PlaceFurniture(snapPos, currentCubeSize);
                }
            }
            else
            {
                previewCube.SetActive(false); // 방 밖이면 숨기기
            }
        }
        else
        {
            previewCube.SetActive(false); // 허공이면 숨기기
        }
    }

    void PlaceFurniture(Vector3 position, float size)
    {
        GameObject newFurn = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newFurn.transform.position = position;
        newFurn.transform.localScale = Vector3.one * size;
        newFurn.name = "Placed_" + imageName;

        Material baseMat = Resources.Load<Material>("BlockMat");
        if (baseMat != null) newFurn.GetComponent<Renderer>().material = new Material(baseMat);

        Texture2D tex = Resources.Load<Texture2D>(imageName);
        if (tex != null)
        {
            newFurn.GetComponent<Renderer>().material.mainTexture = tex;
            newFurn.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void ChangeFurniture(string newImageName)
    {
        imageName = newImageName;
    }
}