using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))] // 닿으면 이동하도록 해주는 BoxCollider
public class Portal : MonoBehaviour
{
    // 다른 포탈(반대편 포탈)
    [field: SerializeField]
    public Portal otherPortal { get; private set; }
    // 포탈 외각선 랜더러(색상)
    [SerializeField] private Renderer outlineRenderer;
    // 포탈 색상
    [field: SerializeField]
    public Color portalColor { get; private set; }

    // 설치 가능 레이어
    [SerializeField][Tooltip("포탈 설치 가능한 레이어")] private LayerMask placementMask;

    // 포탈에 들어온 오브젝트를 저장하는 리스트
    private List<PortalableObject> portalObjects = new List<PortalableObject>();

    // 현재 생성되어있는지 확인하는 변수
    public bool isPlaced { get; private set; } = false;
    private Collider wallCollider;

    public Renderer renderer { get; private set; }
    private BoxCollider collider;

    // 초기화
    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        renderer = GetComponent<Renderer>();
    }

    // 색상을 정해주고 비활성화 시켜 둔다.
    private void Start()
    {
        outlineRenderer.material.SetColor("_OutlineColor", portalColor);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        // 포탈에 들어온 오브젝트 갯숩만큼 반복한다.
        for (int i = 0; i < portalObjects.Count; i++)
        {
            // 포탈에 들어온 오브젝트의 월드 좌표를 포탈 기준의 로컬 좌표로 변환한다.
            Vector3 objectPosition = transform.InverseTransformPoint(portalObjects[i].transform.position);

            if (objectPosition.z > 0.0f && portalObjects[i].CanWarp)
            {
                // 포탈을 넘어가면 Warp한다.
                portalObjects[i].Warp();
            }
        }
    }

    // 콜라이더에 닿았을 때
    private void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponent<PortalableObject>();
        // 닿은 오브젝트가 PortalableObject 을 가지고 있을 때 동작
        if (obj != null)
        {
            // 포탈에 닿은 오브젝트 리스트에 추가
            portalObjects.Add(obj);
            obj.SetIsInPortal(this, otherPortal, wallCollider);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var obj = other.GetComponent<PortalableObject>();

        if (portalObjects.Contains(obj))
        {
            portalObjects.Remove(obj);
            obj.ExitPortal(wallCollider);
        }
    }

    public bool PlacePortal(Collider wallCollider, Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        transform.position -= transform.forward * 0.001f;

        FixOverhangs();
        FixIntersects();

        if (CheckOverlap())
        {
            this.wallCollider = wallCollider;
            gameObject.SetActive(true);
            isPlaced = true;
            return true;
        }
        return false;
    }

    public void RemovePortal()
    {
        gameObject.SetActive(false);
        isPlaced = false;
    }

    // Fix이후 최종적으로 포탈이 생성될 위치에 설치가 가능할 지 검사하는 메서드
    private bool CheckOverlap()
    {
        // 포탈 생성을 위해 체크할 크기(반지름만)
        // 실제 포탈의 통과를 담당하는 BoxCollider의 크기가 2, 4 이기 때문에 그것보다 살짝 작게 설정한다.
        var checkExtents = new Vector3(0.9f, 1.9f, 0.05f);
        // [0] : overlapBox의 중앙
        // [1] ~ [4] : LineCast의 시작점(포탈의 네 모서리)
        // [5] : LineCast의 방향과 거리
        var checkPositions = new Vector3[]
        {
            transform.position + transform.TransformVector(new Vector3( 0.0f, 0.0f, -0.1f)), // 포탈의 '뒤쪽'으로 뒤에 'wallCollider와 닿았는지 확인
            transform.position + transform.TransformVector(new Vector3(-1.0f, -2.0f, -0.1f)),// 왼쪽 아래 모서리
            transform.position + transform.TransformVector(new Vector3(-1.0f, 2.0f, -0.1f)), // 왼쪽 위 모서리
            transform.position + transform.TransformVector(new Vector3( 1.0f, -2.0f, -0.1f)),// 오른쪽 아래 모서리
            transform.position + transform.TransformVector(new Vector3( 1.0f, 2.0f, -0.1f)), // 오른쪽 위 모서리
            transform.TransformVector(new Vector3(0.0f, 0.0f, 0.2f))                         // LineCast의 방향과 거리
            // LineCast는 시작점과 끝 점이 필요한데 "끝 점"을 담당하는 역할이다.
        };
        // 포탈과 동일한 크기의 가상 상자를 만들고 그 가상 상자에 들어오는 모든 콜라이더를 찾는다.
        var intersections = Physics.OverlapBox(checkPositions[0], checkExtents, transform.rotation, placementMask);

        // 찾아낸 콜라이더 수가 2개 이상이면(즉, 포탈이 붙은 벽을 제외한 다른 콜라이더가 있다)
        if (intersections.Length > 1) { return false; } // 다른 콜라이더에 의해 방해를 받아 포탈 생성을 실패한다.

        // 만약 찾은 콜라이더 수가 1개일 때
        else if (intersections.Length == 1)
        {
            // 그게 내가 붙으려는 벽이 아니라면
            if (intersections[0] != collider) { return false; } //똑같이 포탈 생성을 실패한다.
        }

        // 네 모서리 검사를 위한 변수
        bool isOverlapping = true;

        // 네 모서리 검사를 실시한다.
        for (int i = 1; i < checkPositions.Length - 1; ++i)
        {
            // 각 모서리와 모서리에서 살짝 앞으로 나간[5] 선을 그어서, 그 선이 placementMask와 만나는지 검사
            isOverlapping &= Physics.Linecast(checkPositions[i],                                 // &= 즉, AND 연산을 통해 하나라도 false가 출력되면 최종 결과가 false가 된다.
                checkPositions[i] + checkPositions[checkPositions.Length - 1], placementMask);
        }
        return isOverlapping; // 결과를 리턴하며, True여야만 포탈이 생성 가능한 위치라는 것이 된다.
    }

    // 포탈의 가장자리를 보정하는 메서드 (벽 가장자리에 포탈이 생겨서 허공에 튀어나오지 않도록 하는 역할)
    private void FixOverhangs()
    {
        // 포탈의 box 콜라이더는 2, 4크기이기 때문에 그것보다 살짝 크게 한다.
        var testPoints = new List<Vector3>
        {
            new Vector3(-1.1f, 0.0f, 0.1f),     // 왼쪽 바깥
            new Vector3( 1.1f, 0.0f, 0.1f),     // 오른쪽 바깥
            new Vector3( 0.0f, -2.1f, 0.1f),    // 아래쪽 바깥
            new Vector3( 0.0f,  2.1f, 0.1f)     // 위쪽 바깥
        };

        // testPoints의 각 지점에서 포탈 안쪽으로 향하는 방향
        var testDirs = new List<Vector3>
        {
            Vector3.right,   // 왼쪽 바깥 -> 오른쪽(안쪽)으로
            -Vector3.right,  // 오른쪽 바깥 -> 왼쪽(안쪽)으로
            Vector3.up,      // 아래쪽 바깥 -> 위쪽(안쪽)으로
            -Vector3.up      // 위쪽 바깥 -> 아래쪽(안쪽)으로
        };

        for (int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = transform.TransformPoint(testPoints[i]); // 포탈의 로컬 좌표인 testPoints를 실제 게임 월드의 절대 좌표로 변경해서 저장
            Vector3 raycastDir = transform.TransformDirection(testDirs[i]); // 위와 동일하며, 광선을 쏠 방향을 저장

            // 각 raycastPos를 검사해서 이미 포탈이 벽 안에 적당한 위치에 존재하는가 확인
            if (Physics.CheckSphere(raycastPos, 0.05f, placementMask)) { break; }
            // 한 지점이라도 벽 밖에 허공에 생성되게 되면, 벽까지의 거리가 얼마나 남았는지 계산
            // 상세 설명: Ray를 쐈을 때, raycastPos 위치에서 raycastDir(포탈의 안쪽 방향)으로 레이저를 2.1f 길이 만큼 쐈을 때 Layer가 벽이 맞다면 포탈의 한 지점이 허공에 생성된 상태
            else if (Physics.Raycast(raycastPos, raycastDir, out hit, 2.1f, placementMask))
            {
                // 실제 Ray가 부딪힌 벽의 월드좌표 - 광선이 출발했던 허공의 월드 좌표. 즉, 포탈이 정확히 허공에 삐져나온 만큼의 값
                var offset = hit.point - raycastPos;
                // 위에서 계산한 offset의 거리만큼 포탈을 이동시킨다.
                transform.Translate(offset, Space.World);
            }
        }
    }

    // 포탈의 모서리를 보정하는 메서드 (안쪽으로 꺾인 벽이나 기둥 같은 곳에 겹치지 않도록 하는 역할)
    private void FixIntersects()
    {
        // 포탈 안쪽에서 바깥으로 뻗어나가는 방향
        var testDirs = new List<Vector3>
        {
            Vector3.right,   // 중심 -> 오른쪽
            -Vector3.right,  // 중심 -> 왼쪽
            Vector3.up,      // 중심 -> 위쪽
            -Vector3.up      // 중심 -> 아래쪽
        };

        // 각 방향에서 중간에 장애물이 없어야 할 최소 안전 거리
        // 포탈의 크기보다 살짝 크다.
        var testDists = new List<float> { 1.1f, 1.1f, 2.1f, 2.1f };

        for (int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            // 포탈 정 중앙에서 안쪽으로 살짝 들어간 위치
            Vector3 raycastPos = transform.TransformPoint(0.0f, 0.0f, -0.1f);
            Vector3 raycastDir = transform.TransformDirection(testDirs[i]);

            // 포탈 중심에서 각 방향으로 Ray를 발사하여 placementMask가 존재하는 지 확인한다.
            // True라면 해당 방향에 벽이 있기 때문에 해당 방향에 포탈이 끼이는 상황이다.
            if (Physics.Raycast(raycastPos, raycastDir, out hit, testDists[i], placementMask))
            {
                // 부딪힌 벽의 표면까지의 거리를 계산한다.
                var offset = (hit.point - raycastPos);
                // offset.magnitude는 가까운 벽까지와의 실제 거리
                // 확보해야할 거리 - 실제 거리를 통해 부족한 거리를 계산
                // 마지막으로 ray의 반대 방향으로 곱해서 최종 보정 벡터를 완성한다.
                var newOffset = -raycastDir * (testDists[i] - offset.magnitude);
                // 보정 벡터의 위치로 포탈을 이동시킨다. 
                transform.Translate(newOffset, Space.World);
            }
        }
    }
}
