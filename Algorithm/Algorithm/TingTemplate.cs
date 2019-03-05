using System.Collections.Generic;

namespace MahjongAlgorithm.Algorithm {
    internal class TingTemplate {
        // �ο�
        public byte R { get; set; }
        // ���� 0 -> =  1 -> -   2 -> +
        public byte O { get; set; }
        // Key
        public int K { get; set; }
        // Values
        public int[] Vs { get; set; }
    }
}