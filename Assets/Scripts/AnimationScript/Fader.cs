using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public TimerScript timerScript;
    public IEnumerator FadeInSprite(SpriteRenderer spriteRenderer, float fadeDuration)
    {
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer is null. Cannot fade.");
            yield break;
        }

        float elapsedTime = 0f;

        // Get the current color of the sprite
        Color color = spriteRenderer.color;

        // Fade in over the specified duration
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration); // Gradually increase alpha
            spriteRenderer.color = color;
            yield return null;
        }

        // Ensure the alpha is fully opaque
        color.a = 1f;
        spriteRenderer.color = color;
    }

    public IEnumerator FadeOutSprite(SpriteRenderer spriteRenderer, float fadeDuration)
    {
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer is null. Cannot fade.");
            yield break;
        }

        float elapsedTime = 0f;

        // Get the current color of the sprite
        Color color = spriteRenderer.color;

        // Fade out over the specified duration
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            color.a = Mathf.Clamp01(1 - (elapsedTime / fadeDuration)); // Gradually decrease alpha
            spriteRenderer.color = color;
            yield return null;
        }

        // Ensure the alpha is fully transparent
        color.a = 0f;
        spriteRenderer.color = color;
    }

    public IEnumerator FadeInText(Text mytext, float fadeDuration)
    {
        if (mytext == null)
        {
            Debug.LogWarning("Text is null. Cannot fade.");
            yield break;
        }

        float elapsedTime = 0f;

        // Get the current color of the text
        Color color = mytext.color;

        // Fade in over the specified duration
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration); // Gradually increase alpha
            mytext.color = color;
            yield return null;
        }

        // Ensure the alpha is fully opaque
        color.a = 1f;
        mytext.color = color;
    }

    public IEnumerator FadeOutText(Text mytext, float fadeDuration)
    {
        if (mytext == null)
        {
            Debug.LogWarning("Text is null. Cannot fade.");
            yield break;
        }

        float elapsedTime = 0f;

        // Get the current color of the text
        Color color = mytext.color;

        // Fade out over the specified duration
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            color.a = Mathf.Clamp01(1 - (elapsedTime / fadeDuration)); // Gradually decrease alpha
            mytext.color = color;
            yield return null;
        }

        // Ensure the alpha is fully transparent
        color.a = 0f;
        mytext.color = color;
    }

    public IEnumerator FadeOutGameObject(GameObject target, float fadeDuration)
    {
        if (target == null)
        {
            Debug.LogWarning("GameObject is null. Cannot fade.");
            yield break;
        }

        // Collect all SpriteRenderer, Text, Image, and Slider components in the GameObject and its children
        SpriteRenderer[] spriteRenderers = target.GetComponentsInChildren<SpriteRenderer>();
        Text[] texts = target.GetComponentsInChildren<Text>();
        TextMeshProUGUI[] textProUIs = target.GetComponentsInChildren<TextMeshProUGUI>();
        Image[] images = target.GetComponentsInChildren<Image>();
        Slider[] sliders = target.GetComponentsInChildren<Slider>();

        // Disable TimerScript color updates if present
        if (timerScript != null)
        {
            timerScript.SetColorUpdateEnabled(false); // Disable color updates

        }

        float elapsedTime = 0f;

        // Store the initial colors of all components
        Dictionary<Object, Color> initialColors = new Dictionary<Object, Color>();

        foreach (var spriteRenderer in spriteRenderers)
        {
            initialColors[spriteRenderer] = spriteRenderer.color;
        }

        foreach (var text in texts)
        {
            initialColors[text] = text.color;
        }

        foreach(var textPro in textProUIs)
        {
            initialColors[textPro] = textPro.color;
        }

        foreach (var image in images)
        {
            initialColors[image] = image.color;
        }

        // Fade out over the specified duration
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));

            foreach (var kvp in initialColors)
            {
                if (kvp.Key is SpriteRenderer sr)
                {
                    Color color = kvp.Value;
                    color.a = alpha;
                    sr.color = color;
                }
                else if (kvp.Key is Text text)
                {
                    Color color = kvp.Value;
                    color.a = alpha;
                    text.color = color;
                }
                else if (kvp.Key is TextMeshProUGUI textPro)
                {
                    Color color = kvp.Value;
                    color.a = alpha;
                    textPro.color = color;
                }
                else if (kvp.Key is Image img && kvp.Key != null)
                {
                    Color color = kvp.Value;
                    color.a = alpha;
                    img.color = color;
                }
            }

            foreach (var slider in sliders)
            {
                var colors = slider.colors;
                Color baseColor = initialColors.ContainsKey(slider) ? initialColors[slider] : Color.white;
                baseColor.a = alpha;
                colors.normalColor = baseColor;
                colors.highlightedColor = baseColor;
                colors.pressedColor = baseColor;
                colors.selectedColor = baseColor;
                colors.disabledColor = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
                slider.colors = colors;
            }

            yield return null;
        }

        // Set all components to fully transparent
        foreach (var kvp in initialColors)
        {
            if (kvp.Key is SpriteRenderer sr)
            {
                Color color = kvp.Value;
                color.a = 0f;
                sr.color = color;
            }
            else if (kvp.Key is Text text)
            {
                Color color = kvp.Value;
                color.a = 0f;
                text.color = color;
            }
            else if (kvp.Key is TextMeshProUGUI textPro)
            {
                Color color = kvp.Value;
                color.a = 0f;
                textPro.color = color;
            }
            else if (kvp.Key is Image img && kvp.Key != null)
            {
                Color color = kvp.Value;
                color.a = 0f;
                img.color = color;
            }
        }

        foreach (var slider in sliders)
        {
            var colors = slider.colors;
            colors.normalColor = new Color(0f, 0f, 0f, 0f);
            colors.highlightedColor = new Color(0f, 0f, 0f, 0f);
            colors.pressedColor = new Color(0f, 0f, 0f, 0f);
            colors.selectedColor = new Color(0f, 0f, 0f, 0f);
            colors.disabledColor = new Color(0f, 0f, 0f, 0f);
            slider.colors = colors;
        }
    }

    public IEnumerator FadeInGameObject(GameObject target, float fadeDuration)
    {
        if (target == null)
        {
            Debug.LogWarning("GameObject is null. Cannot fade in.");
            yield break;
        }

        // Collect all SpriteRenderer, Text, Image, and Slider components in the GameObject and its children
        SpriteRenderer[] spriteRenderers = target.GetComponentsInChildren<SpriteRenderer>();
        Text[] texts = target.GetComponentsInChildren<Text>();
        TextMeshProUGUI[] textProUIs = target.GetComponentsInChildren<TextMeshProUGUI>();
        Image[] images = target.GetComponentsInChildren<Image>();
        Slider[] sliders = target.GetComponentsInChildren<Slider>();

        float elapsedTime = 0f;

        // Store the initial colors of all components and set alpha to 0
        Dictionary<Object, Color> initialColors = new Dictionary<Object, Color>();

        foreach (var spriteRenderer in spriteRenderers)
        {
            Color color = spriteRenderer.color;
            initialColors[spriteRenderer] = color;
            color.a = 0f;
            spriteRenderer.color = color;
        }

        foreach (var text in texts)
        {
            Color color = text.color;
            initialColors[text] = color;
            color.a = 0f;
            text.color = color;
        }

        foreach (var textPro in textProUIs)
        {
            Color color = textPro.color;
            initialColors[textPro] = textPro.color;
            color.a = 0f;
            textPro.color = color;
        }

        foreach (var image in images)
        {
            if(image != null)
            {
                Color color = image.color;
                initialColors[image] = color;
                color.a = 0f;
                image.color = color;
            }
        }

        foreach (var slider in sliders)
        {
            var colors = slider.colors;
            Color baseColor = colors.normalColor;
            initialColors[slider] = baseColor;
            baseColor.a = 0f;

            colors.normalColor = baseColor;
            colors.highlightedColor = baseColor;
            colors.pressedColor = baseColor;
            colors.selectedColor = baseColor;
            colors.disabledColor = baseColor;
            slider.colors = colors;
        }

        // Fade in over the specified duration
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            foreach (var kvp in initialColors)
            {
                if (kvp.Key is SpriteRenderer sr)
                {
                    Color color = kvp.Value;
                    color.a = (sr.gameObject.CompareTag("Background")) ? Mathf.Min(alpha, 0.6f) : alpha;
                    sr.color = color;
                }
                else if (kvp.Key is Text text)
                {
                    Color color = kvp.Value;
                    color.a = alpha;
                    text.color = color;
                }
                else if (kvp.Key is TextMeshProUGUI textPro)
                {
                    Color color = kvp.Value;
                    color.a = alpha;
                    textPro.color = color;
                }
                else if (kvp.Key is Image img && kvp.Key != null)
                {
                    Color color = kvp.Value;
                    color.a = (img.gameObject.CompareTag("Background")) ? Mathf.Min(alpha, 0.6f) : alpha;
                    img.color = color;
                }
            }

            foreach (var slider in sliders)
            {
                var colors = slider.colors;
                Color baseColor = initialColors[slider];
                baseColor.a = alpha;

                colors.normalColor = baseColor;
                colors.highlightedColor = baseColor;
                colors.pressedColor = baseColor;
                colors.selectedColor = baseColor;
                colors.disabledColor = baseColor;
                slider.colors = colors;
            }

            yield return null;
        }

        // Set all components to their final opacity
        foreach (var kvp in initialColors)
        {
            if (kvp.Key is SpriteRenderer sr)
            {
                Color color = kvp.Value;
                color.a = (sr.gameObject.CompareTag("Background")) ? 0.6f : 1f;
                sr.color = color;
            }
            else if (kvp.Key is Text text)
            {
                Color color = kvp.Value;
                color.a = 1f;
                text.color = color;
            }
            else if (kvp.Key is TextMeshProUGUI textPro)
            {
                Color color = kvp.Value;
                color.a = 1f;
                textPro.color = color;
            }
            else if (kvp.Key is Image img && kvp.Key != null)
            {
                Color color = kvp.Value;
                color.a = (img.gameObject.CompareTag("Background")) ? 0.6f : 1f;
                img.color = color;
            }
        }

        foreach (var slider in sliders)
        {
            var colors = slider.colors;
            Color baseColor = initialColors[slider];
            baseColor.a = 1f;

            colors.normalColor = baseColor;
            colors.highlightedColor = baseColor;
            colors.pressedColor = baseColor;
            colors.selectedColor = baseColor;
            colors.disabledColor = baseColor;
            slider.colors = colors;
        }
    }
}
