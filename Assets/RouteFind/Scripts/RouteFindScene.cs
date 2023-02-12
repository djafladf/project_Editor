using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RouteFindScene : MonoBehaviour
{
    // TODO: ScriptableObject로 생성한 Stage data 클래스 맞춰서 수정
    public List<Object> stageDataList;
    // 점 프리팹
    public GameObject pointPrefab;
    // 경로 프리팹
    public GameObject routePrefab;

    // 현재 스테이지 데이터
    private Object currentStageData;

    // 현재 활성화된 감시관 오브젝트
    public Warden currentWarden;

    // TODO: private로 변경
    public List<Point> pointList;
    public List<Route> routeList;

    void Start() {
        Scene scene = SceneManager.GetActiveScene();
        // TODO: 신 매개변수 따로 설정 필요 (buildIndex 말고)
        int stageNumber = 0;
        stageNumber = scene.buildIndex;
        //currentStageData = stageDataList[stageNumber];

        pointList = new List<Point>();
        routeList = new List<Route>();

        //// 점들 생성
        //for (int i = 0; i < currentStageData.pointDataList.Length; i++)
        //{
        //    Point point = Instantiate(pointPrefab, currentStageData.pointDataList[i].position, Quaternion.identity);
        //    pointList.Add(point);
        //}
        //// 경로 생성
        //for (int i = 0; i < currentStageData.routeDataList.Count; i++)
        //{
        //    // TODO: 자동으로 생성 각도 계산하여 적용되도록
        //    Route route = Instantiate(routePrefab, currentStageData.routeDataList[i].startPoint.position, Quaternion.identity);
        //    route.startPoint = pointList[currentStageData.routeDataList[i].startPointIndex];
        //    route.endPoint = pointList[currentStageData.routeDataList[i].endPointIndex];
        //    route.length = currentStageData.routeDataList[i].length;
        //    routeList.Add(route);
        //}

        // TODO: 감시관 오브젝트에 시작 길이와 총 길이 제한 입력하기
    }

    public Route getRoute(Point startPoint, Point endPoint)
    {
        Route result = null;
        foreach(Route iter in routeList)
        {
            if(iter.startPoint == startPoint)
                if(iter.endPoint == endPoint)
                {
                    result = iter;
                    break;
                }
        }

        return result;
    }
}
