using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RandomMovement : MonoBehaviour
{
    [SerializeField]
    public float range = 0.15f;
    [SerializeField]
    public float durationLength = 12.5f;

    private float baseRange;
    private float baseDurationLength;

    private Vector3 defaultPosition;

    private void Awake()
    {
        defaultPosition = transform.localPosition;

        baseRange = range;
        baseDurationLength = durationLength;
    }

    public void Reset()
    {
        range = baseRange;
        durationLength = baseDurationLength;
    }

    private void OnEnable()
    {
        StartCoroutine(LerpToRandomPosition());
    }

    private void OnDisable()
    {
        transform.localPosition = defaultPosition;
    }

    IEnumerator LerpToRandomPosition()
    {
        while (this.enabled)
        {
            Vector3 target = new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
            yield return StartCoroutine(Lerp(target, Vector3.Distance(defaultPosition, target) * durationLength));
        }
    }

    IEnumerator Lerp(Vector3 target, float time)
    {
        float pastTime = 0;
        Vector3 startPosition = transform.localPosition;

        while(pastTime/time < 1 && this.enabled)
        {
            pastTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPosition, target, pastTime / time);
            yield return new WaitForEndOfFrame();
        }
    }
}
