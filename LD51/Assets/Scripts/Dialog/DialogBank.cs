using System.Collections;
using System.Collections.Generic;
using System;

public static class DialogBank
{
    public static readonly string[] CAPTAIN1= 
    {
        "Saturday, April 21th : Log 210432:19.34",
        "We're on a mission to explore Mariana Trench's.",
        "A problem with the Ballast Tanks occured, we're stuck down here.",
        "We found a cavern, it's the only way to keep going forward.",
        "It seems like our light is no use here...",
    };

    public static readonly string[] BOTANIST1= 
    {
        "Wow... I never saw thick Kelps like this before."
    };

    public static readonly string[] BIOLOGIST1= 
    {
        "Ever heard of Dosidicus gigas ? I hope it's not their burrow!"
    };

    public static readonly string[] BOTANIST_BIOLUM= 
    {
        "Emissive light is such a classic at this depth."
    };

    public static readonly string[] CAPTAIN_GROTTO1= 
    {
        "That's a fucking big cavern."
    };

    public static readonly string[] BIOLOGIST_THREATS= 
    {
        "I expect local lifeforms to be...unusued to our presence."
    };

    public static readonly string[] CAPTAIN_GROTTO2= 
    {
        "Yeah, you like my home-built harpoons now ?"
    };
    public static readonly string[] BIOLOGIST_GROTTO2= 
    {
        "Oh c'mon get over it.",
        "Such a disaster couldn't be forseen."
    };
    public static readonly string[] CAPTAIN_GROTTO3= 
    {
        "Do you guys hear this strange sound ?"
    };
    public static readonly string[] BOTANIST_HEADHURT1= 
    {
        "Yes...This buzz is killing my ears..."
    };
    public static readonly string[] CAPTAIN_WTF1= 
    {
        "What size it this place ? It never ends."
    };
    public static readonly string[] BIOLOGIST_THREATS2= 
    {
        "It's getting cold or am I becoming crazy?"
    };
    public static readonly string[] BOTANIST_BIGGERKELPS= 
    {
        "It's incredible. Kelps are getting bigger as we venture forth."
    };
    public static readonly string[] CAPTAIN_REPBOTA1= 
    {
        "Right now I'm not exactly interested by such futile details."
    };
    public static readonly string[] BOTANIST_REPCAPTAIN1= 
    {
        "Why not ? Are you scared of getting devoured alive by some big fish ? ",
        "It's not like the outcome of this expedition could change."
    };
    public static readonly string[] CAPTAIN_REPBOTA2= 
    {
        "..."
    };
    public static readonly string[] BIOLOGIST_SCARED1= 
    {
        "Fuck!! I saw something move above our head !!"
    };
    public static readonly string[] CAPTAIN_REPBIO1= 
    {
        "Are you scared of ghosts now ?"
    };
    public static readonly string[] BIOLOGIST_REPCAPTAIN1= 
    {
        "You don't have to be a jerk because of the system failure on the submarine you built."
    };
    public static readonly string[] CAPTAIN_REPBIO2= 
    {
        "You never held a screwdriver in your hand and you're talking.."
    };
    public static readonly string[] BIOLOGIST_REPCAPTAIN2= 
    {
        "At least I'm honest with myself."
    };
    public static readonly string[] BOTANIST_HEADHURT2= 
    {
        "This noise...Please make it stop.."
    };
    public static readonly string[] CAPTAIN_FINALP2= 
    {
        "Its'... Not it can't be..."
    };

    public static readonly string[] CAPTAIN_END= 
    {
        "I..."
    };
    public static readonly string[] BIOLOGIST_END= 
    {
        "am..."
    };
    public static readonly string[] BOTANIST_END= 
    {
        "everything..."
    };

    private static List<string[]> bank;

    static DialogBank()
    {
        // load dialog in bank
        bank = new List<string[]>{
             CAPTAIN1, //0
             BOTANIST1, //1
             BIOLOGIST1,//2
            BOTANIST_BIOLUM,//3
            CAPTAIN_GROTTO1,//4
            BIOLOGIST_THREATS,//5
            CAPTAIN_GROTTO2,//6
            BIOLOGIST_GROTTO2,//7
            CAPTAIN_GROTTO3,//8
            BOTANIST_HEADHURT1,//9
            CAPTAIN_WTF1,//10
            BIOLOGIST_THREATS2,//11
            BOTANIST_BIGGERKELPS,//12
            CAPTAIN_REPBOTA1,//13
            BOTANIST_REPCAPTAIN1,//14
            CAPTAIN_REPBOTA2,//15
            BIOLOGIST_SCARED1,//16
            CAPTAIN_REPBIO1,//17
            BIOLOGIST_REPCAPTAIN1,//18
            CAPTAIN_REPBIO2,//19
            BIOLOGIST_REPCAPTAIN2,//20
            BOTANIST_HEADHURT2,//21
            CAPTAIN_FINALP2,//22,
            CAPTAIN_END,//23,
            BIOLOGIST_END,//24
            BOTANIST_END//25
             };
    }

    public static string[] load(int id)
    {
        if ((id < 0) || (id > bank.Count))
            return new string[]{};
        return bank[id] ;
    }

}
