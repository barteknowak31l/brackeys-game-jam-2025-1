using System.Collections;
using StateMachine.states;
using UnityEngine;

namespace Shark
{
    public class Shark : MonoBehaviour
    {
     private SharkState _ctx;
        
        public void Setup(SharkState ctx, Vector3 startPoint, Vector3 endPoint, float height, float duration, bool spawnLeft)
        {
            _ctx = ctx;

            if (spawnLeft)
            {
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 180, 0);
            }
            
            StartCoroutine(MoveAlongParabola(startPoint, endPoint, height, duration));
        }
        private IEnumerator MoveAlongParabola(Vector3 start, Vector3 end, float height, float time)
        {
            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                float t = elapsedTime / time;
                Vector3 currentPos = Parabola(start, end, height, t);
                transform.position = currentPos;

                if (elapsedTime > 0)
                {
                    Vector3 nextPos = Parabola(start, end, height, t + Time.deltaTime / time);
                    transform.forward = (nextPos - currentPos).normalized;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = end;
            Destroy(gameObject);
        }

        private Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
        {
            Vector3 mid = Vector3.Lerp(start, end, t);
            float parabolaHeight = 4 * height * t * (1 - t);
            return new Vector3(mid.x, start.y + parabolaHeight, mid.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _ctx.OnSharkHitPlayer();
            }
        }
    }
}
