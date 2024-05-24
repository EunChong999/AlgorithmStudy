using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
    public float step_cost;
    public float waitTime;

    public List<Node> openList = new List<Node>();
    public List<Node> closeList = new List<Node>();

    public Node curNode;

    WaitForSeconds waitForSeconds;

    private void Start()
    {
        waitForSeconds = new WaitForSeconds(waitTime);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindPath(NodeManager.instance.startNode);
        }
    }

    private void FindPath(Node start)
    {
       curNode = start;
       StartCoroutine(RunPathFInd(curNode, curNode.transform.position));
    }

    IEnumerator RunPathFInd(Node parent, Vector3 pos)
    {
        // �θ� ��带 ���� closeList�� �߰�
        closeList.Add(parent);
        parent.isClosed = true;

        foreach (Transform t in NodeManager.instance.nodeTransforms)
        {
            Node n = t.GetComponent<Node>();

            if (!n.isObs && !n.isClosed)
            {
                // ��ǥ ���� ��ġ�ϸ� ��� �߰�
                if (pos.x == t.position.x + 1 && pos.y == t.position.y)
                {
                    // �ߺ� �߰� ����
                    if (!openList.Contains(n) && !closeList.Contains(n))
                    {
                        if (n == NodeManager.instance.endNode)
                        {
                            parent.VisualizePath();
                            yield break;
                        }

                        n.parentNode = parent;
                        openList.Add(n);
                        n.isOpen = true;
                        n.g_cost = curNode.g_cost + step_cost;
                    }
                }
                // ������
                else if (pos.x == t.position.x - 1 && pos.y == t.position.y)
                {
                    // �ߺ� �߰� ����
                    if (!openList.Contains(n) && !closeList.Contains(n))
                    {
                        if (n == NodeManager.instance.endNode)
                        {
                            parent.VisualizePath();
                            yield break;
                        }

                        n.parentNode = parent;
                        openList.Add(n);
                        n.isOpen = true;
                        n.g_cost = curNode.g_cost + step_cost;
                    }
                }
                // ��
                else if (pos.x == t.position.x && pos.y == t.position.y - 1)
                {
                    // �ߺ� �߰� ����
                    if (!openList.Contains(n) && !closeList.Contains(n))
                    {
                        if (n == NodeManager.instance.endNode)
                        {
                            parent.VisualizePath();
                            yield break;
                        }

                        n.parentNode = parent;
                        openList.Add(n);
                        n.isOpen = true;
                        n.g_cost = curNode.g_cost + step_cost;
                    }
                }
                // �Ʒ�
                else if (pos.x == t.position.x && pos.y == t.position.y + 1)
                {
                    // �ߺ� �߰� ����
                    if (!openList.Contains(n) && !closeList.Contains(n))
                    {
                        if (n == NodeManager.instance.endNode)
                        {
                            parent.VisualizePath();
                            yield break;
                        }

                        n.parentNode = parent;
                        openList.Add(n);
                        n.isOpen = true;
                        n.g_cost = curNode.g_cost + step_cost;
                    }
                }
            }
        }

        // �� �̻� ���� ��尡 ������ ����
        if (openList.Count <= 0)
        {
            yield break;
        }

        // ���� ����� ��带 ����
        Node n1 = openList[0];

        foreach (Node n2 in openList)
        {
            if (n2.g_cost < n1.g_cost)
            {
                n1 = n2;
            }
        }

        // ������ ��带 openList�� closeList���� ����
        openList.Remove(n1);

        // ���� ��带 üũ�ϱ� ���� ��� ȣ��
        yield return waitForSeconds;
        StartCoroutine(RunPathFInd(n1, n1.transform.position));
    }
}
