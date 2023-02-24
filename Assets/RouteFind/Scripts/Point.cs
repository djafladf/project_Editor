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
    ///  ��Ȱ��ȭ ������ ���� ���� Warden�� ���� ��ġ�� �̵��ϴ� �Լ�
    /// </summary>
    public void moveRoute()
    {
        if  (isOn == false)
        {
            Debug.Log("Button Clicked!");
            RouteFindScene sceneData = FindObjectOfType<RouteFindScene>();      // �� ������ �ε�
            Warden activeWarden = sceneData.currentWarden;             // ���� Ȱ��ȭ�� ���ð�
            Point startPoint = activeWarden.currentPoint;               //���ð��� ���� ��ġ = �߰��� ����� ���� ����

            Route activeRoute = sceneData.getRoute(startPoint, this);   // ���� ���� -> this ��ġ ��� �ҷ����� (������ null)

            if (activeRoute)        // �ش��ϴ� ��ΰ� ������ (null�� �ƴϸ�)
            {
                if (activeWarden.AddRoute(activeRoute) == 0)    // ��� �߰�
                    isOn = true;
            }
            return;
        }
        Debug.Log("Point Already Active");
    }
}
