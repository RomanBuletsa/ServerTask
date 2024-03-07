using System;
using System.Collections.Generic;

[Serializable]
public class ButtonData
{
    public string id;
    public string text;
}

[Serializable]
public class ButtonsData
{
    public List<ButtonData> buttons;
}
