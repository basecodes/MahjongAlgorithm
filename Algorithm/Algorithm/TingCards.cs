using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MahjongAlgorithm.Algorithm {

    internal struct EqualityComparer : IEqualityComparer<Card[]> {
        public bool Equals(Card[] x, Card[] y) {
            return x.SequenceEqual(y);
        }

        public int GetHashCode(Card[] obj) {
            var sum = 0;
            foreach (var item in obj) {
                sum ^= (int)item;
            }
            return sum ^ obj.Length;
        }
    }


    public struct TingCard{
        // 癞子牌替换的牌
        public Card[] Cards { get; internal set; }
        // 听的牌
        public Card Card { get; internal set; }
    }
}
