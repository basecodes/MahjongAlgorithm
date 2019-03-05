using MahjongAlgorithm.Algorithm;
using System;
using System.Collections.Generic;

namespace MahjongAlgorithm.Extension {
    public static class Extension {
        public static T GetValue<T>(this Enum obj)
            where T : struct, IConvertible {
            var value = Convert.ChangeType(obj, typeof(T));
            return (T)value;
        }

        public static T ToEnum<T>(this IConvertible convertible)
            where T : Enum {
            var value = Enum.ToObject(typeof(T), convertible);
            return (T)value;
        }

        public static int SortInsert<T>(this List<T> list, T item) {
            var search = list.BinarySearch(item);
            var index = search < 0 ? ~search : search;
            list.Insert(index, item);
            return index;
        }

        public static Card[] CardTypes(this byte[] cards) {
            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            var list = new List<Card>();
            for (var i = 0; i < cards.Length; i++) {
                if (cards[i] != 0) {
                    list.Add((Card)i);
                }
            }
            return list.ToArray();
        }

        public static int SortCards(this byte[] cards,byte[] typeCount) {
            return cards.SortCards(typeCount, c => false);
        }

        public static int SortCards(this Card[] cards,byte[] typeCount) {
            return cards.SortCards(typeCount, c => false);
        }

        public static int SortCards(this byte[] cards, byte[] typeCount, Func<byte, bool> skip) {
            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            if (typeCount == null) {
                throw new ArgumentNullException(nameof(typeCount));
            }

            var count = 0;
            foreach (var card in cards) {
                if (card != 0) {
                    if (!skip.Invoke(card)) {
                        if (typeCount[card] == 0) {
                            count++;
                        }
                        typeCount[card]++;
                    }
                }
            }
            return count;
        }

        public static int SortCards(this Card[] cards, byte[] typeCount,Func<Card,bool> skip) {
            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            if (typeCount == null) {
                throw new ArgumentNullException(nameof(typeCount));
            }

            var count = 0;
            foreach (var card in cards) {
                if (card != 0) {
                    if (!skip.Invoke(card)) {
                        if (typeCount[(byte)card] == 0) {
                            count++;
                        }
                        typeCount[(byte)card]++;
                    }
                }
            }
            return count;
        }

        public static int SortCards(this Card[] cards, Card card,byte[] typeCount) {
            typeCount[(byte)card]++;
            return cards.SortCards(typeCount, c => false) + 1;
        }

        public static Card[] AddType(this Card[] cards, Card card) {
            if (cards == null) {
                throw new ArgumentNullException(nameof(cards));
            }

            var list = new List<Card>();
            list.AddRange(cards);
            if (!list.Contains(card)) {
                list.SortInsert(card);
            }
            return list.ToArray();
        }
    }
}