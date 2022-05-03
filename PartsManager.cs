using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PartsManager : MonoBehaviour
{
    public List<Card> partCards = new List<Card>();

    public GameObject head;
    public GameObject torso;
    public GameObject armL;
    public GameObject armR;
    public GameObject legs;

    [HideInInspector]
    public GameObject[] allParts;

    public Part[] allPartsComponents;

    public Animator[] allAnimators;
    private static readonly string resetAnim = "ResetAnim";

    [Header("DEBUG")]
    //[Range(0, 4)]
    //public int replacePart;
    //public GameObject replacementPrefab;

    public bool doThatAnimTrigger = false;

    [HideInInspector]
    public StateMachine stateMachine;

    public CinemachineStateDrivenCamera[] allCams;

    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            stateMachine = gameObject.GetComponent<StateMachine>();
        }

        if (allParts.Length == 0)
        {
            allParts = new GameObject[] { head, torso, armL, armR, legs };
        }

        RefreshParts();

        // Alright we're doing some hacky shit please bear with me.
        if (gameObject.CompareTag("Player"))
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayAnim(string animToPlay)
    {
        foreach (Animator ac in allAnimators)
        {
            ac.SetTrigger(animToPlay);
        }
    }

    //public void HandCardToHeadPart(Card card)
    //{
    //    head.GetComponent<Part>().cardEffectsToPlay = card;
    //}

    public void ResetAnims()
    {
        foreach (Animator ac in allAnimators)
        {
            ac.SetTrigger(resetAnim);
        }
    }

    public List<Card> RefreshParts()
    {
        allAnimators = GetComponentsInChildren<Animator>();

        partCards.Clear();

        for (int i = 0; i < allParts.Length; i++)
        {
            if (allParts[i] != null)
            {
                if (allParts[i].TryGetComponent(out Part part))
                {
                    part.PassCards();

                    // Populate it this way to keep it in order!
                    allPartsComponents[i] = part;
                }
            }
        }

        if (gameObject.CompareTag("Player"))
        {
            foreach (var cam in allCams)
            {
                cam.m_AnimatedTarget = allAnimators[0];
            }
        }

        return partCards;
    }

    public void AddSticker(Sticker sticker, int index)
    {
        allPartsComponents[index].sticker = sticker;
    }

    public void ReplacePart(int index, GameObject part)
    {
        // If replacePart is called before start somehow (scene switching)
        if (allParts.Length == 0)
        {
            allParts = new GameObject[] { head, torso, armL, armR, legs };
        }

        // Grab the sticker from the part before we delete it
        Sticker transferedSticker = allParts[index].GetComponent<Part>().sticker;
        Destroy(allParts[index]);

        // Create a new part, assign it properly, and give it the sticker.
        GameObject newPart = Instantiate(part, gameObject.transform);
        allParts[index] = newPart;
        newPart.GetComponent<Part>().sticker = transferedSticker;

        // May make an inspector so we can ditch the variables
        // since it holds the same info as the array????
        switch (index)
        {
            case 0:
                head = newPart;
                break;
            case 1:
                torso = newPart;
                break;
            case 2:
                armL = newPart;
                break;
            case 3:
                armR = newPart;
                break;
            case 4:
                legs = newPart;
                break;
            default:
                break;
        }

        RefreshParts();
        ResetAnims();
    }
}
