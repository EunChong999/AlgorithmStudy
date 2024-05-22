using UnityEngine;

public class Node
{
    public Vector3 position; // ����� ��ġ
    public float cost; // ���� ������ ���� �������� ���
    public Node parent; // ���� ���

    public Node(Vector3 pos)
    {
        position = pos;
        cost = Mathf.Infinity;
        parent = null;
    }
}
