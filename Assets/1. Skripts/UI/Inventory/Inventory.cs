using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory Instance;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    public delegate void OnSlotCountChange(int val); // 델리게이트 정의
    public OnSlotCountChange onSlotCountChange; //델리게이트 인스턴스화

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<Item> items = new List<Item>();

    private int slotCnt;

    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            slotCnt = value;
            //onSlotCountChange.Invoke(slotCnt);
        }
    }

    private void Start()
    {
        SlotCnt = 4;
    }

    public bool AddItem(Item _item)
    {
        if (items.Count < SlotCnt)
        {
            items.Add(_item);
            if(onChangeItem != null)
            onChangeItem.Invoke();
            return true;
        }
        return false;
    }

    public void RemoveItem(int _index)
    {
        items.RemoveAt(_index);
        onChangeItem.Invoke();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("FieldItem"))
        {
            FieldItems fieldItems = collision.GetComponent<FieldItems>();
            if(AddItem(fieldItems.GetItem()))
                fieldItems.DestroyItem();
        }
    }
}
