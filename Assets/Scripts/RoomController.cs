using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField]
    GameObject[] targets;

    [SerializeField]
    Transform[] portals;

    CinemachineTargetGroup _targetGroup;
    CinemachineVirtualCamera _virtualCamera;

    void Start()
    {
        _targetGroup = GetComponent<CinemachineTargetGroup>();
        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _virtualCamera.Follow = _targetGroup.transform;
    }

    void OnTriggerxit2D(Collider2D other)
    {
        foreach (var target in targets)
        {
            _targetGroup.AddMember(target.transform, 1, 1);
        }

        foreach (var portal in portals)
        {
            portal.gameObject.SetActive(false);
        } 

        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }
}
