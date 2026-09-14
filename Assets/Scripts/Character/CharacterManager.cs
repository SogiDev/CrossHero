using Unity.VisualScripting;
using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(CharacterLocomotionManager))]
    [RequireComponent(typeof(CharacterAnimationManager))]
    public class CharacterManager : MonoBehaviour
    {
        [Header("Systems")]
        protected CharacterLocomotionManager locomotionManager;
        protected CharacterAnimationManager animationManager;

        [Header("States")]
        protected internal bool isSpringing;
        protected internal bool isCrouching, isJumping;


        protected virtual void Awake()
        {
            locomotionManager = GetComponent<CharacterLocomotionManager>();
            animationManager = GetComponent<CharacterAnimationManager>();
        }
        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

    }
}