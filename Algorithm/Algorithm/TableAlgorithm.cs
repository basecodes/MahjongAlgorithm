using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MahjongAlgorithm.Extension;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MahjongAlgorithm.Algorithm {

    internal sealed class TableAlgorithm : IAlgorithm {
        private class Value {
            public int[] Values { get; set; }
            public Card Card { get; set; }
        }

        private readonly Dictionary<int, TingTemplate[]> _tingDict;
        private readonly Dictionary<int, int[]> _huDict;
        private readonly Dictionary<int, Dictionary<int,LaiTingTemplate[]>> _laiTingDict;

        private readonly ArrayPool<byte> _bytePool;
        private readonly Card[] _Types;
        private const int _maxCards = 14;
        private const int _maxMissCards = 5;
        private readonly int _typeLength;
        public TableAlgorithm(string tingTableFile,string laiTableFile,string huTableFile = null) {
            if (string.IsNullOrEmpty(tingTableFile)) {
                throw new ArgumentNullException(nameof(tingTableFile));
            }

            if (string.IsNullOrEmpty(laiTableFile)) {
                throw new ArgumentNullException(nameof(laiTableFile));
            }

            if (!string.IsNullOrEmpty(huTableFile)) {
                _huDict = FromJsonFile<Dictionary<int, int[]>>(huTableFile);
            }

            _tingDict = FromJsonFile<Dictionary<int, TingTemplate[]>>(tingTableFile);
            _laiTingDict = FromJsonFile<Dictionary<int, Dictionary<int,LaiTingTemplate[]>>>(laiTableFile);

            var maxCard = Enum.GetValues(typeof(Card)).Cast<Card>().Max().GetValue<int>();
            _typeLength = maxCard + 1;

            var temp = new List<Card>();
            foreach (Card item in Enum.GetValues(typeof(Card))) {
                temp.Add(item);
            }
            _Types = temp.ToArray();
            _bytePool = ArrayPool<byte>.Create(ushort.MaxValue, _typeLength);
        }
        private T FromJsonFile<T>(string fileName) {
            if (!File.Exists(fileName)) {
                throw new FileNotFoundException(fileName);
            }

            var serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (var fileStream = new FileStream(fileName, FileMode.Open)) {
                using (var streamReader = new StreamReader(fileStream)) {
                    using (var reader = new JsonTextReader(streamReader)) {
                        return serializer.Deserialize<T>(reader);
                    }
                }
            }
        }

        public Card[] GetAAA(IHuContext huContext) {
            if (huContext == null) {
                throw new ArgumentNullException(nameof(huContext));
            }

            if (!(huContext is TableHuContext)) {
                throw new ArgumentException($"非{nameof(TableAlgorithm)}!");
            }
            var tableHuContext = huContext as TableHuContext;
            var types = tableHuContext.Types;
            var item = tableHuContext.Value;

            var AAACount = item & 0x7;
            var cards = new Card[AAACount];
            for (var i = 0; i < AAACount; i++) {
                var offset = (byte)((item >> (10 + i * 4)) & 0xF);
                cards[i] = types[offset];
            }

            return cards;

        }

        public Card[] GetABC(IHuContext huContext) {
            if (huContext == null) {
                throw new ArgumentNullException(nameof(huContext));
            }

            if (!(huContext is TableHuContext)) {
                throw new ArgumentException($"非{nameof(TableAlgorithm)}!");
            }

            var tableHuContext = huContext as TableHuContext;

            var types = tableHuContext.Types;
            var item = tableHuContext.Value;

            var AAACount = item & 0x7;
            var ABCCount = (item >> 3) & 0x7;
            var bytes = new Card[ABCCount];

            for (var i = 0; i < ABCCount; i++) {
                var cardValue = types[(byte)((item >> (10 + AAACount * 4 + i * 4)) & 0xF)];
                bytes[i] = cardValue;
            }
            return bytes;
        }

        public Card GetCC(IHuContext huContext) {
            if (huContext == null) {
                throw new ArgumentNullException(nameof(huContext));
            }

            if (!(huContext is TableHuContext)) {
                throw new ArgumentException($"非{nameof(TableAlgorithm)}!");
            }

            var tableHuContext = huContext as TableHuContext;
            var types = tableHuContext.Types;
            var item = tableHuContext.Value;

            return types[(byte)((item >> 6) & 0xF)];
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

        private int GetFirstDigit(int number) {
            return number < 10 ? number : GetFirstDigit((number - (number % 10)) / 10);
        }

        private int CountDigits(int number) {
            number = Math.Abs(number);

            if (number >= 10) {
                return CountDigits(number / 10) + 1;
            }
            return 1;
        }

        public Card[] IsTing(Card[] cards) {
            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            var typeCount = _bytePool.Rent(_typeLength);

            cards.SortCards(typeCount);
            var flag = CalculateKey(typeCount);
            var list = new List<Card>();
            TingCards(flag, typeCount.CardTypes(), 
                typeCount, (key,values,card) => {
                list.Add(card);
            });

            _bytePool.Return(typeCount, true);
            return list.ToArray();
        }

        private void HuCard(int key, Card[] types, byte[] typeCount,
            Action<int[]> func) {
            if (_huDict.TryGetValue(key,out var items)) {
                func.Invoke(items);
            }
        }

        private void TingCards(int key, Card[] types, byte[] typeCount,
            Action<int,int[], Card> func) {
            if (_tingDict.TryGetValue(key, out var items)) {
                foreach (var item in items) {
                    Card card = 0;
                    switch (item.O) {
                        case 1:
                            card = types[item.R] - 1;
                            break;
                        case 2:
                            card = types[item.R] + 1;
                            break;
                        case 0:
                            card = types[item.R];
                            break;
                    }

                    if (Enum.IsDefined(typeof(Card), card)) {
                        typeCount[(byte)card]++;
                        if (CalculateKey(typeCount) == item.K) {
                            func.Invoke(item.K, item.Vs, card);
                        }
                        typeCount[(byte)card]--;
                    }
                }
            }
        }

        public Context IsHu(Card[] cards, Card card) {
            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            var typeCount = _bytePool.Rent(_typeLength);
            
            Value value = null;

            void callback(int key,int[] values, Card c) {
                if (c == card) {
                    value = new Value() {
                        Values = values,
                        Card = c
                    };
                }
            }

            Card[] cardTypes = null;

            if (_huDict == null) {
                var count = cards.SortCards(typeCount);
                cardTypes = typeCount.CardTypes();
                var key = CalculateKey(typeCount);
                TingCards(key, cardTypes, typeCount, callback);
                cardTypes.AddType(card);
            } else {
                cards.SortCards(card, typeCount);
                cardTypes = typeCount.CardTypes();
                var key = CalculateKey(typeCount);
                HuCard(key, cardTypes, typeCount, (values)=> {
                    value = new Value() {
                        Values = values
                    };
                });
            }

            if (value == null) {
                return null;
            }

            var huContexts = new IHuContext[value.Values.Length];
            for (var i = 0; i < huContexts.Length; i++) {
                huContexts[i] = new TableHuContext() {
                    Value = value.Values[i],
                    Types = cardTypes
                };
            }

            var gameContext = new Context();
            gameContext.HuCard = card;
            gameContext.HuContexts = huContexts;
            gameContext.Cards = cardTypes;

            _bytePool.Return(typeCount, true);
            return gameContext;
        }

        private int CalculateKey(byte[] typeCount) {
            if (typeCount == null) {
                throw new ArgumentNullException(nameof(typeCount));
            }

            if (typeCount.Length == 0) {
                return 0;
            }

            var key = 0;
            var len = -1;
            var con = false;
            foreach (var b in typeCount) {
                if (b != 0) {
                    len += 1;
                    if (b == 2) {
                        key |= 0b11 << len;
                        len += 2;
                    } else if (b == 3) {
                        key |= 0b1111 << len;
                        len += 4;
                    } else if (b == 4) {
                        key |= 0b111111 << len;
                        len += 6;
                    }
                    con = true;
                } else {
                    if (con) {
                        key |= 0b1 << len;
                        len += 1;
                        con = false;
                    }
                }
            }

            if (typeCount.Last() != 0) {
                key |= 0b1 << len;
            }

            return key;
        }

        private bool HandleLeft(
            int tmpIndex,
            int intervalsIndex,
            List<(byte c, byte n)> tmp,
            Stack<(List<int>, List<Card[]>)> total,
            List<(bool isData,byte left, byte right)> intervals) {
            // 前项遍历
            if (tmpIndex > 0) {
                var frontIndex = intervalsIndex - 1;
                if (frontIndex < 0 || intervals[frontIndex].isData) {
                    return false;
                }
                var front = new List<Card[]>();
                CalculateCombination(front, tmpIndex,
                    intervals[frontIndex].left,
                    intervals[frontIndex].right, new Stack<Card>());

                var nums = new List<int>();
                for (var i = 0; i < tmpIndex; i++) {
                    nums.Add(tmp[i].n);
                }
                total.Push((nums, front));
            }
            return true;
        }

        private bool HandleRight(int tmpIndex,
            int intervalsIndex,
            List<(byte c, byte n)> tmp,
            Stack<(List<int>, List<Card[]>)> total,
            List<(bool isData, byte left, byte right)> intervals) {
            // 后项遍历
            var offset = 0;
            for (var i = offset; i < tmp.Count - tmpIndex; i++) {
                var v = intervalsIndex + offset;
                var tmpOffset = tmpIndex + i;
                if (tmp[tmpOffset].c == 0) {
                    if (v >= intervals.Count || intervals[v].isData) {
                        return false;
                    }

                    var num = 0;
                    var nums = new List<int>();
                    for (var j = tmpOffset; j < tmp.Count; j++) {
                        if (tmp[j].c != 0) {
                            break;
                        }
                        nums.Add(tmp[j].n);
                        num++;
                    }
                    // 跳过连续空的
                    i += num - 1;

                    var back = new List<Card[]>();
                    CalculateCombination(back, num, intervals[v].left,
                        intervals[v].right, new Stack<Card>());
                    total.Push((nums, back));
                } else {
                    // 不使用区间
                    if (!intervals[v].isData) {
                        offset++;
                    }
                }
                offset++;
            }
            return true;
        }
        
        private void HandleSpecial(
            List<(byte c, byte n)> tmp, 
            Stack<(List<int>, List<Card[]>)> total,
            List<(bool isData, byte left, byte right)> intervals) {

            var num = 0;
            var nums = new List<int>();
            foreach (var (c, n) in tmp) {
                nums.Add(n);
                num++;
            }

            var (isData, left, right) = intervals.First();

            var back = new List<Card[]>();
            CalculateCombination(back, num, left, right, new Stack<Card>());
            total.Push((nums, back));
        }

        private int SplitArray(
            List<(byte c, byte n)> typeList,
            List<(bool isData, byte left, byte right)> intervals,
            byte[] typeCount) {

            var startIndex = -1;
            var endIndex = -1;
            // 区间数据起始位置
            var intervalsIndex = -1;

            // 划分区间
            foreach (var item in _Types) {
                var index = (byte)item;
                if (typeCount[index] != 0) {
                    endIndex = index - 1;
                    if (startIndex != -1 && endIndex != -1) {
                        intervals.Add((false, (byte)startIndex, (byte)endIndex));
                    }

                    if (intervalsIndex == -1) {
                        intervalsIndex = intervals.Count;
                    }

                    startIndex = -1;
                    var tuple = (index, typeCount[index]);
                    typeList.Add(tuple);
                    intervals.Add((true, index, typeCount[index]));
                } else {
                    if (startIndex == -1) {
                        startIndex = index;
                    }
                }
            }

            if (startIndex != -1) {
                intervals.Add((false, (byte)startIndex, (byte)_Types.Last()));
            }
            return intervalsIndex;
        }

        private void Ting(int count,int key,Card[] types,byte[] typeCount,Action<Card[]> action) {
           
            if (_laiTingDict.TryGetValue(count,out var dict)) {
                if (dict.TryGetValue(key,out var result)) {

                    var typeList = new List<(byte c, byte n)>();
                    var intervals = new List<(bool isData,byte left, byte right)>();
                    // 区间数据起始位置
                    var intervalsIndex = SplitArray(typeList,intervals,typeCount);
#if !TEST
                    Parallel.ForEach(result, (template) => {
#else
                    foreach(var template in result) {
#endif
                    var map = _bytePool.Rent(typeCount.Length);
                        Array.Copy(typeCount, 0, map, 0, typeCount.Length);
                        var cards = new Stack<Card>(_maxMissCards);
                        var tmp = new List<(byte c, byte n)>();
                        tmp.AddRange(typeList);
                        var tmpIndex = 0;

                        foreach (var offsets in template.Os) {
                            if (offsets[1] > tmp.Count) {
                                break;
                            }

                            // 叠加
                            if (offsets[0] == 0) {
                                var (c, n) = tmp[offsets[1]];
                                // 原有牌上叠加
                                if (c != 0) {
                                    map[c]++;
                                    cards.Push((Card)c);
                                }

                                tmp[offsets[1]] = (c, (byte)(n + 1));
                            }

                            // 添加
                            if (offsets[0] == 1) {
                                if (offsets[1] == tmp.Count) {
                                    tmp.Add((0, 1));
                                } else {
                                    if (offsets[1] == 0) {
                                        tmpIndex++;
                                    }
                                    tmp.Insert(offsets[1], (0, 1));
                                }
                            }
                        }

                        var total = new Stack<(List<int>, List<Card[]>)>();

                        // 只有一个区间
                        if (intervals.Count == 1 && intervalsIndex == -1) {
                            HandleSpecial(tmp,total,intervals);
                        } else {
                            // 前项遍历
                            if(!HandleLeft(tmpIndex, intervalsIndex, tmp, total, intervals)) {
#if !TEST
                                return;
#else
                                continue;
#endif
                            }

                            if (!HandleRight(tmpIndex, intervalsIndex, tmp, total, intervals)) {
#if !TEST
                                return;
#else
                                continue;
#endif
                            }
                        }

                        Calculate(map, total, 0, template, action, cards);
                        _bytePool.Return(map, true);
#if !TEST
                    });
#else
                    }
#endif
                }
            }
        }

        private void Calculate(
            byte[] typeCount,
            Stack<(List<int> counts,List<Card[]> cards)> stack,
            int index,
            LaiTingTemplate template,
            Action<Card[]> action,
            Stack<Card> cards) {

            if (stack.Count == 0) {
                var calcKey = CalculateKey(typeCount);
                if (calcKey == template.M) {
                    var array = cards.ToArray();
                    action(array);
                }
                return;
            }

            var list = stack.Pop();
            foreach (var item in list.cards) {
                for (var i = 0; i < list.counts.Count; i++) {
                    for (int j = 0; j < list.counts[i]; j++) {
                        typeCount[(byte)item[i]]++;
                        cards.Push(item[i]);
                    }
                }

                Calculate(typeCount, stack, index + 1,template,action,cards);

                for (var i = 0; i < list.counts.Count; i++) {
                    for (int j = 0; j < list.counts[i]; j++) {
                        typeCount[(byte)item[i]]--;
                        cards.Pop();
                    }
                }
            }
            stack.Push(list);
        }

        // 连续空的所有组合
        private void CalculateCombination(
            List<Card[]> save,
            int count,
            byte left, 
            byte right,
            Stack<Card> cards) {

            if (count == 0) {
                var array = cards.ToArray();
                Array.Reverse(array);
                save.Add(array);
                return;
            }

            for (var i = left; i <= right; i++) {
                if (Enum.IsDefined(typeof(Card), (Card)i)) {
                    cards.Push((Card)i);
                    CalculateCombination(save, count - 1, (byte)(i + 1), right,cards);
                    cards.Pop();
                }
            }
        }
        
        private List<Card[]> Ting(Card[] cards,byte[] typeCount,int count) {
            if (count == 1) {
                var result = IsTing(cards);
                var array = new List<Card[]>();
                for (var i = 0; i < result.Length; i++) {
                    array.Add(new Card[] { result[i] });
                }

                _bytePool.Return(typeCount, true);
                return array;
            }

            var nums = typeCount.Where(c => c != 0).ToList();
            var key = CalculateKey(nums.ToArray());
            var cardTypes = typeCount.CardTypes();
            var compares = new HashSet<string>();

            var list = new List<Card[]>();

            Ting(count, key, cardTypes, typeCount, (tingCards) => {
                lock (this) {

                    Array.Sort(tingCards);
                    var str = "";
                    foreach (var item in tingCards) {
                        str += "—" + (byte)item;
                    }

                    if (!compares.Contains(str)) {
                        list.Add(tingCards);
                        compares.Add(str);
                    }
                }
            });
            return list;
        }
        public Card[][] IsTing(Card[] cards, Card ignoreCard, int ignoreCount = 4) {
            if (ignoreCount < 1 ) {
                throw new ArgumentOutOfRangeException(nameof(ignoreCount));
            }

            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            var typeCount = _bytePool.Rent(_typeLength);
            var count = 1;
            cards.SortCards(typeCount,c=> {
                if (c == ignoreCard && count <= ignoreCount) {
                    count++;
                    return true;
                }
                return false;
            });

            var list = Ting(cards,typeCount, count);

            _bytePool.Return(typeCount, true);

            return list.ToArray();
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