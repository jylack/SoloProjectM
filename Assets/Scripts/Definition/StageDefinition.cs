using UnityEngine;

/// <summary>
/// 스테이지별 설정을 담는 ScriptableObject입니다.
/// </summary>
[CreateAssetMenu(menuName = "Game/Stage Definition")]
public class StageDefinition : ScriptableObject
{
    [Header("스테이지 기본 정보")]
    public int stageId;
    public string stageName;

    [Header("스테이지 진행 일수 (Day) 수")]
    public int maxDays;

    [Header("이 스테이지에서 사용할 인카운터 테이블")]
    public EncounterTable encounterTable;

    [Header("스테이지 배경 이미지 (UI용)")]
    public Sprite backgroundSprite;

    [Header("적 스폰 위치 부모 Transform")]
    public Transform spawnRoot;

    [Header("스테이지 테마 배경 설정")]
    public BackgroundType backgroundType;  // 배경 종류 (Forest, Desert 등)

    [Header("패럴랙스 배경 설정")]
    public bool parallaxEnabled = true;            // 패럴랙스 동작 여부
    public float parallaxCameraSpeed = 1.5f;       // 카메라 이동 속도
    public float[] parallaxLayerSpeeds = new float[0];  // 레이어별 패럴랙스 속도
}
