using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Row is Y
// Col is X

public class TileController : MonoBehaviour
{
    public Vector2 pos;
    [Space(10)]
    public float smoothingTime = 0.4f;
    public bool moving = false;
    private InputController ic;
    private Animator anim;

    void Start()
    {
        ic = gameObject.transform.parent.gameObject.GetComponent<InputController>();
        anim = gameObject.GetComponent<Animator>();

        unSelect();
    }

    void Update() {
        // if row/col equals actual pos then we are done moving
        if (transform.position.Equals(pos)) {
            moving = false;
        } else {
            // Otherwise update the name and start a move
            moving = true;
            name = $"({pos.x}, {pos.y}) {tag}";
            transform.position = Vector2.MoveTowards(transform.position, pos, smoothingTime * Time.deltaTime);
        }
    }

    private void OnMouseDown() {
        select();
        Vector2 pos = getVector();
        // Emit row, col data to input controller
        ic.click(pos);
    }

    public void unSelect() {
        anim.SetBool("Selected", false);
    }

    public void select() {
        anim.SetBool("Selected", true);
    }


    public void setCoord(Vector2 p) {
        pos = p;
    }

    public Vector2 getVector() {
        return pos;
    }

    public void invalid() {
        anim.SetTrigger("Invalid");
    }
}
