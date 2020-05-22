import sys
from random import uniform
lines = open(sys.argv[1], "r").readlines()
h = lines[0]
lines = lines[1:]
r = float(sys.argv[2])
print(h,end = "")
for i in lines:
    print(float(i)+uniform(-r,r))

