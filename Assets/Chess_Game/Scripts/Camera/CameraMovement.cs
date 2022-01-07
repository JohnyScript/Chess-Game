namespace Chess.camera
{
    using Enums;

    using UnityEngine;

    using System.Collections;

    public class CameraMovement : MonoBehaviour
    {
        public static CameraMovement instance;

        //Have an event that when triggered rotates camera to the correct position
        private Quaternion[] _CameraTurnRotations = new Quaternion[2];

        private float _TimeToCompleteRotation = 1.5f;

        public void Awake()
        {
            if (instance != null)
                Destroy(this);

            instance = this;
        }

        public void Init()
        {
            Quaternion startingRotation = transform.rotation;
            _CameraTurnRotations[0] = startingRotation;
            _CameraTurnRotations[1] = Quaternion.Euler(startingRotation.x, startingRotation.y - 180, startingRotation.z);
        }

        public void RotateCamera(EPieceColor currentTurn)
        {
            StartCoroutine(CameraRotation(currentTurn));
        }

        private IEnumerator CameraRotation(EPieceColor currentTurn)
        {
            float timeCount = 0.0f;
            Quaternion _currentRot = transform.rotation;

            while (timeCount < _TimeToCompleteRotation)
            {
                transform.rotation = Quaternion.Slerp(_currentRot, _CameraTurnRotations[(int)currentTurn % 2], timeCount / _TimeToCompleteRotation);
                timeCount += Time.deltaTime;
                yield return null;
            }

            transform.rotation = _CameraTurnRotations[(int)currentTurn % 2];
        }
    }
}