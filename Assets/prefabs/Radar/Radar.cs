using UnityEngine;
using UnityEngine.UI;
public class Radar : MonoBehaviour
{
    public float insideRadarDistance = 20;
    public float blipSizePercentage = 5;
    public GameObject rawImageBlipEnemy;
    public GameObject rawImageBlipItems;
    private RawImage rawImageRadarBackground;
    private Transform playerTransform;
    private float radarWidth;
    private float radarHeight;
    private float blipHeight;
    private float blipWidth;
    void Start()
    {
        rawImageRadarBackground = GetComponent<RawImage>();
        playerTransform =
       GameObject.FindGameObjectWithTag("Player").transform;
        radarWidth =
       rawImageRadarBackground.rectTransform.rect.width;
        radarHeight =
       rawImageRadarBackground.rectTransform.rect.height;
        blipHeight = radarHeight * blipSizePercentage / 100;
        blipWidth = radarWidth * blipSizePercentage / 100;
    }
    void Update()
    {
        RemoveAllBlips();
        FindAndDisplayBlipsForTag("Enemy", rawImageBlipEnemy);
        FindAndDisplayBlipsForTag("Items",
       rawImageBlipItems);
    }
    private void FindAndDisplayBlipsForTag(string tag,
   GameObject prefabBlip)
    {
        Vector3 playerPos = playerTransform.position;
        GameObject[] targets =
       GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject target in targets)
        {
            Vector3 targetPos = target.transform.position;
            float distanceToTarget =
           Vector3.Distance(targetPos, playerPos);
            if ((distanceToTarget <= insideRadarDistance))
                CalculateBlipPositionAndDrawBlip(playerPos,
               targetPos, prefabBlip);
        }
    }
    private void CalculateBlipPositionAndDrawBlip(Vector3
   playerPos, Vector3 targetPos, GameObject prefabBlip)
    {
        Vector3 normalisedTargetPosition =
       NormaizedPosition(playerPos, targetPos);
        Vector2 blipPosition =
       CalculateBlipPosition(normalisedTargetPosition);
        DrawBlip(blipPosition, prefabBlip);
    }
    public void RemoveAllBlips()
    {
        GameObject[] blips =
       GameObject.FindGameObjectsWithTag("Blip");
        foreach (GameObject blip in blips)
            Destroy(blip);
    }
    private Vector3 NormaizedPosition(Vector3 playerPos,
   Vector3 targetPos)
    {
        float normalisedyTargetX = (targetPos.x -
       playerPos.x) / insideRadarDistance;
        float normalisedyTargetZ = (targetPos.z -
       playerPos.z) / insideRadarDistance;
        return new Vector3(normalisedyTargetX, 0,
normalisedyTargetZ);
    }
    private Vector2 CalculateBlipPosition(Vector3 targetPos)
    {
        float angleToTarget = Mathf.Atan2(targetPos.x,
       targetPos.z) * Mathf.Rad2Deg;
        float anglePlayer = playerTransform.eulerAngles.y;
        float angleRadarDegrees = angleToTarget - anglePlayer
       - 90;
        float normalizedDistanceToTarget =
       targetPos.magnitude;
        float angleRadians = angleRadarDegrees *
       Mathf.Deg2Rad;
        float blipX = normalizedDistanceToTarget *
       Mathf.Cos(angleRadians);
        float blipY = normalizedDistanceToTarget *
       Mathf.Sin(angleRadians);
        blipX *= radarWidth / 2;
        blipY *= radarHeight / 2;
        blipX += radarWidth / 2;
        blipY += radarHeight / 2;
        return new Vector2(blipX, blipY);
    }
    private void DrawBlip(Vector2 pos, GameObject blipPrefab)
    {
        GameObject blipGO =
       (GameObject)Instantiate(blipPrefab);
        blipGO.transform.SetParent(transform.parent);
        RectTransform rt =
       blipGO.GetComponent<RectTransform>();
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
        pos.x, blipWidth);
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,
        pos.y, blipHeight);
    }
}

