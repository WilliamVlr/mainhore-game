using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerTxt;
    [SerializeField] private float timer;
    private int timeDeficit = 1;
    private float maxtime;
    private float upd_r = 0, upd_g = 1;

    [SerializeField] private GameObject timerUI;
    private Slider timerSlider;
    private Image timerSliderFill;

    private bool isColorUpdating = true;

    private void Start()
    {
        timerSlider = timerUI.GetComponent<Slider>();
        timerSliderFill = timerSlider.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();

        timer = timerSlider.value;
        maxtime = timerSlider.value;
    }

    private void Update()
    {
        if(timer > 0 && Time.timeScale == 1)
        {
            timer -= Time.deltaTime * timeDeficit;
            timerSlider.value = timer;

            if (isColorUpdating) // Check if color updates are enabled
            {
                upd_r = (1 - (timer / maxtime));
                upd_g = (timer / maxtime);
                updateColor();
            }

            updateText((int)timer);
        }
    }

    public void updateText(int tm)
    {
        if (tm >= 10)
        {
            timerTxt.text = tm.ToString();
        }
        else
        {
            timerTxt.text = 0 + tm.ToString();
        }
    }

    public void updateColor()
    {
        float progress = timer / maxtime; // Progress is a normalized value between 0 and 1

        if (progress > 0.5f)
        {
            // From green to yellow
            float t = (1 - progress) * 2; // Normalize to [0, 1] within the first half
            timerSliderFill.color = Color.Lerp(new Color(0, 1, 0), new Color(1, 1, 0), t);
        }
        else
        {
            // From yellow to red
            float t = (0.5f - progress) * 2; // Normalize to [0, 1] within the second half
            timerSliderFill.color = Color.Lerp(new Color(1, 1, 0), new Color(1, 0, 0), t);
        }
    }

    public void SetColorUpdateEnabled(bool enabled)
    {
        isColorUpdating = enabled;
    }
}
