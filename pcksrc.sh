#!/bin/bash

csflist=$(find .  -not -path "*/obj/*" | grep "\.cs$")
pyflist=$(find . | grep "\.py$")


for f in $csflist 
do
    echo -e "\n" >> completeSrc.txt
    echo "============================" >> completeSrc.txt
    echo $f >> completeSrc.txt
    echo "============================" >> completeSrc.txt
    cat $f >> completeSrc.txt
done


for f in $pyflist 
do
    echo -e "\n" >> completeSrc.txt
    echo "============================" >> completeSrc.txt
    echo $f >> completeSrc.txt
    echo "============================" >> completeSrc.txt
    cat $f >> completeSrc.txt
done