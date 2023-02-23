import sys
import numpy as np


def reduce(sizes: np.array, means: np.array) -> float:
    return np.dot(sizes, means) / sizes.sum()


def main():
    sizes = []
    means = []

    line = sys.stdin.readline().strip()
    while line:
        spl = line.split("\t")
        sizes += [int(spl[0])]
        means += [float(spl[1])]
        line = sys.stdin.readline().strip()

    print(reduce(np.array(sizes), np.array(means)))


if __name__ == "__main__":
    main()