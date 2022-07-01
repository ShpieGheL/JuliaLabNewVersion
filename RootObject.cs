public class RootObject
{
    public string Backcolor { get; set; }
    public string Textcolor { get; set; }
    public string Objcolor { get; set; }
    public string TextInObjcolor { get; set; }
    public int Elements { get; set; }
    public int FontCB { get; set; }
    public int FontTB { get; set; }
    public int FontButton { get; set; }
}

public class Staff
{
    public string Тип { get; set; }
    public int Количество { get; set; }
    public int Цена { get; set; }
    public int Итог { get; set; }
}

public class Parts
{
    public string Этап { get; set; }
    public string Начало { get; set; }
    public string Конец { get; set; }
}