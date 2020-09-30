using System.Collections;
using System.Collections.Generic;

struct Command
{
    public readonly static ulong Arrow_Down = 0x1;
    public readonly static ulong Arrow_Up = 0x100;
    public readonly static ulong Arrow_Left = 0x10000;
    public readonly static ulong Arrow_Right = 0x1000000;
    public readonly static ulong Attack_J = 0x100000000;
    public readonly static ulong Attack_K = 0x10000000000;
    public readonly static ulong Attack_U = 0x1000000000000;
    public readonly static ulong Attack_I = 0x100000000000000;
    static public Dictionary<string, ulong> CommandKeyList = new Dictionary<string, ulong>();
}