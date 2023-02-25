#!/usr/bin/env python

import sys


def reduce(c1, c2, m1, m2, v1, v2):
    m = (c1 * m1 + c2 * m2) / (c1 + c2)
    v = (c1 * v1 + c2 * v2) / (c1 + c2) + c1 * c2 * (((m2 - m1) / (c1 + c2)) ** 2)

    return c1 + c2, m, v


def main():
    sizes = []
    means = []
    variances = []

    line = sys.stdin.readline().strip()
    while line:
        spl = line.split("\t")[1:]
        sizes += [int(spl[0])]
        means += [float(spl[1])]
        variances += [float(spl[2])]
        line = sys.stdin.readline().strip()

    c, m, v = sizes[0], means[0], variances[0]

    for i in range(1, len(sizes)):
        c, m, v = reduce(c, sizes[i], m, means[i], v, variances[i])

    print(v)


if __name__ == "__main__":
    main()