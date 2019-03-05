using System.Collections.Generic;

namespace MahjongAlgorithm.Algorithm {
    internal class TingTemplate {
        // ²Î¿¼
        public byte R { get; set; }
        // ²Ù×÷ 0 -> =  1 -> -   2 -> +
        public byte O { get; set; }
        // Key
        public int K { get; set; }
        // Values
        public int[] Vs { get; set; }
    }
}