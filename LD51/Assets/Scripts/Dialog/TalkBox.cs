using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBox : MonoBehaviour
{
    [Header("MANDATORY")]
    public string npc_name;
    public int dialog_id;
    public GameObject dialogUI_ref;
    [Header("OPTIONALS")]
    public float timeBetweenDialogFrames = 2f;
    public float delayedStart = 0f;
    public AudioClip[] voices;
    //////////////////////////////////
    private AudioSource audio_source; 
    private UIDialog dialogUI;
    private bool is_in_dialog;
    private string[] loaded_dialog;
    private int curr_dialog_index;
    private float elapsedTimeBetweenFrames = 0f;
    private float elapsedTimeSinceDialogStarted = 0f;
    private bool delayedStartAsked = false;
    //////////////////////////////////

    void Start()
    {
        end_dialog();
        loaded_dialog = DialogBank.load(dialog_id);

        audio_source = GetComponent<AudioSource>();
    }

    void OnDestroy()
    {
    }

    void Update()
    {
        if ( delayedStartAsked )
        {
            elapsedTimeSinceDialogStarted += Time.deltaTime;
            if ( elapsedTimeSinceDialogStarted >= delayedStart )
            { talk(); delayedStartAsked = false; }
        }

        if (is_in_dialog)
        {
            elapsedTimeBetweenFrames += Time.deltaTime;
            if (elapsedTimeBetweenFrames >= timeBetweenDialogFrames)
            { talk(); elapsedTimeBetweenFrames = 0f; }
        }
    }

    void OnTriggerEnter(Collider iCol)
    {
        Debug.Log("talk trigger enter");
        if (iCol.transform.parent?.GetComponent<PlayerController>())
        {
            if ( delayedStart > 0f )
            {
                delayedStartAsked = true;
                return;
            }

            talk();
        }
    }

    void OnTriggerExit(Collider iCol)
    {
        Debug.Log("talk trigger exit");
        if (iCol.GetComponent<PlayerController>())
        {
            //end_dialog();
        }
    }
    
    private void talk()
    {
        if (!is_in_dialog) // Start Dialog
        {
            is_in_dialog    = true;
            dialogUI_ref.SetActive(true);
            dialogUI = dialogUI_ref.GetComponent<UIDialog>();
            curr_dialog_index = 0;
        }

        if ( !dialogUI.message_is_displayed() && 
              dialogUI.has_a_message_to_display() )
            dialogUI.force_display();
        else
        {
            if ( dialogUI.overflows )
            {
                dialogUI.display( npc_name, dialogUI.overflowing_text );
            }
            else 
            {
                if (curr_dialog_index >= loaded_dialog.Length )
                {
                    end_dialog();
                    dialogUI.resetMessage();
                    return;
                }
            
                dialogUI.display( npc_name, loaded_dialog[curr_dialog_index] );
                playVoice(); 
                curr_dialog_index++;
            }

        }

    }

    private void playVoice()
    {
        if ( (voices != null) && (voices.Length > 0 ) )
        {
            var rand = new System.Random();
            int voice_to_play = rand.Next(0, voices.Length);
            audio_source.clip = voices[voice_to_play];
            audio_source.Play();
        }
    }

    private void end_dialog()
    {
        is_in_dialog = false;
        dialogUI_ref.SetActive(false);
        curr_dialog_index  = 0;
        elapsedTimeBetweenFrames = 0f;
        elapsedTimeSinceDialogStarted = 0f;
        delayedStartAsked = false;
    }
}
