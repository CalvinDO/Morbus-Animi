using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAWindToggle : MAButton
{
    public MAFanSpinner fanSpinner;
  

    public override void SetPressed() {

        base.SetPressed();

        this.fanSpinner.Stop();
    }

}
