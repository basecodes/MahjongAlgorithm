
using System;
using System.Collections.Generic;
using System.Text;

namespace MahjongAlgorithm.Algorithm {
    public interface IAlgorithm {
        /// <summary>
        /// 是否可以碰
        /// </summary>
        /// <param name="cards">所有的手牌</param>
        /// <param name="card">要检查是否可以碰的牌</param>
        /// <returns>返回是否可以碰</returns>
        bool IsAAA(Card[] cards, Card card);

        /// <summary>
        /// 是否可以杠
        /// </summary>
        /// <param name="cards">所有的手牌</param>
        /// <param name="card">要检查是否可以杠的牌</param>
        /// <returns>返回是否可以杠 </returns>
        bool IsAAAA(Card[] cards, Card card);

        /// <summary>
        /// 是否可以吃
        /// </summary>
        /// <param name="cards">所有的手牌</param>
        /// <param name="card">要检查是否可以吃的牌</param>
        /// <returns>返回是否可以吃 </returns>
        bool IsABC(Card[] cards, Card card);

        /// <summary>
        /// 是否可以左吃，例如 23，左边缺1
        /// </summary>
        /// <param name="cards">所有的手牌</param>
        /// <param name="card">要检查是否可以吃的牌</param>
        /// <returns>返回是否可以左吃</returns>
        bool IsXBC(Card[] cards, Card card);

        /// <summary>
        /// 是否可以中吃，例如 24，中间缺3
        /// </summary>
        /// <param name="cards">所有的手牌</param>
        /// <param name="card">要检查是否可以吃的牌</param>
        /// <returns>返回是否可以中吃</returns>
        bool IsAXC(Card[] cards, Card card);

        /// <summary>
        /// 是否可以右吃，例如 34，右边缺5
        /// </summary>
        /// <param name="cards">所有的手牌</param>
        /// <param name="card">要检查是否可以吃的牌</param>
        /// <returns>返回是否可以右吃</returns>
        bool IsABX(Card[] cards, Card card);

        /// <summary>
        /// 获取刻子
        /// </summary>
        /// <param name="huContext">胡上下文</param>
        /// <returns>返回刻子</returns>
        Card[] GetAAA(IHuContext huContext);

        /// <summary>
        /// 获取顺子
        /// </summary>
        /// <param name="huContext">胡上下文</param>
        /// <returns>返回顺子</returns>
        Card[] GetABC(IHuContext huContext);

        /// <summary>
        /// 获取雀头
        /// </summary>
        /// <param name="huContext">胡上下文</param>
        /// <returns>返回雀头</returns>
        Card GetCC(IHuContext huContext);

        /// <summary>
        /// 听牌
        /// </summary>
        /// <param name="cards">所有的手牌</param>
        /// <returns>返回可听的牌 </returns>
        Card[] IsTing(Card[] cards);

        /// <summary>
        /// 带癞子的听牌
        /// </summary>
        /// <param name="cards">所有的手牌</param>
        /// <param name="ignoreCard">癞子牌</param>
        /// <param name="ignoreCount">癞子牌张数</param>
        /// <returns>返回可听的牌</returns>
        Card[][] IsTing(Card[] cards, Card ignoreCard,int ignoreCount = 4);

        /// <summary>
        /// 带癞子的听牌
        /// </summary>
        /// <param name="cards">所有的手牌</param>
        /// <param name="ignoreCards">癞子牌</param>
        /// <returns>返回可听的牌</returns>
        Card[][] IsTing(Card[] cards, params Card[] ignoreCards);

        /// <summary>
        /// 是否可胡牌
        /// </summary>
        /// <param name="cards">所有的手牌</param>
        /// <param name="card">要检查是否可以胡的牌 </param>
        /// <returns>返回上下文</returns>
        Context IsHu(Card[] cards, Card card);
    }
}
