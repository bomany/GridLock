using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Grid;
using Grid.Pathfind;

namespace Grid.Example
{
    public class SimpleAI : MonoBehaviour
    {
        GridEntity entity;
        GridNavAgent agent;

        GridEntity target;
        List<GridBlock> grid { get { return GridManager.Instance.grid.Cast<GridBlock>().ToList(); } }

        Rigidbody rb;

        void Awake()
        {
            entity = GetComponent<GridEntity>();
            agent = GetComponent<GridNavAgent>();
            rb = GetComponent<Rigidbody>();
        }

        public void FindNearestPickUp()
        {
            GridEntity pickup = GameObject.FindGameObjectsWithTag("Pickup").Select(x => x.GetComponent<GridEntity>()).OrderBy(x => GridManager.Distance(entity.position, x.position)).FirstOrDefault();
            if (pickup == null) { Debug.Log("No Pickup Found"); return; }
            target = pickup;
            agent.isReseting = true;

            agent.SetStart(entity.position);
            agent.SetTarget(GridManager.Instance.navigation.GetNode(target.position));
            agent.CalculatePath();
        }

        public void CalculatePath()
        {
            Debug.Log("Calculating Path");
            agent.Path();
            Debug.Log("Path Found");
        }

        public void MoveTo()
        {
            Debug.Log("Moving Toggled");
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Triggered by " + other.tag);
            if (other.tag.Equals("Pickup"))
            {
                Destroy(other.gameObject);
                PickUpSpawner.Instance.ReduceCount(1);
            }
        }

        void OnDrawGizmos()
        {
            if (target != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, target.transform.position);
            }
        }


        void LateUpdate()
        {
            if (target == null)
            {
                FindNearestPickUp();
            }
        }
    }
}