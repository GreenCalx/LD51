using System.Collections;
using System.Collections.Generic;
using System;

public static class DialogBank
{
    public static readonly string[] TEST_DIALOG= 
    {
        "Saturday, April 21th : Log 210432:19.34",
        "We're on a mission to explore Mariana Trench's.",
        "A problem with the Ballast Tanks occured, we're stuck down here.",
        "We found a cavern, it's the only way to keep going forward.",
        "It seems like our light is no use here...",
    };


    private static List<string[]> bank;

    static DialogBank()
    {
        // load dialog in bank
        bank = new List<string[]>{
             TEST_DIALOG 
             };
    }

    public static string[] load(int id)
    {
        if ((id < 0) || (id > bank.Count))
            return new string[]{};
        return bank[id] ;
    }

}
