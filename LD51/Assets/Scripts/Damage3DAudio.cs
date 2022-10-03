using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage3DAudio : MonoBehaviour
{

    public enum DAMAGE_3DLOC{
        UP, DOWN, LEFT, RIGHT, FRONT, BACK
    }

    public void playDamageSound(DAMAGE_3DLOC iLoc)
    {
        switch(iLoc)
        {
            case DAMAGE_3DLOC.UP:
                Access.SoundManager().PlayOneShot(Constants.SFX_DAMAGEUP);
                break;
            case DAMAGE_3DLOC.DOWN:
                Access.SoundManager().PlayOneShot(Constants.SFX_DAMAGEDOWN);
                break;
            case DAMAGE_3DLOC.LEFT:
                Access.SoundManager().PlayOneShot(Constants.SFX_DAMAGELEFT);
                break;
            case DAMAGE_3DLOC.RIGHT:
                Access.SoundManager().PlayOneShot(Constants.SFX_DAMAGERIGHT);
                break;
            case DAMAGE_3DLOC.FRONT:
                Access.SoundManager().PlayOneShot(Constants.SFX_DAMAGEUPFRONT);
                break;
            case DAMAGE_3DLOC.BACK:
                Access.SoundManager().PlayOneShot(Constants.SFX_DAMAGEUPBACK);
                break;
            default:
                Access.SoundManager().PlayOneShot(Constants.SFX_DAMAGEUPFRONT);
                break;
        }
    }
}
