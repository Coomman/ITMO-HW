#!/usr/bin/env python

import sys
import numpy as np


def calc_var(prices: np.array) -> str:
    return f'1\t{prices.size}\t{prices.mean()}\t{prices.var()}'


def main():
    prices = []

    line = sys.stdin.readline().strip()
    while line:
        prices += [int(line)]
        line = sys.stdin.readline().strip()

    print(calc_var(np.array(prices)))


if __name__ == "__main__":
    main()
