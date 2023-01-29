using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    Stack<Route> routes = new Stack<Route>();

    // 시작 지점
    public Point startPoint;
    // 총 길이 제한
    public float lengthLimit;
    // 감시관 타입
    public string type;
    

    // 현재 위치
    [SerializeField]
    private Point currentPoint;
    // 현재 총 길이
    public float totalLength = 0f;


    // 새로운 경로 추가 : 동선 하나 긋기
    public void AddRoute(Route newRoute)
    {
        // 첫 시작 지점 지정 : 루트의 첫 시작일 시 현재 위치를 시작지점으로 지정
        if (routes.Count == 0)
            currentPoint = startPoint;

        // 동선 도중에 잇는 위치 대조 : 현재 위치와 동일해야 함
        if (routes.Count > 0)
        {
            Route previousRoute = routes.Peek();

            //위치 불일치
            if (newRoute.startPoint != currentPoint)
            {
                Debug.Log("Route Incorrect : Start point doesn't match.");
                return;
            }
        }

        // 총 길이 제한 대조 : 새로운 루트를 이을 시 길이 제한을 초과하면 안됨
        if (totalLength + newRoute.length > lengthLimit)
        {
            // 길이 제한 초과
            Debug.Log("Route not Available : Length limit exceeded.");
            return;
        }

        // 루트 추가 : 스택에 추가, 길이&위치 갱신
        routes.Push(newRoute);
        totalLength += newRoute.length;
        currentPoint = newRoute.endPoint;
    }

    // 경로 삭제 : 동선 하나 취소, 뒤로 감기
    public void RemoveRoute()
    {
        // 삭제할 루트 존재 : 시작지점 제외
        if (routes.Count > 0)
        {
            Route deleteRoute = routes.Pop();
            totalLength -= deleteRoute.length;

            // 삭제 후 현재 위치 갱신 : 하나 전 위치로 돌아가기
            if (routes.Count > 0)
            {
                currentPoint = routes.Peek().endPoint;
            }
            else 
            {
                currentPoint = startPoint;
            }
        }
    }
}
