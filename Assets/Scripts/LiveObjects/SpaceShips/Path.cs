using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.LiveObjects.Spaceships
{
    [System.Serializable]
    public class Path
    {
        public static float offset = 0.5f;
        [SerializeField] List<Transform> _route;
        public bool IsOccupied;
        public int FinalWaypoint { get { return _route.Count - 1; } }
        public Transform FirstWaypoint { get { return _route[0]; } }
        public bool TryGetNextWaypoint(int index, out int nextIndex)
        {
            if (index >= FinalWaypoint)
            {
                nextIndex = -1;
                return false;
            }
            else
            {
                nextIndex = ++index;
                return true;
            }
        }
        public Vector3 CalculateSpaceshipDirection(int nextWaypoint)
        {
            if (nextWaypoint == 0)
                throw new System.Exception("Path Recived Wrong Waypoint");

            var lastWaypoint = nextWaypoint - 1;
            var xDirection = _route[nextWaypoint].position.x - _route[lastWaypoint].position.x;
            var yDirection = _route[nextWaypoint].position.y - _route[lastWaypoint].position.y;
            var zDirection = _route[nextWaypoint].position.z - _route[lastWaypoint].position.z;
            return new Vector3(xDirection, yDirection, zDirection);
        }
        public bool CheckIFReachedNextWaypoint(Vector3 spaceshipPosition,int nextWaypoint)
        {
            var xOffset = spaceshipPosition.x - _route[nextWaypoint].position.x;
            var yOffset = spaceshipPosition.y - _route[nextWaypoint].position.y;
            var zOffset = spaceshipPosition.z - _route[nextWaypoint].position.z;
            if (Mathf.Abs(xOffset) < offset && Mathf.Abs(yOffset) < offset && Mathf.Abs(zOffset) < offset)
            {
                return true;
            }
            return false;
        }
    }
}