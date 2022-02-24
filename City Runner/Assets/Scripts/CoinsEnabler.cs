using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsEnabler : MonoBehaviour
{
    [SerializeField] GameObject [] AllCoins;
    // Start is called before the first frame update
    void OnEnable()
    {
        foreach(var item in AllCoins)
            {
                if(item !=null)
                    item.SetActive(true);
            }        
    }

}
