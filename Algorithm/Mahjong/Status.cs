using System;

namespace MahjongAlgorithm.Algorithm {
    [Flags]
    public enum Status : byte {
        None = 0x0,
        Put = 0x1,
        Peng = 0x2,
        Gang = 0x4,
        Chi = 0x8,
        Ting = 0x10,
        Hu = 0x20,
        Ignore = 0x40
    }
}
