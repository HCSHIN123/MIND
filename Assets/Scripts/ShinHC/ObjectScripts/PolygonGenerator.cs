using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PolygonGenerator : MonoBehaviour
{
    private Mesh mesh; // �ٰ����� �޽�
    //private List<Vector3> vertices = new(); // �ٰ����� ������
    private int[] indices; // ������ �մ� �ﰢ�� ����
    private MeshCollider meshCollider; // �ٰ����� �浹ü

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
        
        indices = DrawFilledIndices(_vertices.Count);  // �ﰢ�� �ε����迭 ����
        GeneratePolygon(_vertices.ToArray(), indices);     // ��� ������ �޽��� �����Ͽ� ���������
        meshCollider.sharedMesh = mesh;         // �޽��� �ݶ��̴��� ����
    }

    private int[] DrawFilledIndices(int verticesCount)
    {
        // ������ �մ� �ﰢ���� �����Ͽ� �ε��� �迭 ����
        int triangleCount = verticesCount - 2;
        List<int> indices = new List<int>();
        for (int i = 0; i < triangleCount; ++i)
        {
            indices.Add(0);
            indices.Add(i + 2);
            indices.Add(i + 1);
        }
        return indices.ToArray();
    }

    private void GeneratePolygon(Vector3[] vertices, int[] indices)
    {
        mesh.Clear(); // ���� �޽� ������ �ʱ�ȭ
        mesh.vertices = vertices; // ���� ����
        mesh.triangles = indices; // �ﰢ�� ����
        mesh.RecalculateBounds(); // �޽� ��踦 �ٽ� ����Ͽ� ��Ȯ�� �浹ü ����
        mesh.RecalculateNormals(); // ������ �ٽ� ����Ͽ� ������ ǰ�� ���
    }
}
