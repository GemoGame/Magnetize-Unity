using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public PlayerController player;
    private Color currentColor;
    // Start is called before the first frame update
    void Start()
    {
        currentColor = this.gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        player.setHookedTower(this.gameObject);
    }
}
