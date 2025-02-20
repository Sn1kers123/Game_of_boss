using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    public Slider slider;
    public Vector3 offset;
    public Transform boss;
    public float maxHealth;

    void Start()
    {
        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
            if (slider == null)
            {
                Debug.LogError("❌ Ошибка: Компонент Slider не найден в LifeBar!");
                return;
            }
        }
        Debug.Log("✅ LifeBar найден, maxHealth: " + maxHealth);
        Debug.Log("✅ Slider найден, начальное значение: " + slider.value);
    }

    public void HideBar()
    {
        slider.gameObject.SetActive(false);
    }

    public void SetHP(float hp)
    {
        slider.gameObject.SetActive(hp<maxHealth);
        slider.value = hp;
        slider.maxValue = maxHealth;
        Debug.Log("🟢 LifeBar обновлён! HP: " + hp + "/" + maxHealth);
    }



    void Update()
    {
        if (Camera.main == null || boss == null) return;

        // Берём позицию босса + смещение вверх
        Vector3 worldPos = boss.position + offset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // Если объект за камерой, не отображаем
        if (screenPos.z < 0)
        {
            slider.gameObject.SetActive(false);
            return;
        }
        slider.gameObject.SetActive(true);

        // Фиксируем LifeBar в UI
        RectTransform rect = slider.GetComponent<RectTransform>();
        rect.position = screenPos;
    }






}
