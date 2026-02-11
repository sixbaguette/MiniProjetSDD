using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    public GameObject thunderstorm;
    public GameObject moon;
    public GameObject rain;
    public GameObject listPanel;

    public bool rainbool = false;
    public bool thunderstormbool = false;

    private bool isRunning = false;

    private float time;

    private Queue<IEnumerator> queueEvent = new Queue<IEnumerator>();

    private void Start()
    {
        queueEvent.Enqueue(ThunderStorm());
        queueEvent.Enqueue(Rain());
        queueEvent.Enqueue(Moon());
    }

    private void Update()
    {
        time += Time.deltaTime;

        WorldEvent();
    }

    private void WorldEvent()
    {
        if (time >= 20)
        {
            time = 0;

            if (!isRunning)
            {
                StartCoroutine(queueEvent.Dequeue());
            }
        }
    }
    public IEnumerator ThunderStorm()
    {
        Instantiate(thunderstorm, listPanel.transform);

        isRunning = true;

        thunderstormbool = true;

        MutationSystem[] charges = GameObject.FindObjectsOfType<MutationSystem>();

        foreach (MutationSystem charge in charges)
        {
            charge.GetComponent<MutationSystem>().ThunderCharge();
        }

        yield return new WaitForSeconds(10);

        thunderstormbool = false;

        queueEvent.Enqueue(ThunderStorm());

        Destroy(listPanel.transform.GetChild(0).gameObject);

        isRunning = false;
    }

    public IEnumerator Moon()
    {
        Instantiate(moon, listPanel.transform);

        isRunning = true;

        MutationSystem[] mutations = GameObject.FindObjectsOfType<MutationSystem>();

        foreach (MutationSystem mutation in mutations)
        {
            mutation.GetComponent<MutationSystem>().MutationMoon();
        }

        yield return new WaitForSeconds(10);

        queueEvent.Enqueue(Moon());

        Destroy(listPanel.transform.GetChild(0).gameObject);

        isRunning = false;
    }

    public IEnumerator Rain()
    {
        Instantiate(rain, listPanel.transform);

        isRunning = true;

        rainbool = true;

        yield return new WaitForSeconds(10);

        rainbool = false;

        queueEvent.Enqueue(Rain());

        Destroy(listPanel.transform.GetChild(0).gameObject);

        isRunning = false;
    }
}
