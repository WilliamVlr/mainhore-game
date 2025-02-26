using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private TextMeshProUGUI timerTxt;
    [SerializeField] private float timer;
    private int timeDeficit = 1;
    private float maxtime;
    private float upd_r = 0, upd_g = 1;

    [SerializeField] private GameObject timerUI;
    private Slider timerSlider;
    private Image timerSliderFill;

    private bool isColorUpdating = true;

    private void Awake()
    {
        timerSlider = timerUI.GetComponent<Slider>();
        timerSliderFill = timerSlider.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
    }

    private void Start()
    {
        //SetTimerMaxValue(timer);
    }

    public void SetTimerMaxValue(float maxDur)
    {
        if (timerSlider != null)
        {
            timerSlider.maxValue = maxDur;
            timerSlider.value = maxDur;
            maxtime = maxDur;
            timer = maxDur;
            SetTimerText((int) maxDur);
        }
        else
        {
            Debug.LogWarning("Timer Slider not found");
        }
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

    public void SetTimerText(int timer)
    {
        if (timer > 0)
        {
            timerTxt.text = timer.ToString();
        }
    }

    public void updateText(int tm)
    {
        if (tm >= 10)
        {
            if (timerTxt)
            {
                timerTxt.text = tm.ToString();
            }
            
            if (text)
            {
                text.text = tm.ToString();
            }
        }
        else
        {
            if (timerTxt)
            {
                timerTxt.text = 0 + tm.ToString();
            }
            if (text)
            {
                text.text = 0 + tm.ToString();
            }
        }
    }

    public void updateColor()
    {
        float progress = timer / maxtime;
        if (progress > 0.6f)
        {
            float t = 2 * (1 - progress); // Normalize to [0, 1]
            timerSliderFill.color = Color.Lerp(new Color(0, 1, 0), new Color(1, 1, 0), t);
        }
        else
        {
            float t = 2 * (0.5f - progress); // Normalize to [0, 1]
            timerSliderFill.color = Color.Lerp(new Color(1, 1, 0), new Color(1, 0, 0), t);
        }
    }

    public void SetColorUpdateEnabled(bool enabled)
    {
        isColorUpdating = enabled;
    }

    public int timerRemains()
    {
        return (int)timer;
    }
}
