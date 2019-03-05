# coding=utf8

import copy
# 多维数组转一维数组
def flatten(l):
    return flatten(l[0]) + (flatten(l[1:]) if len(l) > 1 else []) if type(l) is list else [l]

def split(array, index):
    left = []
    if index - 1 >= 0:
        left = array[:index]
    right = []
    if index + 1 < len(array):
        right = array[index + 1:]
    return left, right

# 移除重叠
def removeoverlay(array):
    list = copy.deepcopy(array)
    for item in array:
        temp = flatten(item)
        if max(temp) > 2:
            list.remove(item)
    return list
