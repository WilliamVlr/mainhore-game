using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotInitialCharacter : MonoBehaviour, IPointerDownHandler
{
    [Header("Backgrounds")]
    [SerializeField] private GameObject backgroundClicked;
    [SerializeField] private Image charPreview;

    [Header("Data")]
    [SerializeField] private SO_Skin skinData;
    public static SlotInitialCharacter activeSlot;

    private void Awake()
    {
        if(activeSlot == null)
        {
            activeSlot = this;
        }
    }

    private void Start()
    {
        if(activeSlot == this)
        {
            InitialDataManager.Instance.Skin = skinData.ID;
            charPreview.sprite = skinData.sprite;
            activeSlot.OnSlotTouched();
            return;
        } else {
            OnSlotUntouched();
        }
    }

    public string getSkinName()
    {
        return skinData.name;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(activeSlot != null && activeSlot != this)
        {
            activeSlot.OnSlotUntouched();
        }

        activeSlot = this;
        InitialDataManager.Instance.Skin = skinData.ID;
        charPreview.sprite = skinData.sprite;
        activeSlot.OnSlotTouched();
    }

    private void OnSlotUntouched()
    {
        backgroundClicked.SetActive(false);
    }

    private void OnSlotTouched()
    {
        backgroundClicked.SetActive(true);
    }
}