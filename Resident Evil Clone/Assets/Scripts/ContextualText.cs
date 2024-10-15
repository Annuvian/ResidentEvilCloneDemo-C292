using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContextualText : MonoBehaviour
{
    [Tooltip("The text object for displaying contextual text.")]
    [SerializeField] TextMeshProUGUI contextualText;

    [Tooltip("How long the text takes to fade out.")]
    [SerializeField] float fadeDuration = 2f;
    [Tooltip("How long the text is displayed before starting to fade out.")]
    [SerializeField] float textShowDuration = 3f;
    // This is used to keep track of the fading coroutine to see if it's currently running or not.
    private Coroutine fadeCoroutine;

    void Start()
    {
        MyEvents.PickedUpItem.AddListener(UpdateText);
    }

    // This coroutine will handle displaying, and then fading out the text.
    IEnumerator FadeOutText()
    {
        // Locally declared method for setting the transparency value of the text.
        void SetTextAlpha(float alpha)
        {
            // Store the color of the text in a new variable.
            Color color = contextualText.color;
            // Set the alpha (transparency) value to match what we passed in.
            color.a = alpha;
            // Update the color of the text to match.
            contextualText.color = color;
        }

        // Make the text fully visible.
        SetTextAlpha(1f);

        // This just waits for a duration defined in textShowDuration before doing anything else.
        yield return new WaitForSeconds(textShowDuration);

        // Set the elapsed time to 0.
        float elapsedTime = 0f;
        // Loop to update the transparency over time. It runs as long as the elapsed time is less than the fade duration.
        while (elapsedTime < fadeDuration)
        {
            // Use Time.deltaTime to increment the timer, effectively counting it up in seconds.
            elapsedTime += Time.deltaTime;
            // Define a new float using Mathf.Lerp. The Lerp method uses linear interpolation to determine a value.
            // We have the starting value (1) the ending value (0), and finally how we determine the time "through" the linear interpolation.
            // What this does is it will linearly change from a transparency of 1 to a transparency of 0 over fadeDuration seconds.
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            // Update the alpha of the text with the newly calculated value.
            SetTextAlpha(newAlpha);
            // Finish the coroutine.
            yield return null;
        }

        // Ensure the text is fully transparent.
        SetTextAlpha(0f);
    }

    // We'll use this method to actually update the text and trigger the start of the coroutine.
    // NOTE: We pass in the text to be displayed.
    public void UpdateText(string newText)
    {
        // Update the text to display the newText.
        contextualText.text = newText;

        // Check to see if the coroutine is already running, if so, stop it.
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Simultaneously start the coroutine and assign it to the fadeCoroutine field.
        fadeCoroutine = StartCoroutine(FadeOutText());
    }
}