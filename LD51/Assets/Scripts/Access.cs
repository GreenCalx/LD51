using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Access
{
    private class AccessCache
    {
        private GameObject GO_PLAYER;
        private GameObject GO_UISUBMARINE;
        private GameObject GO_SOUNDMANAGER;

        private static AccessCache inst;
        public static AccessCache Instance
        {
            get { return inst ?? (inst = new AccessCache()); }
            private set { inst = value; }
        }
        public GameObject checkCacheObject(string iHolder, ref GameObject iStorage)
        {
            GameObject handler = null;
            handler = !!iStorage ? iStorage : GameObject.Find(iHolder);
            if (!iStorage && !!handler)
                iStorage = handler;
            return handler;
        }
        public T getObject<T>(string iHolder, bool iComponentIsInChildren)
        {
            GameObject handler = null;
            if (iHolder==Constants.GO_PLAYER)
            {
                handler = checkCacheObject(iHolder, ref GO_PLAYER);
            } else if (iHolder==Constants.GO_UISUBMARINE)
            {
                handler = checkCacheObject(iHolder, ref GO_UISUBMARINE);
            } else if (iHolder==Constants.GO_SOUNDMANAGER)
            {
                handler = checkCacheObject(iHolder, ref GO_SOUNDMANAGER);
            }
            else
            { 
                Debug.LogWarning("Trying to access : " + iHolder + " as holding object, but is absent from cache.");
                handler = GameObject.Find(iHolder);
            }
            if (!!iComponentIsInChildren)
                return !!handler ? handler.GetComponentInChildren<T>() : default(T);
            return !!handler ? handler.GetComponent<T>() : default(T);

        }

        public void invalidate()
        {
            GO_PLAYER = null;
            GO_UISUBMARINE = null;
            GO_SOUNDMANAGER = null;
        }
    }

    private static AccessCache cache = AccessCache.Instance;

    public static void invalidate()
    {
        cache.invalidate();
    }

    public static PlayerController Player()
    {
        return cache.getObject<PlayerController>(Constants.GO_PLAYER, true);
    }

    public static UISubmarine UISubmarine()
    {
        return cache.getObject<UISubmarine>(Constants.GO_UISUBMARINE, false);
    }

    
    public static SoundManager SoundManager()
    {
        return cache.getObject<SoundManager>(Constants.GO_SOUNDMANAGER, false);
    } 
    
}
