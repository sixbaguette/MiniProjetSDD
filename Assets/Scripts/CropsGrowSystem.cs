using UnityEngine;

public class CropsGrowSystem : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float growTime;

    public bool isGrowing = false;

    private Vector3 startScale = new Vector3(0.1f, 0.1f, 0.1f);
    private Vector3 endScale = Vector3.one;

    private void Start()
    {
        growTime = MoneySystem.Instance.dicoProduit[gameObject.tag].GrowTime;

        transform.localScale = startScale;
    }

    private void Update()
    {
        if (!isGrowing) return;

        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTime / growTime);

        transform.localScale = Vector3.Lerp(startScale, endScale, t);

        if (t >= 1f)
        {
            isGrowing = false;
        }
    }
}
