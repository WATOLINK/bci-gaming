using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{   
    
    public override float[] GetInput() {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        if (hInput != 0) {
            animator.SetFloat("X", hInput);
        }
        return new float[] {hInput, vInput};
    }


}

