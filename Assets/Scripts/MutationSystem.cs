using UnityEngine;

public class MutationSystem : MonoBehaviour
{
    public CropsData cropsData;
    private float mutationChance;

    public bool isMutated = false;

    public bool asMoonMutation = false;

    public bool isCharged = false;
    public int chargedAmount = 0;

    public bool plantee = false;

    public ParticleSystem particleSystemMutation;
    public ParticleSystem particleSystemMoonMutation;

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

            //particleSystemMutation.Play();
        }
    }

    public void MutationMoon()
    {
        int random = Random.Range(0, 4);

        if (random == 0)
        {
            asMoonMutation = true;

            //particleSystemMoonMutation.Play();
        }
    }

    public void ThunderCharge()
    {
        int random = Random.Range(0, 26);

        if (random == 0)
        {
            isCharged = true;

            chargedAmount++;
        }
    }
}
