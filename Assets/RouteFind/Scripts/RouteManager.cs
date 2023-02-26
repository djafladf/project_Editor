using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    public List<Route> routes;
    public GameObject feedbackTextPrefab;
    private WardenManager wardenManager;

    public Route getRoute(Point point1, Point point2)
    {
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