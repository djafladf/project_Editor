using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Convey_Manager : MonoBehaviour
{
    public Infra_Option Option;
    public bool OptionAble = true;

    private void LateUpdate()
    {
        if (Input.anyKey && Option.gameObject.activeSelf && !Option.OnOption) Option.gameObject.SetActive(false);
    }

}
