using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Camera Settings")]    
    public bool cameraMove;                // 카메라가 자동으로 움직일지 여부
    public float cameraMoveSpeed = 1.5f;   // 카메라 이동 속도

    [Header("Parallax Layers")]    
    public GameObject[] layerObjects;      // 패럴랙스 배경 레이어 오브젝트들
    public float[] layerSpeed;             // 각 레이어의 패럴랙스 이동 속도 (0 = 고정, 1 = 카메라와 동일)

    private Transform _camera;              // 메인 카메라 Transform
    private Vector2[] startPositions;             // 각 레이어별 시작 위치 (카메라 기준 좌표 저장)    
    private float[] layerWidths;            // 각 레이어의 가로 길이 (월드 단위)
    private float boundSizeX;               // 스프라이트 원본의 가로 크기        
    private float sizeX;                    // 배경 스프라이트 오브젝트의 스케일 (x축)
    private void Awake()
    {
        if(layerObjects.Length != layerSpeed.Length)
        {
            Debug.LogError("Layer_Objects and Layer_Speed arrays must have the same length.");            
        }

        _camera = Camera.main.transform;
        int layerCount = layerObjects.Length;
        startPositions = new Vector2[layerCount]; // 각 레이어의 시작 위치를 저장할 배열
        layerWidths = new float[layerCount];

        for(int i = 0; i < layerCount; i++)
        {
            var obj = layerObjects[i];
            startPositions[i] = obj.transform.position; // 각 레이어의 시작 위치 초기화

            var spriteRenderer = obj.GetComponent<SpriteRenderer>();
            float spriteWidth = spriteRenderer.sprite.bounds.size.x; // 스프라이트의 가로 길이
            float scaleX = obj.transform.localScale.x; // 오브젝트의 x축 스케일
            layerWidths[i] = spriteWidth * scaleX; // 레이어의 가로 길이 계산
        }

    }

    //void Start()
    //{
    //    _camera = Camera.main.transform;

    //    // 배경 스프라이트 오브젝트의 스케일
    //    sizeX = Layer_Objects[0].transform.localScale.x;

    //    // 배경 스프라이트 자체의 가로 길이 (world 단위)
    //    boundSizeX = Layer_Objects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;

    //    // 초기 카메라 위치를 각 레이어의 시작 위치로 설정
    //    for (int i = 0; i < Layer_Objects.Length; i++)
    //    {
    //        startPos[i].x = _camera.position.x;
    //        startPos[i].y = Layer_Objects[i].transform.position.y;
    //    }
    //}

    //void Update()
    //{
    //    // 카메라 이동
    //    if (Camera_Move)
    //    {
    //        _camera.position += Vector3.right * Time.deltaTime * Camera_MoveSpeed;
    //    }

    //    // 각 레이어 위치 갱신
    //    for (int i = 0; i < 5; i++)
    //    {
    //        // 카메라와 레이어 속도의 차이값 (보정값)
    //        float temp = _camera.position.x * (1 - Layer_Speed[i]);

    //        // 카메라 이동에 따라 계산된 레이어 위치
    //        float distance = _camera.position.x * Layer_Speed[i];

    //        // 레이어의 위치 갱신 (카메라 Y는 그대로)
    //        Layer_Objects[i].transform.position = new Vector2(startPos[i].x + distance, startPos[i].y);

    //        //boundSizeX * sizeX = 레이어의 가로 길이
    //        // 오른쪽으로 넘어갔을 경우 → 시작 위치를 오른쪽으로 한 칸 이동
    //        if (temp > startPos[i].x + boundSizeX * sizeX)
    //        {
    //            startPos[i].x += boundSizeX * sizeX;
    //        }
    //        // 왼쪽으로 넘어갔을 경우 → 시작 위치를 왼쪽으로 한 칸 이동
    //        else if (temp < startPos[i].x - boundSizeX * sizeX)
    //        {
    //            startPos[i].x -= boundSizeX * sizeX;
    //        }
    //    }
    //}

    void Update()
    {
        // 1) 카메라 자동 이동
        if (cameraMove)
            _camera.position += Vector3.right * cameraMoveSpeed * Time.deltaTime;

        float camX = _camera.position.x;

        // 2) 각 레이어 패럴랙스 및 재배치
        for (int i = 0; i < layerObjects.Length; i++)
        {
            float speed = layerSpeed[i];
            var obj = layerObjects[i];
            Vector3 startPos = startPositions[i];

            // 패럴랙스 오프셋 계산
            float offsetX = (camX - startPos.x) * speed;
            float newX = startPos.x + offsetX;

            // Z 축 보존
            float zPos = obj.transform.position.z;
            obj.transform.position = new Vector3(newX, startPos.y, zPos);

            // 일정 범위를 벗어나면 타일 재배치
            float temp = camX * (1 - speed);
            float tileWidth = layerWidths[i];
            if (temp > startPos.x + tileWidth)
            {
                startPositions[i].x += tileWidth;
            }
            else if (temp < startPos.x - tileWidth)
            {
                startPositions[i].x -= tileWidth;
            }
        }
    }

}
