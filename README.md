# 麻将算法
### 使用 .Net Core 2.2实现的麻将算法，算法分为回溯类型和表类型两种。

## 功能
1. 吃碰杠
2. 平胡、平听
3. 癞子听

## 效率
测试配置：CPU:I7-6700HQ  RAM:DDR4 16G
1. 回溯类型
    * 一般操作

        操作 | 时间
        ------------ | -------------
        平听 x 100 0000 | 62.127s
        平胡 x 100 0000 | 24.786s

    * 癞子听
    
        次数：1000次 | 癞子 x 1 |癞子 x 2 |癞子 x 3 |癞子 x 4
        ------------ | ------------- | ------------- | ------------- | -------------
        总牌数 = 2 | 2.47s | --- | --- | ---
        总牌数 = 5| 3.319s | 1.83m | 0.887h | 28.266h
        总牌数 = 8| 3.613s | 1.918m | 1.123h | 36.083h
        总牌数 = 11| 3.507s | 2.046m | 1.199h | 40.833h
        总牌数 = 14| 3.66s | 2.196m | 1.278h | 43.983h

2. 表类型
    * 一般操作

        操作 | 时间
        ------------ | -------------
        平听 x 100 0000 | 3.226s
        平胡 x 100 0000 | 1.189s

    * 癞子听
    
        次数：1000次 | 癞子 x 1 |癞子 x 2 |癞子 x 3 |癞子 x 4
        ------------ | ------------- | ------------- | ------------- | -------------
        总牌数 = 2 | 60ms | --- | --- | ---
        总牌数 = 5| 334ms | 2.371s |  5.618s |  2.287m
        总牌数 = 8| 583ms | 4.045s | 27.823s |  3.863m
        总牌数 = 11| 740ms | 4.812s | 27.36s | 3.19m
        总牌数 = 14| 1.303s | 7.722s | 42.093s | 3.476m

## 回溯 vs 表
回溯类型启动快，但是胡牌听牌运算量大。表类型启动慢，因为加载表需要时间，但是胡牌听牌运算量相对于回溯类型就小很多了，从上面的数据就可以看出。建议开发阶段首选回溯类型，发布阶段首选表类型。

## 用法
1. 生成表或者从Resources解压出对应生成好的表

    表生成器需要Python执行环境，执行Generator/Main.py里面对应构建生成胡表，听表，癞子表。或者直接使用已经生成好的Resource文件夹下的表。胡表211k，听表约4m，癞子表约311m。如果不需要自行定制生成建议使用已经生成好的表，因为癞子表生成比较花时间，约3个小时。目前这些表几乎涵盖了所有常规胡牌类型以及常规的听牌类型。

2. 获得算法。

    ```C#
    // 无参为DFS算法，有参为Table算法，参数为对应表路径 
    var algorithm = Mahjong.GetAlgorithm()
    ```

3. 执行相应的操作

    ```C#
    // 是否可以碰
    bool IsAAA(Card[] cards, Card card);
    // 是否可以杠
    bool IsAAAA(Card[] cards, Card card);
    // 是否可以吃
    bool IsABC(Card[] cards, Card card);
    // 是否可以左吃，例如 23，左边缺1
    bool IsXBC(Card[] cards, Card card);
    // 是否可以中吃，例如 24，中间缺3
    bool IsAXC(Card[] cards, Card card);
    // 是否可以右吃，例如 34，右边缺5
    bool IsABX(Card[] cards, Card card);
    // 获取刻子
    Card[] GetAAA(IHuContext huContext);
    // 获取顺子
    Card[] GetABC(IHuContext huContext);
    // 获取雀头
    Card GetCC(IHuContext huContext);
    // 听牌
    Card[] IsTing(Card[] cards);
    // 带癞子的听牌
    Card[][] IsTing(Card[] cards, Card ignoreCard,int ignoreCount = 4);
    // 带癞子的听牌
    Card[][] IsTing(Card[] cards, params Card[] ignoreCards);
    // 是否可胡牌
    Context IsHu(Card[] cards, Card card);
    ```