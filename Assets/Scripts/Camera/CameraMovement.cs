namespace Chess.camera
{
    using Enums;

    using UnityEngine;

    using System.Collections;

    public class CameraMovement : MonoBehaviour
    {
        //TODO: Implment Camera movement code
        //Have an event that when triggered rotates camera to the correct position
        private Quaternion[] _CameraTurnRotations = new Quaternion[2];

        private float _TimeToCompleteRotation = 1.5f;

        public void Init()
        {
            Quaternion startingRotation = transform.rotation;
            _CameraTurnRotations[0] = startingRotation;
            _CameraTurnRotations[1] = Quaternion.Euler(startingRotation.x, startingRotation.y - 180, startingRotation.z);
        }

        public void RotateCamera(ETurn currentTurn)
        {
            StartCoroutine(CameraRotation(currentTurn));
        }

        private IEnumerator CameraRotation(ETurn currentTurn)
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