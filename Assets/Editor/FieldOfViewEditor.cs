using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(CartAbilities))]
public class FieldOfViewEditor : Editor {
    private void OnSceneGUI() {
        CartAbilities fov = (CartAbilities)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle01 = directionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = directionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

        if(fov.canSeeBall) {
            Handles.color = Color.green;
            foreach(Collider colider in fov.rayData) {
                Handles.DrawLine(fov.transform.position, colider.transform.position);
            }
        }
    }

    private Vector3 directionFromAngle(float eulerY, float angleInDegrees) {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
