
using System;
using System.Linq;

namespace MahjongAlgorithm.Algorithm {
    public class Context {
        internal Context() {
        }

        // ��������
        public IHuContext[] HuContexts { get; internal set; }
        // �ɺ�����
        public Card HuCard { get; internal set; }
        // �ź������
        public Card[] Cards { get; internal set; }
    }
}