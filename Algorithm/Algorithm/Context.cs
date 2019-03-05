
using System;
using System.Linq;

namespace MahjongAlgorithm.Algorithm {
    public class Context {
        internal Context() {
        }

        // 胡牌种类
        public IHuContext[] HuContexts { get; internal set; }
        // 可胡的牌
        public Card HuCard { get; internal set; }
        // 排好序的牌
        public Card[] Cards { get; internal set; }
    }
}