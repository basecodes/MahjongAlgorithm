using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MahjongAlgorithm.Extension;

namespace MahjongAlgorithm.Algorithm {
    internal sealed class DFSAlgorithm : IAlgorithm {
        private class Value {
            public Card CC { get; set; }
            public Card[] AAA { get; set; }
            public Card[] ABC { get; set; }
        }

        private readonly ArrayPool<byte> _bytePool;
        private readonly Card[] _Types;

        private readonly int _typeLength;
        public DFSAlgorithm() {
            var maxCard = Enum.GetValues(typeof(Card)).Cast<Card>().Max().GetValue<int>();
            _typeLength = maxCard + 1;
            var temp = new List<Card>();
            foreach (Card item in Enum.GetValues(typeof(Card))) {
                temp.Add(item);
            }
            _Types = temp.ToArray();
           _bytePool = ArrayPool<byte>.Create(ushort.MaxValue, _typeLength);
        }

        private int Delete_ABC(byte[] temp,List<Card> list = null) {
            var count = 0;
            for (var a = 0; a < 3; a++) {
                for (var b = 1; b <= 7;) {
                    if (temp[10 * a + b] >= 1 &&
                        temp[10 * a + b + 1] >= 1 &&
                        temp[10 * a + b + 2] >= 1) {

                        temp[10 * a + b]--;
                        temp[10 * a + b + 1]--;
                        temp[10 * a + b + 2]--;
                        count += 3;
                        list?.Add((Card)(10 * a + b));
                    } else {
                        b++;
                    }
                }
            }
            return count;
        }

        private int Delete_AAA(byte[] temp,List<Card> list = null) {
            var count = 0;
            foreach (var item in _Types) {
                var index = (byte)item;
                if (temp[index] >= 3) {
                    temp[index] -= 3;
                    count += 3;
                    list?.Add((Card)index);
                }
            }
            return count;
        }

        private int Delete_CC(byte[] temp,List<Card> list = null) {
            var count = 0;

            foreach (var item in _Types) {
                var index = (byte)item;
                if (temp[index] >= 2) {
                    temp[index] -= 2;
                    count += 2;
                    list?.Add((Card)index);
                }
            }

            return count;
        }

        public Card[] GetAAA(IHuContext huContext) {
            if (huContext == null) {
                throw new ArgumentNullException(nameof(huContext));
            }

            if (!(huContext is DFSHuContext)) {
                throw new ArgumentException($"非{nameof(DFSAlgorithm)}！");
            }

            var dfsHuContext = huContext as DFSHuContext;
            return dfsHuContext.AAA;
        }

        public Card[] GetABC(IHuContext huContext) {
            if (huContext == null) {
                throw new ArgumentNullException(nameof(huContext));
            }

            if (!(huContext is DFSHuContext)) {
                throw new ArgumentException($"非{nameof(DFSAlgorithm)}！");
            }

            var dfsHuContext = huContext as DFSHuContext;
            return dfsHuContext.ABC;
        }

        public Card GetCC(IHuContext huContext) {
            if (huContext == null) {
                throw new ArgumentNullException(nameof(huContext));
            }

            if (!(huContext is DFSHuContext)) {
                throw new ArgumentException($"非{nameof(DFSAlgorithm)}！");
            }

            var dfsHuContext = huContext as DFSHuContext;
            return dfsHuContext.CC;
        }

        public bool IsABC(Card[] cards, Card card) {
            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            if (cards.Length < 2) {
                return false;
            }

            return IsXBC(cards, card) || IsAXC(cards, card) || IsABX(cards, card);
        }

        public bool IsXBC(Card[] cards, Card card) {
            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            if (cards.Length < 2) {
                return false;
            }

            var flag = 0;
            foreach (var item in cards) {
                if (item == card + 1) {
                    flag |= 1;
                }

                if (item == card + 2) {
                    flag |= 0x2;
                }
            }

            return flag == 3 ? true : false;
        }

        public bool IsAXC(Card[] cards, Card card) {
            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            if (cards.Length < 2) {
                return false;
            }

            var flag = 0;
            foreach (var item in cards) {
                if (item == card - 1) {
                    flag |= 1;
                }

                if (item == card + 1) {
                    flag |= 0x2;
                }
            }

            return flag == 3 ? true : false;
        }

        public bool IsABX(Card[] cards, Card card) {
            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            if (cards.Length < 2) {
                return false;
            }

            var flag = 0;
            foreach (var item in cards) {
                if (item == card - 1) {
                    flag |= 1;
                }

                if (item == card - 2) {
                    flag |= 0x2;
                }
            }

            return flag == 3 ? true : false;
        }

        public bool IsAAA(Card[] cards, Card card) {
            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            if (cards.Length < 2) {
                return false;
            }

            var count = cards.Count(c => c == card);
            if (count == 2) {
                return true;
            }
            return false;
        }

        public bool IsAAAA(Card[] cards, Card card) {
            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            if (cards.Length < 3) {
                return false;
            }

            var count = cards.Count(c => c == card);
            if (count == 3) {
                return true;
            }
            return false;
        }

        public Context IsHu(Card[] cards, Card card) {
            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }
            var typeCount = _bytePool.Rent(_typeLength);
            cards.SortCards(card,typeCount);
            var list = CalculateKey(typeCount,cards.Length + 1);
            if (list.Count == 0) {
                return null;
            }

            var huContexts = new List<IHuContext>();
            var cardTypes = typeCount.CardTypes();

            foreach (var v in list) {
                huContexts.Add(new DFSHuContext() {
                    CC = v.CC,
                    ABC = v.ABC,
                    AAA = v.AAA,
                });
            }
           
