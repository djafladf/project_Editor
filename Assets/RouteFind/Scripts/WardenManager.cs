using System.Collections.Generic;
using UnityEngine;

public class WardenManager : MonoBehaviour
{
    public List<Warden> wardens;
    private Warden currentWarden;

    void Start()
    {
        // Set the first Warden as the current Warden
        currentWarden = wardens[0];
    }

    void Update()
    {
        // Check if the user clicked on a Warden in the UI
        // TODO: Add Warden List UI to activate each warden
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        //     if (hit.collider != null)
        //     {
        //         Warden warden = hit.collider.GetComponent<Warden>();
        //         if (warden != null && wardens.Contains(warden))
        //         {
        //             currentWarden = warden;
        //         }
        //     }
        // }

        // Check if the user clicked on a Point to move the current Warden
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                Point point = hit.collider.GetComponent<Point>();
                if (point != null)
                {
                    Debug.Log("Hit Point : " + point.ToString());
                    if (currentWarden != null)
                    {
                        Route route = GameObject.FindObjectOfType<RouteManager>().GetRoute(currentWarden.GetCurrentPoint(), point);
                        // Move the current Warden to the selected Point
                        currentWarden.MoveRoute(route);
                    }
                }
            }
        }

        // Check if the user clicked Revert 
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(currentWarden.ToString() + ": route Revert");
            currentWarden.RevertRoute();
        }
    }
}