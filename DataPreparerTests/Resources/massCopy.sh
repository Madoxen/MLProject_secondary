#!/bin/bash
file=$1
for (( c=1; c<=$2; c++ ))
do
new=$(printf "test_%01d.png" "$c")
cp "$file" "$new"
done