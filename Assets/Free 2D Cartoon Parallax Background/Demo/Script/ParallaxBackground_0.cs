using UnityEngine;

public class ParallaxBackground_0 : MonoBehaviour
{
    // 카메라가 자동으로 움직일지 여부
    public bool Camera_Move;

    // 카메라 이동 속도
    public float Camera_MoveSpeed = 1.5f;

    [Header("Layer Setting")]
    // 각 레이어의 패럴랙스 이동 속도 (0 = 고정, 1 = 카메라와 동일)
    public float[] Layer_Speed = new float[7];

    // 패럴랙스 배경 레이어 오브젝트들
    public GameObject[] Layer_Objects = new GameObject[7];

    // 메인 카메라 Transform
    private Transform _camera;

    // 각 레이어별 시작 위치 (카메라 기준 좌표 저장)
    private float[] startPos = new float[7];

    // 스프라이트 원본의 가로 크기
    private float boundSizeX;

    // 오브젝트의 x축 스케일 (localScale.x)
    private float sizeX;

    void Start()
    {
        _camera = Camera.main.transform;

        // 배경 스프라이트 오브젝트의 스케일
        sizeX = Layer_Objects[0].transform.localScale.x;

        // 배경 스프라이트 자체의 가로 길이 (world 단위)
        boundSizeX = Layer_Objects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        // 초기 카메라 위치를 각 레이어의 시작 위치로 설정
        for (int i = 0; i < 5; i++)
        {
            startPos[i] = _camera.position.x;
        }
    }

    void Update()
    {
        // 카메라 이동
        if (Camera_Move)
        {
            _camera.position += Vector3.right * Time.deltaTime * Camera_MoveSpeed;
        }

        // 각 레이어 위치 갱신
        for (int i = 0; i < 5; i++)
        {
            // 카메라와 레이어 속도의 차이값 (보정값)
            float temp = _camera.position.x * (1 - Layer_Speed[i]);

            // 카메라 이동에 따라 계산된 레이어 위치
            float distance = _camera.position.x * Layer_Speed[i];

            // 레이어의 위치 갱신 (카메라 Y는 그대로)
            Layer_Objects[i].transform.position = new Vector2(startPos[i] + distance, _camera.position.y);

            //boundSizeX * sizeX = 레이어의 가로 길이
            // 오른쪽으로 넘어갔을 경우 → 시작 위치를 오른쪽으로 한 칸 이동
            if (temp > startPos[i] + boundSizeX * sizeX)
            {
                startPos[i] += boundSizeX * sizeX;
            }
            // 왼쪽으로 넘어갔을 경우 → 시작 위치를 왼쪽으로 한 칸 이동
            else if (temp < startPos[i] - boundSizeX * sizeX)
            {
                startPos[i] -= boundSizeX * sizeX;
            }
        }
    }

}
