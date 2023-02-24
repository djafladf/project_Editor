using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public Vector2 position;

    public bool isOn;

    public string type;

    private void Awake() {
        isOn = false;

        transform.Translate(position);
    }

    /// <summary>
    ///  비활성화 지점을 눌러 현재 Warden을 현재 위치로 이동하는 함수
    /// </summary>
    public void moveRoute()
    {
        if  (isOn == false)
        {
            Debug.Log("Button Clicked!");
            RouteFindScene sceneData = FindObjectOfType<RouteFindScene>();      // 씬 데이터 로드
            Warden activeWarden = sceneData.currentWarden;             // 현재 활성화된 감시관
            Point startPoint = activeWarden.currentPoint;               //감시관의 현재 위치 = 추가할 경로의 시작 지점

            Route activeRoute = sceneData.getRoute(startPoint, this);   // 시작 지점 -> this 위치 경로 불러오기 (없으면 null)

            if (activeRoute)        // 해당하는 경로가 있으면 (null이 아니면)
            {
                if (activeWarden.AddRoute(activeRoute) == 0)    // 경로 추가
                    isOn = true;
            }
            return;
        }
        Debug.Log("Point Already Active");
    }
}