            var gameContext = new Context();
            gameContext.HuCard = card;
            gameContext.HuContexts = huContexts.ToArray();
            gameContext.Cards = cardTypes;
            _bytePool.Return(typeCount,true);
            return gameContext;
        }

        public Card[] IsTing(Card[] cards) {
            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            var list = new List<Card>();
            Parallel.ForEach(_Types, (c) => {
                var typeCount = _bytePool.Rent(_typeLength);
                cards.SortCards(typeCount);
                typeCount[(byte)c]++;
                if (TingCard(typeCount, cards.Length + 1)) {
                    lock (this) {
                        list.Add(c);
                    }
                }
                typeCount[(byte)c]--;
                _bytePool.Return(typeCount, true);
            });

            return list.ToArray();
        }

        private List<Value> CalculateKey(byte[] typeCount, int length) {
            if (typeCount == null) {
                throw new ArgumentNullException(nameof(typeCount));
            }

            var ret = new List<Value>();

            Parallel.ForEach(_Types, (item) => {
                var aaa = new List<Card>();
                var abc = new List<Card>();
                var temp = _bytePool.Rent(typeCount.Length);

                var index = (byte)item;
                for (var first = 0; first < 2; first++) {

                    Array.Clear(temp, 0, temp.Length);
                    Array.Copy(typeCount, temp, typeCount.Length);
                    var count = 0;
                    var cc = 0;

                    if (temp[index] >= 2) {
                        temp[index] -= 2;
                        cc = index;
                        count += 2;

                        if (first == 0) {
                            count += Delete_AAA(temp, aaa);
                            count += Delete_ABC(temp, abc);
                        } else {
                            count += Delete_ABC(temp, abc);
                            count += Delete_AAA(temp, aaa);
                        }

                        if (count == length) {
                            var v = new Value() {
                                CC = (Card)cc,
                                AAA = aaa.ToArray(),
                                ABC = abc.ToArray()
                            };
                            lock (this) {
                                ret.Add(v);
                            }
                        }
                    }
                    aaa.Clear();
                    abc.Clear();
                }
                _bytePool.Return(temp, true);
            });
            return ret;
        }

        private bool TingCard(byte[] typeCount, int length) {
            if (typeCount == null) {
                throw new ArgumentNullException(nameof(typeCount));
            }

            var temp = _bytePool.Rent(typeCount.Length);
            foreach (var item in _Types) {
                var index = (byte)item;
                for (var first = 0; first < 2; first++) {
                    Array.Copy(typeCount, temp, typeCount.Length);

                    var count = 0;
                    if (temp[index] >= 2) {
                        temp[index] -= 2;
                        count += 2;
                        if (first == 0) {
                            count += Delete_AAA(temp);
                            count += Delete_ABC(temp);
                        } else {
                            count += Delete_ABC(temp);
                            count += Delete_AAA(temp);
                        }

                        if (length == count) {
                            _bytePool.Return(temp, true);
                            return true;
                        }
                    }
                }
            }
            
            _bytePool.Return(temp, true);
            return false;
        }

        private void Find(int count,byte[] typeCount, Stack<Card> stack ,
            int length,Action<Card[]> action) {

            if (count == 0) {
                if (TingCard(typeCount,  length)) {
                    action?.Invoke(stack.ToArray());
                }
                return;
            }

            foreach (var item in _Types) {
                var index = (byte)item;
                if (typeCount[index]  == 4) {
                    continue;
                }
                typeCount[index] += 1;
                stack.Push(item);

                Find(count - 1, typeCount, stack, length, action);

                stack.Pop();
                typeCount[index] -= 1;
            }
        }

        public Card[][] IsTing(Card[] cards, Card ignoreCard, int ignoreCount = 4) {
            if (ignoreCount < 1) {
                throw new ArgumentOutOfRangeException(nameof(ignoreCard));
            }

            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            var typeCount = _bytePool.Rent(_typeLength);
            var count = 1;
            cards.SortCards(typeCount, c => {
                if (c == ignoreCard && count <= ignoreCount) {
                    count++;
                    return true;
                }
                return false;
            });

            var list = Ting(cards, typeCount, count);

            _bytePool.Return(typeCount, true);
            return list.ToArray();
        }

        private List<Card[]> Ting(Card[] cards, byte[] typeCount, int count) {

            if (count == 1) {
                var result = IsTing(cards);
                var array = new List<Card[]>();
                for (var i = 0; i < result.Length; i++) {
                    array.Add(new Card[] { result[i] });
                }

                _bytePool.Return(typeCount, true);
                return array;
            }

            var list = new List<Card[]>();
            var compares = new HashSet<string>();
            Find(count, typeCount, new Stack<Card>(), cards.Length + 1, (tingCards) => {
                Array.Sort(tingCards);
                var str = "";
                foreach (var item in tingCards) {
                    str += "—" + (byte)item;
                }

                if (!compares.Contains(str)) {
                    list.Add(tingCards);
                    compares.Add(str);
                }
            });

            return list;
        }

        public Card[][] IsTing(Card[] cards, params Card[] ignoreCards) {
            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            var typeCount = _bytePool.Rent(_typeLength);
            var count = 1;
            cards.SortCards(typeCount, c => {
                for (var i = 0; i < ignoreCards.Length; i++) {
                    if (c == ignoreCards[i]) {
                        count++;
                        ignoreCards[i] = 0;
                        return true;
                    }
                }
                return false;
            });

            var list = Ting(cards, typeCount, count);

            _bytePool.Return(typeCount, true);
            return list.ToArray();
        }
    }
}
