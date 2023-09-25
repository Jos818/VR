//OrderedCodeSequenceDoor.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This Door type lets you set a list of buttons to hit IN A SPECIFIC ORDER. The door opens if they are all hit in the correct order. The door will not open if any of the fake buttons are hit or if the order is incorrect.
public class OrderedCodeSequenceDoor : MonoBehaviour
{
    public Animator animator;
    public AudioSource audio;
    public AudioClip open;
    public AudioClip wrongaud;
    bool playaud = true;
    public TextMeshPro codeText;
    public List<SequenceDoorButton> buttons;
    //Lists that convert the buttons into integers to be compared
    public List<int> code;
    public List<int> codecheck;
    bool dontopen = false;
    bool wrong = false;
    //Number of buttons used in the code
    public int codesize;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
        audio = gameObject.GetComponent<AudioSource>();
        dontopen = true;
        CodeGen();
    }


    void Update()
    {

        if (wrong == false)
        {
            foreach (SequenceDoorButton button in buttons)
            {
                //Adds button index to codecheck if not already present
                if (button.active == true && !codecheck.Contains(buttons.IndexOf(button)))
                {
                    //Debug.Log(buttons.IndexOf(button));
                    codecheck.Add(buttons.IndexOf(button));

                }
            }
            //Checks if the player input (codecheck) is the same as the code
            if (codecheck.Count > 0)
            {
                for (int i = 0; i < codecheck.Count; i++)
                {
                    if (codecheck[i] != code[i])
                    {
                        wrong = true;
                        StartCoroutine(WrongCode());
                        break;
                    }
                    }
                if (codecheck.Count == code.Count && wrong == false)
                {

                    dontopen = false;
                }
            }
            
        }

        if (dontopen == false && wrong == false)
        {
            animator.enabled = true;
            animator.SetBool("Open", true);
            if (playaud == true)
            {
                audio.clip = open;
                audio.Play();
                playaud = false;
            }
            this.enabled = false;
        }

    }
    public void CodeGen()
    {
        //Randomizes the buttons used in the code, while preventing duplicates
        code.Clear();
        codeText.text = "";
        for (int i = 0; i < codesize; i++)
        {
            int randomnum;
            do
            {
                randomnum = Random.Range(0, buttons.Count);
            }
            while (code.Contains(randomnum));
            code.Add(randomnum);
            codeText.text += (randomnum+1).ToString();
            //Debug.Log(randomnum);
        }
    }
    //A series of animations and audio clips that tell the player that the code they input is incorrect, then resets the code
    IEnumerator WrongCode()
    {
        audio.clip = wrongaud;
        audio.Play();
        foreach (SequenceDoorButton button in buttons)
        {
            button.locked = true;
            button.active = false;
            button.animator.SetBool("Pressed", false);
            button.audio.clip = button.unpress;
            button.audio.Play();
            button.playaud = true;
        }
        yield return new WaitForSeconds(2);
        codecheck.Clear();
        CodeGen();
        foreach (SequenceDoorButton button in buttons)
        {
            button.locked = false;
        }
        wrong = false;
    }
}
