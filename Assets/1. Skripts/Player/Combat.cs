using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Combat : MonoBehaviour
{
    private Animator anim;
    public int noOfLClicks = 0;
    public int noOfRClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1.5f;
    public bool isAttacking = false;
    public VisualEffect swordEffect; // 검기 이펙트를 연결할 public 변수
    public List<Slash> slashes;
    public Weapon equipWeapon;
    // Start is called before the first frame update
    void Start()
    {
        DisableSlashes();
        anim = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        equipWeapon = GetComponentInChildren<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfLClicks = 0;
            noOfRClicks = 0;
            anim.SetBool("LAtk1", false);
            anim.SetBool("HAtk9", false);
            anim.SetBool("HAtk1", false);
        }

        //left clicks
        if (Input.GetMouseButtonDown(0))
        {
            
            lastClickedTime = Time.time;
            noOfLClicks++;
            if (noOfLClicks == 1)
            {
                equipWeapon.Use();
                isAttacking = true;
                anim.SetBool("LAtk1", true);
                StartCoroutine(SlashAttack());
                PlaySwordEffect();
            }
            noOfLClicks = Mathf.Clamp(noOfLClicks, 0, 5);
        }

        //right clicks
        if (Input.GetMouseButtonDown(1))
        {
            
            lastClickedTime = Time.time;
            noOfRClicks++;
            if (noOfRClicks == 1 && noOfLClicks == 1)
            {
                isAttacking = true;
                equipWeapon.Use();
                anim.SetBool("HAtk4", true);
                StartCoroutine(SlashAttack());
            }
            if (noOfRClicks == 1)
            {
                isAttacking = true;
                equipWeapon.Use();
                anim.SetBool("HAtk1", true);
                StartCoroutine(SlashAttack());
            }
            noOfRClicks = Mathf.Clamp(noOfRClicks, 0, 5);
        }


        if (noOfLClicks == 0 && noOfRClicks == 0)
        {
            isAttacking = false;
        }
        else
        {
            isAttacking = true;
        }
    }
    IEnumerator SlashAttack()
     {
         for (int i = 0; i < slashes.Count; i++)
         {
             yield return new WaitForSeconds(slashes[i].delay);
             slashes[i].slashObj.SetActive(true);
         }
         yield return new WaitForSeconds(1);
         DisableSlashes();
         isAttacking = false;
     }
 void DisableSlashes()
 {
     for (int i = 0; i < slashes.Count; i++)
         slashes[i].slashObj.SetActive(false);
 }
    public void returnLatk1()
    {
        if (noOfLClicks >= 2)
        {
            equipWeapon.Use();
            anim.SetBool("LAtk2", true);
            PlaySwordEffect();
            if (noOfRClicks == 1)
            {
                equipWeapon.Use();
                anim.SetBool("HAtk6", true);
                PlaySwordEffect();
            }
        }
        else
        {
            anim.SetBool("LAtk2", false);
            anim.SetBool("LAtk1", false);
            noOfLClicks = 0;
        }
    }
    public void returnLatk2()
    {
        if (noOfLClicks >= 3)
        {
            equipWeapon.Use();
            anim.SetBool("LAtk3", true);
            PlaySwordEffect();
            if (noOfRClicks == 1)
            {
                equipWeapon.Use();
                anim.SetBool("HAtk9", true);
                PlaySwordEffect();
            }
        }
        else
        {
            anim.SetBool("LAtk3", false);
            anim.SetBool("LAtk2", false);
            anim.SetBool("HAtk9", false);
            noOfLClicks = 0;
        }
    }
    public void returnLatk3()
    {
        if (noOfLClicks >= 4)
        {
            equipWeapon.Use();
            anim.SetBool("LAtk4", true);
            PlaySwordEffect();
        }
        else
        {
            anim.SetBool("LAtk4", false);
            anim.SetBool("LAtk3", false);
            noOfLClicks = 0;
        }
    }
    public void returnLatk4()
    {
        if (noOfLClicks >= 5)
        {
            equipWeapon.Use();
            anim.SetBool("LAtk5", true);
            PlaySwordEffect();
        }
        else
        {
            anim.SetBool("LAtk5", false);
            anim.SetBool("LAtk4", false);
            noOfLClicks = 0;
        }
    }
    public void returnLatk5()
    {
        anim.SetBool("LAtk1", false);
        anim.SetBool("LAtk2", false);
        anim.SetBool("LAtk3", false);
        anim.SetBool("LAtk4", false);
        anim.SetBool("LAtk5", false);
        noOfLClicks = 0;
    }
    public void returnHatk4()
    {
        if (noOfRClicks >= 2)
        {
            equipWeapon.Use();
            anim.SetBool("HAtk5", true);
            PlaySwordEffect();
        }
        else
        {
            anim.SetBool("HAtk5", false);
            anim.SetBool("HAtk4", false);
            noOfRClicks = 0;
        }
    }
    public void returnHatk5()
    {
        anim.SetBool("HAtk4", false);
        anim.SetBool("HAtk5", false);
        noOfRClicks = 0;
    }
    public void returnHatk6()
    {
        if (noOfRClicks >= 2)
        {
            equipWeapon.Use();
            anim.SetBool("HAtk7", true);
            PlaySwordEffect();
        }
        else
        {
            anim.SetBool("HAtk7", false);
            anim.SetBool("HAtk6", false);
            noOfRClicks = 0;
        }
    }
    public void returnHatk7()
    {
        if (noOfRClicks >= 3)
        {
            equipWeapon.Use();
            anim.SetBool("HAtk8", true);
            PlaySwordEffect();
        }
        else
        {
            anim.SetBool("HAtk8", false);
            anim.SetBool("HAtk7", false);
            noOfRClicks = 0;
        }
    }
    public void returnHatk8()
    {
        anim.SetBool("HAtk6", false);
        anim.SetBool("HAtk7", false);
        anim.SetBool("HAtk8", false);
        noOfRClicks = 0;
    }
    public void returnHatk1()
    {
        if (noOfRClicks >= 2)
        {
            equipWeapon.Use();
            anim.SetBool("HAtk2", true);
            PlaySwordEffect();
        }
        else
        {
            anim.SetBool("HAtk2", false);
            anim.SetBool("HAtk1", false);
            noOfRClicks = 0;
        }
    }
    public void returnHatk2()
    {
        if (noOfRClicks >= 3)
        {
            equipWeapon.Use();
            anim.SetBool("HAtk3", true);
            PlaySwordEffect();
        }
        else
        {
            anim.SetBool("HAtk3", false);
            anim.SetBool("HAtk2", false);
            noOfRClicks = 0;
        }
    }
    public void returnHatk3()
    {
        anim.SetBool("HAtk1", false);
        anim.SetBool("HAtk2", false);
        anim.SetBool("HAtk3", false);
        noOfRClicks = 0;
    }
    public void PlaySwordEffect()
    {
        if (swordEffect != null)
        {
            // 검기 이펙트를 재생
            swordEffect.Play();
        }
    }
}
[System.Serializable]
public class Slash
{
    public GameObject slashObj;
    public float delay;
}
