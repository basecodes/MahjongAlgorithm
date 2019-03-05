using MahjongAlgorithm;
using MahjongAlgorithm.Algorithm;
using NUnit.Framework;
using System;
using System.Diagnostics;


namespace Test {
    [TestFixture]
    public class TestTableSpeed {

        private IAlgorithm _algorithm;

        [OneTimeSetUp]
        public void Loading() {
            _algorithm = Mahjong.GetAlgorithm(
                "./Resources/TingTable.json",
                "./Resources/LaiTable.json",
                "./Resources/HuTable.json");
        }

        [Test]
        public void TestIsHuSpeed() {
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

            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000000; i++) {
                _algorithm.IsHu(cards, Card.Bam_2);
            }
            stopwatch.Stop();
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestIsTingSpeed() {
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

            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000000; i++) {
                _algorithm.IsTing(cards);
            }
            stopwatch.Stop();
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_2_1() {
            var cards = new Card[] {
                Card.Bam_1 // 癞子
            };

            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Bam_1, 1);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_5_1() {
            var cards = new Card[] {
                Card.Bam_1, // 癞子
                Card.Crak_2, 
                Card.Crak_2,
                Card.Crak_2, 
            };

            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Bam_1);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_5_2() {
            var cards = new Card[] {
                Card.Bam_1, // 癞子
                Card.Bam_1, // 癞子
                Card.Crak_2,
                Card.Crak_2,
                
            };

            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Bam_1);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_5_3() {
            var cards = new Card[] {
                Card.Bam_1, // 癞子
                Card.Bam_1, // 癞子
                Card.Bam_1, // 癞子
                Card.Crak_2, 
            };

            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Bam_1);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_5_4() {
            var cards = new Card[] {
                Card.Bam_1, // 癞子
                Card.Bam_1, // 癞子
                Card.Bam_1, // 癞子
                Card.Bam_1, // 癞子
            };

            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Bam_1);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_8_1() {
            var cards = new Card[] {
                Card.Bam_1, 
                Card.Bam_1, 
                Card.Crak_3, 
                Card.Crak_4,
                Card.Crak_5,
                Card.Crak_6,// 癞子
                Card.Dot_3
            };

            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Crak_6);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_8_2() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Dot_3
            };
            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Crak_6);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_8_3() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Crak_3,
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Dot_3
            };
            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Crak_6);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_8_4() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_1,
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Dot_3
            };
            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Crak_6);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_11_1() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_2,
                Card.Bam_3,
                Card.Crak_6,// 癞子
                Card.Dot_3,
                Card.Dot_4,
                Card.Dot_5,
                Card.Dot_6,
                Card.East,
                Card.East,
            };
            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Crak_6);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_11_2() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_2,
                Card.Bam_3,
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Dot_4,
                Card.Dot_5,
                Card.Dot_6,
                Card.East,
                Card.East,
            };
            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Crak_6);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_11_3() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_2,
                Card.Bam_3,
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Dot_5,
                Card.Dot_6,
                Card.East,
                Card.East,
            };
            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Crak_6);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_11_4() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_2,
                Card.Bam_3,
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Dot_6,
                Card.East,
                Card.East,
            };
            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Crak_6);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_14_1() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_2,
                Card.Bam_3,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_5,
                Card.Crak_6,// 癞子
                Card.Dot_6,
                Card.Dot_7,
                Card.Dot_8,
                Card.Dot_9,
                Card.East,
                Card.East,
            };
            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Crak_6);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_14_2() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_2,
                Card.Bam_3,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_5,
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Dot_7,
                Card.Dot_8,
                Card.Dot_9,
                Card.East,
                Card.East,
            };
            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Crak_6);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_14_3() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_2,
                Card.Bam_3,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Dot_7,
                Card.Dot_8,
                Card.Dot_9,
                Card.East,
                Card.East,
            };
            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Crak_6);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }

        [Test]
        public void TestTingSpeed_14_4() {
            var cards = new Card[] {
                Card.Bam_1,
                Card.Bam_2,
                Card.Bam_3,
                Card.Crak_3,
                Card.Crak_4,
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Crak_6,// 癞子
                Card.Dot_8,
                Card.Dot_9,
                Card.East,
                Card.East,
            };
            Card[][] result = null;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++) {
                result = _algorithm.IsTing(cards, Card.Crak_6);
            }
            stopwatch.Stop();
            Assert.That(true, Is.EqualTo(result.Length != 0));
            Console.WriteLine($"时间：{stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
