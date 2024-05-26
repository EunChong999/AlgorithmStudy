using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Astar : MonoBehaviour
{
    public bool applyHeuristic;
    public bool allowDiagonal;
    public bool crossCorners;

    private float straightStepCost = 1.0f;
    private float diagonalStepCost = Mathf.Sqrt(2.0f);

    private PriorityQueue<Node> openList = new PriorityQueue<Node>();
    private List<Node> closeList = new List<Node>();

    public Node curNode;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindPath(NodeManager.instance.startNode);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void FindPath(Node start)
    {
        curNode = start;

        start.g_cost = 0;

        if (applyHeuristic)
        {
            start.h_cost = CalculateHeuristic(start, NodeManager.instance.endNode);
            start.f_cost = start.g_cost + start.h_cost;
        }

        StartCoroutine(CheckNeighbours(curNode));
    }

    private float CalculateHeuristic(Node node, Node endNode)
    {
        Vector3 nodePos = node.transform.position;
        Vector3 endPos = endNode.transform.position;
        return Vector3.Distance(nodePos, endPos); // Euclidean Distance
    }

    IEnumerator CheckNeighbours(Node parent)
    {
        Vector3 pos = parent.transform.position;
        closeList.Add(parent);
        parent.isClosed = true;

        foreach (Transform t in NodeManager.instance.nodeTransforms)
        {
            Node n = t.GetComponent<Node>();

            if (!n.isObs && !n.isClosed)
            {
                float additionalCost = 0;
                bool isNeighbor = false;

                // �����¿� �� �밢�� ���� �˻�
                if (Mathf.Approximately(pos.x, t.position.x + 1) && Mathf.Approximately(pos.y, t.position.y))
                {
                    isNeighbor = true;
                    additionalCost = straightStepCost; // ������
                }
                else if (Mathf.Approximately(pos.x, t.position.x - 1) && Mathf.Approximately(pos.y, t.position.y))
                {
                    isNeighbor = true;
                    additionalCost = straightStepCost; // ����
                }
                else if (Mathf.Approximately(pos.x, t.position.x) && Mathf.Approximately(pos.y, t.position.y + 1))
                {
                    isNeighbor = true;
                    additionalCost = straightStepCost; // ��
                }
                else if (Mathf.Approximately(pos.x, t.position.x) && Mathf.Approximately(pos.y, t.position.y - 1))
                {
                    isNeighbor = true;
                    additionalCost = straightStepCost; // �Ʒ�
                }
                else if (allowDiagonal)
                {
                    if (Mathf.Approximately(pos.x, t.position.x + 1) && Mathf.Approximately(pos.y, t.position.y + 1))
                    {
                        bool isLeftObs = false;
                        bool isBelowObs = false;
                        isLeftObs = NodeManager.instance.nodeTransforms.Find(x => Mathf.Approximately(x.position.x, pos.x - 1) && Mathf.Approximately(x.position.y, pos.y))?.GetComponent<Node>().isObs ?? true;
                        isBelowObs = NodeManager.instance.nodeTransforms.Find(x => Mathf.Approximately(x.position.x, pos.x) && Mathf.Approximately(x.position.y, pos.y - 1))?.GetComponent<Node>().isObs ?? true;

                        if (crossCorners)
                        {
                            if (!isLeftObs || !isBelowObs)
                            {
                                isNeighbor = true;
                                additionalCost = diagonalStepCost; // ������ �� �밢��
                            }
                        }
                        else
                        {
                            if (!isLeftObs && !isBelowObs)
                            {
                                isNeighbor = true;
                                additionalCost = diagonalStepCost; // ������ �� �밢��
                            }
                        }
                    }
                    else if (Mathf.Approximately(pos.x, t.position.x + 1) && Mathf.Approximately(pos.y, t.position.y - 1))
                    {
                        bool isAboveObs = false;
                        bool isLeftObs = false;
                        isAboveObs = NodeManager.instance.nodeTransforms.Find(x => Mathf.Approximately(x.position.x, pos.x) && Mathf.Approximately(x.position.y, pos.y + 1))?.GetComponent<Node>().isObs ?? true;
                        isLeftObs = NodeManager.instance.nodeTransforms.Find(x => Mathf.Approximately(x.position.x, pos.x - 1) && Mathf.Approximately(x.position.y, pos.y))?.GetComponent<Node>().isObs ?? true;

                        if (crossCorners)
                        {
                            if (!isAboveObs || !isLeftObs)
                            {
                                isNeighbor = true;
                                additionalCost = diagonalStepCost; // ������ �Ʒ� �밢��
                            }
                        }
                        else
                        {
                            if (!isAboveObs && !isLeftObs)
                            {
                                isNeighbor = true;
                                additionalCost = diagonalStepCost; // ������ �Ʒ� �밢��
                            }
                        }
                    }
                    else if (Mathf.Approximately(pos.x, t.position.x - 1) && Mathf.Approximately(pos.y, t.position.y + 1))
                    {
                        bool isRightObs = false;
                        bool isBelowObs = false;
                        isRightObs = NodeManager.instance.nodeTransforms.Find(x => Mathf.Approximately(x.position.x, pos.x + 1) && Mathf.Approximately(x.position.y, pos.y))?.GetComponent<Node>().isObs ?? true;
                        isBelowObs = NodeManager.instance.nodeTransforms.Find(x => Mathf.Approximately(x.position.x, pos.x) && Mathf.Approximately(x.position.y, pos.y - 1))?.GetComponent<Node>().isObs ?? true;

                        if (crossCorners)
                        {
                            if (!isRightObs || !isBelowObs)
                            {
                                isNeighbor = true;
                                additionalCost = diagonalStepCost; // ���� �� �밢��
                            }
                        }
                        else
                        {
                            if (!isRightObs && !isBelowObs)
                            {
                                isNeighbor = true;
                                additionalCost = diagonalStepCost; // ���� �� �밢��
                            }
                        }
                    }
                    else if (Mathf.Approximately(pos.x, t.position.x - 1) && Mathf.Approximately(pos.y, t.position.y - 1))
                    {
                        bool isRightObs = false;
                        bool isAboveObs = false;
                        isRightObs = NodeManager.instance.nodeTransforms.Find(x => Mathf.Approximately(x.position.x, pos.x + 1) && Mathf.Approximately(x.position.y, pos.y))?.GetComponent<Node>().isObs ?? true;
                        isAboveObs = NodeManager.instance.nodeTransforms.Find(x => Mathf.Approximately(x.position.x, pos.x) && Mathf.Approximately(x.position.y, pos.y + 1))?.GetComponent<Node>().isObs ?? true;

                        if (crossCorners)
                        {
                            if (!isRightObs || !isAboveObs)
                            {
                                isNeighbor = true;
                                additionalCost = diagonalStepCost; // ���� �Ʒ� �밢��
                            }
                        }
                        else
                        {
                            if (!isRightObs && !isAboveObs)
                            {
                                isNeighbor = true;
                                additionalCost = diagonalStepCost; // ���� �Ʒ� �밢��
                            }
                        }
                    }
                }

                if (isNeighbor && !openList.Contains(n))
                {
                    if (n == NodeManager.instance.endNode)
                    {
                        parent.VisualizePath();
                        yield break;
                    }

                    n.parentNode = parent;

                    n.g_cost = parent.g_cost + additionalCost;

                    if (applyHeuristic)
                    {
                        n.h_cost = Vector3.Distance(n.transform.position, NodeManager.instance.endNode.transform.position); // Heuristic cost
                        n.f_cost = n.g_cost + n.h_cost; // Total cost
                        openList.Enqueue(n, n.f_cost);
                    }
                    else
                    {
                        openList.Enqueue(n, parent.g_cost + additionalCost);
                    }

                    n.isOpen = true;
                }
            }
        }

        // �� �̻� ���� ��尡 ������ ����
        if (openList.Count <= 0)
        {
            yield break;
        }

        // ���� ���� f_cost�� ���� ��带 ����
        Node lowestN = openList.Dequeue();

        // ���� ��带 üũ�ϱ� ���� ��� ȣ��
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(CheckNeighbours(lowestN));
    }
}
