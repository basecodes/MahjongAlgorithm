using MahjongAlgorithm;
using MahjongAlgorithm.Algorithm;
using NUnit.Framework;
using System;

namespace Test {
    [TestFixture]
    public class TestTableAlgorithm {
        private IAlgorithm _algorithm;

        [OneTimeSetUp]
        public void Loading() {
            _algorithm = Mahjong.GetAlgorithm(
                "./Resources/TingTable.json",
                "./Resources/LaiTable.json",
                "./Resources/HuTable.json");
        }

        [Test]
        public void TestTing() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_1,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_5,
                Card.Crak_6,
                Card.Crak_6,
                Card.Crak_6,
                Card.Bam_2
            };

            var tingCards = _algorithm.IsTing(cards);
            Assert.Contains(Card.Bam_2, tingCards);
            Assert.Contains(Card.Bam_3, tingCards);
        }

        [Test]
        public void TestTing2() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_2,
                Card.Bam_2
            };

            var tingCards = _algorithm.IsTing(cards);
            Assert.Contains(Card.Bam_1, tingCards);
            Assert.Contains(Card.Bam_2, tingCards);
        }

        [Test]
        public void TestHu() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_1,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_5,
                Card.Crak_6,
                Card.Crak_6,
                Card.Crak_6,
                Card.Bam_2
            };

            var gameContext = _algorithm.IsHu(cards, Card.Bam_2);
            Assert.NotNull(gameContext);
        }

        [Test]
        public void TestAAA() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_1,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_5,
                Card.Crak_6,
                Card.Crak_6,
                Card.Crak_6,
                Card.Bam_2
            };

            var gameContext = _algorithm.IsHu(cards, Card.Bam_2);

            var huCards2 = _algorithm.GetAAA(gameContext.HuContexts[0]);
            Assert.Contains(Card.Bam_1, huCards2);
            Assert.Contains(Card.Crak_6, huCards2);
        }

        [Test]
        public void TestABC() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_2,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_5,
                Card.Crak_6,
                Card.Crak_6,
                Card.Crak_6,
            };

            var gameContext = _algorithm.IsHu(cards, Card.Bam_2);

            var huCards2 = _algorithm.GetABC(gameContext.HuContexts[0]);
            Assert.Contains(Card.Crak_3, huCards2);
        }

        [Test]
        public void TestCC() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_2,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_5,
                Card.Crak_6,
                Card.Crak_6,
                Card.Crak_6,
            };

            var gameContext = _algorithm.IsHu(cards, Card.Bam_2);

            var cc2 = _algorithm.GetCC(gameContext.HuContexts[0]);
            Assert.That(Card.Bam_2, Is.EqualTo(cc2));
        }

        [Test]
        public void TestPeng() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_2,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_5,
                Card.Crak_6,
                Card.Crak_6,
                Card.Crak_6,
            };

            var result = _algorithm.IsAAA(cards, Card.Bam_1);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void TestGang() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_2,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_5,
                Card.Crak_6,
                Card.Crak_6,
                Card.Crak_6,
            };

            var result = _algorithm.IsAAAA(cards, Card.Crak_6);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void TestChi() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_2,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_5,
                Card.Crak_6,
                Card.Crak_6,
                Card.Crak_6,
            };

            var result = _algorithm.IsABC(cards, Card.Crak_4);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void TestLeftChi() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_2,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_5,
                Card.Crak_6,
                Card.Crak_6,
                Card.Crak_6,
            };

            var result = _algorithm.IsXBC(cards, Card.Crak_2);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void TestMiddleChi() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_2,
                Card.Crak_3,
                Card.Crak_5,
                Card.Crak_6,
                Card.Crak_6,
                Card.Crak_6,
            };

            var result = _algorithm.IsAXC(cards, Card.Crak_4);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void TestRightChi() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_2,
                Card.Crak_3,
                Card.Crak_5,
                Card.Crak_6,
                Card.Crak_6,
                Card.Crak_6,
            };

            var result = _algorithm.IsABX(cards, Card.Crak_7);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void Test_Ting_5_1() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_2,

                Card.Bam_3 // 癞子
            };

            var tingCards = _algorithm.IsTing(cards, Card.Bam_3);
             Assert.That(4, Is.EqualTo(tingCards.Length));
        }

        [Test]
        public void Test_Ting_5_2() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_2,// 癞子
                Card.Bam_3 // 癞子
            };

            var tingCards = _algorithm.IsTing(cards, Card.Bam_2,Card.Bam_3);
            Assert.That(87, Is.EqualTo(tingCards.Length));
        }

        [Test]
        public void Test_Ting_8_2() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Bam_2,
                Card.Bam_3,// 癞子
                Card.Bam_3,// 癞子
                Card.Bam_4,
                Card.Bam_5,
            };

            var tingCards = _algorithm.IsTing(cards, Card.Bam_3);
            Assert.That(11, Is.EqualTo(tingCards.Length));
        }

        [Test]
        public void Test_Ting_11_3() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_2,
                Card.Bam_3,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_5,// 癞子
                Card.Crak_5,// 癞子
                Card.Crak_5,// 癞子
                Card.Crak_6,
                Card.Dot_1
            };

            var tingCards = _algorithm.IsTing(cards, Card.Crak_5);
            Assert.That(16, Is.EqualTo(tingCards.Length));
        }

        [Test]
        public void Test_Ting_11_4() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_2,
                Card.Bam_3,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_5,// 癞子
                Card.Crak_5,// 癞子
                Card.Crak_5,// 癞子
                Card.Crak_5,// 癞子
                Card.Dot_1
            };

            var tingCards = _algorithm.IsTing(cards, Card.Crak_5);
            Assert.That(257, Is.EqualTo(tingCards.Length));
        }
    }
}
