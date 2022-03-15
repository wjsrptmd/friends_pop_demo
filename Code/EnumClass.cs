using System.Collections;
using System.Collections.Generic;

public enum EnumBlockType
{
    None,
    Apeach,
    Muzi,
    Neo,
    Ryan
}

public enum EnumMove
{
    Stop,
    Moving,
    Moved
}

public class EnumClass
{
    static public EnumBlockType IntToEnumBlock(int type)
    {
        EnumBlockType ret = EnumBlockType.None;

        switch (type)
        {
            case 1:
                ret = EnumBlockType.Apeach;
                break;
            case 2:
                ret = EnumBlockType.Muzi;
                break;
            case 3:
                ret = EnumBlockType.Neo;
                break;
            case 4:
                ret = EnumBlockType.Ryan;
                break;
            default:
                break;
        }

        return ret;
    }

    static public int EnumBlockToInt(EnumBlockType type)
    {
        return (int)type;
    }
}