namespace Chess.camera
{
    using Enums;

    using UnityEngine;

    using System;
    using System.Threading.Tasks;

    [RequireComponent(typeof(Animator))]
    public class CameraMovement : MonoBehaviour
    {
        public static CameraMovement instance;
        private Animator _CameraAnimator;

        public void Awake()
        {
            if (instance != null)
                Destroy(this);

            instance = this;
        }

        public void Init()
        {
            _CameraAnimator = GetComponent<Animator>();
        }

        public async Task RotateCamera(EPieceColor currentTurn)
        {
            _CameraAnimator.SetTrigger(currentTurn.ToString());
            AnimationClip currentlyPlayingClip = Array.Find(_CameraAnimator.runtimeAnimatorController.animationClips, (clip) => clip.name.Contains(currentTurn.ToString()));
            await Task.Delay((int)(currentlyPlayingClip.length * 1000));
        }
    }
}