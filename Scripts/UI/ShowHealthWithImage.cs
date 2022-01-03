using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // UI에 필요한 스크립트라면 UnityEngine.UI를 꼭 사용

public class ShowHealthWithImage : MonoBehaviour
{
    public Gauge target;    // 누굴 보여 줄지

    [SerializeField]
    private List<Image> healthImageList = new List<Image>();    // 체력바 이미지를 끄거나 키기 위해
    private RectTransform rectTransform;
    private float initSizeX;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initSizeX = rectTransform.sizeDelta.x;
        
    }

    void Update()
    {
        float healthRate = target.cur / target.max;

        if (healthRate > 0 && healthRate < 1)   // 생명력이 0이 아니고 풀피가 아니일 때 실행
        {
            rectTransform.sizeDelta = new Vector2(initSizeX * healthRate, rectTransform.sizeDelta.y);

            foreach(Image current in healthImageList)
            {
                if(current != null) current.enabled = true;    // 컴포넌트 켜기
            }
        }

        else
        {
            foreach(Image current in healthImageList)
            {
                if(current != null) current.enabled = false;    // 컴포넌트 끄기
            }
        };
        
        

        // 풀피일 때 1, 피가 없을 때 0이 되는 수치
        //transform.localScale = new Vector3(target.cur / target.max, 1, 1);
    }
}
