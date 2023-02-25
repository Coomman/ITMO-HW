#!/usr/bin/env python

import sys
import numpy as np


def calc_mean(prices: np.array) -> str:
    return f'1\t{prices.size}\t{prices.mean()}'


def main():
    prices = []

    line = sys.stdin.readline().strip()
    while line:
        prices += [int(line)]
        line = sys.stdin.readline().strip()

    print(calc_mean(np.array(prices)))


if __name__ == "__main__":
    main()
