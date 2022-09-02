using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class WheelTrail : MonoBehaviour
{

    public LayerMask groundLayer;
    bool isGrounded = false;
    TrailRenderer trailRenderer;
    float distance = .5f;
    float wheelWidth = .2f;
    bool isDrift = false;

    // Update is called once per frame
    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void SetIsDrift(bool isIt) {  isDrift = isIt; }
    void Update()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);
        

        if (isGrounded && isDrift)
        {
            trailRenderer.emitting = true;
            if(hit.point != Vector3.zero)
                Debug.DrawLine(transform.position, hit.point, Color.green);
        }
        else {
            trailRenderer.emitting = false;
            if (hit.point != Vector3.zero)
                Debug.DrawLine(transform.position, hit.point, Color.green);
        }
    }
}
