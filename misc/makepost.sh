#!/bin/bash

if [ -z "$1" ]; then
    echo "Missing slug!"
    exit 1
else 
    slug=$1
fi

if [ -z "$2" ]; then
    echo "Missing title!"
    exit 1
else
    title=$2
fi

date=`date +%F`
target=$slug.md

cp ./template.md $target

sed -i "s/@slug/$slug/" $target
sed -i "s/@date/$date/" $target
sed -i "s/@title/$title/" $target
