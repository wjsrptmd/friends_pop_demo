using System.Collections;
using System.Collections.Generic;

public class Util
{
    static public int max(int a, int b)
    {
        int ret = a;
        if (a < b) ret = b;
        return ret;
    }
}
