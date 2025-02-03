using System.Collections;
using UnityEngine;

public class AnimationEventsController : MonoBehaviour
{
    [SerializeField]
    Character2DController characterController;

    [SerializeField]
    float damage = 100.0f;

    public void onAttack()
    {
        if (characterController != null)
        {
            characterController.Attack(damage);
        }
    }
}
