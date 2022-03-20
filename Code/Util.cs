using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static Color GetColor(EnumBlockType block_type)
    {
        Color color = new Color();
        switch (block_type)
        {
            case EnumBlockType.Apeach:
                ColorUtility.TryParseHtmlString("#F4C1C0", out color);
                break;
            case EnumBlockType.Muzi:
                ColorUtility.TryParseHtmlString("#FED300", out color);
                break;
            case EnumBlockType.Neo:
                ColorUtility.TryParseHtmlString("#7392B2", out color);
                break;
            case EnumBlockType.Ryan:
                ColorUtility.TryParseHtmlString("#DB9A27", out color);
                break;
            default:
                break;
        }
        return color;
    }
}
