using UnityEngine;

public class CropsGrowSystem : MonoBehaviour
{
    private float time = 0;
    private float growTime = 0;

    public bool isGrowing = false;

    private void Update()
    {
        time = Time.deltaTime;

        CropsGrowing(gameObject.tag);
    }

    private void CropsGrowing(string produit)
    {
        growTime = MoneySystem.Instance.dicoProduit[produit].GrowTime;

        growTime /= 360f;

        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3(1, 1, 1), growTime * time);
    }
}
