using UnityEngine;

// 이 스크립트는 카메라가 특정 타겟(방 중앙)을 중심으로 빙글빙글 돌게 만듭니다.
public class CameraOrbit : MonoBehaviour
{
    [Header("Target (Rotation Center)")]
    public Transform target; // 카메라가 회전할 중심점

    [Header("Settings")]
    public float distance = 15.0f; // 중심과의 거리 
    public float xSpeed = 120.0f; // 가로 회전 속도
    public float ySpeed = 120.0f; // 세로 회전 속도

    // 화면이 뒤집히지 않도록 세로 각도 제한
    public float yMinLimit = 5f;  // 거의 바닥에서 보는 각도
    public float yMaxLimit = 85f; // 거의 꼭대기에서 보는 각도

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        // 초기 카메라 각도 가져오기
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // 타겟이 없다면 자동으로 RoomManager를 찾아 중심점으로 설정 시도
        if (target == null)
        {
            GameObject rm = GameObject.Find("RoomManager");
            if(rm != null) target = rm.transform;
        }
    }

    void LateUpdate()
    {
        if (target)
        {
            // 1. 화면 드래그 (방 회전) - 손가락이 1개일 때만 작동
            if (Input.GetMouseButton(0) && Input.touchCount < 2)
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }

            // 2. 두 손가락 줌인/줌아웃 (Pinch to Zoom)
            if (Input.touchCount == 2)
            {
                Touch t0 = Input.GetTouch(0);
                Touch t1 = Input.GetTouch(1);

                Vector2 t0Prev = t0.position - t0.deltaPosition;
                Vector2 t1Prev = t1.position - t1.deltaPosition;

                float prevMag = (t0Prev - t1Prev).magnitude;
                float currentMag = (t0.position - t1.position).magnitude;
                float difference = currentMag - prevMag;

                distance -= difference * 0.15f; // 줌 속도 조절
                distance = Mathf.Clamp(distance, 1f, 100f); // 너무 뚫고 들어가거나 멀어지지 않게 한계 지정
            }

            // 카메라 위치 최종 적용
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    // 각도를 제한 범위 내로 고정해주는 함수
    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}