# coding=utf8

import json
from Mahjong import *
from threading import *


def table(mahjong):
    array = []
    # ----------------------------------------
    # [[1, 1, 1], [1, 1, 1], [1, 1, 1], [1, 1, 1], [2]]
    array += mahjong.combination([[1, 1, 1], [1, 1, 1], [1, 1, 1], [1, 1, 1], [2]])

    # [[1,1,1],[1,1,1],[1,1,1],[3],[2]]
    array += mahjong.combination([[1, 1, 1], [1, 1, 1], [1, 1, 1], [3], [2]])

    # [[1,1,1],[1,1,1],[3],[3],[2]]
    array += mahjong.combination([[1, 1, 1], [1, 1, 1], [3], [3], [2]])

    # [[1,1,1],[3],[3],[3],[2]]
    array += mahjong.combination([[1, 1, 1], [3], [3], [3], [2]])

    # [[3],[3],[3],[3],[2]]
    array += mahjong.combination([[3], [3], [3], [3], [2]])

    # [[2],[2],[2],[2],[2],[2],[2]],7个2自由组合及叠加组合,去除叠加,即含有4的
    array += removeoverlay(mahjong.combination([[2], [2], [2], [2], [2], [2], [2]]))

    # ----------------------------------------

    # [[1, 1, 1], [1, 1, 1], [1, 1, 1], [2]]
    array += mahjong.combination([[1, 1, 1], [1, 1, 1], [1, 1, 1], [2]])

    # [[1, 1, 1], [1, 1, 1], [3], [2]]
    array += mahjong.combination([[1, 1, 1], [1, 1, 1], [3], [2]])

    # [[1, 1, 1], [3], [3], [2]]
    array += mahjong.combination([[1, 1, 1], [3], [3], [2]])

    # [[3], [3], [3], [2]]
    array += mahjong.combination([[3], [3], [3], [2]])

    # ----------------------------------------

    # [[1, 1, 1], [1, 1, 1], [2]]
    array += mahjong.combination([[1, 1, 1], [1, 1, 1], [2]])

    # [[1, 1, 1], [3], [2]]
    array += mahjong.combination([[1, 1, 1], [3], [2]])

    # [[3], [3], [2]]
    array += mahjong.combination([[3], [3], [2]])

    # ----------------------------------------

    # [[1, 1, 1], [2]]
    array += mahjong.combination([[1, 1, 1], [2]])

    # [[3], [2]]
    array += mahjong.combination([[3], [2]])

    # ----------------------------------------

    # [[2]]
    array += mahjong.combination([[2]])
    return array


def testTable(mahjong):
    array = []
    array += mahjong.combination([[1, 1, 1], [2]])
    return array


def tingTable(mahjong):
    array = table(mahjong)
    ting = mahjong.tingBuilder(array)
    print("Ting Table:", len(ting))
    file = open('TingTable.json', 'w+')
    file.write(json.dumps(ting))


def huTable(mahjong):
    array = table(mahjong)
    hu = mahjong.huBuilder(array)
    print("Hu Table:", len(hu))
    file = open('HuTable.json', 'w+')
    file.write(json.dumps(hu))


def laiTable(mahjong):
    array = table(mahjong)
    dicts = mahjong.threadLaiHuBuilder(array)
    print("Lai Table:", len(dicts))
    file = open('LaiTable.json', 'w')
    file.write(json.dumps(dicts))


if __name__ == '__main__':
    start = timeit.default_timer()
    mahjong = Mahjong()
    laiTable(mahjong)
    stop = timeit.default_timer()
    time = stop - start
    print("Time:" + str(time))
