using UnityEngine;

public class ParallaxBackground_0 : MonoBehaviour
{
    // ī�޶� �ڵ����� �������� ����
    public bool Camera_Move;

    // ī�޶� �̵� �ӵ�
    public float Camera_MoveSpeed = 1.5f;

    [Header("Layer Setting")]
    // �� ���̾��� �з����� �̵� �ӵ� (0 = ����, 1 = ī�޶�� ����)
    public float[] Layer_Speed = new float[7];

    // �з����� ��� ���̾� ������Ʈ��
    public GameObject[] Layer_Objects = new GameObject[7];

    // ���� ī�޶� Transform
    private Transform _camera;

    // �� ���̾ ���� ��ġ (ī�޶� ���� ��ǥ ����)
    private Vector2[] startPos = new Vector2[7];

    // ��������Ʈ ������ ���� ũ��
    private float boundSizeX;

    // ������Ʈ�� x�� ������ (localScale.x)
    private float sizeX;

    void Start()
    {
        _camera = Camera.main.transform;

        // ��� ��������Ʈ ������Ʈ�� ������
        sizeX = Layer_Objects[0].transform.localScale.x;

        // ��� ��������Ʈ ��ü�� ���� ���� (world ����)
        boundSizeX = Layer_Objects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        // �ʱ� ī�޶� ��ġ�� �� ���̾��� ���� ��ġ�� ����
        for (int i = 0; i < 5; i++)
        {
            startPos[i].x = _camera.position.x;
            startPos[i].y = Layer_Objects[i].transform.position.y;
        }
    }

    void Update()
    {
        // ī�޶� �̵�
        if (Camera_Move)
        {
            _camera.position += Vector3.right * Time.deltaTime * Camera_MoveSpeed;
        }

        // �� ���̾� ��ġ ����
        for (int i = 0; i < 5; i++)
        {
            // ī�޶�� ���̾� �ӵ��� ���̰� (������)
            float temp = _camera.position.x * (1 - Layer_Speed[i]);

            // ī�޶� �̵��� ���� ���� ���̾� ��ġ
            float distance = _camera.position.x * Layer_Speed[i];

            // ���̾��� ��ġ ���� (ī�޶� Y�� �״��)
            Layer_Objects[i].transform.position = new Vector2(startPos[i].x + distance, startPos[i].y);

            //boundSizeX * sizeX = ���̾��� ���� ����
            // ���������� �Ѿ�� ��� �� ���� ��ġ�� ���������� �� ĭ �̵�
            if (temp > startPos[i].x + boundSizeX * sizeX)
            {
                startPos[i].x += boundSizeX * sizeX;
            }
            // �������� �Ѿ�� ��� �� ���� ��ġ�� �������� �� ĭ �̵�
            else if (temp < startPos[i].x - boundSizeX * sizeX)
            {
                startPos[i].x -= boundSizeX * sizeX;
            }
        }
    }

}
