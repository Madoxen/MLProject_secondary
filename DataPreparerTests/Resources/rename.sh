#!/bin/bash
a=1
for file in test*; do
new=$(printf "test_%01d.png" "$a")
mv "$file" "$new"
let a=a+1
done