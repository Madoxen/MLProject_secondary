#!/bin/bash

csflist=$(find .  -not -path "*/obj/*" | grep "\.cs$")
pyflist=$(find . | grep "\.py$")


for f in $csflist 
do
    echo -e "\n" >> completeSrc.txt
    filename=$(echo $f | grep -o '[^/]\+$')
    echo "\subsubsection{$filename}" >> completeSrc.txt
    echo "\begin{lstlisting}" >> completeSrc.txt
    cat $f >> completeSrc.txt
    echo "\end{lstlisting}" >> completeSrc.txt
    echo "\pagebreak" >> completeSrc.txt    
  
done


for f in $pyflist 
do
    echo -e "\n" >> completeSrc.txt
    filename=$(echo $f | grep -o '[^/]\+$')
    echo "\subsubsection{$filename}" >> completeSrc.txt
    echo "\begin{lstlisting}" >> completeSrc.txt
    cat $f >> completeSrc.txt
    echo "\end{lstlisting}" >> completeSrc.txt
    echo "\pagebreak" >> completeSrc.txt
done