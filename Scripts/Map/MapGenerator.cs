using Oculus.Interaction.Surfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Grid {
    // ���� �ϴ��� ���������� �Ѵ�
    private int x;
    private int y;
    private int width;
    private int height;

    public int X { get { return this.x; } }
    public int Y { get { return this.y; } }
    public int Width { get { return this.width; } }
    public int Height { get { return this.height; } }

    public Grid(int x, int y, int width, int height) {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public Vector2 GetPosition(int cellSize) { return new Vector2(x * cellSize, y * cellSize); }

    public Vector2 GetSize(int cellSize) { return new Vector2(width * cellSize, height * cellSize); }
}

public class Node {
    public Node leftNode;
    public Node rightNode;
    public Node parNode;
    public Grid nodeGrid; //�и��� ������ �׸�������
    public Node(Grid nodeGrid) {
        this.nodeGrid = nodeGrid;
    }
}

[System.Serializable]
public class Map {
    public GameObject[] maps;
}

public class MapGenerator : MonoBehaviour {
    [SerializeField] Vector2Int mapSize; //���ϴ� ��ü �׸����� ũ��
    [SerializeField] int cellSize; //�׸��� �� ĭ�� ũ��
    [SerializeField][Range(0, 1)] float minimumDevideRate; //������ �������� �ּ� ����
    [SerializeField][Range(0, 1)] float maximumDivideRate; //������ �������� �ִ� ����
    [SerializeField] int maximumSize; //�� ������ �ִ� �׸��� ũ��
    [SerializeField] Map[] mapPrefabs;
    public GameObject trashBlockTest;
    private Unity.AI.Navigation.NavMeshSurface navMeshSurface;

    public void MapGenStart() {
        navMeshSurface = GetComponent<Unity.AI.Navigation.NavMeshSurface>();
        Node root = new Node(new Grid(0, 0, mapSize.x, mapSize.y)); //��ü �� ũ���� ��Ʈ��带 ����
        Divide(root);
        GenerateRoom(root);
        navMeshSurface.BuildNavMesh();
    }

    public Vector3 GetRandomPos() {
        Vector3 tmp = Vector3.zero;
        tmp.x = Random.Range(transform.position.x, mapSize.x * cellSize);
        tmp.z = Random.Range(transform.position.z, mapSize.y * cellSize);
        tmp.y = 1;
        return tmp;
    }

    public void Divide(Node node) {
        if (node.nodeGrid.Width <= maximumSize && node.nodeGrid.Height <= maximumSize) return;

        //�� ���� ��쿡��

        int maxLength = Mathf.Max(node.nodeGrid.Width, node.nodeGrid.Height);
        //���ο� ������ �� ����� ������, ���ΰ� ��ٸ� �� ��, ��� ���ΰ� �� ��ٸ� ��, �Ʒ��� �����ְ� �� ���̴�.
        int split = Mathf.RoundToInt(Random.Range(maxLength * minimumDevideRate, maxLength * maximumDivideRate));
        //���� �� �ִ� �ִ� ���̿� �ּ� �����߿��� �������� �� ���� ����
        if (node.nodeGrid.Width >= node.nodeGrid.Height) //���ΰ� �� ����� ��쿡�� �� ��� ������ �� ���̸�, �� ��쿡�� ���� ���̴� ������ �ʴ´�.
        {
            node.leftNode = new Node(new Grid(node.nodeGrid.X, node.nodeGrid.Y, split - 1, node.nodeGrid.Height));
            //���� ��忡 ���� ������ 
            //��ġ�� ���� �ϴ� �����̹Ƿ� ������ ������, ���� ���̴� ������ ���� �������� �־��ش�.
            node.rightNode = new Node(new Grid(node.nodeGrid.X + split, node.nodeGrid.Y, node.nodeGrid.Width - split, node.nodeGrid.Height));
            //���� ��忡 ���� ������ 
            //��ġ�� ���� �ϴܿ��� ���������� ���� ���̸�ŭ �̵��� ��ġ�̸�, ���� ���̴� ���� ���α��̿��� ���� ���� ���ΰ��� �� ������ �κ��� �ȴ�.

        } else {
            node.leftNode = new Node(new Grid(node.nodeGrid.X, node.nodeGrid.Y, node.nodeGrid.Width, split - 1));
            node.rightNode = new Node(new Grid(node.nodeGrid.X, node.nodeGrid.Y + split, node.nodeGrid.Width, node.nodeGrid.Height - split));
        }


        node.leftNode.parNode = node; //�ڽĳ����� �θ��带 �������� ���� ����
        node.rightNode.parNode = node;
        Divide(node.leftNode); //����, ������ �ڽ� ���鵵 �����ش�.
        Divide(node.rightNode);//����, ������ �ڽ� ���鵵 �����ش�.
    }

    int cnt = 1;
    public void GenerateRoom(Node node) {

        if (node.leftNode == null && node.rightNode == null) //�ش� ��尡 ��������� ���� ����� �� ���̴�.
        {
            if (node.nodeGrid.Width > 1 && node.nodeGrid.Height > 1) { // ����� ũ�Ⱑ �ּ� ������ �̻��� ���
                Debug.Log("<<" + cnt + ">> Width : " + node.nodeGrid.Width + ", Height : " + node.nodeGrid.Height);
                GameObject prefab = mapPrefabs[node.nodeGrid.Width - 2].maps[node.nodeGrid.Height - 2];
                Vector3 position = new Vector3(node.nodeGrid.GetPosition(cellSize).x + transform.position.x, 0f, node.nodeGrid.GetPosition(cellSize).y + transform.position.z);
                GameObject tmp_g = Instantiate(prefab, position, Quaternion.identity, transform);
                tmp_g.transform.name = cnt.ToString();
                cnt++;
            } else { // ����� ũ�Ⱑ �ּ� ������ ������ ���
                Vector3 position = new Vector3(node.nodeGrid.GetPosition(cellSize).x + transform.position.x, 0f, node.nodeGrid.GetPosition(cellSize).y + transform.position.z);
                for (int i = 0; i < node.nodeGrid.Height; i++) {
                    for (int j = 0; j < node.nodeGrid.Width; j++) {
                        Vector3 tmp = position + new Vector3(j * cellSize, 0f, i * cellSize);
                        Instantiate(trashBlockTest, tmp, Quaternion.identity, transform);
                    }
                }
            }
        } else {
            GenerateRoom(node.leftNode);
            GenerateRoom(node.rightNode);
        }
    }
}