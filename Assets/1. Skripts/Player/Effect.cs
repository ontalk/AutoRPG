using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public GameObject AtkPrefab;
    private GameObject atkObject;
    public float duration;
    AtkPetSkill skill;
    private Transform Target;

    private void Awake()
    {
        skill = GetComponent<AtkPetSkill>();
        Target = GameObject.FindGameObjectWithTag("Player").transform;

    }
    private void Update()
    {
        if (atkObject != null)
        {
            atkObject.transform.position = Target.position;
        }
    }
    public void StartAtkCoroutine()
    {
        StartCoroutine(AtkEffect());
    }
    public IEnumerator AtkEffect()
    {
        atkObject = Instantiate(AtkPrefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(duration);

        DestroyAtkEffectObject();
    }

    void DestroyAtkEffectObject()
    {
        // Check if the healing object exists before trying to destroy it
        if (atkObject != null)
        {
            Destroy(atkObject);
        }
    }
}
