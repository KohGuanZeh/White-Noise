using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoints : MonoBehaviour {
    public Transform[] points;
    public bool drawDebugPoints;

    public Transform GetPoint(int point) {
        if (point == 0) {
            return transform;
        }

        if (point <= points.Length) {
            return points[point - 1];
        }

        return null;
    }

    private void OnDrawGizmos() {
        if (!drawDebugPoints) {
            return;
        }

        if (points.Length > 0) {
            Gizmos.DrawLine(transform.position, points[0].position);
            for (int i = 0; i < points.Length - 1; i++) {
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
            }
        }
    }
}
