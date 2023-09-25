//TrajectoryLine.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script creates a trajectory line to show where a thrown object will land
public class TrajectoryLine : MonoBehaviour
{
    [SerializeField]
    private LineRenderer LineRenderer;
    [SerializeField]
    [Range(10, 100)]
    private int LinePoints = 25;
    [SerializeField] private Player_Move_Update player;
    [SerializeField] private Rigidbody grabobj;
    private LayerMask collisionMask;
    [Range(0.01f, 0.25f)]
    [SerializeField] private float TimeBetweenPoints = 0.1f;

    //Prevents the trajectory line from colliding with anything
    private void Awake()
    {
        TurnOff();
        int objectlayer = grabobj.gameObject.layer;
        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(objectlayer, i))
            {
                collisionMask |= 1 << i;
            }
        }
    }
    //Draws the line based on the objects mass and projected force
    public void DrawProjection()
    {
        LineRenderer.enabled = true;
        LineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;
        Vector3 startPosition = player.inhand.position;
        Vector3 startVelocity = (player.dropforce * player.dropvec / grabobj.mass)/113;
        int i = 0;
        LineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < LinePoints; time += TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 10f * time * time);

            LineRenderer.SetPosition(i, point);

            Vector3 lastPosition = LineRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition,
                (point - lastPosition).normalized,
                out RaycastHit hit,
                (point - lastPosition).magnitude,
                collisionMask))
            {
                LineRenderer.SetPosition(i, hit.point);
                LineRenderer.positionCount = i + 1;
                return;
            }
        }
    }
    //Hides the line when not in use
    public void TurnOff()
    {
        LineRenderer.enabled = false;
    }
}
