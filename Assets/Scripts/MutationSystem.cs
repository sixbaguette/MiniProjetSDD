using UnityEngine;

public class MutationSystem : MonoBehaviour
{
    public CropsData cropsData;
    private float mutationChance;

    public bool isMutated = false;

    public bool plantee = false;

    public ParticleSystem particleSystem;

    public void IfPlantee()
    {
        if (plantee)
        {
            mutationChance = cropsData.mutationChance;

            Mutation();
        }
    }

    public void Mutation()
    {
        float random = UnityEngine.Random.Range(0f, 1f);

        if (random <= mutationChance)
        {
            isMutated = true;

            particleSystem.Play();
        }
    }
}
