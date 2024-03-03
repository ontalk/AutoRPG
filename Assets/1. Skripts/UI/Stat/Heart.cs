using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Heart : MonoBehaviour
{
    private int curHeart;
    private int addHeart;

    private int prevCurHeart;
    private int prevAddHeart;

    public Text publishingHeart;
    public Text gameOverHeart;
    public Text gameOverAddHeart;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize previous values to something that would not match the initial values
        prevCurHeart = -1;
        prevAddHeart = -1;
    }

    // Update is called once per frame
    void Update()
    {
        curHeart = GameManager.Instance.curHeart;
        addHeart = GameManager.Instance.addHeart;

        // Check if there is a change in values before updating the text
        if (curHeart != prevCurHeart)
        {
            publishingHeart.text = curHeart.ToString();
            gameOverHeart.text = curHeart.ToString();

            // Update previous values to the current values
            prevCurHeart = curHeart;
        }

        if (addHeart != prevAddHeart)
        {
            gameOverAddHeart.text = addHeart.ToString();

            // Update previous values to the current values
            prevAddHeart = addHeart;
        }
    }
}
