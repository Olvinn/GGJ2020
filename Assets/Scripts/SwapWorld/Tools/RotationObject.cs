using System.Collections;
using UnityEngine;

namespace SwapWorld.Tools
{
    public class RotationObject : MonoBehaviour
    {
        [SerializeField] private Transform rotatedTransform;
        [SerializeField] private Vector3 axis;
        [SerializeField] private float toAngle;
        [SerializeField] private float speed;

        private void Start()
        {
            if (rotatedTransform != null)
                StartCoroutine(RotateTransform());
        }

        private IEnumerator RotateTransform()
        {
            while (true)
            {
                var angle = Mathf.PingPong(Time.time * speed, toAngle);
                var rotVector = axis * angle;
                var rotation = Quaternion.Euler(rotVector);
                rotatedTransform.rotation = rotation;

                yield return null;
            }
        }
    }
}