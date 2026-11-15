using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

[Serializable]
public sealed class UIConfig
{
    [SerializeField] PlayerStateRefs _player;
    [SerializeField] MessageRefs _messages;
    [SerializeField] TimerRefs _timer;
    [SerializeField] PaperRefs _paper;

    public PlayerStateRefs Player => _player;
    public MessageRefs Messages => _messages;
    public TimerRefs Timer => _timer;
    public PaperRefs Paper => _paper;
}

[Serializable]
public sealed class PlayerStateRefs
{
    [SerializeField] Image _stamina;

    public Image Stamina => _stamina;
}

[Serializable]
public sealed class MessageRefs
{
    [Serializable]
    public struct ResultMessage
    {
        public InteractResult result;
        public LocalizedString text;
    }

    [SerializeField] Image _hold;
    [SerializeField] GameObject _messageObject;
    [SerializeField] ResultMessage[] _results;

    public Image Hold => _hold;
    public GameObject MessageObject => _messageObject;

    public LocalizedString Get(InteractResult result)
    {
        for (int i = 0; i < _results.Length; i++)
            if (_results[i].result == result)
                return _results[i].text;

        return null;
    }
}

[Serializable]
public sealed class TimerRefs
{
    [SerializeField] TimerCounter _counter;
    [SerializeField] TextMeshProUGUI _text;

    public TimerCounter Counter => _counter;
    public TextMeshProUGUI Text => _text;
}

[Serializable]
public sealed class PaperRefs
{
    [SerializeField] GameObject _panel;
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _text;

    public GameObject Panel => _panel;
    public Image Image => _image;
    public TextMeshProUGUI Text => _text;
}