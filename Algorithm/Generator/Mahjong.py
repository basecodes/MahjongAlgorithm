# coding=utf8

from Utils import *
from threading import *
import timeit


class Mahjong:
    def __init__(self):
        pass

    # 排列
    def arrangement(self, array):
        temp = []
        for item in array:
            # 克隆数组
            tmp = copy.deepcopy(array)

            # 依次删除对应位置元素
            offset = array.index(item)
            del tmp[offset]

            # 将剩余元素递归
            list = self.arrangement(tmp)
            if len(list) == 0:
                temp.append([item])
            else:
                for element in list:
                    nums = [item] + element
                    # 忽略已处理排列
                    if temp.count(nums) == 0:
                        temp.append(nums)
        return temp

    # 组合
    def combination(self, array):
        if len(array) == 1:
            return [array]

        # 图案不重叠(自由排列)
        ret = self.arrangement(array)

        # 重叠模式
        firstSet = set()
        length = len(array)

        for i in range(0, length):
            for j in range(i + 1, length):

                # [[left],0,[right]]
                left = array[i]
                right = array[j]

                # 加0防止3+1=2+2
                key = [left, 0, right]

                # 排除已处理组合
                if str(key) not in firstSet:

                    firstSet.add(str(key))
                    twoSet = set()

                    # 组合最长长度
                    total = len(left) + len(right)

                    # 在移动范围内同时叠加[i]和[j]
                    for k in range(0, total + 1):

                        # [right,left,right] 包含顺序
                        template = [0] * len(right) + left + [0] * len(right)
                        rightLength = len(right)

                        # k <= offset <= k + rightLength,right整体以k位置为起始右移
                        for m in range(0, rightLength):
                            value = right[m]
                            offset = k + m
                            template[offset] += value

                        # 删除额外的0
                        while template.count(0) > 0:
                            template.remove(0)

                        # 检查是否有大于一个杠的
                        if max(template) > 4:
                            continue

                        # 检查它长度是否超过两个杠
                        if len(template) > 9:
                            continue

                        if str(template) not in twoSet:
                            twoSet.add(str(template))
                            temp = copy.deepcopy(array)
                            del temp[i]
                            del temp[j - 1]

                            # 组合剩余元素
                            values = self.combination([template] + temp)
                            # 去重
                            for value in values:
                                if ret.count(value) == 0:
                                    ret.append(value)
        return ret

    # 计算编码
    def calcKey(self, array):

        if array == [[]]:
            return 0

        ret = 0
        len = -1
        # 从右向左编码
        for item in array:
            for value in item:
                # 添加0
                len += 1
                if value == 2:
                    ret |= 0b11 << len
                    len += 2
                elif value == 3:
                    ret |= 0b1111 << len
                    len += 4
                elif value == 4:
                    ret |= 0b111111 << len
                    len += 6
                else:
                    pass
            # 添加10
            ret |= 0b1 << len
            len += 1

        return ret

    # 计算值
    def calcValue(self, array):
        ret = []
        CCCount = 0

        # 组合个数
        for i in range(0, len(array)):
            # 元素个数
            for j in range(0, len(array[i])):
                # 找到CC
                if array[i][j] >= 2:
                    # AAA 和 ABC 不同取法
                    for y in range(0, 2):
                        temp = copy.deepcopy(array)
                        # CC清0
                        temp[i][j] -= 2

                        # 位置指针
                        position = 0
                        # AAA的位置数组
                        AAAPositions = []
                        # ABC的位置数组
                        ABCPositions = []

                        for k in range(0, len(temp)):
                            for m in range(0, len(temp[k])):
                                if y == 0:
                                    # 取出AAA
                                    if temp[k][m] >= 3:
                                        temp[k][m] -= 3
                                        AAAPositions.append(position)

                                    # 取出ABC
                                    while len(temp[k]) - m >= 3 and temp[k][m] >= 1 and temp[k][m + 1] >= 1 and temp[k][
                                        m + 2] >= 1:
                                        temp[k][m] -= 1
                                        temp[k][m + 1] -= 1
                                        temp[k][m + 2] -= 1
                                        ABCPositions.append(position)
                                else:
                                    # 取出ABC
                                    while len(temp[k]) - m >= 3 and temp[k][m] >= 1 and temp[k][m + 1] >= 1 and \
                                            temp[k][m + 2] >= 1:
                                        temp[k][m] -= 1
                                        temp[k][m + 1] -= 1
                                        temp[k][m + 2] -= 1
                                        ABCPositions.append(position)

                                    # 取出AAA
                                    if temp[k][m] >= 3:
                                        temp[k][m] -= 3
                                        AAAPositions.append(position)
                                position += 1
                        # 是否都取完了
                        if max(flatten(temp)) == 0:
                            value = len(AAAPositions) + (len(ABCPositions) << 3) + (CCCount << 6)
                            offset = 10
                            for x in AAAPositions:
                                value |= x << offset
                                offset += 4

                            for x in ABCPositions:
                                value |= x << offset
                                offset += 4

                            if len(array) == 1:
                                if array == [[4, 1, 1, 1, 1, 1, 1, 1, 3]] or \
                                        array == [[3, 2, 1, 1, 1, 1, 1, 1, 3]] or \
                                        array == [[3, 1, 2, 1, 1, 1, 1, 1, 3]] or \
                                        array == [[3, 1, 1, 2, 1, 1, 1, 1, 3]] or \
                                        array == [[3, 1, 1, 1, 2, 1, 1, 1, 3]] or \
                                        array == [[3, 1, 1, 1, 1, 2, 1, 1, 3]] or \
                                        array == [[3, 1, 1, 1, 1, 1, 2, 1, 3]] or \
                                        array == [[3, 1, 1, 1, 1, 1, 1, 2, 3]] or \
                                        array == [[3, 1, 1, 1, 1, 1, 1, 1, 4]]:
                                    value |= 1 << 27

                            # [111111111]
                            if len(array) <= 3 and len(ABCPositions) >= 3:
                                index = 0
                                for element in array:
                                    if len(element) == 9:
                                        if element.count(0) > 0 and element.count(3) > 0 and element.count(6) > 0:
                                            value |= 1 << 28
                                    index += len(element)

                            # [222][222]
                            if len(ABCPositions) == 4 and ABCPositions[0] == ABCPositions[1] and ABCPositions[2] == \
                                    ABCPositions[3]:
                                value |= 1 << 29
                            elif len(ABCPositions) >= 2 and len(AAAPositions) + len(ABCPositions) == 4:
                                if len(ABCPositions) - len(set(flatten(ABCPositions))) >= 1:
                                    value |= 1 << 30
                            ret.append(value)
                CCCount += 1

        if len(ret) > 0:
            ret = list(set(ret))
            for index in range(0, len(ret)):
                ret[index] = ret[index]
            return ret

        flattenArray = flatten(array)
        if sum(flattenArray) == 14 and flattenArray.count(2) == 7:
            return [1 << 26]
        return None

    # 构建胡表
    def huBuilder(self, array):
        predict = dict()
        for element in array:
            key = self.calcKey(element)
            value = self.calcValue(element)

            predict[key] = value
        return predict

    # 构建听表
    def tingBuilder(self, array):
        predict = dict()

        for element in array:
            def func(key, dicts):
                if not predict.has_key(key):
                    predict[key] = [dicts]
                else:
                    predict[key] += [dicts]

            self.parseElement(element, func)
        return predict

    def parseElement(self, element, action):
        index = 0
        key = self.calcKey(element)
        value = self.calcValue(element)

        for i in range(len(element)):
            for j in range(len(element[i])):
                tmp = copy.deepcopy(element)
                tmp[i][j] -= 1

                dicts = dict()
                dicts["K"] = key
                dicts["Vs"] = value

                # 拆分
                if tmp[i][j] == 0:
                    # 组内含有多种类型元素
                    if 0 < j <= len(element[i]):
                        # 前面位置+1
                        dicts["R"] = index - 1
                        dicts["O"] = 2
                    elif j == 0:
                        # 后面位置前置，不用做任何操作
                        dicts["R"] = index
                        dicts["O"] = 1

                    left, right = split(tmp[i], j)
                    tmp[i] = right
                    tmp.insert(i, left)
                    for _ in range(tmp.count([])):
                        tmp.remove([])
                else:
                    # 相同元素
                    dicts["R"] = index
                    dicts["O"] = 0

                if action is not None:
                    action(self.calcKey(tmp), dicts)

                index += 1

    def find(self, count, offsets, length, array, offset, store):
        if count == 0:
            element = [length, offsets, array]
            if element not in store:
                store.append(element)
            return

        for i in range(offset, len(array)):
            if array[i] == 0:
                continue

            copyArray = copy.deepcopy(array)
            newArray = copy.deepcopy(offsets)

            copyArray[i] -= 1
            if copyArray[i] == 0:
                newArray.append([1, i])
                del copyArray[i]
            else:
                newArray.append([0, i])

            self.find(count - 1, newArray, length, copyArray, i, store)

    def threadLaiHuBuilder(self, array):
        predict = dict()
        length = len(array)
        threads = []
        for i in range(length):
            if i % 6 == 0:
                for t in threads:
                    t.join()
                threads = []

            def func(element, count, index):
                print("Start: " + str(element) + " length:" + str(count) + " index:" + str(index))
                start = timeit.default_timer()
                self.laiHuBuilder(element, predict)
                stop = timeit.default_timer()
                time = stop - start
                print("End: " + str(element) + " length:" + str(count) + " index:" + str(index) + " Time:" + str(
                    time))

            t = Thread(target=func, args=(array[i], length, i))
            t.start()
            threads.append(t)

        for t in threads:
            t.join()

        return predict

    # 构建癞子表
    def laiHuBuilder(self, element, predict):
        # 展开元素
        elements = flatten(element)
        num = sum(elements)

        store = []
        for j in range(1, 6, 1):
            if num >= j:
                self.find(j, [], j, elements, 0, store)

        key = self.calcKey(element)
        # 删除多个癞子牌后组合
        for item in store:
            length = item[0]
            dicts = dict()
            dicts["Os"] = item[1][::-1]
            dicts["M"] = key
            #dicts["F"] = str(element)

            with Lock():
                if length not in predict:
                    predict[length] = dict()

                tmpKey = self.calcKey([item[2]])
                if tmpKey not in predict[length]:
                    predict[length][tmpKey] = []

                if dicts not in predict[length][tmpKey]:
                    predict[length][tmpKey].append(dicts)
