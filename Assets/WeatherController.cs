using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeatherController : MonoBehaviour
{
    public enum WeatherType
    {
        sunny,
        cloudy,
        rain
    }

    [Serializable]
    class WeatherEvents
    {
        public WeatherType type;
        public UnityEvent weatherEvent;

        public WeatherEvents(WeatherType type, UnityEvent weatherEvent)
        {
            this.type = type;
            this.weatherEvent = weatherEvent;
        }
    }

    [SerializeField] List<WeatherEvents> weatherEvents = new List<WeatherEvents>();

    [SerializeField] private WeatherType weather;

    public void SetWeather(WeatherType weatherType)
    {
        this.weather = weatherType;
        weatherEvents.Find(weather => weather.type == this.weather).weatherEvent?.Invoke();
    }

    private void Start()
    {
        SetWeather(weather);
    }
}
