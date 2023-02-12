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

    public void moveRoute()
    {
        if  (isOn == false)
        {
            Debug.Log("Button Clicked!");
            RouteFindScene sceneData = FindObjectOfType<RouteFindScene>();
            Warden activeWarden = sceneData.GetComponent<Warden>();
            Point startPoint = activeWarden.currentPoint;

            Route activeRoute = sceneData.getRoute(startPoint, this);

            if (activeRoute)
            {
                if (activeWarden.AddRoute(activeRoute) == 0)
                    isOn = true;
            }
            return;
        }
        Debug.Log("Point Already Active");
    }
}
