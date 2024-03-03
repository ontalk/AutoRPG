using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    public float rate;//����
    public float damage;//������
    public float CriticalDamage; //CriticalDamage = damage *2;
    public BoxCollider Area;//����
    public TrailRenderer Effect;//ȿ�� ����

    
    private void Start()
    {
        Use();
        damage = GameManager.Instance.curDamage;
        CriticalDamage = damage * 2f;
    }

    private void Update()
    {
       
    }
    


     public void Use()
     {
         if (gameObject)
         {
             StopCoroutine("Swing");
             StartCoroutine("Swing");
         }

     }


     IEnumerator Swing() //������ �Լ� Ŭ����
     {

         //yield return null;//yield ����� �����ϴ� Ű���� //1������ ���
         yield return new WaitForSeconds(0.01f);//0.45�� ���
         Area.enabled = true;

         yield return new WaitForSeconds(0.2f);
         Area.enabled = false;
     }
}
