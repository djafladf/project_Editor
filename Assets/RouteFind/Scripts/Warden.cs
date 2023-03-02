using System.Collections.Generic;
using UnityEngine;

public class Warden : MonoBehaviour
{
    public int totalLengthLimit;
    private int remainingLength;
    private Point currentPoint;

    public Stack<Route> patrolRoutes;

    void Start()
    {
        patrolRoutes = new Stack<Route>();
        Reset();
    }

    /// <summary>
    /// 이동 가능한 경로인지 확인
    /// - 경로 존재 여부, 중복 경로, 길이 제한, 출발 지점 확인
    /// </summary>
    public bool CanMove(Route route)
    {
        bool isRouteExist = false;
        foreach(Route iter in GameObject.FindObjectOfType<RouteManager>().routes)
        {
            if (iter.points == route.points)
            {
                isRouteExist = true;
                break;
            }
        }
        if (!isRouteExist)
        {
            Debug.Log(this.ToString() + ": Route don't Exist");
            return false;
        }
        if (patrolRoutes.Contains(route))
        {
            Debug.Log(this.ToString() + ": Route Already Patroled");
            return false;
        }
        if (remainingLength < route.length)
        {
            Debug.Log(this.ToString() + ": Length Limit over");
            return false;
        }
        if (!route.points.Contains(currentPoint))
        {
            Debug.Log(this.ToString() + ": Route Can't Reach");
            return false;
        }
        else 
            return true;
    }

    /// <summary>
    ///  경로를 따라 이동
    /// </summary>
    public void Move(Route route)
    {
        if (CanMove(route))
        {
            remainingLength -= route.length;
            currentPoint = currentPoint == route.points[0] ? route.points[1] : route.points[0];
            transform.position = currentPoint.transform.position;
            patrolRoutes.Push(route);
            return;
        }
        Debug.Log(this.ToString() + ": Move Route Failed");
    }

    /// <summary>
    ///  하나 이전 위치로 이동
    /// </summary>
    public void Revert()
    {
        if(patrolRoutes.Count > 0)
        {
            Route recentRoute = patrolRoutes.Pop();
            remainingLength += recentRoute.length;
            currentPoint = currentPoint == recentRoute.points[0] ? recentRoute.points[1] : recentRoute.points[0];
            transform.position = currentPoint.transform.position;
            return;
        }
        Debug.Log(this.ToString() + ": Already First Point");
    }

    /// <summary>
    ///  경로 초기화
    /// - 남은 길이 초기화, 첫 위치로 돌아가기, 경로 스택 클리어
    /// </summary>
    public void Reset()
    {
        remainingLength = totalLengthLimit;
        currentPoint = transform.parent.GetComponent<Point>();
        transform.position = currentPoint.transform.position;
        patrolRoutes.Clear();
    }

    /// <summary>
    ///  현재 위치 불러오기
    /// </summary>
    public Point GetCurrentPoint()
    {
        return currentPoint;
    }
}