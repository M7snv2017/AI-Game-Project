using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class player : MonoBehaviour
{
    [Header("Pizza Property")]
    public int Pizza = 0;
    public int DeliveredPizza = 0;

    [Header("Tilemap & Sprites")]
    public Tilemap tilemap;
    public Sprite up, down, left;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;          // world-units per second
    public float cellArrivalTolerance = 0.01f;

    public int extra = 0;

    private Vector3Int goalCell;
    private bool isMoving = false;
    private Transform tr;
    private SpriteRenderer sp;

    private void Start()
    {
        tr = transform;
        sp = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) )//&& !isMoving
        {
            StopAllCoroutines();
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;
            goalCell = tilemap.WorldToCell(mouseWorld);

            Vector3Int startCell = tilemap.WorldToCell(tr.position);
            if (goalCell != startCell && tilemap.HasTile(goalCell))
            {
                List<Vector3Int> path = FindPath(startCell, goalCell);
                if (path != null && path.Count > 1)
                    StartCoroutine(FollowPath(path));
            }
        }

    }

    private IEnumerator FollowPath(List<Vector3Int> path)
    {
        isMoving = true;

        // skip index 0 because that's the current cell
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 targetWorld = tilemap.GetCellCenterWorld(path[i]);

            // move smoothly until center of that cell
            while (Vector3.Distance(tr.position, targetWorld) > cellArrivalTolerance)
            {
                Vector3 delta = targetWorld - tr.position;

                // sprite direction
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                {
                    sp.sprite = left;
                    Vector3 s = tr.localScale;
                    s.x = delta.x < 0 ? 1 : -1;
                    tr.localScale = s;
                }
                else
                {
                    sp.sprite = delta.y < 0 ? down : up;
                }

                tr.position = Vector3.MoveTowards(tr.position, targetWorld, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        isMoving = false;
    }

    // --- A* Pathfinding on Vector3Int grid ---

    private class NavigationNode
    {
        public Vector3Int pos;
        public float gCost, hCost;
        public NavigationNode parent;
        public float fCost => gCost + hCost;
        public NavigationNode(Vector3Int p) => pos = p;
    }

    private struct Neighbor
    {
        public Vector3Int pos;
        public float cost;
        public Neighbor(Vector3Int p, float c) { pos = p; cost = c; }
    }

    private List<Neighbor> Get8Neighbors(Vector3Int c)
    {
        var list = new List<Neighbor>(8);
        for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                var n = new Vector3Int(c.x + dx, c.y + dy, c.z);
                float cost = (dx != 0 && dy != 0) ? 1.4f : 1f;
                list.Add(new Neighbor(n, cost));
            }
        return list;
    }

    private bool IsWalkable(Vector3Int c)
        => tilemap.HasTile(c);

    private float Heuristic(Vector3Int a, Vector3Int b)
        => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

    private List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        var open = new Dictionary<Vector3Int, NavigationNode>();
        var closed = new HashSet<Vector3Int>();

        var startNode = new NavigationNode(start) { gCost = 0, hCost = Heuristic(start, goal) };
        open[start] = startNode;

        while (open.Any())
        {
            var current = open.Values.OrderBy(n => n.fCost).First();

            if (current.pos == goal)
                return ReconstructPath(current);

            open.Remove(current.pos);
            closed.Add(current.pos);

            foreach (var nb in Get8Neighbors(current.pos))
            {
                if (!IsWalkable(nb.pos) || closed.Contains(nb.pos))
                    continue;

                float gTentative = current.gCost + nb.cost;
                if (!open.TryGetValue(nb.pos, out var neighborNode))
                {
                    neighborNode = new NavigationNode(nb.pos);
                    open[nb.pos] = neighborNode;
                }
                else if (gTentative >= neighborNode.gCost)
                    continue;

                neighborNode.gCost = gTentative;
                neighborNode.hCost = Heuristic(nb.pos, goal);
                neighborNode.parent = current;
            }
        }

        return null; // no path
    }

    private List<Vector3Int> ReconstructPath(NavigationNode end)
    {
        var path = new List<Vector3Int>();
        for (var n = end; n != null; n = n.parent)
            path.Add(n.pos);
        path.Reverse();
        return path;
    }
}
