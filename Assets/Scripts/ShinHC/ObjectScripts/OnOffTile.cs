using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffTile : MonoBehaviour
{
    public Lux lux;
    [SerializeField]
    private GameObject lightTile;
    [SerializeField]
    private GameObject darkTile;
    [SerializeField]
    private bool isLight = false;   
    private void Awake()
    {
        SetTileState();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.E) && Vector3.Distance(lux.transform.position, transform.position) <= 4f)
            InputProcess();
    }

    public void InputProcess()
    {
        if(isLight)
            TurnOffLight();
        else
            TurnOnLight();

        SetTileState();
    }

    public void TurnOnLight()
    {
        if (!lux.UseLight())
            return;
        isLight = true;
    }
    public void TurnOffLight()
    {
        lux.AddLight();
        isLight = false;
    }
    private void SetTileState()
    {
        lightTile.SetActive(isLight);
        darkTile.SetActive(!isLight);
    }
}
