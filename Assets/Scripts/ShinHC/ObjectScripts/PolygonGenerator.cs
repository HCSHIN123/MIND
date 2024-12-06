using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PolygonGenerator : MonoBehaviour
{
    private Mesh mesh; // �ٰ����� �޽�
    private int[] indices; // ������ �մ� �ﰢ�� ����
    private MeshCollider meshCollider; // �ٰ����� �浹ü

    private List<int> indicesTemp = new List<int>();
    private void Awake()
    {
        mesh = new Mesh();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;
        meshCollider.isTrigger = true;
    }

    private void Update()
    {
        transform.rotation = Quaternion.identity; // ȸ������ �ʱ�ȭ�Ͽ� �������� �׸���
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
            Debug.Log("�׸��ڿ� ����");
    }

    public void DrawPolygon(List<Vector3> _vertices)
    {
        // �ﰢ�� �ε��� �迭�� �����Ͽ� ������ �׸��� �غ�
        DrawFilledIndices(_vertices.Count);

        // ���� ������ �������� �������� �����ϰ� �̸� �޽��� ����
        GeneratePolygon(_vertices.ToArray(), indices);

        // ������ �޽��� �ݶ��̴��� �����Ͽ� ������ �浹 ó���� �����ϰ� ��
        meshCollider.sharedMesh = mesh;
    }

    private void DrawFilledIndices(int verticesCount)
    {
        indicesTemp.Clear();    // ���� �ӽ��ε��� ���� �ʱ�ȭ
        int triangleCount = verticesCount - 2;
        // �ﰢ���� �׸��� ���� �ε������� ����
        for (int i = 0; i < triangleCount; ++i)
        {
            indicesTemp.Add(0);
            indicesTemp.Add(i + 2);
            indicesTemp.Add(i + 1);
        }
        indices = indicesTemp.ToArray(); // �迭 ���·� �ε������� ���� ����
    }

    private void GeneratePolygon(Vector3[] vertices, int[] indices)
    {
        mesh.Clear(); // ���� �޽� ������ �ʱ�ȭ
        mesh.vertices = vertices; // ���� ���� ����
        mesh.triangles = indices; // �ε��� ���� ����
    }
}
