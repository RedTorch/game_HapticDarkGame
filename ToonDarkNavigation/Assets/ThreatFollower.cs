using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatFollower : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform followTarget;
    [SerializeField] private Vector3 goalPosition;

    private float teleportCooldown = 0f;
    [SerializeField] private float maxTeleportCooldown = 5f;

    private bool frontAttacker = false;

    private float playerFollowDuration;
    [SerializeField] private float playerFollowDurationMax = 10f;

    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Activate()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive)
        {
            return;
        }
        Vector3 moveDirection = (followTarget.position-transform.position).normalized * moveSpeed * Time.deltaTime;
        transform.Translate(moveDirection);
        teleportCooldown -= Time.deltaTime;
        if(Vector3.Distance(followTarget.position, transform.position) < 20f)
        {
            playerFollowDuration += Time.deltaTime;
            if(playerFollowDuration > playerFollowDurationMax)
            {
                playerFollowDuration = 0f;
                attemptTeleport();
            }
        }
        else if(teleportCooldown<=0f && Vector3.Distance(followTarget.position, transform.position) > 15f)
        {
            playerFollowDuration = 0f;
            teleportCooldown = maxTeleportCooldown;
            attemptTeleport();
        }
    }

    private void attemptTeleport()
    {
        if(frontAttacker)
        {
            teleportToFront();
        }
        else
        {
            teleportToRandom();
        }
    }

    private void teleportToRandom()
    {
        float angle = Random.Range(0f,360f);
        float distance = Random.Range(11f, 15f);
        Vector3 randPos = new Vector3(Mathf.Cos(angle)*distance, 0f, Mathf.Sin(angle)*distance);
        transform.position = followTarget.position + randPos;
    }

    private void teleportToFront()
    {
        Vector3 frontPosition = (followTarget.position-transform.position).normalized * -15f;
        transform.position = followTarget.position + frontPosition;
    }

    public void StartFrontAttacker()
    {
        frontAttacker = true;
        moveSpeed = 4f;
    }
}
