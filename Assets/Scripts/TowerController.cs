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

    void OnMouseDown()
    {

        player.setHookedTower(this.gameObject);
        currentColor = Color.green;
        this.gameObject.GetComponent<SpriteRenderer>().color = currentColor;
    }

    void OnMouseUp()
    {
        player.setHookedTower(null);
        currentColor = Color.white;
        this.gameObject.GetComponent<SpriteRenderer>().color = currentColor;
    }
}
