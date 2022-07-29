using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private Material firstMaterial;
    private Material secondMaterial;

    private Quaternion currentRotation;
    [HideInInspector]
    public bool revealed = false;
    private bool clicked = false;
    private int index;
    private CardManager cardManager;
    public AudioClip clickCardSound;
    private AudioSource audioSource;

    public void SetIndex(int id)
    {
        index = id;
    }

    public int GetIndex()
    {
        return index;
    }


    void Start()
    {
        clicked = false;
        revealed = false;
        currentRotation = gameObject.transform.rotation;
        cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clickCardSound;
    }

    private void OnMouseDown()
    {
        //If a card is clicked while not rotating, play audio clip
        if (clicked == false)
        {
            cardManager.currentCardState = CardManager.CardState.CardRotating;
            if (GameSettings.Instance.IsSoundMuted() == false)
            {
                audioSource.Play();
            }

            StartCoroutine(LoopRotation(45, false));
            clicked = true;
        }
    }


    public void FlipBack()
    {
        //When cards are flipped back to their hidden state and sound is not muted the soundclip is played.
        if (gameObject.activeSelf)
        {
            cardManager.currentCardState = CardManager.CardState.CardRotating;
            revealed = false;

            if (GameSettings.Instance.IsSoundMuted() == false)
            {
                audioSource.Play();
            }
            StartCoroutine(LoopRotation(45, true));
        }
    }

    IEnumerator LoopRotation(float angle, bool FirstMat)
    {
        var rot = 0f;
        const float dir = 1f;
        const float rotSpeed = 180f;
        const float rotSpeed1 = 90f;
        var startAngle = angle;
        var assigned = false;

        if (FirstMat)
            //Rotates the card
        {
            while (rot < angle)
            {
                var step = Time.deltaTime * rotSpeed1;
                gameObject.GetComponent<Transform>().Rotate(new Vector3(0, 2, 0) * step * dir);
                if (rot >= (startAngle -2) && assigned == false)
                {
                    ApplyFirstMaterial();
                    assigned = true;
                }

                rot += (1 * step * dir);
                yield return null;
            }
        }
        else
        //Rotates the card back
        {
            while (angle > 0)
            {
                float step = Time.deltaTime * rotSpeed;
                gameObject.GetComponent<Transform>().Rotate(new Vector3(0, 2, 0) * step * dir);
                angle -= (1 * step * dir);
                yield return null;
            }
        }
        //Resets the rotation to currentRotation which I stored in the start method
        gameObject.GetComponent<Transform>().rotation = currentRotation;

        if (!FirstMat)
        {
            revealed = true;
            ApplySecondMaterial();
            cardManager.CheckCard();
        }
        else
        {
            cardManager.cardRevealedNumber = CardManager.RevealedState.NoRevealed;
            cardManager.currentCardState = CardManager.CardState.CanRotate;
        }

        clicked = false;
    }

    public void SetFirstMaterial(Material mat, string texturePath)
    {
        firstMaterial = mat;
        firstMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
    }

    public void SetSecondMaterial(Material mat, string texturePath)
    {
        secondMaterial = mat;
        secondMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
    }

    public void ApplyFirstMaterial()
    {
        gameObject.GetComponent<Renderer>().material = firstMaterial;
    }

    public void ApplySecondMaterial()
    {
        gameObject.GetComponent<Renderer>().material = secondMaterial;
    }

    public void Deactivate()
    {
        StartCoroutine(DeactivateCoroutine());
    }

    private IEnumerator DeactivateCoroutine()
    {
        revealed = false;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
