using Oculus.Interaction.Surfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Grid {
    // 좌측 하단을 기준점으로 한다
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
    public Grid nodeGrid; //분리된 공간의 그리드정보
    public Node(Grid nodeGrid) {
        this.nodeGrid = nodeGrid;
    }
}

[System.Serializable]
public class Map {
    public GameObject[] maps;
}

public class MapGenerator : MonoBehaviour {
    [SerializeField] Vector2Int mapSize; //원하는 전체 그리드의 크기
    [SerializeField] int cellSize; //그리드 한 칸의 크기
    [SerializeField][Range(0, 1)] float minimumDevideRate; //공간이 나눠지는 최소 비율
    [SerializeField][Range(0, 1)] float maximumDivideRate; //공간이 나눠지는 최대 비율
    [SerializeField] int maximumSize; //한 공간의 최대 그리드 크기
    [SerializeField] Map[] mapPrefabs;
    public GameObject trashBlockTest;
    private Unity.AI.Navigation.NavMeshSurface navMeshSurface;

    public void MapGenStart() {
        navMeshSurface = GetComponent<Unity.AI.Navigation.NavMeshSurface>();
        Node root = new Node(new Grid(0, 0, mapSize.x, mapSize.y)); //전체 맵 크기의 루트노드를 만듬
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

        //그 외의 경우에는

        int maxLength = Mathf.Max(node.nodeGrid.Width, node.nodeGrid.Height);
        //가로와 세로중 더 긴것을 구한후, 가로가 길다면 위 좌, 우로 세로가 더 길다면 위, 아래로 나눠주게 될 것이다.
        int split = Mathf.RoundToInt(Random.Range(maxLength * minimumDevideRate, maxLength * maximumDivideRate));
        //나올 수 있는 최대 길이와 최소 길이중에서 랜덤으로 한 값을 선택
        if (node.nodeGrid.Width >= node.nodeGrid.Height) //가로가 더 길었던 경우에는 좌 우로 나누게 될 것이며, 이 경우에는 세로 길이는 변하지 않는다.
        {
            node.leftNode = new Node(new Grid(node.nodeGrid.X, node.nodeGrid.Y, split - 1, node.nodeGrid.Height));
            //왼쪽 노드에 대한 정보다 
            //위치는 좌측 하단 기준이므로 변하지 않으며, 가로 길이는 위에서 구한 랜덤값을 넣어준다.
            node.rightNode = new Node(new Grid(node.nodeGrid.X + split, node.nodeGrid.Y, node.nodeGrid.Width - split, node.nodeGrid.Height));
            //우측 노드에 대한 정보다 
            //위치는 좌측 하단에서 오른쪽으로 가로 길이만큼 이동한 위치이며, 가로 길이는 기존 가로길이에서 새로 구한 가로값을 뺀 나머지 부분이 된다.

        } else {
            node.leftNode = new Node(new Grid(node.nodeGrid.X, node.nodeGrid.Y, node.nodeGrid.Width, split - 1));
            node.rightNode = new Node(new Grid(node.nodeGrid.X, node.nodeGrid.Y + split, node.nodeGrid.Width, node.nodeGrid.Height - split));
        }


        node.leftNode.parNode = node; //자식노드들의 부모노드를 나누기전 노드로 설정
        node.rightNode.parNode = node;
        Divide(node.leftNode); //왼쪽, 오른쪽 자식 노드들도 나눠준다.
        Divide(node.rightNode);//왼쪽, 오른쪽 자식 노드들도 나눠준다.
    }

    int cnt = 1;
    public void GenerateRoom(Node node) {

        if (node.leftNode == null && node.rightNode == null) //해당 노드가 리프노드라면 방을 만들어 줄 것이다.
        {
            if (node.nodeGrid.Width > 1 && node.nodeGrid.Height > 1) { // 노드의 크기가 최소 사이즈 이상일 경우
                Debug.Log("<<" + cnt + ">> Width : " + node.nodeGrid.Width + ", Height : " + node.nodeGrid.Height);
                GameObject prefab = mapPrefabs[node.nodeGrid.Width - 2].maps[node.nodeGrid.Height - 2];
                Vector3 position = new Vector3(node.nodeGrid.GetPosition(cellSize).x + transform.position.x, 0f, node.nodeGrid.GetPosition(cellSize).y + transform.position.z);
                GameObject tmp_g = Instantiate(prefab, position, Quaternion.identity, transform);
                tmp_g.transform.name = cnt.ToString();
                cnt++;
            } else { // 노드의 크기가 최소 사이즈 이하일 경우
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