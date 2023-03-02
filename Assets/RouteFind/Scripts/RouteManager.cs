using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    public List<Route> routes;
    public GameObject feedbackTextPrefab;
    private WardenManager wardenManager;

    /// <summary>
    /// 두 지점을 잇는 경로 반환
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns>경로 반환, 경로가 없거나 두 지점이 같으면 null 반환</returns>
    public Route GetRoute(Point point1, Point point2)
    {
        if (point1 == point2)
            return null;
        foreach (Route iter in routes)
        {
            if (iter.points.Contains(point1) && iter.points.Contains(point2))
            {
                return iter;
            }
        }
        return null;
    }

    void Update()
    {
        // Check if all must-go routes have been crossed by more than one Warden
        // foreach (Route route in routes)
        // {
        //     foreach (Warden warden in wardens)
        //     {
        //         if (Vector3.Distance(warden.transform.position, route.transform.position) <= 0.1f)
        //         {
        //             numWardensOnRoute++;
        //         }
        //     }
        //     if (numWardensOnRoute < 2)
        //     {
        //         // Display feedback to the user that the route is not valid
        //         Vector3 feedbackTextPosition = Camera.main.WorldToScreenPoint(route.transform.position);
        //         GameObject feedbackTextObject = Instantiate(feedbackTextPrefab, feedbackTextPosition, Quaternion.identity);
        //         feedbackTextObject.transform.SetParent(Canvas.transform, false);
        //         feedbackTextObject.GetComponent<Text>().text = "Route not valid";
        //     }
        // }
    }
}