using UnityEngine;

//레이어 오브젝트의 스프라이트를 변경하는 스크립트
public class BackgroundControl_0 : MonoBehaviour
{
    [Header("BackgroundNum 0 -> 3")]
    public int backgroundNum; //현재 적용된 배경종류
    public Sprite[] Layer_Sprites; // 배경종류 전체 스프라이트
    private GameObject[] Layer_Object = new GameObject[5];//레이어 받을 오브젝트 
    private int max_backgroundNum = 3; // 배경종류 최대값
    void Start()
    {
        //배경종류 0~3까지의 스프라이트를 Layer_Sprites에 넣어주고,
        //Layer_Object에 레이어 오브젝트를 넣어준다.
        for (int i = 0; i < Layer_Object.Length; i++)
        {
            Layer_Object[i] = GameObject.Find("Layer_" + i);
        }

        ChangeSprite();
    }


    // 배경종류에 맞는 스프라이트로 레이어 오브젝트의 스프라이트를 변경한다.
    void ChangeSprite()
    {
        Layer_Object[0].GetComponent<SpriteRenderer>().sprite = Layer_Sprites[backgroundNum * 5];

        //각 오브젝트에 (여분 포함)스프라이트를 넣어준다.
        for (int i = 1; i < Layer_Object.Length; i++)
        {
            Sprite changeSprite = Layer_Sprites[backgroundNum * 5 + i];
            //Change Layer_1->7
            Layer_Object[i].GetComponent<SpriteRenderer>().sprite = changeSprite;
            //Change "Layer_(*)x" sprites in children of Layer_1->7
            Layer_Object[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = changeSprite;
            Layer_Object[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = changeSprite;
        }
    }

    public void NextBG()
    {
        backgroundNum = backgroundNum + 1;
        if (backgroundNum > max_backgroundNum) backgroundNum = 0;
        ChangeSprite();
    }
    public void BackBG()
    {
        backgroundNum = backgroundNum - 1;
        if (backgroundNum < 0) backgroundNum = max_backgroundNum;
        ChangeSprite();
    }
}
