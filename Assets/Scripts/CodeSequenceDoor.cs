//CodeSequenceDoor.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This Door type lets you set a list of buttons to hit. The door opens if they are all hit. The door will not open if any of the fake buttons are hit. Order does not matter in this variant.
public class CodeSequenceDoor : MonoBehaviour
{
    public Animator animator;
    public AudioSource audio;
    public AudioClip open;
    public AudioClip wrongaud;
    public AudioClip close;
    bool playaud = true;
    public TextMeshPro codeText;
    public List<SequenceDoorButton> buttons;
    public List<SequenceDoorButton> fakebuttons;
    public List<int> code;
    public List<int> codecheck;
    bool dontopen = false;
    bool wrong = false;
    public int codesize;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
        audio = gameObject.GetComponent<AudioSource>();
        dontopen = true;
        //Randomizes the buttons used in the code, while preventing duplicates
        CodeGen();
    }

    void Update()
    {

        if (wrong == false)
        {
            foreach (SequenceDoorButton button in fakebuttons)
            {
                if (button.active == true)
                {
                    wrong = true;
                    StartCoroutine(WrongCode());
                }
            }
            foreach (SequenceDoorButton button in buttons)
            {
                //Adds button index to codecheck if not already present
                if (button.active == true && !codecheck.Contains(buttons.IndexOf(button)))
                {
                    //Debug.Log(buttons.IndexOf(button));
                    codecheck.Add(buttons.IndexOf(button));
                }
                if (button.timed == true && button.active == false && codecheck.Contains(buttons.IndexOf(button)))
                {
                    //Debug.Log(buttons.IndexOf(button));
                    codecheck.Remove(buttons.IndexOf(button));
                }
            }
            //Checks if the player input (codecheck) is the same as the code
            if (codecheck.Count == code.Count)
            {
                for (int i = 0; i < codecheck.Count; i++)
                {
                    if (!codecheck.Contains(code[i]))
                    {
                        if (dontopen == false && wrong == false)
                        {
                            animator.SetBool("Open", false);
                            if (playaud == false)
                            {
                                audio.clip = close;
                                audio.Play();
                                playaud = true;
                            }
                        }
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
        if (dontopen == false && codecheck.Count < code.Count)
        {
            animator.SetBool("Open", false);
            if (playaud == false)
            {
                audio.clip = close;
                audio.Play();
                playaud = true;
            }
            dontopen = true;
        }

        if (dontopen == false && wrong == false)
        {
            
            if (playaud == true)
            {
                audio.clip = open;
                audio.Play();
                playaud = false;
            }
            animator.SetBool("Open", true);
            animator.enabled = true;
            foreach (SequenceDoorButton button in buttons)
            {
                if (button.timed == false)
                {
                    this.enabled = false;
                }
            }
            
        }

    }
    public void CodeGen()
    {
        //Randomizes the buttons used in the code, while preventing duplicates
        code.Clear();
        fakebuttons.Clear();
        if (codeText != null)
        {
            codeText.text = "";
        }
        for (int i = 0; i < codesize; i++)
        {
            int randomnum;
            do
            {
                randomnum = Random.Range(0, buttons.Count);
            }
            while (code.Contains(randomnum));
            code.Add(randomnum);
            if (codeText != null)
            {
                codeText.text += (randomnum+1).ToString();
            }
            //Debug.Log(randomnum);
        }
        foreach (SequenceDoorButton button in buttons)
        {
            if (!code.Contains(buttons.IndexOf(button)))
            {
                fakebuttons.Add(button);
            }
        }
    }
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
