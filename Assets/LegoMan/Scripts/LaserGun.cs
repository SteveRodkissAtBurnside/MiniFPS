using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    LineRenderer lineRenderer;
    Transform lineTransform;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineTransform = lineRenderer.transform;
        audioSource = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        //playe the sound
        audioSource.Play();
        //shoot the linerenderer
        Ray ray = new Ray(lineTransform.position, lineTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            //we hit something- draw a line to the hit point
            StartCoroutine(DrawLine(lineTransform.position, hit.point));
        }
        else
        {
            //draw a line 100 units forward
            StartCoroutine(DrawLine(lineTransform.position, lineTransform.position + (lineTransform.forward * 100f)));
        }
    }

    IEnumerator DrawLine(Vector3 start, Vector3 end)
    {
        //set the linerenderer points wait a bit then unset them
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        yield return new WaitForSeconds(0.1f);
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
    }

}
