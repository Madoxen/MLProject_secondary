import sys
from random import randint
lines = open(sys.argv[1], "r").readlines()
h = lines[0]
lines = lines[1:]
num = int(sys.argv[2])

print(h,end = "")
s = len(lines)

for i in range(num):
    lines.append(lines[randint(0,s)])

for i in lines:
        print(float(i))


