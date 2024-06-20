using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Abilities:

1:  Nothing (Hoots)
2:  1 DMG (Peck)
3:  1 DMG (Peck)
4:  2 DMG (Slash)
5:  Dodge attacks for 1 turn (Fly)
6: Jumpscare (Deals 3 DMG)
‌
Item Drop: Familiar Knife (Mumei knife)
*/

public class KnifeOwl : Enemy
{
    protected override void Start()
    {
        base.Start();

        /*
         * Set values in prefab instead
         * 
        abilityValues[0] = 0;
        abilityValues[1] = 1;
        abilityValues[2] = 1;
        abilityValues[3] = 2;
        abilityValues[4] = 1;
        abilityValues[5] = 3;

        abilityLength[0] = 1;
        abilityLength[1] = 2;
        abilityLength[2] = 2;
        abilityLength[3] = 2;
        abilityLength[4] = 1;
        abilityLength[5] = 2;

        offensive[0] = false;
        offensive[1] = true;
        offensive[2] = true;
        offensive[3] = true;
        offensive[4] = false;
        offensive[5] = true;

        */
    }
}
